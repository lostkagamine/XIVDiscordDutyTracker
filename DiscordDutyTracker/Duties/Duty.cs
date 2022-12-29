using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDutyTracker.Duties
{
    internal enum DutyClassification
    {
        Normal,
        HighEnd,
        NotInDuty
    }

    internal struct Duty
    {
        public string Name;
        public DutyClassification Classification;
    }
}
