using System;
using System.Runtime.InteropServices;
using ManagedPowerStateLibrary;

namespace MyComServer
{
    [ComVisible(true)]
    [Guid("DD917AE7-D175-491B-AC84-7BC612C8AEA4")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IComPowerStateLibrary
    {
        int GetTheAnswerToTheMainQuestion(int a);
        ulong GetLastWakeTime();
        ulong GetLastSleepTime();
        IntPtr GetSystemBatteryState();
        IntPtr GetSystemPowerInformation();
        bool EnableHiberFile();
        bool DisableHiberFile();
        void Hibernate(bool bHibernate, bool bForce, bool bWakeupEventsDisabled);
    }

    [ComVisible(true)]
    [Guid("12B5C4D1-B66A-407D-9A0A-69CECE4ABBD5")]
    public class ComPowerStateLibrary : IComPowerStateLibrary
    {
        public int GetTheAnswerToTheMainQuestion(int a)
        {
            return a;
        }

        public ulong GetLastWakeTime()
        {
            try
            {
                //System.Diagnostics.Debugger.Launch();
                return PowerStateLibraryInterop.GetLastWakeTime();
            }
            catch (Exception e)
            {
                return 123;
            }

        }

        public ulong GetLastSleepTime()
        {
            return PowerStateLibraryInterop.GetLastSleepTime();
        }

        public IntPtr GetSystemBatteryState()
        {
            return Marshal.ReadIntPtr(PowerStateLibraryInterop.GetSystemBatteryState());
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
