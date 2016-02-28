using UnityEngine;
using System.Collections;

public class init : MonoBehaviour {

	public bool callPopupWindow;
	
	
	public Rect windowRect = new Rect(250,200,200,80);

	// Use this for initialization
	void Start () {
		Falcon_Control.StartHaptics();
		StartCoroutine(_initHaptics());
	}
	
	private IEnumerator _initHaptics() {
		while (!Falcon_Control.IsDeviceCalibrated()) {
			Debug.LogWarning("Please calibrate the device!");
			//callPopupWindow = true;
			yield return new WaitForSeconds(1.5f);
		}
		if (Falcon_Control.IsDeviceReady ())
			callPopupWindow = false;
		if (!Falcon_Control.IsDeviceReady())
			Debug.LogError("Device is not ready!");

	}
	
	void OnGUI(){
		if (callPopupWindow) {
			windowRect = GUI.Window(0, windowRect, DoMyWindow, "Please calibrate the device!");
		}
	}
	
	void DoMyWindow(int windowID){
		if (GUI.Button (new Rect (50, 50, 100, 20), "ok")) {
			print ("Got a click");
			callPopupWindow = false;
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
