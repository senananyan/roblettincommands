using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace RoblettinCommands
{
    public class RoblettinCommandsSubModule : MBSubModuleBase
    {
        public static RoblettinCommandsSubModule Instance { get; private set; }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Debug.Print("** CHAT COMMANDS BY ROBLETTIN LOADED **", 0, Debug.DebugColor.Green);

            CommandManager cm = new CommandManager();
        }

        protected override void OnSubModuleUnloaded() {
            Debug.Print("** CHAT COMMANDS BY ROBLETTIN UNLOADED **", 0, Debug.DebugColor.Green);
        }

        public override void OnMultiplayerGameStart(Game game, object starterObject) {
            Debug.Print("** CHAT HANDLER ADDED **", 0, Debug.DebugColor.Green);
            game.AddGameHandler<ChatHandler>();
        }

        public override void OnGameEnd(Game game) {
            game.RemoveGameHandler<ChatHandler>();
        }

    }
}
