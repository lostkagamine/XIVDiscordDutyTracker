using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Game.ClientState;
using Dalamud.Logging;
using Lumina.Excel.GeneratedSheets;
using DiscordDutyTracker.Duties;

// Parts of this are based on the implementation of DutyStatus from KamiLib.

namespace DiscordDutyTracker.Util
{
    internal unsafe class DutyStatus : IDisposable
    {
        private delegate byte DutyEventDelegate(void* a1, void* a2, ushort* a3);
        [Signature("48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC ?? 48 8B D9 49 8B F8 41 0F B7 08", DetourName = nameof(DutyEventFunction))]
        private readonly Hook<DutyEventDelegate>? DutyEventHook = null;

        public bool IsInDuty { get; private set; }
        public bool IsDutyCompleted { get; private set; }

        public bool IsInInstance { get; private set; }

        public Duty CurrentDuty { get; private set; }

        public delegate void DutyDelegate(Duty d);
        public event DutyDelegate? OnEnterDuty;
        public event DutyDelegate? OnLeaveDuty;
        public event DutyDelegate? OnDutyComplete;
        public event DutyDelegate? OnDutyReset;
        public event DutyDelegate? OnDutyWipe;

        DutyStatus()
        {
            SignatureHelper.Initialise(this);

            ServiceHolder.ClientState.TerritoryChanged += OnTerritoryChange;
        }

        public void Dispose()
        {
            DutyEventHook?.Dispose();

            ServiceHolder.ClientState.TerritoryChanged -= OnTerritoryChange;
        }

        private void OnTerritoryChange(object? sender, ushort t)
        {
            var duty = DutyManager.Instance.GetCurrentDuty();
            if (duty.Classification == DutyClassification.NotInDuty)
            {
                if (!IsInInstance)
                {
                    // This isn't a duty, give up.
                    return;
                }
                // The duty has ended.
                IsInInstance = false;
                if (!IsDutyCompleted)
                {
                    OnLeaveDuty?.Invoke(CurrentDuty);
                }
                IsDutyCompleted = false;
            }
            else
            {
                // Duty has begun.
                CurrentDuty = duty;
                IsInInstance = true;
                OnEnterDuty?.Invoke(duty);
            }
        }

        private static DutyStatus? _Instance = null!;
        public static DutyStatus Instance => _Instance ??= new();

        private byte DutyEventFunction(void* a1, void* a2, ushort* a3)
        {
            try
            {
                var category = *(a3);
                var type = *(uint*)(a3 + 4);
                if (category == 0x6D)
                {
                    switch (type)
                    {
                        // Duty start
                        case 0x40000001:
                            IsInDuty = true;
                            break;
                        // Wipe
                        case 0x40000005:
                            IsInDuty = false;
                            OnDutyWipe?.Invoke(CurrentDuty);
                            break;
                        // Restart
                        case 0x40000006:
                            IsInDuty = true;
                            OnDutyReset?.Invoke(CurrentDuty);
                            break;
                        // Completed
                        case 0x40000003:
                            IsInDuty = false;
                            IsDutyCompleted = true;
                            OnDutyComplete?.Invoke(CurrentDuty);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed DutyStatus");
            }
            return DutyEventHook!.Original(a1, a2, a3);
        }
    }
}
