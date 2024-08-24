﻿using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace RoblettinCommands
{
    class ChatHandler : GameHandler
    {
        public override void OnAfterSave() { }

        public override void OnBeforeSave() { }

        protected override void OnGameNetworkBegin()
        {
            this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
        }

        protected override void OnPlayerConnect(VirtualPlayer peer) {
            if (BanManager.IsPlayerBanned(peer)) {
                DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(peer.Id, false);
            }
        }

        protected override void OnGameNetworkEnd()
        {
            base.OnGameNetworkEnd();
            this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
        }

        private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
        {
            GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
            if (GameNetwork.IsServer) {
                networkMessageHandlerRegisterer.Register<NetworkMessages.FromClient.PlayerMessageAll>(new GameNetworkMessage.ClientMessageHandlerDelegate<NetworkMessages.FromClient.PlayerMessageAll>(this.HandleClientEventPlayerMessageAll));
            }
        }

        private bool HandleClientEventPlayerMessageAll(NetworkCommunicator networkPeer, NetworkMessages.FromClient.PlayerMessageAll message)
        {
            Debug.Print(networkPeer.UserName + " user send a message: " + message.Message, 0, Debug.DebugColor.Green);
            try
            {
                if (message.Message.StartsWith("!"))
                {
                    string[] argsWithCommand = message.Message.Split(' ');
                    string command = argsWithCommand[0];
                    string[] args = argsWithCommand.Skip(1).ToArray();
                    CommandManager.Instance.Execute(networkPeer, command, args);
                }
            } catch (Exception ex)
            {
                Debug.Print(ex.Message, 0, Debug.DebugColor.Green);
            }
            return true;
        }
    }
}
