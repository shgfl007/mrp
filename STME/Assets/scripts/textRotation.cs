using UnityEngine;
using System.Collections;

public class textRotation : MonoBehaviour {

	private GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = player.transform.rotation;
	}
}
