using UnityEngine;
using System.Collections;

public class switch_sphere : MonoBehaviour {

	private GameObject stone;
	private GameObject clay;
	private GameObject[] list;
	private GameObject cut;

	private int index;
	// Use this for initialization
	void Start () {
		list = new GameObject[3];

		stone = GameObject.Find ("stone");
		if (stone == null)
			print ("stone not found");
		else
			list [0] = stone;

		clay = GameObject.Find ("clay");
		if (clay == null)
			print ("clay not found");
		else
			list [1] = clay;

		clay = GameObject.Find ("cut");
		if (clay == null)
			print ("cut not found");
		else
			list [2] = clay;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1"))
			index = 1;
		if (Input.GetKeyDown ("2"))
			index = 2;
		if (Input.GetKeyDown ("3"))
			index = 3;

		if (index != 0)
			switchSphere (index-1);
	}

	void switchSphere(int index)
	{
		for(int i = 0; i < 2; i++)
		{
			if(i == index)
			{
				list[i].SetActive(true);
				list[i].transform.position = new Vector3(0.15f, 0.65f, -0.21f);
			}
			else
				list[i].SetActive(false);
		}
	}
}
