using NetworkMessages.FromServer;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using System.Collections.Generic;

namespace RoblettinCommands.Commands
{
    class KickClan : RoblettinCommand
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!kickclan";
        }

        public string Description()
        {
            return "Kicks players whose names contain the provided input. Usage !kick <Player Name>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please provide a username. Players that contain the provided input will be kicked."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            string targetNameFragment = string.Join(" ", args);
            List<NetworkCommunicator> playersToKick = new List<NetworkCommunicator>();

            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.UserName.Contains(targetNameFragment))
                {
                    playersToKick.Add(peer);
                }
            }

            if (playersToKick.Count == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("No players found with the provided name fragment."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            foreach (NetworkCommunicator targetPeer in playersToKick)
            {
                GameNetwork.BeginBroadcastModuleEvent();
                GameNetwork.WriteMessage(new ServerMessage("Player " + targetPeer.UserName + " is kicked from the server"));
                GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);

                DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(targetPeer.VirtualPlayer.Id, false);
            }

            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage(playersToKick.Count + " players kicked by admin."));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);

            return true;
        }
    }
}
