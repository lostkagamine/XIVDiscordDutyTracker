using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Game.ClientState;
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
            DutyStatus.Instance.OnDutyComplete += OnCompleteDuty;
        }

        private void OnEnterDuty(Duty d)
        {
            // TODO fix this atrocity
            DiscordApi.Instance.PostMessage(
                MessageFormatter.FormatMessage(Plugin.Configuration.DutyEnteredTemplate, d));
        }

        private void OnLeaveDuty(Duty d)
        {
            DiscordApi.Instance.PostMessage(
                MessageFormatter.FormatMessage(Plugin.Configuration.DutyEndedTemplate, d));
        }

        private void OnCompleteDuty(Duty d)
        {
            DiscordApi.Instance.PostMessage(
                MessageFormatter.FormatMessage(Plugin.Configuration.DutyCompletedTemplate, d));
        }

        public void Dispose()
        {
            DutyStatus.Instance.OnEnterDuty -= OnEnterDuty;
            DutyStatus.Instance.OnLeaveDuty -= OnLeaveDuty;
            DutyStatus.Instance.OnDutyComplete -= OnCompleteDuty;

            DutyStatus.Instance.Dispose();
        }
    }
}
