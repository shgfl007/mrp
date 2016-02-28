using UnityEngine;
using System.Collections;

public class onExit : MonoBehaviour {

	void OnApplicationQuit() {
		Falcon_Control.StopHaptics();
	}
}
