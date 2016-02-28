using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;
using UnityEngine.UI; 

public class falcon_control : MonoBehaviour {
	private Vector3 current_position;
	public GameObject controller;

	public Text debug_currentPos;
	public Text debug_mappedPos;
	const string falcon = "Falcon Wrapper_test1.dll";
	
	[DllImport(falcon)]
	private static extern void StartHaptics();
	[DllImport(falcon)]
	private static extern void StopHaptics();

	// Use this for initialization
	void Start () {
		StartHaptics();
	}
	
	// Update is called once per frame
	void Update () {
		current_position = GetServoPos ();
		debug_currentPos.text = current_position.ToString ();
		Vector3 temp = FalconToMouse (current_position);
		debug_mappedPos.text = temp.ToString();
		_charaMove (temp);
	}

	Vector3 GetServoPos()
	{
		return new Vector3 ((float)falcon_statemachine.GetXPos(), (float)falcon_statemachine.GetYPos(), (float)falcon_statemachine.GetZPos());
	}

	Vector3 FalconToMouse(Vector3 position)
	{
//		float height = (float)Screen.height;
//		float width = (float)Screen.width;

		float height = 20f;
		float width = 30f;

		float min = -2f;
		float max = 2f;
		
		float old_range = max - min;
		float new_x = ((position.x - min) * width) / old_range;
		float new_y = ((position.y - min) * height) / old_range;
		float new_z = -((position.z - min) * 8f) / old_range;
		
		//float new_x = Mathf.Lerp (0f, width, position.x);
		
		return new Vector3 (new_x, new_y, new_z);
		
	}

	private void _charaMove(Vector3 position) {
		controller.transform.position = position;
		
	}

	void OnApplicationQuit() {
		StopHaptics();
	}

}
