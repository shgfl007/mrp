 using UnityEngine;
using System.Collections;

public class switch_sphere : MonoBehaviour {

	private GameObject stone;
	private GameObject clay;
	private GameObject[] list;
	private GameObject cut;
	private GameObject clay_head;
	private int index;
	private bool isChanged;
	private GameObject[] cut_list;

	public static int switch_to;
	public static int current_index;
	// Use this for initialization
	void Start () {
		//put all scultable objs in "list"
		list = GameObject.FindGameObjectsWithTag ("Sculpt_obj");
		//Debug.Log ("list has " + list.Length.ToString() + " objs");
		for (int i = 0; i < list.Length; i++) {
			list[i].SetActive(false);
			Debug.Log(list[i].name);
		}
		//set the first one active
		//list [0].SetActive (true);

		//put all cutable objs in cut_list
		cut_list = GameObject.FindGameObjectsWithTag ("Cut_obj");
		//Debug.Log ("cut_list has " + cut_list.Length.ToString() + " objs");

		for (int i = 0; i < cut_list.Length; i++) {
			cut_list[i].SetActive(false);
		}
		cut_list [0].SetActive (true);

		isChanged = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1") || switch_to == 1) {
			//index = 1;
			isChanged = true;
			//current_index = 1;
			for (int i = 0; i < list.Length; i++)
			{
				if(list[i].name == "step1")
					current_index = i;
			}
			index = current_index;
		}
		if (Input.GetKeyDown ("2") || switch_to == 2) {
			//index = 2;
			isChanged = true;
			//current_index = 2;
			for (int i = 0; i < list.Length; i++)
			{
				if(list[i].name == "step2")
					current_index = i;
			}
			index = current_index;
		}
//		if (Input.GetKeyDown ("3") || switch_to == 3) {
//			index = 3;
//			isChanged = true;
//			current_index = 3;
//		}
		if (Input.GetKeyDown ("4") || switch_to == 4) {
			index = 4;
			isChanged = false;
			if(!combine_mesh_click.isCombined)
			{
				for(int i = 0; i < list.Length; i++)
				{
					if(list[i].name.Contains("clay"))
						list[i].SetActive(true);
					else
						list[i].SetActive(false);
				}
				for(int i = 0; i < cut_list.Length; i++)
				{
					cut_list[i].SetActive(false);
				}
				current_index = 4;
			}
		}
		//5 as cut stage
		if (Input.GetKeyDown ("5") || switch_to == 5) {
			for(int i = 0; i < cut_list.Length; i++)
			{
				cut_list[i].SetActive(true);
			}
			for(int i = 0; i < list.Length; i++)
			{
				list[i].SetActive(false);
			}
			isChanged = false;
			index = 5;
			current_index = 5;
		}
		if (index != 5 && index!=4&& index!=3 && isChanged)
			switchSphere (index);


		isChanged = false;
		//countSpheresByTag ("Sculpt_obj");

		//if is at cutting stage, update the list
		if (isFunction.shouldFunction) {
			cut_list = GameObject.FindGameObjectsWithTag("Cut_obj");
		}

		for (int i = 0; i < cut_list.Length; i++) {
			if(cut_list[i].activeSelf)
				Debug.Log(cut_list[i].name);
		}
	}

	void switchSphere(int index)
	{
		//set the selected sphere active
		for(int i = 0; i < list.Length; i++)
		{
			if(i == index)
			{
				list[i].SetActive(true);
				//list[i].transform.position = new Vector3(0.15f, 0.65f, -0.21f);
			}
			else //set the unselected spheres non-active
				list[i].SetActive(false);
		}

		for (int i = 0; i<cut_list.Length; i++) {
			cut_list[i].SetActive(false);
		}
		current_index = index;
	}

	void countSpheresByTag(string tag)
	{
		GameObject[] temp = GameObject.FindGameObjectsWithTag (tag);
		//Debug.Log (temp.Length.ToString () + " game objects with the " + tag);
		for (int i = 0; i < temp.Length; i++) {
			string name = temp[i].gameObject.name;
			Debug.Log(name); 
		}
	}
}
