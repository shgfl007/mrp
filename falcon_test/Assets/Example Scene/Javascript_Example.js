private var CursorVisibility : boolean = true;
var TestCoords : Vector2;

function Start () {
ScreenCursor.SetSavesLength(2); // Set the maximun save files ( it can be called at any time !! )
}
function Update () {
// WARNING ! THIS SCRIPT MAY NOT WORK IN SOME UNITY VERSIONS
// GO --> Edit -> Project Settings -> Player -> Other Settings -> Api Compability Level => .NET 2.0
// In order for any values to be correctly stored
// you will have to drag and drop the "Cursor" C# script from the Plugins folder
// in one object, it doesnt matter what object it is , it can be anything !!

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
 	ScreenCursor.LoadPosition();		// say that when we release our Right mouse click, the curor's position will be restored
 	CursorVisibility = true;			// and then it will turn visible again ( note: we do this in that frame order so no visible mouse teleporting happens )
}

if (Input.GetKeyDown(KeyCode.B)) {			// Lines 32,33
ScreenCursor.SetPosition(TestCoords.x,TestCoords.y); }// say that when we press the "B" buttonm the Cursor will go to TestCoords that were set from the inspector

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

}