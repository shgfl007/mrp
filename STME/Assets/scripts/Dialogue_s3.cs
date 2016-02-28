using UnityEngine;
using System.Collections;

public class Dialogue_s3 : MonoBehaviour {

	private GameObject[] dialogues;
	public static int dialogue_index;

	// Use this for initialization
	void Start () {
		dialogues = GameObject.FindGameObjectsWithTag ("dialogue");
		for (int i = 0; i < dialogues.Length; i++) {
			Debug.Log(dialogues[i].name);
			dialogues[i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (dialogue_index.ToString());
		for (int i = 0; i < dialogues.Length; i++) {
			if(dialogues[i].name == dialogue_index.ToString())
				dialogues[i].SetActive(true);
			else
				dialogues[i].SetActive(false);
		}
	}
}
