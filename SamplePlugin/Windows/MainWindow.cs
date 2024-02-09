using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System.Runtime.InteropServices;

namespace SamplePlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private IDalamudTextureWrap GoatImage;
    private Plugin Plugin;

    public MainWindow(Plugin plugin, IDalamudTextureWrap goatImage) : base(
        "I LOEV CC", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.GoatImage = goatImage;
        this.Plugin = plugin;
    }

    public void Dispose()
    {
        this.GoatImage.Dispose();
    }

    public unsafe override void Draw()
    {     
        ImGui.Spacing();
        ImGui.SetWindowFontScale(2);
        ImGui.Text("You have played " + Plugin.whatcount().ToString() + "  games of ranked CC, you monster");
        ImGui.Text("You have won " + Plugin.whatcountwin().ToString() + " games of ranked CC, you ape");
        ImGui.Text("your win rate is " + String.Format("{0:P2}.", ((float)Plugin.whatcountwin() / Plugin.whatcount())) + "%, GGs");
        ImGui.Indent(55);
        ImGui.Image(this.GoatImage.ImGuiHandle, new Vector2(this.GoatImage.Width, this.GoatImage.Height));
        ImGui.Unindent(55);
    }
}
