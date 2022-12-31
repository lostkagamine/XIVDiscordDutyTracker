using DiscordDutyTracker.Duties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDutyTracker.Util
{
    internal class MessageFormatter
    {
        public static string FormatMessage(string a, Duty d)
        {
            // Lord forgive me this is bad
            return a.Replace("{name}", CharaNameUtil.Instance.CurrentName)
                .Replace("{duty}", d.Name);
        }
    }
}
