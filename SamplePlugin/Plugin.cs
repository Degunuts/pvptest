using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Game.Text.SeStringHandling;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SamplePlugin.Windows;
using Dalamud.Game.Text;
﻿using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace SamplePlugin
{


    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Sample Plugin";
        private const string CommandName = "/pmycommand";
        private SeString BuildChatMessage(string message)
	    {
		return new SeStringBuilder()
			.AddUiForeground("[Orchestrion] ", 35)
			.AddText(message)
			.Build();
       }
       public class DalamudApi
       
        {
            public static void Initialize(DalamudPluginInterface pluginInterface) => pluginInterface.Create<DalamudApi>();
        
            // [PluginService] public static IAetheryteList AetheryteList { get; private set; } = null;
            // [PluginService] public static IBuddyList BuddyList { get; private set; } = null;    
            [PluginService] public static IChatGui ChatGui { get; private set; } = null;
            [PluginService] public static IClientState ClientState { get; private set; } = null;
            [PluginService] public static ICommandManager CommandManager { get; private set; } = null;
            // [PluginService] public static ICondition Condition { get; private set; } = null;
            [PluginService] public static DalamudPluginInterface PluginInterface { get; private set; } = null;
            [PluginService] public static IDataManager DataManager { get; private set; } = null;
            [PluginService] public static IDtrBar DtrBar { get; private set; } = null;
            // [PluginService] public static IFateTable FateTable { get; private set; } = null;
            // [PluginService] public static IFlyTextGui FlyTextGui { get; private set; } = null;
            [PluginService] public static IFramework Framework { get; private set; } = null;
            [PluginService] public static IGameGui GameGui { get; private set; } = null;
            // [PluginService] public static IGameNetwork GameNetwork { get; private set; } = null;
            // [PluginService] public static IGamepadState GamePadState { get; private set; } = null;
            // [PluginService] public static IJobGauges JobGauges { get; private set; } = null;
            // [PluginService] public static IKeyState KeyState { get; private set; } = null;
            // [PluginService] public static ILibcFunction LibcFunction { get; private set; } = null;
            // [PluginService] public static IObjectTable ObjectTable { get; private set; } = null;
            // [PluginService] public static IPartyFinderGui PartyFinderGui { get; private set; } = null;
            // [PluginService] public static IPartyList PartyList { get; private set; } = null;
            [PluginService] public static ISigScanner SigScanner { get; private set; } = null;
            // [PluginService] public static ITargetManager TargetManager { get; private set; } = null;
            // [PluginService] public static IToastGui ToastGui { get; private set; } = null;
            [PluginService] public static IGameInteropProvider Hooks { get; private set; } = null;
            [PluginService] public static IPluginLog PluginLog { get; private set; } = null;
            // [PluginService] public static ITitleScreenMenu TitleScreenMenu { get; private set; } = null;
        }
        private DalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SamplePlugin");

        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }
        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream
            var imagePath = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "goat.png");
            var goatImage = this.PluginInterface.UiBuilder.LoadImage(imagePath);

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this, goatImage);
            
            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            
        }
        public struct PvPProfile;
        public ushort CrystallineConflictRankedMatches;
        private string Ccstring(ushort matches)
        {
            return matches.ToString();
        }
        
        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            
            this.CommandManager.RemoveHandler(CommandName);
        }
        public unsafe bool IsPlayerMentor() {
            var playerStatePtr = PlayerState.Instance();
            return playerStatePtr->IsMentor();
        }
        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            MainWindow.IsOpen = true;
            DalamudApi.ChatGui.Print(BuildChatMessage(Ccstring(CrystallineConflictRankedMatches)));
            DalamudApi.ChatGui.Print(BuildChatMessage("TEST"));


        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            ConfigWindow.IsOpen = true;
        }
    }
}
