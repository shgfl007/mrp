using UnityEngine;
using System.Collections;

public class trans_camera : MonoBehaviour {

	public GameObject sp0;
	public GameObject sp1;
	public GameObject sp2;
	public GameObject sp3;
	public GameObject sp4;
	GameObject[] sp;

	public GameObject mainCamera;
	// Use this for initialization

	private Vector3 position;
	private int index=0;

	void Start () {
		position = mainCamera.transform.position;

		sp = new GameObject[5];
		sp [0] = sp0;
		sp [1] = sp1;
		sp [2] = sp2;
		sp [3] = sp3;
		sp [4] = sp4;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("1"))
			index = 1;
		if(Input.GetKeyDown("2"))
			index = 2;
		if(Input.GetKeyDown("3"))
			index = 3;
		if(Input.GetKeyDown("4"))
			index = 4;
		if(Input.GetKeyDown("5"))
			index = 5;
		
		if(index != 0)
			transCam(index-1);
	}

	void transCam(int index){
		for (int i = 0; i<5; i++) 
		{
			if(i==index)
			{
				position = sp[i].transform.position;
				mainCamera.transform.position = position;
			}

		}
	
	}

}
