using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace DiscordDutyTracker.Windows;

public class ConfigWindow : Window, IDisposable
{
    private Configuration Configuration;

    public ConfigWindow(Plugin plugin) : base(
        "Discord Duty Tracker: Configuration",
        ImGuiWindowFlags.AlwaysAutoResize| ImGuiWindowFlags.NoResize |
        ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.Configuration = Plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        // I'm sorry lmao
        {
            var config = Configuration.WebhookURL;
            if (ImGui.InputText("Webhook URL", ref config, 2048u))
            {
                Configuration.WebhookURL = config;
            }
        }
        {
            var config = Configuration.DutyEnteredTemplate;
            if (ImGui.InputText("Template: Duty Entered", ref config, 2048u))
            {
                Configuration.DutyEnteredTemplate = config;
            }
        }
        {
            var config = Configuration.DutyCompletedTemplate;
            if (ImGui.InputText("Template: Duty Complete", ref config, 2048u))
            {
                Configuration.DutyCompletedTemplate = config;
            }
        }
        {
            var config = Configuration.DutyCompletedFirstTimeTemplate;
            if (ImGui.InputText("Template: Duty Complete (First Time)", ref config, 2048u))
            {
                Configuration.DutyCompletedFirstTimeTemplate = config;
            }
        }
        {
            var config = Configuration.DutyEndedTemplate;
            if (ImGui.InputText("Template: Duty Ended (Incomplete)", ref config, 2048u))
            {
                Configuration.DutyEndedTemplate = config;
            }
        }

        if (ImGui.Button("Save and close"))
        {
            Configuration.Save();
            IsOpen = false;
        }
    }
}
