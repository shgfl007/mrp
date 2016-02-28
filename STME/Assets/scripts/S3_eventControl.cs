using UnityEngine;
using System.Collections;

public class S3_eventControl : MonoBehaviour {

	private int state = -1;

	public static bool isCut;
	private float minutes;
	private float seconds;
	private bool isFirstTime;
	// Use this for initialization
	void Start () {
		//start state = -1
		state = -1;
		isFirstTime = true;
		minutes = 0f; seconds = 3f;
		timer.isTimeUp = false; timer.Seconds = seconds; timer.Minutes = minutes;
		Dialogue_s3.dialogue_index = state;
	}
	
	// Update is called once per frame
	void Update () {
		//start state, state = -1 and timer count down 
		//state = -1 init state
		//state = 0 intro 
		//state = 1 default dialogue/cut stage
		//state = 2 cut to shaping intro 
		//state = 3 basic shaping
		//state = 4 detail sculpting
		//state = 5 combine 

		//check if it is the first time
		if (isFirstTime) {
			//intro
			if (state == -1 && timer.isTimeUp) {
				Dialogue_s3.dialogue_index = 1;
				minutes = 0f;
				seconds = 3f;
				timer.Seconds = seconds;
				timer.Minutes = minutes;
				timer.isTimeUp = false;
				state = 0;
			}
			//asking the user to cut clay
			else if (state == 0 && timer.isTimeUp) {
				Dialogue_s3.dialogue_index = 2;
				//isFirstTime = false;
				//set to default game control description
				state = 1;

				//set the timer 
				seconds = 5f; minutes = 0f;
				timer.Seconds = seconds; timer.Minutes = minutes;
				timer.isTimeUp = false;
			}else if(state == 1 && timer.isTimeUp)
			{
				Dialogue_s3.dialogue_index = 0;

				isFirstTime = false;
				seconds = 30f; minutes = 0f; 
				timer.Seconds = seconds; timer.Minutes = minutes;
				timer.isTimeUp = false;
			}
		} else {
			//not the first time senerio

			// if it is not the first time, have the default dialogue bubble when no event is triggered
			if(state == 1)
			{
				Dialogue_s3.dialogue_index = 0;
			}

			//------------------------- main events ------------------------------------------------------
			//dealing with the events
			if (!isCut && timer.isTimeUp && state == 1)
			{
				//user hasn't cut the obj within the preset time slot
				Dialogue_s3.dialogue_index = 4;
			}

			//user either cut the obj or choose to skip
			if((isCut || switch_sphere.switch_to == 2) && state == 1)
			{
				state = 2;
				Dialogue_s3.dialogue_index = 3;

				if(switch_sphere.current_index != 1)
					switch_sphere.switch_to = 1;

				seconds = 5f; minutes = 0f;
				timer.Seconds = seconds; timer.Minutes = minutes;
				timer.isTimeUp = false;
			}
			if(state == 2 && timer.isTimeUp)
			{
				Dialogue_s3.dialogue_index = 5;

				seconds = 40f; minutes = 0f;
				timer.Seconds = seconds; timer.Minutes = minutes;
				timer.isTimeUp = false;
				state = 3;
			}

			//either time is up for basic shaping or user choose to skip
			if((state == 3 && timer.isTimeUp) || (state == 3 && switch_sphere.switch_to == 2))
			{
				state = 4;
				Dialogue_s3.dialogue_index = 6;

				seconds = 40f; minutes = 0f;
				timer.Seconds = seconds; timer.Minutes = minutes;
				timer.isTimeUp = false;
			}

			//either time is up for detail scupting or user choose to skip
			if((state == 4 && timer.isTimeUp) || (state == 4 && switch_sphere.switch_to == 3))
			{
				state = 5;
				Dialogue_s3.dialogue_index = 7;

				if(switch_sphere.switch_to != 3)
					switch_sphere.switch_to = 3;
			}
			//---------------------- end of main events section -----------------------------------

			//---------------------- wrong tool events --------------------------------------------
			//cutting stage
			if(state == 1 && !isCut && change_tools.toolNum != 2)
			{
				 //if is using any other tool than the cutter
					Dialogue_s3.dialogue_index = 12;
			}
			else if(state == 3 && change_tools.toolNum != 0)
			{
				//basic shaping stage

				//user is using the spatulas
				if(change_tools.toolNum == 1)
					Dialogue_s3.dialogue_index = 9;
				else if (change_tools.toolNum == 2) //user is using cutter
					Dialogue_s3.dialogue_index = 8;
				else if (change_tools.toolNum == 3) //user is using hand mode 2
					Dialogue_s3.dialogue_index = 11; //need to change this
			}
			else if (state == 4 && change_tools.toolNum != 1)
			{
				//detail stage
				if(change_tools.toolNum == 0)
					Dialogue_s3.dialogue_index = 6;
				else if(change_tools.toolNum == 2)
					Dialogue_s3.dialogue_index = 8;
				else if (change_tools.toolNum == 3)
					Dialogue_s3.dialogue_index = 11;
			}
			else if (state == 5 && change_tools.toolNum != 3)
			{
				Dialogue_s3.dialogue_index = 10;
			}

			if(state == 5 && combine_mesh_click.isCombined)
				Dialogue_s3.dialogue_index = 13;
		}
	}
}
