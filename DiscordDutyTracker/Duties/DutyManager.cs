using DiscordDutyTracker.Util;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDutyTracker.Duties
{
    internal class DutyManager
    {
        public Duty GetCurrentDuty()
        {
            var currentType = ServiceHolder.ClientState.TerritoryType;
            var collection =
                ServiceHolder.DataManager.GetExcelSheet<ContentFinderCondition>()!
                             .Where(a => a.TerritoryType.Row == currentType);
            var cfc = collection.DefaultIfEmpty(null).FirstOrDefault();
            if (cfc == null)
            {
                return new Duty()
                {
                    Classification = DutyClassification.NotInDuty,
                    Name = ""
                };
            }
            var duty = new Duty()
            {
                Name = cfc.Name,
                Classification = cfc.HighEndDuty ? DutyClassification.HighEnd : DutyClassification.Normal,
            };
            return duty;
        }

        private static DutyManager? _Instance = null!;
        public static DutyManager Instance => _Instance ??= new();
    }
}
