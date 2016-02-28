using UnityEngine;
using System.Collections;

public class timer : MonoBehaviour {

	public static float Seconds = 59;
	public static float Minutes = 0;

	public static bool isTimeUp;
	// Use this for initialization
	void Start () {
		isTimeUp = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Seconds <= 0) {
			Seconds = 59;
			if (Minutes >= 1) {
				Minutes--;
				isTimeUp = false;
			} else {
				Minutes = 0;
				Seconds = 0;
				isTimeUp = true;
			}
		} else {
			Seconds -= Time.deltaTime;
			isTimeUp = false;
		}
	}
}
