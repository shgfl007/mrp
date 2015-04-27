using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class statemachine : MonoBehaviour {

	public static int state;
	public static Vector3 norm;

	public Text debug;
	// Use this for initialization
	void Start () {
		//initiate the state
		state = 0;
		//norm = new Vector3[3];
	}
	
	// Update is called once per frame
	void Update () {
		debug.text = state.ToString ();
	}

	void OnCollisionEnter(Collision col)
	{
		if (state == 0) {
			state = 1;
			norm = col.contacts [0].normal;
			//debug.text = "enter function called";
		}
	}

	void OnCollisionStay(Collision col)
	{
		if (state == 1)
			norm = col.contacts [0].normal;
	}

	void OnCollisionExit(Collision col) 
	{
		if (state == 1) {
			state = 0;
			debug.text = "exit function called";
		}
		Debug.Log ("exit function called");
	}
}
