using NetworkMessages.FromServer;
using System.Linq;
using TaleWorlds.MountAndBlade;

namespace RoblettinCommands.Commands
{
    class Counts : RoblettinCommand
    {
        public bool CanUse(NetworkCommunicator networkPeer)
        {
            return true; // Komutu tüm oyuncular kullanabilir.
        }

        public string Command()
        {
            return "!counts";
        }

        public string Description()
        {
            return "Counts the number of players whose name contains the provided tag. Usage: !counts <Tag>";
        }

        public bool Execute(NetworkCommunicator networkPeer, string[] args)
        {
            if (args.Length == 0)
            {
                GameNetwork.BeginModuleEventAsServer(networkPeer);
                GameNetwork.WriteMessage(new ServerMessage("Please provide a tag."));
                GameNetwork.EndModuleEventAsServer();
                return true;
            }

            string tag = args[0].ToUpper(); // Büyük harfe çevir
            int count = GetPlayerCountWithTag(tag);

            GameNetwork.BeginModuleEventAsServer(networkPeer);
            GameNetwork.WriteMessage(new ServerMessage($"[{tag}] Currently has {count} Players online."));
            GameNetwork.EndModuleEventAsServer();

            return true;
        }

        private int GetPlayerCountWithTag(string tag)
        {
            // Mevcut tüm oyunculara erişim.
            var allPeers = GameNetwork.NetworkPeers; // Bağlı olan tüm NetworkCommunicator'lar

            // İsimlerinde tag'i içeren oyuncuların sayısını döndür.
            return allPeers.Count(peer => peer.UserName.Contains($"[{tag}]"));
        }
    }
}
