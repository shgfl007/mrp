using UnityEngine;
using System.Collections;

public class mouse_test : MonoBehaviour {
	public GameObject mouse;
	private Vector3 temp;
	public Camera mainCamera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//mouse.transform.position = new Vector3 (Input.mousePosition.x/10,Input.mousePosition.y/10, mouse.transform.position.z);
		//mouse.transform.position = Input.mousePosition;
		Debug.Log (Input.mousePosition);
		//Debug.Log (mouse.transform.position);
		temp = Input.mousePosition;
		temp.z = mouse.transform.position.z - mainCamera.transform.position.z;
		mouse.transform.position = mainCamera.ScreenToWorldPoint (temp);
	}

	void OnCollisionEntry(Collision col)
	{
		Debug.Log ("collision!!");
		Debug.Log (col.contacts [0].normal);
	}
}
