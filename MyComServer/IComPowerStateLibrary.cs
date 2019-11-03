using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ManagedPowerStateLibrary;

namespace MyComServer
{
    [ComVisible(true)]
    [Guid("A5D8ED67-BF8C-48E7-AB6E-10CB757CAF3B")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IComPowerStateLibrary
    {
        ulong GetLastWakeTime();
        ulong GetLastSleepTime();
        void GetSystemBatteryState(ref SYSTEM_BATTERY_STATE ptr);
        IntPtr GetSystemPowerInformation();
        bool EnableHiberFile();
        bool DisableHiberFile();
        void Hibernate(bool bHibernate, bool bForce, bool bWakeupEventsDisabled);
    }

    [ComVisible(true)]
    [Guid("195EB5F8-952C-48E2-BD0F-DA8229C5A07D")]
    [ClassInterface(ClassInterfaceType.None)]
    public class ComPowerStateLibrary : IComPowerStateLibrary
    {
        public ulong GetLastWakeTime()
        {
            return PowerStateLibraryInterop.GetLastWakeTime();
        }

        public ulong GetLastSleepTime()
        {
            return PowerStateLibraryInterop.GetLastSleepTime();
        }

        public void GetSystemBatteryState(ref SYSTEM_BATTERY_STATE ptr)
        {
            PowerStateLibraryInterop.GetSystemBatteryState(ref ptr);
        }

        public IntPtr GetSystemPowerInformation()
        {
            return PowerStateLibraryInterop.GetSystemPowerInformation();
        }

        public bool EnableHiberFile()
        {
            return PowerStateLibraryInterop.EnableHiberFile();
        }

        public bool DisableHiberFile()
        {
            return PowerStateLibraryInterop.DisableHiberFile();
        }

        public void Hibernate(bool bHibernate, bool bForce, bool bWakeupEventsDisabled)
        {
            PowerStateLibraryInterop.Hibernate(bHibernate, bForce, bWakeupEventsDisabled);
        }
    }
}
