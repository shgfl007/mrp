using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class statemachine : MonoBehaviour {

	public static int state;
	public static Vector3 norm;
	public static bool paint;
	public static Collider hit;
	public Text debug;
	public static bool isSelected;
	public static bool pull;
	public static bool push;
	public static Vector3 start_point;
	private Vector3 current_point;
	public float offset;

	private MeshCollider temp;
	private Vector3 center;
	private float start_distance;

	// Use this for initialization
	void Start () {
		//initiate the state
		state = 0;
		isSelected = false;
		//norm = new Vector3[3]; 
		offset = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
		debug.text = isSelected.ToString();
		current_point = new Vector3 ((float)falcon_statemachine.GetXPos (), (float)falcon_statemachine.GetYPos (), (float)falcon_statemachine.GetZPos ());

		if (!falcon_statemachine.isButton0Down ()) {
			isSelected = false;
			hit = null;
		}

		//------- pull and push test -----------------
		//------- test only when an obj is selected ----------
		if (isSelected) {
			temp = (MeshCollider)hit;
			center = temp.bounds.center;
			float distance1 = Vector3.Distance(center, current_point);
			if(start_distance>distance1 + offset)
			{
				push=true;
				pull = false;
				debug.text = "push";
			}else if (start_distance < distance1+offset)
			{
				push = false;
				pull = true;
				debug.text = "pull";
			}
		}
		//------------------------------------------------------------
	}

	void OnCollisionEnter(Collision col)
	{
		if (state == 0) {
			state = 1;
			norm = col.contacts [0].normal;
			//debug.text = "enter function called";
		}
		if (falcon_statemachine.isButton0Down()) {
			paint = true;
			;
			if(!isSelected && col.gameObject.tag == "Sculpt_obj")
			{
				hit = col.collider;
				isSelected = true;
				start_point = new Vector3((float)falcon_statemachine.GetXPos(), (float)falcon_statemachine.GetYPos(),(float)falcon_statemachine.GetZPos());
				start_distance = Vector3.Distance(hit.bounds.center, start_point);
			}
		}
//		else {
//			//button is released
//			if(isSelected)
//				isSelected = false;
//
//			paint = false;
//			hit = null;
//		}
	}

	void OnCollisionStay(Collision col)
	{
		if (state == 1)
			norm = col.contacts [0].normal;
		if (falcon_statemachine.isButton0Down()) {
			paint = true;
			//hit = col.collider;
			if(!isSelected && col.gameObject.tag == "Sculpt_obj")
			{
				hit = col.collider;
				isSelected = true;
			}
		} else {
			paint = false;
			hit = null;
			if(isSelected)
				isSelected = false;
		}
	}

	void OnCollisionExit(Collision col) 
	{
		if (state == 1) {
			state = 0;
			//debug.text = "exit function called";
		}
		//Debug.Log ("exit function called");
//		if (!falcon_statemachine.isButton0Down ()) {
//			//button released
//			isSelected = false;
//		}
//		paint = false;
//		hit = null;
	}
}
