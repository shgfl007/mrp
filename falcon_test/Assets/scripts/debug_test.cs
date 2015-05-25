using UnityEngine;
using System.Collections;

public class debug_test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Debug.isDebugBuild)
			DebugConsole.Log ("This is a debug build", "normal");
	}
}
