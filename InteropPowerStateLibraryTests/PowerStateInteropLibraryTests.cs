﻿using System;
using System.Runtime.InteropServices;
using ManagedPowerStateLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InteropPowerStateLibraryTests
{
    [TestClass]
    public class PowerStateInteropLibraryTests
    {
        [TestMethod]
        public void GetLastWakeTime()
        {
            var t = PowerStateLibraryInterop.GetLastWakeTime();
            Console.WriteLine(t);
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
            var btState = new SYSTEM_BATTERY_STATE();
            PowerStateLibraryInterop.GetSystemBatteryState(ref btState);
            Console.WriteLine(btState);
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
