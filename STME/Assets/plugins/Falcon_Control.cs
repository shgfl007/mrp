using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using UnityEngine.UI;

public class Falcon_Control : MonoBehaviour {
	#region wrapper Variables
	
	//const string falcon = "Falcon_Wrapper_new.dll";
	const string falcon = "Falcon Wrapper_test1.dll";
	
	[DllImport(falcon)]
	public static extern void StartHaptics();
	[DllImport(falcon)]
	public static extern void StopHaptics();
	[DllImport(falcon)]
	public static extern bool IsDeviceCalibrated();
	[DllImport(falcon)]
	public static extern bool IsDeviceReady();
	[DllImport(falcon)]
	public static extern double GetXPos();
	[DllImport(falcon)]
	public static extern double GetYPos();
	[DllImport(falcon)]
	public static extern double GetZPos();
	[DllImport(falcon)]
	private static extern void SetServo(double[] speed);
	[DllImport(falcon)]
	private static extern void SetServoPos(double[] pos, double strength);
	[DllImport(falcon)]
	private static extern bool IsHapticButtonDepressed();
	[DllImport(falcon)]
	public static extern int GetButtonsDown();
	[DllImport(falcon)]
	public static extern bool isButton0Down();
	[DllImport(falcon)]
	public static extern bool isButton1Down();
	[DllImport(falcon)]
	public static extern bool isButton2Down();
	[DllImport(falcon)]
	public static extern bool isButton3Down();
	[DllImport(falcon)]
	public static extern void SetForce(double[] norm, double strength);
	
	public static float stiffness;
	public static double[] norm;

	public static Falcon_Control main;

	#endregion
	
	#region Variables
	[Range(-10f, 100f)]
	public static float SpeedX = 0f;
	[Range(-10f, 100f)]
	public static float SpeedY = 0f;
	[Range(-10f, 100f)]
	public static float SpeedZ = -10f;
	
	[Range(-1.5f, 1.5f)]
	public float PosX = 0f;
	[Range(-1.5f, 1.5f)]
	public float PosY = 0f;
	[Range(-1.5f, 1.5f)]
	public float PosZ = 0f;
	
	[Range(0f, 100f)]
	public static float Strength = 0f;
	
	//protected CharacterController Charactor = null;
	public float TranslationSensitivity     = 10.0f;
	public bool callPopupWindow;


	public Rect windowRect = new Rect(500,300,120,50);


	#endregion

	// Use this for initialization
//	void Start () {
////		StartHaptics();
////		StartCoroutine(_initHaptics());
//	}


	// Update is called once per frame
//	void Update () {
//		_feedback();
//	}

	public static void _feedback() {

		SetServo(new double[3] { SpeedX, SpeedY, SpeedZ });
		//SetServoPos(new double[3] { PosX, PosY, PosZ }, Strength);
		//SetServoPos(new double[3] { GetServoPos().x, GetServoPos().y, GetServoPos().z }, 0f);
		SetForce (norm, Strength);
	}

	void OnApplicationQuit() {
		StopHaptics();
	}

	public static Vector3 GetServoPos() {
		return new Vector3((float)(GetXPos()*2f), (float)(GetYPos()*2f), -(float)(GetZPos()*2f));
	}

	public static Vector3 Falcon_To_Mouse()
	{
		float height = (float)Camera.main.pixelHeight;
		float width = (float)Camera.main.pixelWidth;
		Vector3 temp = new Vector3 (width, height, 0f);
		Vector3 mappedTemp = Camera.main.ScreenToWorldPoint (temp);
		Vector3 mappedMin = Camera.main.ScreenToWorldPoint (new Vector3(0f, 0f, 0f));
		float min = -2f;
		float max = 2f;
		
		float old_range = max - min;
		float new_x = (((float)GetXPos() - min) * width) / old_range;
		float new_y = (((float)GetYPos() - min) * height) / old_range;
//		float new_x = (((float)GetXPos() - min) * (mappedTemp.x) )/ old_range;
//		float new_y = (((float)GetYPos() - min) * (mappedTemp.y) )/ old_range;
//		float new_x = (float)GetXPos () * 2;
//		float new_y = (float)GetYPos () * 2;
		float new_z = (((float)GetZPos() - min) * 4f) / old_range;
//		float new_z = (float)GetZPos ();
		//float new_z = 0;
		//float new_x = Mathf.Lerp (0f, width, position.x);
		Debug.Log ("falcon mapped " + new_x.ToString() + "," + new_y.ToString() + "," + new_z.ToString()+ ". temp is " + temp.ToString() + ". mapped temp is "
		           + mappedTemp.ToString() + ". Mapped min is " + mappedMin.ToString());
		return new Vector3 (new_x, new_y, -new_z);
	}
}
