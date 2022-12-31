using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace DiscordDutyTracker.Util
{
    internal class DiscordApi
    {
        private static DiscordApi? _Instance;
        public static DiscordApi Instance = _Instance ??= new();

        private async Task PostMessageAsync(string msg)
        {
            await Plugin.Configuration.WebhookURL.PostJsonAsync(new {
                content = msg,
            });
        }

        public void PostMessage(string msg)
        {
            // Closure abuse time :)
            Task.Run(() => PostMessageAsync(msg));
        }
    }
}
