using UnityEngine;
using System.Collections;

public class isFunction : MonoBehaviour {

	public static bool shouldFunction = false;
	public GameObject slicer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (shouldFunction)
			slicer.SetActive (true);
		else
			slicer.SetActive (false);
	}
}
