using UnityEngine;
using System.Collections;

public class s2_eventControl : MonoBehaviour {

	private int state;
	private float minutes;
	private float seconds;

	// Use this for initialization
	void Start () {
		state = 0;

		minutes = 0f;
		seconds = 3f;
		timer.Seconds = seconds;
		timer.Minutes = minutes;
	}
	
	// Update is called once per frame
	void Update () {
		//init state, 2 dialogues needed
		// 3 seconds in between and leave the second bubble until the next state
		if (state == 0 && timer.isTimeUp) {
			dialogue_s2.dialogueIndex = 1;
			seconds = 3f;
			minutes = 0f;
			timer.Seconds = seconds;
			timer.Minutes = minutes;
			timer.isTimeUp = false;
			state++;
		} else if (state == 1 && timer.isTimeUp) {
			dialogue_s2.dialogueIndex = 2;
			seconds = 15f;
			minutes = 0f;
			timer.Seconds = seconds;
			timer.Minutes = minutes;
			timer.isTimeUp = false;
			state++;
		}
		//state 2 => event 1
		//timers goes 1 min 15 sec
		else if (state == 2 && timer.isTimeUp) {
			dialogue_s2.dialogueIndex = 3;
			seconds = 10f;
			minutes = 0f;
			timer.Seconds = seconds;
			timer.Minutes = minutes;
			timer.isTimeUp = false;
			state++;
		}
		//state 3 => event 2
		else if (state == 3 && timer.isTimeUp) {
			dialogue_s2.dialogueIndex = 4;
			seconds = 3f;
			minutes = 0f;
			timer.Seconds = seconds;
			timer.Minutes = minutes;
			timer.isTimeUp = false;
			state++;
		}
		//state 4 => to next scene
		else if (state == 4 && timer.isTimeUp) {
			skip_level.isSkip = true;
			Debug.Log("state 4 ready to skip");
			Application.LoadLevel(2);
		}
	}
}
