using UnityEngine;
using System.Collections;

public class switch_dialogue : MonoBehaviour {
	private GameObject[] dialogues;
	private int count = 0;
	private float temp_timer;
	private bool isTimeUp = false;
	// Use this for initialization
	void Start () {
		dialogues = GameObject.FindGameObjectsWithTag("dialogue");
		for (int i = 0; i < dialogues.Length; i++) {
			//Debug.Log ("name is " + dialogues [i].name);
			dialogues[i].SetActive(false);
		}
		timer.Seconds = 3;
		temp_timer = 500f;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("count is " + count.ToString());
		//Debug.Log ("is time up " + isTimeUp.ToString ());
		if(timer.isTimeUp)
		{
			//show speech bubble 1
			if (count == 0) {
				for (int i = 0; i < dialogues.Length; i++) {
					if(dialogues[i].name == "001")
						dialogues[i].SetActive(true);
					else
						dialogues[i].SetActive(false);
				}
				//temp_timer = 500f;
				timer.Seconds = 3;
				timer.isTimeUp = false;
				count=1;
				Debug.Log("count 1");
			}

			//show speech bubble 2
			else if(count == 1){
				for(int i = 0; i < dialogues.Length; i++){
					if(dialogues[i].name == "002")
						dialogues[i].SetActive(true);
					else
						dialogues[i].SetActive(false);
				}
				//temp_timer = 500f;
				timer.Seconds = 3;
				timer.isTimeUp = false;
				count=2;
				Debug.Log("count 2");
			}

			//show speech bubble 3
			else if(count == 2){
				for(int i = 0; i < dialogues.Length; i++){
					if(dialogues[i].name == "003")
						dialogues[i].SetActive(true);
					else
						dialogues[i].SetActive(false);
				}
				//temp_timer = 500f;
				timer.Seconds = 3;
				timer.isTimeUp = false;
				count=0;
			}
		}
		temp_timer--;
		if (temp_timer == 0)
			isTimeUp = true;
		else
			isTimeUp = false;
	}
}
