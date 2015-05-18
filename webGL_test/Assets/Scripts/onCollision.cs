using UnityEngine;
using System.Collections;

public class onCollision : MonoBehaviour {

	void OnCollisionEnter (Collision collision)
	{
		Debug.Log("Enter Called");
	}

	void OnCollisionStay(Collision collision)
	{
		Debug.Log ("Stay occuring...");
	}

	void OnCollisionExit (Collision collision)
	{
		Debug.Log("Exit Called..");
	}
}
