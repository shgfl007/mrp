using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sketch_test : MonoBehaviour {

	private GameObject controller;
	public Camera mainCamera;
	private Vector3 falcon_pos;
	public static Camera camera;
	public static bool isDraw;
	public Text falconP;

	private float zThreshold;
	// Use this for initialization
	void Start () {
		//mainCamera = Camera.main;
		//mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		//camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		camera = mainCamera;
		controller = GameObject.FindGameObjectWithTag("Controller");
		isDraw = false;

		zThreshold = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		//read and store falcon position
		falcon_pos = Falcon_Control.GetServoPos ();
		falcon_pos.x = falcon_pos.x / 2f;falcon_pos.y = falcon_pos.y / 2f;falcon_pos.z = falcon_pos.z / 2f;
		falconP.text = "falcon position is " + falcon_pos.ToString ();

		//check if the controller is close to paper
		//threshold = 1
		if (falcon_pos.z >= zThreshold)
			isDraw = true;
		else
			isDraw = false;

		_moveController ();
	}

	public static Vector3 FalconToMouse(Vector3 position)
	{
//		float height = (float)mainCamera.pixelHeight;
//		float width = (float)mainCamera.pixelWidth;

		float height = (float)camera.pixelHeight;
		float width = (float)camera.pixelWidth;
		float min = -2f;
		float max = 2f;
		
		float old_range = max - min;
		float new_x = ((position.x - min) * width) / old_range;
		float new_y = ((position.y - min) * height) / old_range;
		//float new_z = ((position.z - min) * Camera.main.depth) / old_range;
		//float new_z = -5f;
		float new_z = 0;
		//float new_z = ((position.z - min) * (obj.transform.position.z - Camera.main.transform.position.z)) / old_range;
		//float new_x = Mathf.Lerp (0f, width, position.x);
		
		return new Vector3 (new_x, new_y, new_z);
		
	}

	void _moveController(){
		Vector3 temp = FalconToMouse (falcon_pos);
		//temp.z = controller.transform.position.z - Camera.main.transform.position.z;
		temp.z = controller.transform.position.z - camera.transform.position.z;
		controller.transform.position = camera.ScreenToWorldPoint (temp);
		//controller.transform.position = falcon_pos;
	}


}
