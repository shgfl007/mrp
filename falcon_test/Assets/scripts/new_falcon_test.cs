using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using UnityEngine.UI;


public class new_falcon_test : MonoBehaviour {
	
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
	
	public static new_falcon_test main;
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

		_feedback();

		_charaMove();

		float smoothTime = 0.3f;
		float velocity = 0.0f;
		//gameObject.transform.position = new Vector3((float)GetXPos() * -2, (float)GetYPos() * 2, (float)GetZPos() * 2);
		if (Strength != 0f) {
			Strength--;
			//Strength = Mathf.SmoothDamp(Strength, 0f, ref velocity,smoothTime);
		}
		//Debug.Log(isButton0Down() + " , " + isButton1Down() + " , " + isButton2Down() + " , " + isButton3Down());
		//Debug.Log ("strength is " +Strength);
	}


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
		//SetServoPos(new double[3] { GetServoPos().x, GetServoPos().y, GetServoPos().z }, 0f);
		SetForce (norm, Strength);

		normText.text = norm[0].ToString()+ "," + norm[1].ToString() + "," + norm[2].ToString();
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
				temp_strength = 5f;
				isFirsthit = false;
				ContactPoint contact = col.contacts[0];
				norm[0] = contact.normal.x;
				norm[1] = contact.normal.y;
				norm[2] = contact.normal.z;
			}else			//if it is not... dump!
			{
 
				if(Strength == 0f)
				{
					isFirsthit = true; //change state
					debugTxt.text = "is first hit = " + isFirsthit;

				}
			}
		}
	}
	
	void OnCollisionStay(Collision col)
	{
		Debug.Log ("continue....");
		if (col.gameObject.name == "powerCube") {
//			foreach (ContactPoint contact in col.contacts){
//				Debug.Log(contact.thisCollider.name + " hit " + contact.otherCollider.name);
//				Debug.Log("contact norm is " + contact.normal);
//				Debug.DrawRay(contact.point, contact.normal,Color.red);
//			}

			if(isFirsthit)		//if it is first time hit, change the strength
			{
				temp_strength = 5f;
				isFirsthit = false;
				ContactPoint contact = col.contacts[0];
				norm[0] = contact.normal.x;
				norm[1] = contact.normal.y;
				norm[2] = contact.normal.z;
			}else			//if it is not... dump!
			{
				//				for(int i = 0; i < 100; i++)
				//				{
				//					//do nothing!!
				//					Debug.Log("waiting....." + i);
				//				}
				if(Strength==0f)
				{
					isFirsthit = true; //change state
					debugTxt.text = "on stay is first hit : " + isFirsthit.ToString();
				}
			}			
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
	Vector3 GetServoPos() {
		return new Vector3((float)GetXPos(), (float)GetYPos(), -(float)GetZPos());
	}
	
	#endregion
}