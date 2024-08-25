using NetworkMessages.FromServer;
using System.IO;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using System.Collections.Generic;

namespace RoblettinCommands.Commands
{
    class BanTag : RoblettinCommand
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!bantag";
        }

        public string Description()
        {
            return "Bans players whose name contains the provided input. Usage: !ban <Player Name>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (RoblettinConfigs.isBanTagActive == true)
            {
                RoblettinConfigs.isBanTagActive = false;

                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Unallowed Tags are not allowed"));
                GameNetwork.EndModuleEventAsServer();
            }
            else
            {
                RoblettinConfigs.isBanTagActive = true;

                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Unallowed tags are allowed"));
                GameNetwork.EndModuleEventAsServer();
            }

            return true;
        }
    }
}
