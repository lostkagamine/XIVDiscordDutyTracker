using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace DiscordDutyTracker
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public string WebhookURL { get; set; } = "";
        public string DutyEnteredTemplate { get; set; } = "";
        public string DutyEndedTemplate { get; set; } = "";
        public string DutyCompletedTemplate { get; set; } = "";
        public string DutyCompletedFirstTimeTemplate { get; set; } = "";

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
