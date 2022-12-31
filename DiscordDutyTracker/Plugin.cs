using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;
using Dalamud.Interface.Windowing;
using DiscordDutyTracker.Util;
using DiscordDutyTracker.Windows;

namespace DiscordDutyTracker
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Discord Duty Tracker";
        private const string CommandName = "/ddt";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public static Configuration Configuration { get; private set; }
        public WindowSystem WindowSystem = new("DiscordDutyTracker");
        
        private DutyTracker _tracker { get; set; }

        private ServiceHolder _holder { get; set; }

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            _holder = pluginInterface.Create<ServiceHolder>();

            Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(this.PluginInterface);

            WindowSystem.AddWindow(new ConfigWindow(this));

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "Opens the Discord Duty Tracker configuration."
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

            CharaNameUtil.Instance.InstallListeners();
            CharaNameUtil.Instance.InstallTemporaryListener();

            _tracker = new DutyTracker();
            _tracker.Start();
        }

        public void Dispose()
        {
            _tracker.Dispose();
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            if (args == "config")
            {
                DrawConfigUI();
            }
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {
            WindowSystem.GetWindow("Discord Duty Tracker: Configuration").IsOpen = true;
        }
    }
}
