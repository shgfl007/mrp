﻿using UnityEngine;
using System.Collections;

public class change_tools : MonoBehaviour {

	public GameObject tool1;
	public GameObject tool2;
	public GameObject tool3;
	public static int toolNum;

	private GameObject[] tools_list;
	private GameObject controller;
	private GameObject cutter_controller;
	// Use this for initialization
	void Start () {
		tools_list = new GameObject[3];
		if (tool1 != null && tool2 != null && tool3 != null) {
			tools_list[0] = tool1;
			tools_list[1] = tool2;
			tools_list[2] = tool3;
		}
		toolNum = 0;
		controller = GameObject.Find("controller");
		cutter_controller = GameObject.Find("Cutter");
		cutter_controller.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("q")) {
			controller.GetComponent<MeshFilter>().mesh = tools_list[0].GetComponent<MeshFilter>().mesh;
			Debug.Log("change filter 1");
			toolNum = 0;
			isFunction.shouldFunction = false;
			controller.SetActive(true);
			cutter_controller.SetActive(false);
		}
		if (Input.GetKeyDown ("w")) {
			controller.GetComponent<MeshFilter>().mesh = tools_list[1].GetComponent<MeshFilter>().mesh;
			Debug.Log("change filter 2");
			toolNum = 1;
			isFunction.shouldFunction = false;
			controller.SetActive(true);
			cutter_controller.SetActive(false);
		}
		if (Input.GetKeyDown ("e")) {
			controller.GetComponent<MeshFilter>().mesh = tools_list[2].GetComponent<MeshFilter>().mesh;
			Debug.Log("change filter 3");
			toolNum = 2;
			isFunction.shouldFunction = true;
			controller.SetActive(false);
			cutter_controller.SetActive(true);
		}
		controller.transform.localScale = tools_list [toolNum].transform.localScale;
		for(int i = 0; i < tools_list.Length; i++)
		{	
			//if tool is not been chosen, show the tool
			if(i != toolNum)
				tools_list[i].SetActive(true);
			else
				tools_list[i].SetActive(false);
		}
	}
}
