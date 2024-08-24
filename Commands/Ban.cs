using NetworkMessages.FromServer;
using System.IO;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.DedicatedCustomServer;
using System.Collections.Generic;

namespace RoblettinCommands.Commands
{
    class Ban : Command
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return networkPeer.IsAdmin;
        }

        public string Command()
        {
            return "!banclan";
        }

        public string Description()
        {
            return "Bans players whose name contains the provided input. Usage: !ban <Player Name>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please provide a username fragment. All players that match will be banned."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            string targetNameFragment = string.Join(" ", args);
            List<NetworkCommunicator> playersToBan = new List<NetworkCommunicator>();

            // Bulunan tüm oyuncuları listele
            foreach (NetworkCommunicator peer in GameNetwork.NetworkPeers)
            {
                if (peer.UserName.Contains(targetNameFragment))
                {
                    playersToBan.Add(peer);
                }
            }

            // Eğer hiç oyuncu bulunamadıysa mesaj gönder
            if (playersToBan.Count == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("No players found with the provided name fragment."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            // Banlanacak oyuncular için işlemler
            foreach (NetworkCommunicator targetPeer in playersToBan)
            {
                using (StreamWriter sw = File.AppendText(BanManager.BanListPath()))
                {
                    sw.WriteLine(targetPeer.UserName + "|" + targetPeer.VirtualPlayer.Id.ToString());
                }

                GameNetwork.BeginBroadcastModuleEvent();
                GameNetwork.WriteMessage(new ServerMessage("Player " + targetPeer.UserName + " is banned from the server"));
                GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);

                DedicatedCustomServerSubModule.Instance.DedicatedCustomGameServer.KickPlayer(targetPeer.VirtualPlayer.Id, false);
            }

            // Toplam banlanan oyuncu sayısını bildir
            GameNetwork.BeginBroadcastModuleEvent();
            GameNetwork.WriteMessage(new ServerMessage(playersToBan.Count + " players banned by admin."));
            GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None);

            return true;
        }
    }
}
