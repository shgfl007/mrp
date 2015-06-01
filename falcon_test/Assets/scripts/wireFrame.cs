using UnityEngine;
using System.Collections;

public class wireFrame : MonoBehaviour {

	public bool isw = false;

	void OnPostRender()
	{
		GL.wireframe = isw;
	}

	void OnPreRender()
	{
		GL.wireframe = isw;

	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
