using System;
using System.Runtime.InteropServices;
using ManagedPowerStateLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyComServer;

namespace InteropPowerStateLibraryTests
{
    [TestClass]
    public class PowerStateInteropLibraryTests
    {
        [TestMethod]
        public void GetLastWakeTime()
        {
            dynamic comObject = Activator.CreateComInstanceFrom(@"C:\Users\Yan_Kmita\Source\Repos\MyMentoringTasks\MyComServer\bin\Debug\MyComServer.dll", "MyComServer.ComPowerStateLibrary").Unwrap();
            var obj = comObject.GetLastWakeTime();
            //Console.WriteLine((obj.GetType()obj).GetLastWakeTime());
        }

        [TestMethod]
        public void GetLastSleepTime()
        {
            var t = PowerStateLibraryInterop.GetLastSleepTime();
            Console.WriteLine(t);
        }

        [TestMethod]
        public void GetSystemBatteryState()
        {
            var btState = PowerStateLibraryInterop.GetSystemBatteryState();
            SYSTEM_BATTERY_STATE batteryState = (SYSTEM_BATTERY_STATE)Marshal.PtrToStructure(btState, typeof(SYSTEM_BATTERY_STATE));
            Console.WriteLine(batteryState);
        }

        [TestMethod]
        public void GetSystemPowerInformation()
        {
            var pState = PowerStateLibraryInterop.GetSystemPowerInformation();
            SYSTEM_POWER_INFORMATION powerState = (SYSTEM_POWER_INFORMATION)Marshal.PtrToStructure(pState, typeof(SYSTEM_POWER_INFORMATION));
            Console.WriteLine(powerState);
        }

        [TestMethod]
        public void ManipulateHiberFile()
        {
            var pState1 = PowerStateLibraryInterop.DisableHiberFile();
            Console.WriteLine(pState1);

            var pState2 = PowerStateLibraryInterop.EnableHiberFile();
            Console.WriteLine(pState2);
        }

        [TestMethod]
        [Ignore]
        public void Hibernate()
        {
            PowerStateLibraryInterop.Hibernate(true, false, false);
        }

        static DateTime GetDtcTime(ulong nanoseconds)
        {
            DateTime startTime = new DateTime(2000, 1, 1, 0, 0, 0, 0);
            startTime.AddTicks((long) nanoseconds);
            return startTime.AddTicks((long)nanoseconds);
        }
    }
}
