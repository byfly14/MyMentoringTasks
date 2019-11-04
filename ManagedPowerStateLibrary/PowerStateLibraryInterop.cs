using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ManagedPowerStateLibrary
{
    public class PowerStateLibraryInterop
    {
        [DllImport("UnmanagedPowerStateLibraray.dll", 
            EntryPoint = "GetLastWakeTime", 
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U8)]
        public static extern ulong GetLastWakeTime();

        [DllImport("UnmanagedPowerStateLibraray.dll",
            EntryPoint = "GetLastSleepTime",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.U8)]
        public static extern ulong GetLastSleepTime();

        [DllImport("UnmanagedPowerStateLibraray.dll",
            EntryPoint = "GetSystemBatteryState",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetSystemBatteryState();

        [DllImport("UnmanagedPowerStateLibraray.dll",
            EntryPoint = "GetSystemPowerInformation",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetSystemPowerInformation();

        [DllImport("UnmanagedPowerStateLibraray.dll",
            EntryPoint = "EnableHiberFile",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool EnableHiberFile();

        [DllImport("UnmanagedPowerStateLibraray.dll",
            EntryPoint = "DisableHiberFile",
            CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool DisableHiberFile();

        [DllImport("UnmanagedPowerStateLibraray.dll",
            EntryPoint = "Hibernate",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern void Hibernate([MarshalAs(UnmanagedType.I1)]bool bHibernate, [MarshalAs(UnmanagedType.I1)]bool bForce, [MarshalAs(UnmanagedType.I1)]bool bWakeupEventsDisabled);

    }

    public struct SYSTEM_BATTERY_STATE
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool AcOnLine;
        [MarshalAs(UnmanagedType.I1)]
        public bool BatteryPresent;
        [MarshalAs(UnmanagedType.I1)]
        public bool Charging;
        [MarshalAs(UnmanagedType.I1)]
        public bool Discharging;
        public byte Spare0;
        public byte Spare1;
        public byte Spare2;
        public byte Spare3;
        public uint MaxCapacity;
        public uint RemainingCapacity;
        public int Rate;
        public uint EstimatedTime;
        public uint DefaultAlert1;
        public uint DefaultAlert2;

        public override string ToString()
        {
            return $"AcOnLine : {AcOnLine}" + Environment.NewLine +
                   $"BatteryPresent : {BatteryPresent}" + Environment.NewLine +
                   $"Charging : {Charging}" + Environment.NewLine +
                   $"Discharging : {Discharging}" + Environment.NewLine +
                   $"Spare0 : {Spare0}" + Environment.NewLine +
                   $"Spare1 : {Spare1}" + Environment.NewLine +
                   $"Spare2 : {Spare2}" + Environment.NewLine +
                   $"Spare3 : {Spare3}" + Environment.NewLine +
                   $"MaxCapacity : {MaxCapacity}" + Environment.NewLine +
                   $"RemainingCapacity : {RemainingCapacity}" + Environment.NewLine +
                   $"Rate : {Rate}" + Environment.NewLine +
                   $"EstimatedTime : {EstimatedTime}" + Environment.NewLine +
                   $"DefaultAlert1 : {DefaultAlert1}" + Environment.NewLine +
                   $"DefaultAlert2 : {DefaultAlert2}" + Environment.NewLine;
        }
    }

    public struct SYSTEM_POWER_INFORMATION
    {
        public uint MaxIdlenessAllowed;
        public uint Idleness;
        public uint TimeRemaining;
        public byte CoolingMode;

        public override string ToString()
        {
            return $"MaxIdlenessAllowed : {MaxIdlenessAllowed}" + Environment.NewLine +
                   $"Idleness : {Idleness}" + Environment.NewLine +
                   $"TimeRemaining : {TimeRemaining}" + Environment.NewLine +
                   $"CoolingMode : {CoolingMode}" + Environment.NewLine;
        }
    }
}
