using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using SamplePlugin.Windows;

namespace SamplePlugin
{
    namespace FFXIVClientStructs.FFXIV.Client.Game.UI;

    [StructLayout(LayoutKind.Explicit, Size = 0x7C)]
    public unsafe partial struct PvPProfile
    {
        [StaticAddress("48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 8B 43 08", 3)]
        public static partial PvPProfile* Instance();

        [FieldOffset(0x0)] public byte IsLoaded;

        [FieldOffset(0x4)] public uint ExperienceMaelstrom;
        [FieldOffset(0x8)] public uint ExperienceTwinAdder;
        [FieldOffset(0xC)] public uint ExperienceImmortalFlames;
        [FieldOffset(0x10)] public byte RankMaelstrom;
        [FieldOffset(0x11)] public byte RankTwinAdder;
        [FieldOffset(0x12)] public byte RankImmortalFlames;

        [FieldOffset(0x1E)] public byte Series;
        [FieldOffset(0x1F)] public byte SeriesCurrentRank;
        [FieldOffset(0x20)] public byte SeriesClaimedRank;

        [FieldOffset(0x22)] public ushort SeriesExperience;
        [FieldOffset(0x24)] public byte PreviousSeriesClaimedRank;
        [FieldOffset(0x25)] public byte PreviousSeriesRank;

        [FieldOffset(0x28)] public uint FrontlineTotalMatches;
        [FieldOffset(0x2C)] public uint FrontlineTotalFirstPlace;
        [FieldOffset(0x30)] public uint FrontlineTotalSecondPlace;
        [FieldOffset(0x34)] public uint FrontlineTotalThirdPlace;
        [FieldOffset(0x38)] public ushort FrontlineWeeklyMatches;
        [FieldOffset(0x3A)] public ushort FrontlineWeeklyFirstPlace;
        [FieldOffset(0x3C)] public ushort FrontlineWeeklySecondPlace;
        [FieldOffset(0x3E)] public ushort FrontlineWeeklyThirdPlace;

        [FieldOffset(0x41)] public byte CrystallineConflictSeason;
        [FieldOffset(0x42)] public ushort CrystallineConflictCasualMatches;
        [FieldOffset(0x44)] public ushort CrystallineConflictCasualMatchesWon;
        [FieldOffset(0x46)] public ushort CrystallineConflictRankedMatches;
        [FieldOffset(0x48)] public ushort CrystallineConflictRankedMatchesWon;

        [FieldOffset(0x4E)] public ushort CrystallineConflictCurrentCrystalCredit;
        [FieldOffset(0x50)] public ushort CrystallineConflictHighestCrystalCredit;

        [FieldOffset(0x54)] public byte CrystallineConflictCurrentRank;
        [FieldOffset(0x55)] public byte CrystallineConflictHighestRank;
        [FieldOffset(0x56)] public byte CrystallineConflictCurrentRiser;
        [FieldOffset(0x57)] public byte CrystallineConflictHighestRiser;
        [FieldOffset(0x58)] public byte CrystallineConflictCurrentRisingStars;
        [FieldOffset(0x59)] public byte CrystallineConflictHighestRisingStars;

        [FieldOffset(0x6C)] public uint RivalWingsTotalMatches;
        [FieldOffset(0x70)] public uint RivalWingsTotalMatchesWon;
        [FieldOffset(0x74)] public uint RivalWingsWeeklyMatches;
        [FieldOffset(0x78)] public uint RivalWingsWeeklyMatchesWon;

        public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Sample Plugin";
        private const string CommandName = "/pmycommand";

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

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            
            ConfigWindow.Dispose();
            MainWindow.Dispose();
            
            this.CommandManager.RemoveHandler(CommandName);
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
