using UnityEngine;
using System.Collections;

public class camera_manager : MonoBehaviour {

	
	GameObject _cameraFP = null;
	GameObject _camera1 = null;
	GameObject _camera2 = null;
	GameObject _camera3 = null;
	GameObject _camera4 = null;
	GameObject _camera5 = null;
	//GameObject _camera6 = null;
	//GameObject _camera7 = null;

	private GameObject[] camera_group;
	public int cameraIndex;

	void Start ()
	{
		camera_group = new GameObject[5];
		// Init camera
		_cameraFP = GameObject.Find("Main Camera");
		if (_cameraFP == null)
			Debug.Log("Start(): First Person Camera not found");
		
		_camera1 = GameObject.Find("Camera_1");
		if (_camera1 == null)
			Debug.Log ("Start(): Camera 1 not found");
		else
			camera_group [0] = _camera1;

		_camera2 = GameObject.Find("Camera_2");
		if (_camera1 == null)
			Debug.Log("Start(): Camera 2 not found");
		else
			camera_group [1] = _camera2;


		_camera3 = GameObject.Find("Camera_3");
		if (_camera1 == null)
			Debug.Log("Start(): Camera 3 not found");
		else
			camera_group [2] = _camera3;


		_camera4 = GameObject.Find("Camera_4");
		if (_camera1 == null)
			Debug.Log("Start(): Camera 4 not found");
		else
			camera_group [3] = _camera4;


		_camera5 = GameObject.Find("Camera_5");
		if (_camera1 == null)
			Debug.Log("Start(): Camera 5 not found");
		else
			camera_group [4] = _camera5;


//		_camera5 = GameObject.Find("Camera_6");
//		if (_camera1 == null)
//			Debug.Log("Start(): Camera 6 not found");
//		else
//			camera_group [5] = _camera6;
//
//
//		_camera5 = GameObject.Find("Camera_7");
//		if (_camera1 == null)
//			Debug.Log("Start(): Camera 7 not found");
//		else
//			camera_group [6] = _camera7;

		_cameraFP.GetComponent<Camera> ().enabled = true;
		for (int i = 0; i<5; i++) 
		{
			if(camera_group[i].GetComponent<Camera>()!=null)
				camera_group[i].GetComponent<Camera>().enabled = false;
		}
	}
	
	/*public void SelectCamera(int cameraIndex)
	{
		if (_cameraFP != null)
			_cameraFP.GetComponent<Camera>().enabled = (cameraIndex == 0);
		//if (_cameraWV != null)
		//	_cameraWV.GetComponent<Camera>().enabled = (cameraIndex == 1);
		
	}*/

	void Update ()
	{
		if (cameraIndex != 0)
			SwitchCamera (cameraIndex);
		else
			_cameraFP.GetComponent<Camera> ().enabled = true;
	}

	public void SwitchCamera(int cameraIndex)
	{
		if (_cameraFP != null)
			_cameraFP.GetComponent<Camera> ().enabled = false;
		for (int i = 0; i<5; i++) 
		{
			if((i+1)==cameraIndex)
				camera_group[i].GetComponent<Camera>().enabled = true;
			else
				camera_group[i].GetComponent<Camera>().enabled = false;
		}
	}
}
