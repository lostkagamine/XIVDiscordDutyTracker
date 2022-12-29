using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace DiscordDutyTracker
{
    internal class ServiceHolder
    {
        [PluginService]
        public static DalamudPluginInterface PluginInterface { get; private set; } = null!;
        [PluginService]
        public static DataManager DataManager { get; private set; } = null!;
        [PluginService]
        public static ClientState ClientState { get; private set; } = null!;
        [PluginService]
        public static Framework Framework { get; private set; } = null!;
        [PluginService]
        public static ChatGui ChatGui { get; private set; } = null!;
    }
}
