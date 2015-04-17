using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

/// <summary>
/// NovintFalconã‚’æ“ä½œãƒ‡ãƒã‚¤ã‚¹ã¨ã—ã¦ä½¿ã†script â€»Windowsç”¨ï¼ˆdllä¾å­˜ï¼‰
///
/// usage:
///  1. http://forum.unity3d.com/threads/6494-Novint-falcon ã‹ã‚‰
///     Falcon Wrapper Source - Update V1.zip ã‚’DLã€FalconWrapper.dllã‚’Assetsã¨åŒéšŽå±¤ã¸ã‚³ãƒ”ãƒ¼
///     ãƒ“ãƒ«ãƒ‰å¾Œã¯Windowsã®PathãŒé€šã£ã¦ã„ã‚‹ã¨ã“ã‚ã«FalconWrapper.dllã‚’ç½®ãã‹ã€ãƒã‚¤ãƒŠãƒªã¨åŒéšŽå±¤ã«ç½®ã
///  2. ã“ã®ã‚¹ã‚¯ãƒªãƒ—ãƒˆã‚’ãƒ—ãƒ­ã‚¸ã‚§ã‚¯ãƒˆã¸In
///  3. æ“ä½œã—ãŸã„å¯¾è±¡ã®GameObjectã«Add Component
///  4. Inspectorã§é©å½“ã«å¼„ã£ã¦å¯Ÿã™ã‚‹
/// â€»å¯¾è±¡ã®å‹•ãã‚’NovintFalconã¸Hapticã™ã‚‹æ–¹æ³•ã¯_feedbackå‚ç…§
///
/// NovintFalconã®æ­£ã—ã„ä½¿ã„æ–¹ - Unity Advent Calendar 2013 vol.23
/// http://yunojy.github.io/blog/2013/12/23/how-to-use-novintfalcon-unity-advent-calendar-2013-vol-dot-23/
/// </summary>
public class falcon_test : MonoBehaviour {
	
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
	
	public static falcon_test main;
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
	private float temp_strength = 0f;
	bool isFirsthit = true;
	public GUIText debugTxt;
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
		if (Strength > 3f)
			Strength--;
		//Debug.Log(isButton0Down() + " , " + isButton1Down() + " , " + isButton2Down() + " , " + isButton3Down());
		//Debug.Log ("strength is " +Strength);
	}
	
	/// <summary>
	/// ãƒ‡ãƒã‚¤ã‚¹å‹•ä½œåˆ¶å¾¡
	/// </summary>
	private void _feedback() {
		// NovintFalconã®ã‚°ãƒªãƒƒãƒ—ã‚’ãƒ‡ãƒ•ã‚©ä½ç½®ã«æˆ»ã™
//		if (temp_strength != 3f && isFirsthit) {
//			Strength = temp_strength;
//			temp_strength = 0f;
//		} else
//			Strength = 3f;
		if (temp_strength != 0f && isFirsthit) {
			Strength = temp_strength;
			temp_strength = 0f;
		}
		SetServo(new double[3] { SpeedX, SpeedY, SpeedZ });
		//SetServoPos(new double[3] { PosX, PosY, PosZ }, Strength);
		SetServoPos(new double[3] { GetServoPos().x, GetServoPos().y, GetServoPos().z }, Strength);

		Debug.Log(GetServoPos());
	}

	/// </summary>
	private void _charaMove() {
		obj.transform.position = GetServoPos();

	}

	//detect collision
	void OnCollisionEnter(Collision col)
	{
		//Debug.Log("collision funtion called");
		debugTxt.text = "collision enter"; 
		if (col.gameObject.name == "powerCube") {
			//check if it is the first time hit
			//***** check the logic here!!! might be something wrong with the 
			if(isFirsthit)		//if it is first time hit, change the strength
			{
				temp_strength = 10f;
				isFirsthit = false;
			}else			//if it is not... dump!
			{
//				for(int i = 0; i < 100; i++)
//				{
//					//do nothing!!
//					Debug.Log("waiting....." + i);
//				}
				isFirsthit = true; //change state
			}
			//Destroy(col.gameObject);
			//Rigidbody rb = obj.GetComponent<Rigidbody>();
			//rb.isKinematic = true;
		}
	}

	void OnCollisionStay(Collision col)
	{
		Debug.Log ("continue....");
		if (col.gameObject.name == "powerCube") {
			foreach (ContactPoint contact in col.contacts){
				Debug.Log(contact.thisCollider.name + " hit " + contact.otherCollider.name);
				Debug.Log("contact norm is " + contact.normal);
				Debug.DrawRay(contact.point, contact.normal,Color.red);
			}
			if(isFirsthit)		//if it is first time hit, change the strength
			{
				temp_strength = 20f;
				isFirsthit = false;
			}else			//if it is not... dump!
			{
				//				for(int i = 0; i < 100; i++)
				//				{
				//					//do nothing!!
				//					Debug.Log("waiting....." + i);
				//				}
				isFirsthit = true; //change state
			}			Debug.Log (temp_strength);
		}
		if (col.gameObject.name == "Plane")
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