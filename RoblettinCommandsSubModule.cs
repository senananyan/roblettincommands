using System.IO;
using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.PlayerServices;

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
        // Delegate to handle log writing
        private void AddConsoleCommands() => DedicatedServerConsoleCommandManager.AddType(typeof(ConsoleCommands));
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
