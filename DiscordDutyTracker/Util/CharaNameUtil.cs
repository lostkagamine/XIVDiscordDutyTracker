using Dalamud.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDutyTracker.Util
{
    internal class CharaNameUtil : IDisposable
    {
        private static CharaNameUtil _Instance;
        public static CharaNameUtil Instance => _Instance ??= new();

        public string? CurrentName { get; private set; }

        public void InstallListeners()
        {
            ServiceHolder.ClientState.Login += _OnLogin;
            ServiceHolder.ClientState.Logout += _OnLogout;
        }

        public void InstallTemporaryListener()
        {
            ServiceHolder.Framework.Update += _TemporaryGetNameTick;
        }

        private void _OnLogin(object? _e, EventArgs _a)
        {
            if (ServiceHolder.ClientState.LocalPlayer != null)
            {
                CurrentName = ServiceHolder.ClientState.LocalPlayer.Name.TextValue;
            } else
            {
                ServiceHolder.Framework.Update += _TemporaryGetNameTick;
            }
        }

        private void _OnLogout(object? _e, EventArgs _a)
        {
            CurrentName = null;
        }

        private void _TemporaryGetNameTick(Framework fw)
        {
            if (ServiceHolder.ClientState.LocalPlayer != null)
            {
                CurrentName = ServiceHolder.ClientState.LocalPlayer.Name.TextValue;
                fw.Update -= _TemporaryGetNameTick;
            }
        }

        public void Dispose()
        {
            ServiceHolder.ClientState.Login -= _OnLogin;
            ServiceHolder.ClientState.Logout -= _OnLogout;
        }
    }
}
