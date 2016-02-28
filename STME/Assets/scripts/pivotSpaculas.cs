 using UnityEngine;
using System.Collections;

public class pivotSpaculas : MonoBehaviour {

	private GameObject pivotPointS;
	private GameObject pivotPointB;
	private GameObject controller;

	private Vector3 center;
	private Vector3 tempPos;
	private Vector3 up;

	private bool isFirst = true;
	// Use this for initialization
	void Start () {
		controller = GameObject.Find("controller");
		pivotPointS = GameObject.Find ("pivotPointS");
		pivotPointB = GameObject.Find ("pivotPointB");
		
		//set the pivot centre 
		pivotPointB.transform.position = GameObject.Find ("clay_body").GetComponent<Collider> ().bounds.center;
		pivotPointS.transform.position = GameObject.Find ("clay_head").GetComponent<Collider> ().bounds.center;

		tempPos = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (isFirst) {
			Vector3 temp = controller.transform.up;
			up = controller.transform.rotation * Vector3.forward;
			//up.y = 1f;up.x = 1f;
			up = up.normalized;
			Debug.Log("temp up is " + temp.ToString() + ". up is " + up.ToString());
			isFirst = false;
		}
		//check if the current tool is the spactulas and an obj is selected
//		if (change_tools.toolNum == 1 && statemachine.isSelected) {
//			//set the center point, so that the tool can be facing the center point all the time
//			if(statemachine.hit.gameObject.name.Equals("clay_head"))
//			{
//				center = pivotPointS.transform.position;
//			}else if(statemachine.hit.gameObject.name.Equals("clay"))
//			{
//				center = pivotPointB.transform.position;
//			}else if(statemachine.hit.gameObject.name.Equals("clay_combined"))
//			{
//				center = GameObject.Find("Combined").transform.position;
//			}
//			Vector3 falcon_pos = Falcon_Control.GetServoPos();
//
//		
//		} else
//			return;

		//rotate around y axis  
		Vector3 falcon_pos = Falcon_Control.GetServoPos ();
		Vector3 dir1 = falcon_pos - center;
		Vector3 dir2;
		
		if(tempPos == Vector3.zero)
		{
			tempPos = falcon_pos;
		}
		
		dir2 = tempPos - center;
		
		float angle = Vector3.Angle (dir2, dir1);

		//set up the pivot point
		if (change_tools.toolNum == 1 && switch_sphere.current_index == 2) {
			center = pivotPointS.transform.position;
			controller.transform.parent = pivotPointS.transform;
		} else if (change_tools.toolNum == 1 && switch_sphere.current_index == 3) {
			center = pivotPointB.transform.position;
			controller.transform.parent = pivotPointB.transform;
		} else if (change_tools.toolNum == 1 && switch_sphere.current_index == 4) {
			if(combine_mesh_click.isCombined)
				center = GameObject.Find("Combined").transform.position;
			else
				center = pivotPointB.transform.position;
		}


		Debug.Log ("angle is " + angle.ToString());
		Debug.Log ("center is " + center.ToString());
		//if(angle > 1f)
//		if (controller.transform.parent == pivotPointB)
//			pivotPointB.transform.RotateAround (center, Vector3.up, angle);
//		else if (controller.transform.parent == pivotPointS)
//			pivotPointS.transform.RotateAround (center, Vector3.up, angle);
		//controller.transform.parent = 

		//point to the center if the x position is lower than the center
		if (controller.transform.position.x < center.x) {
			Debug.Log("under center");

		}

		//rotate around y-axis
		controller.transform.RotateAround (center, Vector3.up, angle);

		//LookAt function
		//controller.transform.LookAt (center,up);


		//save last falcon position
		tempPos = falcon_pos;
	}


}
