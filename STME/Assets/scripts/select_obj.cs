using UnityEngine;
using System.Collections;

public class select_obj : MonoBehaviour {

	//this class is used to switch the model according to user's choice


	private GameObject[] display_list;

	// Use this for initialization
	void Start () {
		GameObject[] display_list = GameObject.FindGameObjectsWithTag ("Display");
		for (int i = 0; i < display_list.Length; i++) {
			Debug.Log("Display list " + i.ToString() + " is " + display_list[i].name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(statemachine.isSwitch)
		{
			//Debug.Log("switch is triggered");
			try{
			GameObject temp = statemachine.hit.gameObject;
//			GameObject test = GameObject.Find("test");
//			test.GetComponent<MeshFilter>().mesh = temp.GetComponent<MeshFilter>().mesh;
//			test.transform.localScale = temp.transform.localScale;
//			test = temp;

			if(temp.name == "001")
				switch_sphere.switch_to = 1;
			if(temp.name == "002")
				switch_sphere.switch_to = 2;
			if(temp.name == "003")
					switch_sphere.switch_to = 4;}
			catch(System.Exception e)
			{Debug.Log(e.ToString());}
		}
	}
}
