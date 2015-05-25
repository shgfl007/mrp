using UnityEngine;
using System.Collections;
public class Csharp_Example : MonoBehaviour {
Vector2 RandomVector2;
bool CursorVisibility = true;
public Vector2 TestCoords;
string X;
string Y;
	void Start () {
		ScreenCursor.SetSavesLength(2); // Set the maximun save files ( it can be called at any time but its recommended to be done at start or awake!! )
	}
	void Update () {
		
		if (!CursorVisibility) {				// Lines 14,15,16 
			Cursor.visible = false;				// say that when the "CursorVisibility" variable is true/false also hide/show the cursor
		} else { Cursor.visible = true; }	// with Unity's way of doing it.
		
		if (Input.GetKey(KeyCode.Q)) {			// Lines 18,19,20
			Screen.lockCursor = true; 				// say that when we press and hold the "Q" buttonm the Cursor will be locked and stay invisible
		} else { Screen.lockCursor = false; }	// in the center of the screen with Unity's way of doing it.
		
		
		if (Input.GetKeyDown(KeyCode.Mouse1)) { // Lines 23,24,25
			CursorVisibility = false;			// say that when we pres down our Right mouse click, the cursor will turn invisible
			ScreenCursor.SavePosition();		// and its position will be stored in a variable
		}
		if (Input.GetKeyUp(KeyCode.Mouse1)) {	// Lines 27,28,29
			ScreenCursor.LoadPosition();				// say that when we release our Right mouse click, the curor's position will be restored
			CursorVisibility = true;			// and then it will turn visible again ( note: we do this in that frame order so no visible mouse teleporting happens )
		}
		
		if (Input.GetKeyDown(KeyCode.B)) {			// Lines 32,33
			ScreenCursor.SetPosition((int) TestCoords.x,(int) TestCoords.y); }// say that when we press the "B" buttonm the Cursor will go to TestCoords that were set from the inspector
		
		if (Input.GetKeyDown(KeyCode.C)) {			// Lines 35,36 do exacly as above but with random coordinates whenever we press "C"
			ScreenCursor.SetPosition(Random.Range(0,Screen.width),Random.Range(0,Screen.height)); }
		
		if (Input.GetKeyDown("1")) {			// Lines 38,39 Save first position when we press "1"
			ScreenCursor.SavePosition(1); }
		
		if (Input.GetKeyDown("2")) {			// Lines 41,42 Save second position when we press "2"
			ScreenCursor.SavePosition(2); }
		
		if (Input.GetKeyDown("3")) {			// Lines 44,45 Load first position when we press "3"
			ScreenCursor.LoadPosition(1); }
		
		if (Input.GetKeyDown("4")) {			// Lines 47,48 Load second position when we press "4"
			ScreenCursor.LoadPosition(2); }
		
		if (float.TryParse(X,out TestCoords.x)) {}
		if (float.TryParse(Y,out TestCoords.y)) {}
		// WARNING !
		// Read the InstalationGuide.txt and the ReadMe.txt
				
		if (Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.U)) {
			RandomVector2 = new Vector2(Random.Range(0,Screen.width),Random.Range(0,Screen.height));
		}
		if (Input.GetKey(KeyCode.H)) {  //Press H to simulate an automatic random cursor move
			ScreenCursor.SimulateAutoMove(RandomVector2,0.2f);
		}
		if (Input.GetKey(KeyCode.U)) {  //Press U to simulate a manual random cursor move
			ScreenCursor.SimulateMove(RandomVector2,0.2f);
		}
		if (Input.GetKeyDown(KeyCode.P)) {  //Press P to store cursor position as a Vector2
			TestCoords = ScreenCursor.GetPosition(); }
		
		if (Input.GetKeyDown(KeyCode.Z)) {
			ScreenCursor.LeftClick(); //Translate "Z" keydown to a full left mouse click
		}
		if (Input.GetKeyDown(KeyCode.X)) {
			ScreenCursor.RightClick(); //Translate "X" keydown to a full right mouse click
		}
		
		ScreenCursor.LeftClickEquals(KeyCode.Space); //Defines the "Space" key as a left mouse
	}
	
	void OnGUI () {
		GUI.Label(new Rect(5,0,80,20),"X :");
		X = GUI.TextArea(new Rect(20,0,80,20),""+TestCoords.x);
		GUI.Label(new Rect(100,0,80,20),"Y :");
		Y = GUI.TextArea(new Rect(115,0,80,20),""+TestCoords.y);
		GUI.Label(new Rect(5,30,800,20),"Only numbers above or else values will return Zero");
		
		if ( GUI.Button(new Rect(200,0,100,20),"Set Cursor")) {
			ScreenCursor.SetPosition((int) TestCoords.x,(int) TestCoords.y); 
		}
	}
}