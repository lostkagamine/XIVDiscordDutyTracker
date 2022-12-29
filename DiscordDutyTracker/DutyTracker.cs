using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Game.Gui;
using Dalamud.Logging;
using DiscordDutyTracker.Duties;
using DiscordDutyTracker.Util;

namespace DiscordDutyTracker
{
    internal class DutyTracker : IDisposable
    {
        public void Start()
        {
            DutyStatus.Instance.OnEnterDuty += OnEnterDuty;
            DutyStatus.Instance.OnLeaveDuty += OnLeaveDuty;
        }

        private void OnEnterDuty(Duty d)
        {
            ServiceHolder.ChatGui.Print($"[DDT Debug] {d.Name} has begun.");
        }

        private void OnLeaveDuty(Duty d)
        {
            ServiceHolder.ChatGui.Print($"[DDT Debug] {d.Name} has ended.");
        }

        public void Dispose()
        {
            DutyStatus.Instance.OnEnterDuty -= OnEnterDuty;
            DutyStatus.Instance.OnLeaveDuty -= OnLeaveDuty;
            DutyStatus.Instance.Dispose();
        }
    }
}
