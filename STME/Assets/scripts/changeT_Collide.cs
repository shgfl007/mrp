using UnityEngine;
using System.Collections;

public class changeT_Collide : MonoBehaviour {

	private GameObject controller;

	// Use this for initialization
	void Start () {
		controller = GameObject.Find ("controller");
		if (controller == null) {
			Debug.Log("controller not found");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (statemachine.switchTool) {
			//check which tool has been selected

			try
			{
				GameObject selected = statemachine.hit.gameObject;
				if(selected.name == "cutter")
				{
				//change mesh filter to cutter
				controller.GetComponent<MeshFilter>().mesh = selected.GetComponent<MeshFilter>().mesh;
				controller.transform.rotation = selected.transform.rotation;
				controller.transform.localScale = selected.transform.localScale;
				Debug.Log("change filter 3");
				change_tools.toolNum = 2;
				//set cut function to true
				isFunction.shouldFunction = true;

			}else if(selected.name == "Hand_SculptMode")
			{
				//change mesh filter to hand_sculptMode
				controller.GetComponent<MeshFilter>().mesh = selected.GetComponent<MeshFilter>().mesh;
				controller.transform.rotation = selected.transform.rotation;
				Debug.Log("change filter 1");
				change_tools.toolNum = 0;
				isFunction.shouldFunction = false;
				controller.transform.localScale = selected.transform.localScale;

			}else if(selected.name == "Hand_EditingMode")
			{
				//change mesh filter to hand_editingMode
				controller.GetComponent<MeshFilter>().mesh = selected.GetComponent<MeshFilter>().mesh;
				Debug.Log("Hand Mode 2");
				controller.transform.localScale = selected.transform.localScale;
				
				change_tools.toolNum = 3;
			}else if(selected.name == "pCylinder9")
			{
				//change mesh filter to spaculas
				controller.GetComponent<MeshFilter>().mesh = selected.GetComponent<MeshFilter>().mesh;
				controller.transform.rotation = selected.transform.rotation;
				controller.transform.rotation = Quaternion.AngleAxis(45, Vector3.forward);
				
				Debug.Log("change filter 2");
				change_tools.toolNum= 1;
				isFunction.shouldFunction = false;
				controller.transform.localScale = selected.transform.localScale;
			}
			}catch(System.Exception e)
			{
				Debug.Log("exception");
			}
		
		}
	}
}
