using UnityEngine;
using System.Collections;

public class switch_controller : MonoBehaviour {

	GameObject controller;
	GameObject _controller1;
	GameObject _controller2;
	GameObject _controller3;
	GameObject _controller4;
	GameObject _controller5;

	GameObject[] controllers;
	public int c_index;
	// Use this for initialization
	void Start () {
		controllers = new GameObject[5];

		controller = GameObject.Find ("FPSController");
		if (_controller1 == null)
			Debug.Log("Start(): FPS not found");


		_controller1 = GameObject.Find ("c0");
		if (_controller1 == null)
			Debug.Log ("Start(): c0 not found");
		else
			controllers [0] = _controller1;

		_controller2 = GameObject.Find ("c1");
		if (_controller2 == null)
			Debug.Log("Start(): c1 not found");
		else
			controllers [1] = _controller2;

		_controller3 = GameObject.Find ("c2");
		if (_controller3 == null)
			Debug.Log("Start(): c2 not found");
		else
			controllers [2] = _controller3;
		
		_controller4 = GameObject.Find ("c3");
		if (_controller4 == null)
			Debug.Log("Start(): c3 not found");
		else
			controllers [3] = _controller4;

		_controller5 = GameObject.Find ("c4");
		if (_controller5 == null)
			Debug.Log("Start(): c4 not found");
		else
			controllers [4] = _controller5;


		controller.SetActive (true);

		for (int i = 0; i<5; i++)
			controllers [i].SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		switchController (c_index);
	}

	void switchController(int index)
	{
		if (index == 0) {
			controller.SetActive (true); 
			_controller1.SetActive (false);
			_controller2.SetActive (false);
		} else {
			for(int i =0; i<5; i++)
			{

				if(i==(index-1))
					controllers[i].SetActive(true);
				else
					controllers[i].SetActive(false);
			}
		}
	}
}
