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
public class DeviceNovintFalcon : MonoBehaviour {
	
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
	
	public static DeviceNovintFalcon main;
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
	
	protected CharacterController Charactor = null;
	private Vector3 MoveThrottle            = Vector3.zero;
	public float TranslationSensitivity     = 10.0f;
	#endregion
	
	//-------------------------------------------------
	#region Start()
	
	void Start() {
		StartHaptics();
		StartCoroutine(_initHaptics());
		Charactor = gameObject.GetComponent<CharacterController>();
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
		
		gameObject.transform.position = new Vector3((float)GetXPos() * -2, (float)GetYPos() * 2, (float)GetZPos() * 2);
		
		Debug.Log(isButton0Down() + " , " + isButton1Down() + " , " + isButton2Down() + " , " + isButton3Down());
	}
	
	/// <summary>
	/// ãƒ‡ãƒã‚¤ã‚¹å‹•ä½œåˆ¶å¾¡
	/// </summary>
	private void _feedback() {
		// NovintFalconã®ã‚°ãƒªãƒƒãƒ—ã‚’ãƒ‡ãƒ•ã‚©ä½ç½®ã«æˆ»ã™
		SetServo(new double[3] { SpeedX, SpeedY, SpeedZ });
		SetServoPos(new double[3] { PosX, PosY, PosZ }, Strength);
		Debug.Log(GetServoPos());
	}
	
	/// <summary>
	/// ã‚­ãƒ£ãƒ©ã‚¯ã‚¿ãƒ¼å‹•ä½œåˆ¶å¾¡
	/// </summary>
	private void _charaMove() {
		if ((float)GetZPos() >= 0.8f)  MoveThrottle += __moveVector(Vector3.forward);
		if ((float)GetZPos() <= -0.8f) MoveThrottle += __moveVector(Vector3.back);
		if ((float)GetXPos() >= 0.8f)  MoveThrottle += __moveVector(Vector3.left);
		if ((float)GetXPos() <= -0.8f) MoveThrottle += __moveVector(Vector3.right);
		if ((float)GetYPos() >= 0.8f)  MoveThrottle += __moveVector(Vector3.up * 2);
		if ((float)GetYPos() <= -0.8f) MoveThrottle += __moveVector(Vector3.down);
		
		float motorDamp = (1.0f + 0.15f);
		MoveThrottle.x /= motorDamp;
		MoveThrottle.y = (MoveThrottle.y > 0.0f) ? (MoveThrottle.y / motorDamp) : MoveThrottle.y;
		MoveThrottle.z /= motorDamp;
		
		Charactor.Move(MoveThrottle);
	}
	
	private Vector3 __moveVector(Vector3 moveVector) {
		return Charactor.transform.TransformDirection(
			-1* moveVector * Time.deltaTime * TranslationSensitivity);
	}
	
	#endregion
	
	//-------------------------------------------------
	#region Misc
	
	void OnApplicationQuit() {
		StopHaptics();
	}
	public Vector3 GetServoPos() {
		return new Vector3(-(float)GetXPos(), -(float)GetYPos(), -(float)GetZPos());
	}
	
	#endregion
}