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
using Dalamud.Game;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Runtime.InteropServices;

namespace SamplePlugin
{


    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Sample Plugin";
        private const string CommandName = "/ccwr";
        private SeString BuildChatMessage(string message)
        {
            return new SeStringBuilder()
                .AddUiForeground("[Orchestrion] ", 35)
                .AddText(message)
                .Build();
        }

        private IDalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("SamplePlugin");

        private ConfigWindow ConfigWindow { get; init; }
        private MainWindow MainWindow { get; init; }
        public Plugin(
            IDalamudPluginInterface pluginInterface,
            ICommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);

            // you might normally want to embed resources and load them from the manifest stream

            ConfigWindow = new ConfigWindow(this);
            MainWindow = new MainWindow(this);

            WindowSystem.AddWindow(ConfigWindow);
            WindowSystem.AddWindow(MainWindow);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = ""
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;


        }
        public unsafe ushort whatcount()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->CrystallineConflictRankedMatches;
        }
        public unsafe ushort whatcountwin()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->CrystallineConflictRankedMatchesWon;
        }
        public unsafe ushort casualplayed()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->CrystallineConflictCasualMatches;
        }
        public unsafe ushort casualswon()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->CrystallineConflictCasualMatchesWon;
        }
        public unsafe uint fltotalmatches()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->FrontlineTotalMatches;
        }
        public unsafe uint flfirst()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->FrontlineTotalFirstPlace;
        }
        public unsafe uint flsecond()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->FrontlineTotalSecondPlace;
        }
        public unsafe uint flthird()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->FrontlineTotalThirdPlace;
        }
        public unsafe uint rwtotal()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->RivalWingsTotalMatches;
        }
        public unsafe uint rwwins()
        {
            var matchcount = PvPProfile.Instance();
            return matchcount->RivalWingsTotalMatchesWon;
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();

            ConfigWindow.Dispose();
            MainWindow.Dispose();

            this.CommandManager.RemoveHandler(CommandName);
        }
        public unsafe bool IsPlayerMentor()
        {
            var playerStatePtr = PlayerState.Instance();
            return playerStatePtr->IsMentor();
        }
        private void OnCommand(string command, string args)
        {
            // in response to the slash command, just display our main ui
            MainWindow.IsOpen = true;

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
