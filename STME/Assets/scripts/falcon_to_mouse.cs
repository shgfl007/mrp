using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

public class falcon_to_mouse : MonoBehaviour {

	//falcon stuff
	#region Falcon Wrapper.dll Variables
	
	const string falcon = "Falcon Wrapper_test1.dll";
	
	[DllImport(falcon)]
	private static extern void StartHaptics();
	[DllImport(falcon)]
	private static extern void StopHaptics();
	[DllImport(falcon)]
	private static extern bool IsDeviceCalibrated();
	[DllImport(falcon)]
	private static extern bool IsDeviceReady();
	[DllImport(falcon)]
	private static extern double GetXPos();
	[DllImport(falcon)]
	private static extern double GetYPos();
	[DllImport(falcon)]
	private static extern double GetZPos();
	[DllImport(falcon)]
	private static extern void SetServo(double[] speed);
	[DllImport(falcon)]
	private static extern void SetServoPos(double[] pos, double strength);
	[DllImport(falcon)]
	private static extern bool IsHapticButtonDepressed();
	[DllImport(falcon)]
	private static extern int GetButtonsDown();
	[DllImport(falcon)]
	private static extern bool isButton0Down();
	[DllImport(falcon)]
	private static extern bool isButton1Down();
	[DllImport(falcon)]
	private static extern bool isButton2Down();
	[DllImport(falcon)]
	private static extern bool isButton3Down();
	
	public static falcon_to_mouse main;
	//public static Vector3 FalconToMouse (Vector3 position);
	#endregion
	
	#region Variables
	[Range(-10f, 100f)]
	public float SpeedX = 0f;
	[Range(-10f, 100f)]
	public float SpeedY = 0f;
	[Range(-10f, 100f)]
	public float SpeedZ = -10f;
	
	[Range(-1.5f, 1.5f)]
	public float PosX = 0;
	[Range(-1.5f, 1.5f)]
	public float PosY = 0;
	[Range(-1.5f, 1.5f)]
	public float PosZ = 0;
	
	[Range(0f, 100f)]
	public float Strength = 3f;
	
	public float TranslationSensitivity     = 10.0f;
	//-------------------------------------------------
	float x;
	float y;
	#endregion
	
	//-------------------------------------------------
	#region Start()
	
	void Start() {
		StartHaptics();
		StartCoroutine(_initHaptics());
	}

	private IEnumerator _initHaptics() {
		while (!IsDeviceCalibrated()) {
			Debug.LogWarning("Please calibrate the device!");
			yield return new WaitForSeconds(1.5f);
		}
		if (!IsDeviceReady())
			Debug.LogError("Device is not ready!");
		main = this;
	}
	#endregion
	
	//-------------------------------------------------

	// Update is called once per frame
	void Update () {
		//ScreenCursor.SetPosition ((int)GetXPos(),(int)GetYPos());
		//Debug.Log (GetServoPos ());
		//move falcon as a mouse
		x = GetServoPos ().x;
		y = GetServoPos ().y;
		int newX = (int)map (-1.5f, 1.5f, 2000f, 0f, x);
		int newY = (int)map (-1.5f, 1.5f, 0f, 2000f, y);
		//Debug.Log ("x is " + newX);
		//Debug.Log ("y is " + newY);
		ScreenCursor.SetPosition(newX,newY);

		//left click
		if (isButton0Down ()) {
			ScreenCursor.LeftClick ();
			Strength = 10f;
			//SetServoPos (new double[3] { GetServoPos ().x, GetServoPos ().y, GetServoPos ().z }, Strength);
		} else
			Strength = 3f;

		SetServoPos(new double[3] { GetServoPos().x, GetServoPos().y, GetServoPos().z }, Strength);

	}

	void OnApplicationQuit() {
		StopHaptics();
	}
	public Vector3 GetServoPos() {
		return new Vector3(-(float)GetXPos(), -(float)GetYPos(), -(float)GetZPos());
	}

	private float map(float OldMin, float OldMax, float newMin, float newMax, float value)
	{
		float oldRange = OldMax - OldMin;
		float newRange = newMax - newMin;
		float newValue = (((value - OldMin) * newRange) / oldRange) + newMin;
		return (newValue);
	}

	public static Vector3 FalconToMouse(Vector3 position)
	{
		float height = (float)Screen.height;
		float width = (float)Screen.width;
		
		float min = -2f;
		float max = 2f;
		
		float old_range = max - min;
		float new_x = ((position.x - min) * width) / old_range;
		float new_y = ((position.y - min) * height) / old_range;
		float new_z = ((position.z - min) * 8f) / old_range;
		
		//float new_x = Mathf.Lerp (0f, width, position.x);
		
		return new Vector3 (new_x, new_y, new_z);
		
	}
}
