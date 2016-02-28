using UnityEngine;
using System.Collections;

public class dialogue_s2 : MonoBehaviour {

	private GameObject[] dialogues;

	public static int dialogueIndex = 0;
	// Use this for initialization
	void Start () {
		dialogues = GameObject.FindGameObjectsWithTag("dialogue");
		for (int i = 0; i < dialogues.Length; i++) {
			Debug.Log(dialogues[i].name);
			dialogues[i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (dialogueIndex == 1) {
			for (int i = 0; i < dialogues.Length; i++) {
				if (dialogues [i].name == "001")
					dialogues [i].SetActive (true);
				else
					dialogues [i].SetActive (false);
			}
		} else if (dialogueIndex == 2) {
			for (int i = 0; i < dialogues.Length; i++) {
				if (dialogues [i].name == "002")
					dialogues [i].SetActive (true);
				else
					dialogues [i].SetActive (false);
			}
		} else if (dialogueIndex == 3) {
			for (int i = 0; i < dialogues.Length; i++) {
				if (dialogues [i].name == "003")
					dialogues [i].SetActive (true);
				else
					dialogues [i].SetActive (false);
			}
		} else if (dialogueIndex == 4) {
			for (int i = 0; i < dialogues.Length; i++) {
				if (dialogues [i].name == "004")
					dialogues [i].SetActive (true);
				else
					dialogues [i].SetActive (false);
			}
		} else {
			for(int i = 0; i < dialogues.Length; i++)
				dialogues[i].SetActive(false);
		}
	}
}
