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
	public static bool isSwitch;
	public static bool switchTool;
	private bool pushnpull;

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
		isSwitch = false;
		switchTool = false; pushnpull = false;
	}
	
	// Update is called once per frame
	void Update () {
		debug.text = isSelected.ToString();
		current_point = Falcon_Control.GetServoPos ();
		if (!Falcon_Control.isButton0Down ()) {
			isSelected = false;
			hit = null;
		}

		//------- pull and push test -----------------
		//------- test only when an obj is selected ----------
		if (isSelected && pushnpull) {
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

		}
		if (Falcon_Control.isButton0Down()) {
			paint = true;

			if(!isSelected && col.gameObject.tag == "Sculpt_obj")
			{
				hit = col.collider;
				isSelected = true;
				start_point = Falcon_Control.GetServoPos();
				start_distance = Vector3.Distance(hit.bounds.center, start_point);
				pushnpull = true;
			}
			if(!isSelected && col.gameObject.tag == "Display")
			{
				isSwitch = true;pushnpull = false;
				hit = col.collider;
			}
			if(!isSelected && col.gameObject.tag == "Tool")
			{
				isSwitch = false;pushnpull = false;
				switchTool = true;
				hit = col.collider;
			}
		}

	}

	void OnCollisionStay(Collision col)
	{
		if (state == 1)
			norm = col.contacts [0].normal;

		if (Falcon_Control.isButton0Down()) {
			paint = true;
			//hit = col.collider;
			if(!isSelected && col.gameObject.tag == "Sculpt_obj")
			{
				hit = col.collider;
				//isSelected = true;
			}
			if(!isSelected && col.gameObject.tag == "Display")
			{
				hit = col.collider;
				isSwitch = true;
			}
			if(!isSelected && col.gameObject.tag == "Tool")
			{
				hit = col.collider;
				switchTool = true;
			}
		} else {
			paint = false;
			hit = null;
			if(isSelected)
				isSelected = false;
			if(isSwitch)
				isSwitch = false;
			if(switchTool)
				switchTool = false;
		}
	}

	void OnCollisionExit(Collision col) 
	{
		if (state == 1) {
			state = 0;
		}
		if (isSwitch)
			isSwitch = false;
		if(switchTool)
			switchTool = false;
	}
}
