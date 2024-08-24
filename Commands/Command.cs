using TaleWorlds.MountAndBlade;

namespace RoblettinCommands.Commands
{
    interface Command
    {
        string Command();
        bool CanUse(NetworkCommunicator networkPeer);
        bool Execute(NetworkCommunicator networkPeer, string[] args);
        string Description();
    }
}
