﻿using NetworkMessages.FromServer;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using System.Collections.Generic;

namespace RoblettinCommands.Commands
{
    class Kill : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!killclan";
        }

        public string Description()
        {
            return "Kills players whose names contain the provided input. Usage !kill <Player Name>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please provide a username. Players that contain the provided input will be killed."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            string targetNameFragment = string.Join(" ", args);
            List<NetworkCommunicator> playersToKill = new List<NetworkCommunicator>();

            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.UserName.Contains(targetNameFragment))
                {
                    playersToKill.Add(peer);
                }
            }

            if (playersToKill.Count == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("No players found with the provided name fragment."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            foreach (NetworkCommunicator targetPeer in playersToKill)
            {
                if (!targetPeer.ControlledAgent.Equals(null))
                {
                    var agent = targetPeer.ControlledAgent;
                    var blow = new Blow(agent.Index);
                    blow.DamageType = TaleWorlds.Core.DamageTypes.Pierce;
                    blow.BoneIndex = agent.Monster.HeadLookDirectionBoneIndex;
                    blow.GlobalPosition = agent.Position;
                    blow.GlobalPosition.z = blow.GlobalPosition.z + agent.GetEyeGlobalHeight();
                    blow.BaseMagnitude = 2000f;
                    blow.WeaponRecord.FillAsMeleeBlow(null, null, -1, -1);
                    blow.InflictedDamage = 2000;
                    blow.SwingDirection = agent.LookDirection;
                    MatrixFrame frame = agent.Frame;
                    blow.SwingDirection = frame.rotation.TransformToParent(new Vec3(-1f, 0f, 0f, -1f));
                    blow.SwingDirection.Normalize();
                    blow.Direction = blow.SwingDirection;
                    blow.DamageCalculated = true;
                    sbyte mainHandItemBoneIndex = agent.Monster.MainHandItemBoneIndex;
                    AttackCollisionData attackCollisionDataForDebugPurpose = AttackCollisionData.GetAttackCollisionDataForDebugPurpose(false, false, false, true, false, false, false, false, false, false, false, false, CombatCollisionResult.StrikeAgent, -1, 0, 2, blow.BoneIndex, BoneBodyPartType.Head, mainHandItemBoneIndex, Agent.UsageDirection.AttackLeft, -1, CombatHitResultFlags.NormalHit, 0.5f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, Vec3.Up, blow.Direction, blow.GlobalPosition, Vec3.Zero, Vec3.Zero, agent.Velocity, Vec3.Up);
                    agent.RegisterBlow(blow, attackCollisionDataForDebugPurpose);
                }
            }

            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage(playersToKill.Count + " players killed by admin."));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);

            return true;
        }
    }
}
