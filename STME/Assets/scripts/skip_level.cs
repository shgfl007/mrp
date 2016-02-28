using UnityEngine;
using System.Collections;

public class skip_level : MonoBehaviour {

	public static bool isSkip;
	// Use this for initialization
	void Start () {
		isSkip = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			//Falcon_Control.StopHaptics();
			Application.LoadLevel(2);
		}
		if (Falcon_Control.isButton3Down ()) {
			Application.LoadLevel(2);
		}
//		if (timer.isTimeUp)
//			Application.LoadLevel(2);
		if (isSkip)
			Application.LoadLevel (2);
	
	}
}
