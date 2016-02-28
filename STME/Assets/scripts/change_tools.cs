using UnityEngine;
using System.Collections;

public class change_tools : MonoBehaviour {

	public GameObject tool1; 	//hand - sculpting mode - toolNum = 0
	public GameObject tool2;	//Spaculas - push mode - toolNum = 1
	public GameObject tool3;	//Cutter - cut - toolNum = 2
	public GameObject tool4;	//hand - Editing mode - toolNum = 3
	public static int toolNum;

	private GameObject[] tools_list;
	private GameObject controller;
	private GameObject cutter_controller;

	private GameObject pivotPointS;
	private GameObject pivotPointB;

	// Use this for initialization
	void Start () {

		tools_list = new GameObject[4];
		//tools_list = GameObject.FindGameObjectsWithTag("Tool");
		if (tool1 != null && tool2 != null && tool3 != null && tool4!= null) {
			tools_list[0] = tool1;
			tools_list[1] = tool2;
			tools_list[2] = tool3;
			tools_list[3] = tool4;
		}
		toolNum = 0;
		controller = GameObject.Find("controller");
		pivotPointS = GameObject.Find ("pivotPointS");
		pivotPointB = GameObject.Find ("pivotPointB");

		//set the pivot centre 
		pivotPointB.transform.position = GameObject.Find ("clay_body").GetComponent<Collider> ().bounds.center;
		pivotPointS.transform.position = GameObject.Find ("clay_head").GetComponent<Collider> ().bounds.center;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("q")) {
			controller.GetComponent<MeshFilter>().mesh = tools_list[0].GetComponent<MeshFilter>().mesh;
			controller.transform.rotation = tools_list[0].transform.rotation;
			Debug.Log("change filter 1");
			toolNum = 0;
			isFunction.shouldFunction = false;
			controller.transform.localScale = tools_list [toolNum].transform.localScale;
		}
		if (Input.GetKeyDown ("w"))  {
			controller.GetComponent<MeshFilter>().mesh = tools_list[1].GetComponent<MeshFilter>().mesh;
			controller.transform.rotation = tools_list[1].transform.rotation;
			controller.transform.rotation = Quaternion.AngleAxis(45, Vector3.forward);

			Debug.Log("change filter 2");
			toolNum = 1;
			isFunction.shouldFunction = false;
			controller.transform.localScale = tools_list [toolNum].transform.localScale;
		}
		if (Input.GetKeyDown ("e")) {
			controller.GetComponent<MeshFilter>().mesh = tools_list[2].GetComponent<MeshFilter>().mesh;
			controller.transform.rotation = tools_list[2].transform.rotation;
			controller.transform.localScale = tools_list[2].transform.localScale;
			Debug.Log("change filter 3");
			toolNum = 2;
			//set cut function to true
			isFunction.shouldFunction = true;

		}
		if (Input.GetKeyDown ("r")) {
			controller.GetComponent<MeshFilter>().mesh = tools_list[0].GetComponent<MeshFilter>().mesh;
			Debug.Log("Hand Mode 2");
			controller.transform.localScale = tools_list[3].transform.localScale;

			toolNum = 3;

		}
		controller.GetComponent<MeshRenderer>().material = tools_list[toolNum].GetComponent<MeshRenderer>().material;
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
