
#include "pch.h"
#include "Powerbase.h"

extern "C" {
#include <Powrprof.h>

}
#pragma comment(lib, "Powrprof.lib")

typedef struct _SYSTEM_POWER_INFORMATION {
	ULONG MaxIdlenessAllowed;
	ULONG Idleness;
	ULONG TimeRemaining;
	UCHAR CoolingMode;
} SYSTEM_POWER_INFORMATION, * PSYSTEM_POWER_INFORMATION;

extern "C" __declspec(dllexport) ULONGLONG GetLastWakeTime()
{
	ULONGLONG last_wake_time;
	const DWORD dw_size = sizeof last_wake_time;

	CallNtPowerInformation(LastWakeTime, nullptr, 0, &last_wake_time, dw_size);
	return last_wake_time;
}

extern "C" __declspec(dllexport) ULONGLONG GetLastSleepTime()
{
	ULONGLONG last_sleep_time;
	const DWORD dw_size = sizeof last_sleep_time;

	CallNtPowerInformation(LastSleepTime, nullptr, 0, &last_sleep_time, dw_size);
	return last_sleep_time;
}

extern "C" __declspec(dllexport) SYSTEM_BATTERY_STATE GetSystemBatteryState()
{
	SYSTEM_BATTERY_STATE system_battery_state;
	const DWORD dw_size = sizeof system_battery_state;

	CallNtPowerInformation(SystemBatteryState, nullptr, 0, &system_battery_state, dw_size);
	return system_battery_state;
}

extern "C" __declspec(dllexport) SYSTEM_POWER_INFORMATION GetSystemPowerInformation()
{
	SYSTEM_POWER_INFORMATION system_power_information;
	const DWORD dw_size = sizeof system_power_information;

	CallNtPowerInformation(SystemBatteryState, nullptr, 0, &system_power_information, dw_size);
	return system_power_information;
}

extern "C" __declspec(dllexport) BOOLEAN DisableHiberFile()
{
	BOOLEAN enable = FALSE;
	const DWORD dw_size = sizeof enable;

	CallNtPowerInformation(SystemBatteryState, &enable, dw_size, nullptr, 0);
	return enable;
}

extern "C" __declspec(dllexport) BOOLEAN EnableHiberFile()
{
	BOOLEAN enable = TRUE;
	const DWORD dw_size = sizeof enable;

	CallNtPowerInformation(SystemBatteryState, &enable, dw_size, nullptr, 0);
	return enable;
}

extern "C" __declspec(dllexport) void Hibernate(BOOLEAN bHibernate, BOOLEAN bFroce, BOOLEAN bWakeupEventsDisabled)
{
	SetSuspendState(bHibernate, bFroce, bWakeupEventsDisabled);
}