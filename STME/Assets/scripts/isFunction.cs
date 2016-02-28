using UnityEngine;
using System.Collections;

public class isFunction : MonoBehaviour {

	public static bool shouldFunction = false;
	public GameObject slicer;
	public GameObject barrier;
	private GameObject[] objList;
	private Vector3[] initPos;
	private int count;
	private int temp_length;
	private GameObject displayGroup;
	// Use this for initialization
	void Start () {
		objList = GameObject.FindGameObjectsWithTag("Cut_obj");
		initPos = new Vector3[objList.Length];
		temp_length = 0;
		displayGroup = GameObject.Find ("Display_group");
	}
	
	// Update is called once per frame
	void Update () {
		if (shouldFunction) {
			objList = GameObject.FindGameObjectsWithTag("Cut_obj");
			//if the list contains more than one object, tell event control that user has cut the object



			//if the list is different, update the position list
			if(temp_length != objList.Length)
			{
				initPos = new Vector3[objList.Length];
			
				for(int i = 0; i < objList.Length; i++)
				{
					initPos[i] = objList[i].transform.position;
				}
				if(objList.Length > 1)
					S3_eventControl.isCut = true;
			}

			slicer.SetActive (true);
			barrier.SetActive(true);
			temp_length = objList.Length;
			//displayGroup.SetActive(false);

			if(Input.GetKeyDown(KeyCode.Space))
			{
				for(int i = 0; i < objList.Length; i++)
				{
					objList[i].transform.position = initPos[i];
					objList[i].GetComponent<Rigidbody>().freezeRotation = true;
					objList[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
					objList[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
				}
			}else
			{
				for(int i = 0; i < objList.Length; i++)
				{
					objList[i].GetComponent<Rigidbody>().freezeRotation = false;
					objList[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
				}
			}
		} else {
			slicer.SetActive (false);
			barrier.SetActive(false);
			displayGroup.SetActive(true);
		}
	}
}
