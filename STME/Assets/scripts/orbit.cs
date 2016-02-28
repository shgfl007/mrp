using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class orbit : MonoBehaviour {
	public Text debug_text;
	public float threshold = 10;

	private int buttonNum;
	private float angle;
	private GameObject r_obj;
	private Vector3 falcon_position;
	private GameObject[] sculpt_objs;
	private int length;
	private Vector3 center;
	private Collider select;
	private bool isSelected;
	private GameObject temp;
	private float[] distances;
	// Use this for initialization
	void Start () {
		//angle = transform.eulerAngles;
		//save all the sculptable object to a list
		sculpt_objs = GameObject.FindGameObjectsWithTag ("Sculpt_obj");
		length = sculpt_objs.Length;

	}
	
	// Update is called once per frame
	void Update () {
		sculpt_objs = GameObject.FindGameObjectsWithTag ("Sculpt_obj");
		length = sculpt_objs.Length;

		buttonNum = Falcon_Control.GetButtonsDown ();
		debug_text.text = buttonNum.ToString ();

		if (r_obj != null) {
			temp = r_obj;
		}
		falcon_position = Falcon_Control.GetServoPos ();
		//go through the list see if the falcon is in active zone 
		for (int i = 0; i < length; i++) {
			Bounds bounds;
			try
			{
				bounds = sculpt_objs[i].GetComponent<MeshCollider>().bounds;
				center = bounds.center;
				float distance = Vector3.Distance(center, falcon_position);
				debug_text.text = distance.ToString();
				float r = Vector3.Distance(bounds.min, center);
				if(distance <= (r + threshold))
				{
					isSelected = true;
					//check if the conroller is in a new active zone
					//if(temp!=r_obj)
					if(sculpt_objs[i].name.Contains("combined"))
						r_obj = sculpt_objs[i].transform.parent.gameObject;
					else
						r_obj = sculpt_objs[i];
					break;
				}
			} catch (System.Exception e)
			{
				return;
			}
		}



		//button 2 down, rotating left
		if (Falcon_Control.isButton1Down()) {
			if(r_obj != null)
			{
				if(r_obj.name == "step2")
					r_obj.transform.Rotate(Vector3.up * 1.5f);
				else
					r_obj.transform.Rotate(Vector3.forward * 1.5f);
			}
			debug_text.text = "rotate left";
		} 
		//button 3 down, zoom in/out
		else if (Falcon_Control.isButton2Down()) {
			if(r_obj != null)
				r_obj.transform.Rotate(Vector3.right * 1.5f);
			debug_text.text = "rotate up";
		}
		//button 4 down, rotating right
		else if (Falcon_Control.isButton3Down()) {
			if(r_obj != null)
			{
				if(r_obj.name == "step2")
					r_obj.transform.Rotate(Vector3.up * -1.5f);
				else
					r_obj.transform.Rotate(Vector3.forward * -1.5f);
			}
			debug_text.text = "rotate right";
		}
	}
}
