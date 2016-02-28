using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

public class controll_falcon : MonoBehaviour {

	#region Falcon Wrapper.dll Variables
	
	const string falcon = "Falcon Wrapper.dll";
	
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
	
	public static controll_falcon main;
	#endregion
	
	#region Variables
	[Range(-10f, 100f)]
	public float SpeedX = 0f;
	[Range(-10f, 100f)]
	public float SpeedY = 0f;
	[Range(-10f, 100f)]
	public float SpeedZ = -10f;
	
	[Range(-1.5f, 1.5f)]
	public float PosX = 0f;
	[Range(-1.5f, 1.5f)]
	public float PosY = 0f;
	[Range(-1.5f, 1.5f)]
	public float PosZ = 0f;
	
	[Range(0f, 100f)]
	public float Strength = 3f;
	
	//protected CharacterController Charactor = null;
	public float TranslationSensitivity     = 10.0f;
	public GameObject obj;
	private float temp_strength = 3f;
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
	#region Update()
	
	void Update() {
		// ãƒ‡ãƒã‚¤ã‚¹å‹•ä½œåˆ¶å¾¡
		_feedback();
		// ã‚­ãƒ£ãƒ©ã‚¯ã‚¿ãƒ¼å‹•ä½œåˆ¶å¾¡
		_charaMove();
		
		//gameObject.transform.position = new Vector3((float)GetXPos() * -2, (float)GetYPos() * 2, (float)GetZPos() * 2);
		
		//Debug.Log(isButton0Down() + " , " + isButton1Down() + " , " + isButton2Down() + " , " + isButton3Down());
		Debug.Log ("strength is " +Strength);
	}
	
	/// <summary>
	/// ãƒ‡ãƒã‚¤ã‚¹å‹•ä½œåˆ¶å¾¡
	/// </summary>
	private void _feedback() {
		// NovintFalconã®ã‚°ãƒªãƒƒãƒ—ã‚’ãƒ‡ãƒ•ã‚©ä½ç½®ã«æˆ»ã™
//		if (temp_strength != 3f) {
//			Strength = temp_strength;
//			temp_strength = 3f;
//		}
		SetServo(new double[3] { SpeedX, SpeedY, SpeedZ });
		//SetServoPos(new double[3] { PosX, PosY, PosZ }, Strength);
		SetServoPos(new double[3] { GetServoPos().x, GetServoPos().y, GetServoPos().z }, Strength);
		
		//Debug.Log(GetServoPos());
	}
	
	/// </summary>
	private void _charaMove() {
		obj.transform.position = GetServoPos();
		
	}
	
	//detect collision
	void OnCollisionEnter(Collision col)
	{
		Debug.Log("collision funtion called");
		if (col.gameObject.name == "powerCube") {
			temp_strength = 20f;
			Debug.Log("collision!!!!!");
		}
	}
	
	void OnCollisionStay(Collision col)
	{
		Debug.Log ("continue....");
		if (col.gameObject.name == "powerCube") {
			temp_strength = 20f;
			Debug.Log (temp_strength);
		}
		if (col.gameObject.name == "")
			Debug.Log ("plane!!");
	}
	
	#endregion
	
	//-------------------------------------------------
	#region Misc
	
	void OnApplicationQuit() {
		StopHaptics();
	}
	public Vector3 GetServoPos() {
		return new Vector3((float)GetXPos(), (float)GetYPos(), -(float)GetZPos());
	}
	
	#endregion
}
