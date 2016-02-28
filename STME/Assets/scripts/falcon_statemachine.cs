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
	private static extern void SetForce(double[] norm, double strength);

	public static float stiffness;

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

	public bool callPopupWindow;
	public Text debugTxt;
	public Text normText;
	public Text slicer_norm_debug;
	private double[] norm = new double[3];
	public Rect windowRect = new Rect(500,300,120,50);

	#endregion
	
	
	
	//-------------------------------------------------
	#region Start()
	
	void Start() {
		StartHaptics();
		StartCoroutine(_initHaptics());
		stiffness = 3f;

	}
	
	private IEnumerator _initHaptics() {
		while (!IsDeviceCalibrated()) {
			Debug.LogWarning("Please calibrate the device!");
			//callPopupWindow = true;
			yield return new WaitForSeconds(1.5f);
		}
		if (IsDeviceReady ())
			callPopupWindow = false;
		if (!IsDeviceReady())
			Debug.LogError("Device is not ready!");
		main = this;
	}

	void OnGUI(){
//		if (callPopupWindow) {
//			windowRect = GUI.Window(0, windowRect, DoMyWindow, "Please calibrate the device!");
//		}
	}

	void DoMyWindow(int windowID){
//		if (GUI.Button (new Rect (50, 50, 100, 20), "ok")) {
//			print ("Got a click");
//			callPopupWindow = false;
//		}
	}
	#endregion
	
	//-------------------------------------------------
	#region Update()
	
	void Update() {

		if (statemachine.state == 0 && Strength>1f)
			Strength--;
		else if (statemachine.state == 1) {
			Strength = 5f;
			norm[0] = statemachine.norm.x;
			norm[1] = statemachine.norm.y;
			norm[2] = statemachine.norm.z;
		}

		if (statemachine.isSelected) {
			Strength = stiffness;
			//get the direction of the user's movement
			//direction = (to-from)/distance
			Vector3 temp = GetServoPos() - statemachine.start_point;
			float scale = Vector3.Distance(statemachine.start_point, GetServoPos());
			temp = temp/scale;
			norm[0] = temp.x;
			norm[1] = temp.y;
			norm[2] = temp.z;
		}
		//--------------------- force feedback for slicing -----------
		if (isFunction.shouldFunction) {
//			norm[0] = Slicer.slice_norm.x;
//			norm[1] = Slicer.slice_norm.y;
//			norm[2] = Slicer.slice_norm.z;
			norm[0] = 0f; norm[1] = 1f; norm[2] = 0f;
			//slicer_norm_debug.text = "slicer norm is " + Slicer.slice_norm.ToString();
			slicer_norm_debug.text = "slicer norm is " + norm[0].ToString() + ", " + norm[1].ToString() + ", " + norm[2].ToString();
		}
		//debugTxt.text = "strength is " + Strength.ToString ();

		_feedback();
		
		_charaMove();

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
