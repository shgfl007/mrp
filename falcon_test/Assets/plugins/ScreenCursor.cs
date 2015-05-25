using UnityEngine;
using System.Collections;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

public class ScreenCursor : MonoBehaviour {
[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
public static extern void mouse_event(uint dwFlags, uint cButtons, uint dwExtraInfo);
public static ScreenCursor Self;
public static int SavesLength;
public static Point[] Positions;
public static float Simulating;
public static Vector2 Simulation;
public static Vector2 WindowOffset;
public static float OffSet;
public static float ExtraAccurancy;
public static bool TriggeredLeft;
//============== Awake
void Awake () {
//--------------------------------------------------------------------------------------------------------- Initialize values
Self = this;
Positions = new Point[0];
WindowOffset = new Vector2(System.Windows.Forms.Cursor.Position.X - Input.mousePosition.x,System.Windows.Forms.Cursor.Position.Y - (UnityEngine.Screen.height-Input.mousePosition.y));
}
//============== Update
void Update () {
//--------------------------------------------------------------------------------------------------------- Calculate WindowOffset
WindowOffset = new Vector2(System.Windows.Forms.Cursor.Position.X - Input.mousePosition.x,System.Windows.Forms.Cursor.Position.Y - (UnityEngine.Screen.height-Input.mousePosition.y));	
//--------------------------------------------------------------------------------------------------------- Stabilize values
if (Simulating == 0) {
if (OffSet != 0) {OffSet = 0;}
if (Simulation != Vector2.zero) {Simulation = Vector2.zero;}
if (ExtraAccurancy != 0) {ExtraAccurancy =0;}
}
//--------------------------------------------------------------------------------------------------------- Recognize Saves Length
if (SavesLength != Positions.Length) {
SavesLength = Positions.Length; }
//--------------------------------------------------------------------------------------------------------- SimulateSmoothMove Automatically		
if (Simulating != 0) {
float destinationX = Mathf.Lerp(System.Windows.Forms.Cursor.Position.X,Simulation.x,Simulating+ExtraAccurancy);
float destinationY = Mathf.Lerp(System.Windows.Forms.Cursor.Position.Y,Simulation.y,Simulating+ExtraAccurancy);
System.Windows.Forms.Cursor.Position = new Point((int)destinationX,(int)destinationY);
if (Vector2.Distance(new Vector2(System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y) ,Simulation) <= 10) {
ExtraAccurancy = 1; }
if (Vector2.Distance(new Vector2(System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y) ,Simulation) <= 1) {
Simulating = 0;	}
}}
//============== Others
//--------------------------------------------------------------------------------------------------------- SetPosition
public static void SetPosition (int x,int y) {
System.Windows.Forms.Cursor.Position = new Point(x,y); }
//--------------------------------------------------------------------------------------------------------- SetLocalPosition
public static void SetLocalPosition (int x,int y) {
System.Windows.Forms.Cursor.Position = new Point(x+(int)WindowOffset.x,y+(int)WindowOffset.y); }
//--------------------------------------------------------------------------------------------------------- GetPosition
public static Vector2 GetPosition () {
return new Vector2(System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y); }
//--------------------------------------------------------------------------------------------------------- GetLocalPosition
public static Vector2 GetLocalPosition () {
return Input.mousePosition; }
//--------------------------------------------------------------------------------------------------------- SavePosition[]
public static void SavePosition () {
SavePosition(0); }
public static void SavePosition (int SaveNumber) {
if (SaveNumber < SavesLength) {
if (SaveNumber >= 0) {
Positions[SaveNumber] = System.Windows.Forms.Cursor.Position;
} else {
Debug.LogError ("The requested Save file cant be below 0 (zero)."); }
} else {
Point[] TempPositions = Positions;
Positions = new Point[SaveNumber+1];
	for(int Temp = 0; Temp < TempPositions.Length; Temp++) {
	Positions[Temp] = TempPositions[Temp];
	}
Positions[SaveNumber] = System.Windows.Forms.Cursor.Position;
Debug.Log ("The requested Save file exceeded the number of defined saves ! Error Fixed."); }
}
//--------------------------------------------------------------------------------------------------------- LoadPosition[]
public static void LoadPosition () {
LoadPosition(0); }	
public static void LoadPosition (int LoadNumber) {
if (LoadNumber < SavesLength) {
if (LoadNumber >= 0) {
System.Windows.Forms.Cursor.Position = Positions[LoadNumber];
} else {
Debug.LogError ("The requested Load file cant be below 0 (zero)."); }				
} else {
System.Windows.Forms.Cursor.Position = new Point(0,0);
Debug.Log ("The requested Load file exceeds the number of defined saves ! Using default position at (0,0)."); }
}
//--------------------------------------------------------------------------------------------------------- SetSavesLength
public static void SetSavesLength (int Saves) {
if (Saves >= SavesLength) {
Point[] tempPositions = Positions;
Positions = new Point[Saves+1];
	for(int temp = 0; temp < tempPositions.Length; temp++) {
	Positions[temp] = tempPositions[temp];
	}
} else {
if (Saves < 0) {
Debug.LogError ("Cursor.SavesLength cant be less than 0 (zero) !");
} else {
Debug.LogError ("There are already more Save slots than "+ Saves +", Current slots: "+SavesLength +"."); }
}}
//--------------------------------------------------------------------------------------------------------- SimulateSmoothMove Manually
public static void SimulateAutoMove (Vector2 FinalPosition,float Speed) {
SimulateMove(FinalPosition,Speed,true);}
public static void SimulateMove (Vector2 FinalPosition,float Speed) {
SimulateMove(FinalPosition,Speed,false);}
public static void SimulateMove (Vector2 FinalPosition,float Speed,bool Automaticaly) {
if (!Automaticaly) {
Point CursorCurrentPosition = System.Windows.Forms.Cursor.Position;
float DestinationX = Mathf.Lerp(CursorCurrentPosition.X,FinalPosition.x,Speed);
float DestinationY = Mathf.Lerp(CursorCurrentPosition.Y,FinalPosition.y,Speed);
System.Windows.Forms.Cursor.Position = new Point((int)DestinationX,(int)DestinationY);
} else {
OffSet = 0;
Simulation = FinalPosition;
Simulating = Speed;
}}
//--------------------------------------------------------------------------------------------------------- SimulateSmoothLocalMove Manually
public static void SimulateAutoLocalMove (Vector2 FinalPosition,float Speed) {
SimulateLocalMove(FinalPosition,Speed,true);}
public static void SimulateLocalMove (Vector2 FinalPosition,float Speed) {
SimulateLocalMove(FinalPosition,Speed,false);}
public static void SimulateLocalMove (Vector2 FinalPosition,float Speed,bool Automaticaly) {
FinalPosition += WindowOffset;
if (!Automaticaly) {
Point CursorCurrentPosition = System.Windows.Forms.Cursor.Position;
float DestinationX = Mathf.Lerp(CursorCurrentPosition.X,FinalPosition.x,Speed);
float DestinationY = Mathf.Lerp(CursorCurrentPosition.Y,FinalPosition.y,Speed);
System.Windows.Forms.Cursor.Position = new Point((int)DestinationX,(int)DestinationY);
} else {
OffSet = 0;
Simulation = FinalPosition;
Simulating = Speed;
}}
//--------------------------------------------------------------------------------------------------------- SimulateController
public static void SimulateController (float Horizontal,float Vertical) {
System.Windows.Forms.Cursor.Position = new Point((int)(System.Windows.Forms.Cursor.Position.X+Horizontal),(int)(System.Windows.Forms.Cursor.Position.Y-Vertical));
}
public static void SimulateController (float Horizontal,float Vertical,float Speed) {
System.Windows.Forms.Cursor.Position = new Point((int)(System.Windows.Forms.Cursor.Position.X+(Horizontal*Speed)),(int)(System.Windows.Forms.Cursor.Position.Y+(Vertical*-Speed)));
}

public static void SimulateSmoothController (float Horizontal,float Vertical,float Speed,float SpeedScale) {
float horizontal = Mathf.Lerp(System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.X+(Horizontal*Speed),SpeedScale);
float vertical = Mathf.Lerp(System.Windows.Forms.Cursor.Position.Y,System.Windows.Forms.Cursor.Position.Y+(Vertical*-Speed),SpeedScale);
System.Windows.Forms.Cursor.Position = new Point((int)(horizontal),(int)(vertical));
}
//------------------------------------------------------------------------------------------------------- Trigger Clicks
public static void LeftClick() {
	mouse_event(0x02 | 0x04, 0, 0);
}
public static void LeftClickDown() {
	mouse_event(0x02, 0, 0);
}
public static void LeftClickUp() {
	mouse_event(0x04, 0, 0);
}
public static void RightClick() {
	mouse_event(0x08 | 0x10, 0, 0);
}
public static void RightClickDown() {
	mouse_event(0x08, 0, 0);
}
public static void RightClickUp() {
	mouse_event(0x10, 0, 0);
}
public static void LeftClickEquals(KeyCode KeycodeId) {
if (Input.GetKeyDown(KeycodeId)) { mouse_event(0x02, 0, 0);}
if (Input.GetKeyUp(KeycodeId)) { mouse_event(0x04, 0, 0);}
}
public static void RightClickEquals(KeyCode KeycodeId) {
if (Input.GetKeyDown(KeycodeId)) { mouse_event(0x08, 0, 0);}
if (Input.GetKeyUp(KeycodeId)) { mouse_event(0x10, 0, 0);}
}}