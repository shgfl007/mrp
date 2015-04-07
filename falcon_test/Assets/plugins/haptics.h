// Copyright 2007 Novint Technologies, Inc. All rights reserved.
// Available only under license from Novint Technologies, Inc.

// Make sure this header is included only once
#ifndef HAPTICS_H
#define HAPTICS_H

#include <hdl/hdl.h>
#include <hdlu/hdlu.h>

// Know which face is in contact
enum RS_Face {
    FACE_NONE = -1,
    FACE_NEAR, FACE_RIGHT, 
    FACE_FAR, FACE_LEFT, 
    FACE_TOP, FACE_BOTTOM,
    FACE_DEFAULT,
    FACE_LAST               // reserved to allow iteration over faces
    };

// Blocking values
const bool bNonBlocking = false;
const bool bBlocking = true;

class HapticsClass 
{

// Define callback functions as friends
friend HDLServoOpExitCode ContactCB(void *data);
friend HDLServoOpExitCode GetStateCB(void *data);

public:
    // Constructor
    HapticsClass();

    // Destructor
    ~HapticsClass();

    // Initialize
    void init();

    // Clean up
    void uninit();

	// Update
	void update();

    // Get position
    void getPosition(double pos[3]);

	// Set speed
	void setServo(double speed[3]);

	// Move to position
	void setServoPos(double pos[3], double strength);

    // Get state of device button
    bool isButtonDown();

	// Get the integer for all buttons pressed
	int getButtonsDown();

	// Checks if each individual button is pressed
	bool isButton0Down();
	bool isButton1Down();
	bool isButton2Down();
	bool isButton3Down();

    // synchFromServo() is called from the application thread when it wants to exchange
    // data with the HapticClass object.  HDAL manages the thread synchronization
    // on behalf of the application.
    void synchFromServo();

    // Get ready state of device.
    bool isDeviceCalibrated();

	// Check if device is ready.
	bool isDeviceReady();

private:
    // Move data between servo and app variables
    void synch();

    // Calculate contact force with cube
    void cubeContact();

    // Matrix multiply
    void vecMultMatrix(double srcVec[3], double mat[16], double dstVec[3]);

    // Check error result; display message and abort, if any
    void testHDLError(const char* str);

    // Nothing happens until initialization is done
    bool m_inited;

    // Transformation from Device coordinates to Application coordinates
    double m_transformMat[16];
    
    // Variables used only by servo thread
    double m_positionServo[3];
    bool   m_buttonServo;
	int    m_buttonActive;
    double m_forceServo[3];
	double m_newPositionServo[3];
	double m_strength;
	bool   m_forcedPosition;

    // Variables used only by application thread
    double m_positionApp[3];
    bool   m_buttonApp;

    // Keep track of last face to have contact
    int    m_lastFace;

    // Handle to device
    HDLDeviceHandle m_deviceHandle;

    // Handle to Contact Callback 
    HDLServoOpExitCode m_servoOp;

    // Device workspace dimensions
    double m_workspaceDims[6];

    // Size of cube
    double m_cubeEdgeLength;

    // Stiffness of cube
    double m_cubeStiffness;
};

#endif // HAPTICS_H