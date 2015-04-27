using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using UnityEngine.UI;

public class falcon_statemachine : MonoBehaviour {

	#region wrapper Variables
	
	//const string falcon = "Falcon_Wrapper_new.dll";
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
	private static extern int GetButtonsDown();
	[DllImport(falcon)]
	public static extern bool isButton0Down();
	[DllImport(falcon)]
	private static extern bool isButton1Down();
	[DllImport(falcon)]
	private static extern bool isButton2Down();
	[DllImport(falcon)]
	private static extern bool isButton3Down();
	[DllImport(falcon)]
	private static extern void SetForce(double[] norm, double strength);
	
	public static falcon_statemachine main;
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
	public float Strength = 0f;
	
	//protected CharacterController Charactor = null;
	public float TranslationSensitivity     = 10.0f;
	public GameObject obj;

	public Text debugTxt;
	public Text normText;
	private double[] norm = new double[3];
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

		if (statemachine.state == 0)
			Strength = 1f;
		else if (statemachine.state == 1) {
			Strength = 5f;
			norm[0] = statemachine.norm.x;
			norm[1] = statemachine.norm.y;
			norm[2] = statemachine.norm.z;
		}
		debugTxt.text = "strength is " + Strength.ToString ();

		_feedback();
		
		_charaMove();
		

		//gameObject.transform.position = new Vector3((float)GetXPos() * -2, (float)GetYPos() * 2, (float)GetZPos() * 2);
		//if (Strength != 0f) {
			//Strength--;
			//Strength = Mathf.SmoothDamp(Strength, 0f, ref velocity,smoothTime);
		//}
		//Debug.Log(isButton0Down() + " , " + isButton1Down() + " , " + isButton2Down() + " , " + isButton3Down());
		//Debug.Log ("strength is " +Strength);
	}
	
	
	private void _feedback() {


		SetServo(new double[3] { SpeedX, SpeedY, SpeedZ });
		//SetServoPos(new double[3] { PosX, PosY, PosZ }, Strength);
		//SetServoPos(new double[3] { GetServoPos().x, GetServoPos().y, GetServoPos().z }, 0f);
		SetForce (norm, Strength);
		
		normText.text = norm[0].ToString()+ "," + norm[1].ToString() + "," + norm[2].ToString();
	}
	
	/// </summary>
	private void _charaMove() {
		obj.transform.position = GetServoPos();
		
	}
	#endregion
	
	//-------------------------------------------------
	#region Misc
	
	void OnApplicationQuit() {
		StopHaptics();
	}
	Vector3 GetServoPos() {
		return new Vector3((float)GetXPos(), (float)GetYPos(), -(float)GetZPos());
	}
	
	#endregion
}
