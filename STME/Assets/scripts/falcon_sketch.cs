using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using UnityEngine.UI;



public class falcon_sketch : MonoBehaviour {

	#region wrapper Variables
	
	//const string falcon = "Falcon_Wrapper_new.dll";
	const string falcon = "Falcon Wrapper_test1.dll";
	
	[DllImport(falcon)]
	private static extern void StartHaptics();
	[DllImport(falcon)]
	public static extern void StopHaptics();
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
	
	public static falcon_sketch main;
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
	public Text falcon_position;
	public Text mapped_position;
	public Text mouse_position;
	private double[] norm = new double[3];
	public Rect windowRect = new Rect(500,300,120,50);
	Vector3 newRange;
	private Vector3 ScreenZero;
	private Vector3 ScreenMax;
	private Camera mainCamera;
	#endregion
	
	
	
	//-------------------------------------------------
	#region Start()
	
	void Start() {
		StartHaptics();
		StartCoroutine(_initHaptics());
		stiffness = 3f;
		mainCamera = Camera.main;
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
		ScreenZero = mainCamera.ScreenToWorldPoint (Vector3.zero);
		ScreenMax = mainCamera.ScreenToWorldPoint (new Vector3 ((float)Screen.width, (float)Screen.height, 0f));
		//_feedback();
		
		_charaMove();
		falcon_position.text = "falcon position is " + GetServoPos ().ToString ();
		mouse_position.text = "mapped position is " + Camera.main.ScreenToWorldPoint(GetServoPos()).ToString();
		//mouse_position.text = "mapped position is " + FalconToWorld (FalconToMouse (GetServoPos ())).ToString ();
		//mouse_position.text = "mouse position is " + Input.mousePosition.ToString ();
		mapped_position.text = "obj position is " + obj.transform.position.ToString ();
	}
	
	
	private void _feedback() {
		
		
		SetServo(new double[3] { SpeedX, SpeedY, SpeedZ });
		//SetServoPos(new double[3] { PosX, PosY, PosZ }, Strength);
		//SetServoPos(new double[3] { GetServoPos().x, GetServoPos().y, GetServoPos().z }, 0f);
		SetForce (norm, Strength);
		
	}

	public static Vector3 Falcon2Mouse(Vector3 position)
	{
		float height = (float)Camera.main.pixelHeight;
		float width = (float)Camera.main.pixelWidth;
		
		float min = -2f;
		float max = 2f;
		
		float old_range = max - min;
		float new_x = ((position.x - min) * width) / old_range;
		float new_y = ((position.y - min) * height) / old_range;
		float new_z = ((position.z - min) * 1f) / old_range;
		//float new_z = 0;
		//float new_x = Mathf.Lerp (0f, width, position.x);
		
		return new Vector3 (new_x, new_y, -new_z);
		
	}

	private Vector3 FalconToMouse(Vector3 position)
	{
		float height = (float)Camera.main.pixelHeight;
		float width = (float)Camera.main.pixelWidth;
		
		float min = -2f;
		float max = 2f;
		
		float old_range = max - min;
		float new_x = ((position.x - min) * width) / old_range;
		float new_y = ((position.y - min) * height) / old_range;
		//float new_z = ((position.z - min) * Camera.main.depth) / old_range;
		float new_z = 0;
		//float new_z = ((position.z - min) * (obj.transform.position.z - Camera.main.transform.position.z)) / old_range;
		//float new_x = Mathf.Lerp (0f, width, position.x);
		
		return new Vector3 (new_x, new_y, new_z);
		
	}

	//send the return value of falcon to mouse to this function, it will transfer the screen position to world position 
	private Vector3 FalconToWorld(Vector3 position)
	{
		float height = (float)Camera.main.pixelHeight;
		float width = (float)Camera.main.pixelWidth;

		Vector3 tempRange = new Vector3 (width, height, 0f);
		newRange = Camera.main.ScreenToWorldPoint (tempRange);
		float min = -2f;
		float max = 2f;
		
		float old_range = max - min;
		float new_x = (position.x * newRange.x) / height;
		float new_y = (position.y * newRange.y) / width;
		//float new_z = ((position.z - min) * Camera.main.depth) / old_range;
		float new_z = position.z;
		//float new_z = ((position.z - min) * (obj.transform.position.z - Camera.main.transform.position.z)) / old_range;
		//float new_x = Mathf.Lerp (0f, width, position.x);
		
		return new Vector3 (new_x, new_y, new_z);
		
	}

	
	/// </summary>
	private void _charaMove() {
		//obj.transform.position = FalconToMouse(GetServoPos());
		//obj.transform.position = Input.mousePosition;
		//obj.transform.localPosition = GetServoPos ();
		Vector3 temp = FalconToMouse (GetServoPos ());
		Vector3 fp = GetServoPos ();
		temp.z = obj.transform.position.z - Camera.main.transform.position.z;
		obj.transform.position = mainCamera.ScreenToWorldPoint (temp);
		//obj.transform.position = Camera.main.ScreenToWorldPoint(temp);
		//obj.transform.position = FalconToWorld (FalconToMouse (fp));
		//Vector3 temp0 = Camera.main.ScreenToWorldPoint (temp);
		//temp0.z = fp.z;
		//mapped_position.text = "mapped position is " + Camera.main.ScreenToWorldPoint(temp);
		//obj.transform.position = Camera.main.ScreenToWorldPoint (temp);
	}
	#endregion
	
	//-------------------------------------------------
	#region Misc
	
	void OnApplicationQuit() {
		StopHaptics();
	}
	public Vector3 GetServoPos() {
		return new Vector3((float)GetXPos(), (float)GetYPos(), (float)GetZPos());
	}
	
	#endregion
}
