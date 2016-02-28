using UnityEngine;
using System.Collections;

public class switch_level : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		Application.LoadLevel (1);
		Debug.Log ("change level");
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("triggered");
		if (other.tag.Equals ("Player")) {
			Application.LoadLevel(1);
		}
	}
}
