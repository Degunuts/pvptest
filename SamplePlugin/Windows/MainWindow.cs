using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System.Runtime.InteropServices;
using System.IO;
using Dalamud.Plugin;

namespace SamplePlugin.Windows
{
    public class MainWindow : Window, IDisposable
    {
        private Plugin Plugin;
        private IDalamudPluginInterface PluginInterface { get; init; }
        private int currentTab = 0; // Index of the currently selected tab

        public MainWindow(Plugin plugin) : base(
            "I LOEV CC", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(375, 330),
                MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
            };


            this.Plugin = plugin;

        }

        public void Dispose()
        {

        }


        public unsafe override void Draw()
        {
            ImGui.BeginTabBar("MyTabBar");

            if (ImGui.BeginTabItem("Casual CC"))
            {
                DrawCCTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Ranked CC"))
            {
                DrawRankedTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Rival Wings"))
            {
                DrawRivalWingsTab();
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Frontlines"))
            {
                DrawFrontlineTab();
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
        }

        private void DrawCCTab()
        {
            ImGui.Spacing();
            ImGui.SetWindowFontScale(1.5f);
            ImGui.Text("You have played " + Plugin.casualplayed().ToString() + "  games of casual CC, you monster");
            ImGui.Text("You have won " + Plugin.casualswon().ToString() + " games of casual CC, you ape");
            ImGui.Text("your win rate is " + String.Format("{0:P2}.", ((float)Plugin.casualswon() / Plugin.casualplayed())) + "%, GGs");

        }

        private void DrawRankedTab()
        {
            ImGui.Spacing();
            ImGui.SetWindowFontScale(1.5f);
            ImGui.Text("You have played " + Plugin.whatcount().ToString() + "  games of ranked CC, you monster");
            ImGui.Text("You have won " + Plugin.whatcountwin().ToString() + " games of ranked CC, you ape");
            ImGui.Text("your win rate is " + String.Format("{0:P2}.", ((float)Plugin.whatcountwin() / Plugin.whatcount())) + "%, GGs");

        }

        private void DrawRivalWingsTab()
        {
            ImGui.Spacing();
            ImGui.SetWindowFontScale(1.5f);
            ImGui.Text("You have played " + Plugin.rwtotal().ToString() + "  games of rival wings, good choice");
            ImGui.Text("You have won " + Plugin.rwwins().ToString() + " games of rival wings, well done");
            ImGui.Text("your win rate is " + String.Format("{0:P2}.", ((float)Plugin.rwwins() / Plugin.rwtotal())) + "%, GGs");
        }

        private void DrawFrontlineTab()
        {
            ImGui.Spacing();
            ImGui.SetWindowFontScale(1.5f);
            ImGui.Text("You have played " + Plugin.fltotalmatches().ToString() + "  games of frontlines, you monster");
            ImGui.Text("You have placed first in " + Plugin.flfirst().ToString() + " games of frontlines, you ape");
            ImGui.Text("You have placed second in " + Plugin.flsecond().ToString() + " games of frontlines, you ape");
            ImGui.Text("You have placed third in " + Plugin.flthird().ToString() + " games of frontlines, you ape");
        }
    }
}
