using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using JetBrains.Annotations;
using RoblettinCommands.Commands;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.PlayerServices;

namespace RoblettinCommands
{
    public class ConsoleCommands
    {
        [UsedImplicitly]
        [ConsoleCommandMethod("BanTag",
        "[True/False] Set whether to show messages in chat when someone joins or leaves the server.")]
        private static void SetIsActive(string show)
        {
            if (!bool.TryParse(show, out bool isBanTagActive))
            {
                Debug.PrintError($"dat_set_show_joinleave_messages: Could not parse boolean (True/False) from '{show}'");
                return;
            }

            RoblettinConfigs.isBanTagActive = isBanTagActive;

            Debug.Print($"Set isBanTagActive to {isBanTagActive}");
        }
        private static readonly Action<string> HandleConsoleCommand =
        (Action<string>)Delegate.CreateDelegate(typeof(Action<string>),
            typeof(DedicatedServerConsoleCommandManager).GetStaticMethodInfo("HandleConsoleCommand"));
    }

}