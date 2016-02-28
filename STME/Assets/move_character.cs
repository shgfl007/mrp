using UnityEngine;
using System.Collections;

public class move_character : MonoBehaviour {

	private GameObject character;
	private CharacterController controller;
	public float speed = 1f;
	private bool isAutoMove = false;
	// Use this for initialization
	void Start () {
		character = GameObject.FindGameObjectWithTag ("Player");
		Debug.Log (character.name);
		controller = character.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {

		//press space/enter to enable/disable automove
		if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown (KeyCode.Space)) {
			if(isAutoMove)
				isAutoMove = false;
			else
				isAutoMove = true;
		}
		//character.transform.Translate (Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * speed);
		if(isAutoMove)
			controller.SimpleMove (Vector3.right);
	}
}
