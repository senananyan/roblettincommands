using TaleWorlds.MountAndBlade;

namespace RoblettinCommands.Commands
{
    interface RoblettinCommand
    {
        string Command();
        bool CanUse(NetworkCommunicator networkPeer);
        bool Execute(NetworkCommunicator networkPeer, string[] args);
        string Description();
    }
}
