using UnityEngine;
using System.Collections;

public class switch_sphere : MonoBehaviour {

	GameObject sp0;
	GameObject sp1;
	GameObject sp2;
	GameObject sp3;
	GameObject sp4;
	GameObject[] sp;

	public int index;
	// Use this for initialization
	void Start () {
		sp = new GameObject[5];

		sp0 = GameObject.Find("sph0");
		if (sp0 == null)
			print ("sph0 not found");
		else
			sp [0] = sp0;

		sp1 = GameObject.Find("sph1");
		if (sp1 == null)
			print ("sph1 not found");
		else
			sp [1] = sp1;

		sp2 = GameObject.Find("sph2");
		if (sp2 == null)
			print ("sph2 not found");
		else
			sp [2] = sp2;

		sp3 = GameObject.Find("sph3");
		if (sp3 == null)
			print ("sph3 not found");
		else
			sp [3] = sp3;

		sp4 = GameObject.Find("sph4");
		if (sp4 == null)
			print ("sph4 not found");
		else
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
			switchSphere(index-1);
	}

	void switchSphere(int index)
	{
		for (int i = 0; i<5; i++) 
		{
			if(i==index)
			{
				sp[i].SetActive(true);
				sp[i].transform.position = new Vector3(3,2,1);
			}
			else
				sp[i].SetActive(false);
		}
	}
}
