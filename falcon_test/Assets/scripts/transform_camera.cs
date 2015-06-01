using UnityEngine;
using System.Collections;

public class transform_camera : MonoBehaviour {

	public GameObject sp0;
	public GameObject sp1;
	private GameObject[] group;

	public GameObject mainCamera;

	private Vector3 position;
	private int index = 0;

	// Use this for initialization
	void Start () {
		position = mainCamera.transform.position;

		group = new GameObject[5];
		group [0] = sp0;
		group [1] = sp1;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("1"))
			index = 1;
		if (Input.GetKeyDown ("2"))
			index = 2;

		if (index != 0)
			transCam(index-1);
	
	}

	void transCam(int index){
		for (int i = 0; i<5; i++) {
			if(i == index)
			{
				position = group[i].transform.position;
				mainCamera.transform.position = position;
			}
		}
	}
}
