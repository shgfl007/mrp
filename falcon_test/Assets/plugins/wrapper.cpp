// Wrapper class for connecting haptics class

#include "stdafx.h"
#include "wrapper.h"

// engine specific header file
#define DLL_USE	// always define before including adll.h
#include "haptics.h"

const double SCALE_FACTOR = 1; 

HapticsClass gHaptics;

// Standard boilerplate for DLLs
BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    if (ul_reason_for_call == DLL_PROCESS_DETACH)
    {
        gHaptics.uninit();
    }

    return TRUE;
}

// bt_ prefix used for Basic Test
// GameStudio looks through all DLLs for entry point names.
// Since acknex_plugins directory may have many DLLs, it is
// good practice to keep their names unique to the application.

DLLFUNC void StartHaptics()
{
	gHaptics.init();
}

DLLFUNC void StopHaptics()
{
    gHaptics.uninit();
}

// Called each graphics cycle to trigger async between threads
DLLFUNC void SyncHaptics()
{
    gHaptics.synchFromServo();
}


// Get Position
DLLFUNC double GetXPos()
{
    double pos[3];

    gHaptics.getPosition(pos);

    return SCALE_FACTOR * pos[0];
}


DLLFUNC double GetYPos()
{
    double pos[3];

    gHaptics.getPosition(pos);

    return SCALE_FACTOR * pos[1];
}

DLLFUNC double GetZPos()
{
    double pos[3];

    gHaptics.getPosition(pos);

    return SCALE_FACTOR * pos[2];
}

DLLFUNC void SetServo(double speed[3]) 
{
    gHaptics.setServo(speed);
}

DLLFUNC void SetServoPos(double pos[3], double strength) 
{
    gHaptics.setServoPos(pos, strength);
}



// Haptic button state
DLLFUNC bool IsHapticButtonDepressed()
{
    return gHaptics.isButtonDown();
}

DLLFUNC int GetButtonsDown()
{
	return gHaptics.getButtonsDown();
}

// Returns state of each individual button

DLLFUNC bool isButton0Down()
{
	return gHaptics.isButton0Down();
}

DLLFUNC bool isButton1Down()
{
	return gHaptics.isButton1Down();
}

DLLFUNC bool isButton2Down()
{
	return gHaptics.isButton2Down();
}

DLLFUNC bool isButton3Down()
{
	return gHaptics.isButton3Down();
}

// Calibration

DLLFUNC bool IsDeviceCalibrated()
{
    return gHaptics.isDeviceCalibrated();
}

DLLFUNC bool IsDeviceReady()
{
    return gHaptics.isDeviceReady();
}
