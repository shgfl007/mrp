using UnityEngine;
using System.Collections;

public class falcon_sculpt : MonoBehaviour {

	private GameObject controller;
	private double[] norm = new double[3];
	private float stiffness;
	private Vector3 falcon_pos;
	// Use this for initialization
	void Start () {
		controller = GameObject.FindGameObjectWithTag ("Controller");
		stiffness = 8f;
	}
	
	// Update is called once per frame
	void Update () {

		falcon_pos = Falcon_Control.GetServoPos ();

		if (statemachine.state == 0 && Falcon_Control.Strength > 0f) {
			Falcon_Control.Strength--;
			//Falcon_Control.Strength = 0f;
		}
		else if (statemachine.state == 1) {
			Falcon_Control.Strength = 5f;
			norm[0] = statemachine.norm.x;
			norm[1] = statemachine.norm.y;
			norm[2] = statemachine.norm.z;
		}
		
		if (statemachine.isSelected) {
			Falcon_Control.Strength = stiffness;
			//get the direction of the user's movement
			//direction = (to-from)/distance
			Vector3 temp = falcon_pos - statemachine.start_point;
			float scale = Vector3.Distance(statemachine.start_point, falcon_pos);
			temp = temp/scale;
			norm[0] = temp.x;
			norm[1] = temp.y;
			norm[2] = temp.z;
		}
		//--------------------- force feedback for slicing -----------
		if (isFunction.shouldFunction) {
			//			norm[0] = Slicer.slice_norm.x;
			//			norm[1] = Slicer.slice_norm.y;
			//			norm[2] = Slicer.slice_norm.z;
			norm[0] = 0f; norm[1] = 1f; norm[2] = 0f;
			//slicer_norm_debug.text = "slicer norm is " + Slicer.slice_norm.ToString();
		}

		//debugTxt.text = "strength is " + Strength.ToString ();
		Falcon_Control.norm = norm;
		if(!paint_new.isSculpting)
			Falcon_Control.SetForce(norm, 4f);
		else
			Falcon_Control._feedback ();
		_charaMove();
	}

	private void _charaMove() {
		//controller.transform.position = Falcon_Control.Falcon_To_Mouse ();
		controller.transform.position = falcon_pos;
	}
}
