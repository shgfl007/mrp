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
		buttonNum = falcon_statemachine.GetButtonsDown ();
		debug_text.text = buttonNum.ToString ();

		if (r_obj != null) {
			temp = r_obj;
		}
		falcon_position = new Vector3 ((float)falcon_statemachine.GetXPos(), (float)falcon_statemachine.GetYPos(), (float)falcon_statemachine.GetZPos());
		//go through the list see if the falcon is in active zone 
		for (int i = 0; i < length; i++) {
			Bounds bounds = sculpt_objs[i].GetComponent<MeshCollider>().bounds;
			center = bounds.center;
			float distance = Vector3.Distance(center, falcon_position);
			debug_text.text = distance.ToString();
			float r = Vector3.Distance(bounds.min, center);
			if(distance <= (r + threshold))
			{
				isSelected = true;
				//check if the conroller is in a new active zone
				//if(temp!=r_obj)
				r_obj = sculpt_objs[i];
				break;
			}
		}


		//The following lines use 2 buttons to rotate
//		if (statemachine.isSelected) {
//			//get the object to rotate
//			r_obj = statemachine.hit.gameObject;
//		} else {
//			//set r_obj to null
//			r_obj = null;
//		}


		//button 2 down, rotating left
		if (falcon_statemachine.isButton1Down()) {
			if(r_obj != null)
			r_obj.transform.Rotate(Vector3.up * 1.5f);
			debug_text.text = "rotate left";
		} 
		//button 3 down, zoom in/out
		else if (falcon_statemachine.isButton2Down()) {
			if(r_obj != null)
				r_obj.transform.Rotate(Vector3.right * 1.5f);
			debug_text.text = "rotate up";
		}
		//button 4 down, rotating right
		else if (falcon_statemachine.isButton3Down()) {
			if(r_obj != null)
				r_obj.transform.Rotate(Vector3.up * -1.5f);
			debug_text.text = "rotate right";
		}
	}
}
