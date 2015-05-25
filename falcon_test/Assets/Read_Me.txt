Welcome to the "Set Cursor Position" Unity Asset.

Summary:
"Set Cursor Position" is an Unity Plugin that gives you the ability to control the actual position of your Cursor in your screen via Scripting.
[NEW] You can now use keyboard to perform mouse clicks !!
There are tons of creative ways to use it and implement it to your games/programms.
The package comes with an Example Scene and a script to help you start and understand its use.
Can be combined with Unity's current API to achieve great effects !
NOTE : It has been tested on Windows Unity Pro and Indie, other platforms are unknown!
So if you purchase for another platform do it at your own risk !!

What you actually get:
 ~New Scripting API.
  -ScreenCursor.SetPosition (int,int);
  -ScreenCursor.SetLocalPosition (int,int);
  -ScreenCursor.GetPosition ();
  -ScreenCursor.GetLocalPosition ();
  -ScreenCursor.SetSavesLength (int);
  -ScreenCursor.SavePosition ();
  -ScreenCursor.SavePosition (int);
  -ScreenCursor.LoadPosition ();
  -ScreenCursor.LoadPosition (int);
  -ScreenCursor.SimulateMove (Vector2,float);
  -ScreenCursor.SimulateMove (Vector2,float,bool);
  -ScreenCursor.SimulateAutoMove (Vector2,float);
  -ScreenCursor.SimulateLocalMove (Vector2,float);
  -ScreenCursor.SimulateLocalMove (Vector2,float,bool);
  -ScreenCursor.SimulateAutoLocalMove (Vector2,float);
  -ScreenCursor.SimulateController (float,float);
  -ScreenCursor.SimulateController (float,float,float);
  -ScreenCursor.SimulateSmoothController (float,float,float,float);
  -ScreenCursor.LeftClick();
  -ScreenCursor.LeftClickDown();
  -ScreenCursor.LeftClickUp();
  -ScreenCursor.RightClick();
  -ScreenCursor.RightClickDown();
  -ScreenCursor.RightClickUp();
  -ScreenCursor.LeftClickEquals(KeyCode);
  -ScreenCursor.RightClickEquals(KeyCode);
  
 ~Added: Cursor can be set to go
  outside of the unity window !!

Unity uselful API:
 -Screen.lockCursor : boolean;
 -Screen.showCursor : boolean
 -Input.GetKey(KeyCode.Mouse0) // Mouse1 and Mouse2 and so on...
 -Input.GetKeyDown(KeyCode.Mouse0)
 -Input.GetKeyUp(KeyCode.Mouse0) 
and many more...

Example Uses:
Simulate mouse movement with external controllers like the ones from PS3 or Xbox.
Rotate a character with a mouse button and when u press it down turn the cursor invisible and store its position so u can turn it visible and load its position when u release the button.
Lock the cursor in a desired position not only in the center by constantly calling ScreenCursor.SetCursorPosition() every frame or so.
Make the cursor teleport in random coordinates.
and many more, just use your imagination and creativity...

How to install the plugin:
 The first thing you may want to do is use it , but you will have to make a few steps before beeing able to do so.
1.Open Unity and go to Edit -> Project Settings -> Player -> Other Settings -> Api Compability Level and set it to ".NET 2.0"
2.Drag the "ScreenCursor" C# script located inside of Plugins folder and drop it on any form of gameobject inside your scene.
(2.1 Warning: Do not deactivate the script. Drop it once , having multiple "Cursor" scripts inside one scene is not supported.)
3.Do not change the location of the System.Drawing or System.Windows.Forms files located inside the Plugins folder.
4.If you dont already know, if you change the location of "ScreenCursor" C# script you wont be able to call any of the API above from any UnityScript !
5.For more accurate installation guide read the Installation_Guide.txt

How to correctly use it:
---------------------------------------------------------------
"ScreenCursor.SetPosition (int,int);"
Is a function that can be called at any time and requires 2 integers ( int for short ) one for the X coordinate and one for the Y coordinate of the screen.
Once its called the Cursor will automaticaly teleport to the coordinates that was beeing told to go.
---------------------------------------------------------------
"ScreenCursor.GetPosition ();"
Is a function that can be called at any time and returns the real cursor values related to your screen and not unity.
---------------------------------------------------------------
"ScreenCursor.SetLocalPosition (int,int);"
Is a function that can be called at any time and requires 2 integers ( int for short ) one for the X coordinate and one for the Y coordinate of the Unity-local screen.
Once its called the Cursor will automaticaly teleport to the coordinates that was beeing told to go.
---------------------------------------------------------------
"ScreenCursor.GetLocalPosition ();"
Same as Input.mousePosition.
---------------------------------------------------------------
"ScreenCursor.SetSavesLength (int);"
Is one very basic function to be called if the user wants to use "ScreenCursor.SavePosition (int);" or "ScreenCursor.LoadPosition (int);"
What it actualy does is to tell the system to create as many Save Slots as the number defined in the brackets.
That's why it requires 1 integer ( int for short ) to determine the save slots length.
If you know from the biggining how many save slots you should determine it from the biggining but the new system is automated now so it doesnt really matter :P
Note: it can be called as many times as you want and at any time although it is recomended to call it once at the Start () function.
--------------------------------------------------------------
"ScreenCursor.SavePosition (int);"
Is a function that can be called at any time and requires 1 integers ( int for short ) that determines the Save Slot you want to save.
Save Slots are beeing created by the use of "ScreenCursor.SetSavesLength (int);" (Read Above)
In other words this function will Store the current mouse position so it can be used later with "ScreenCursor.LoadPosition (int);" (Read Below)
Overwriting an already saved slot is possible and supported !!
--------------------------------------------------------------
"ScreenCursor.SavePosition();"
It is used as a QuickSave default int value is 0 (zero).
--------------------------------------------------------------
"ScreenCursor.LoadPosition (int);"
Is a function that can be called at any time and requires 1 integers ( int for short ) that determines the Save Slot you want to load.
Save Slots are beeing created by the use of "ScreenCursor.SetSavesLength (int);" (Read Above)
In other words this function will Load/Teleport the Cursor to the position that was stored in this Save Slot by "ScreenCursor.SavePosition (int);" (Read Above)
So it is important to call first "ScreenCursor.SavePosition (int);" and then "ScreenCursor.LoadPosition (int);" or else the default position will be loaded ( 0,0 ).
Loading the same saved position more than one times is possible and supported !!
--------------------------------------------------------------
"ScreenCursor.LoadPosition();"
It is used as a QuickLoad default int value is 0 (zero).
--------------------------------------------------------------
"SimulateMove (Vector2,float,bool)"
Is a function that can be called at any time and requires one Vector2 as the final destination in Global screen pixels, one float as the speed value that determines the speed
of the cursor, one boolean that determines whether the cursor should persist no matter what to go to its final position and stop until it does so or if the function
should be a one time only meaning that you will have to call it in some form of update (see the example).
Once its called the Cursor will start moving toward the coordinates that was beeing told to go.
--------------------------------------------------------------
"SimulateMove (Vector2,float)"
Same as SimulateMove but as default the bool is set to false;
--------------------------------------------------------------
"SimulateAutoMove (Vector2,float)"
Same as SimulateMove but as default the bool is set to true;
--------------------------------------------------------------
"SimulateLocalMove (Vector2,float,bool)"
Is a function that can be called at any time and requires one Vector2 as the final destination in Unity-local screen pixels, one float as the speed value that determines the speed
of the cursor, one boolean that determines whether the cursor should persist no matter what to go to its final position and stop until it does so or if the function
should be a one time only meaning that you will have to call it in some form of update (see the example).
Once its called the Cursor will start moving toward the coordinates that was beeing told to go.
--------------------------------------------------------------
"SimulateLocalMove (Vector2,float)"
Same as SimulateLocalMove but as default the bool is set to false;
--------------------------------------------------------------
"SimulateAutoLocalMove (Vector2,float)"
Same as SimulateLocalMove but as default the bool is set to true;
--------------------------------------------------------------
"SimulateController (float,float,float)" --> [RECOMMENDED]
Is a function that can be called at any time and requires 3 float values one float as the horizontal input , on as the vertical input and one as the speed value that
determines the speed of the cursor. This function should be called once every frame to work and is frame dependent (see the example).
--------------------------------------------------------------
"SimulateSmoothController (float,float,float)" --> [NOT RECOMMENDED]
Is like the SimulateController function but a filter is applyied that doent make any visual difference and neither does improve the performance so its not recomended to use this function.
Use the SimulateController instead !!
--------------------------------------------------------------
"LeftClick()"
A fully left mouse click.
--------------------------------------------------------------
"LeftClickDown()"
A left click down - useful for dragging stuff or drag-drop with the keyboard.
--------------------------------------------------------------
"LeftClickUp()"
A left click release.
--------------------------------------------------------------
"RightClick()"
A fully right mouse click.
--------------------------------------------------------------
"RightClickDown()"
A right click down.
--------------------------------------------------------------
"RightClickUp()"
A right click release.
--------------------------------------------------------------
"LeftClickEquals(KeyCode)"
Binds the given keycode as a left mouse. In other words when you press that keycode a left mouseclick will happen, when you release it a left mouse release will happen.
--------------------------------------------------------------
"RightClickEquals(KeyCode)"
Binds the given keycode as a right mouse. In other words when you press that keycode a right mouseclick will happen, when you release it a right mouse release will happen.
--------------------------------------------------------------
--------------------------------------------------------------

Missing .dll files:
 There are supposed to be 2 .dll files in the Plugins folder , in case the .dll files are missing you can get them from
 ...Unity/Editor/Data/Mono/lib/mono/2.0/ in your computer and then copy the files named "System.Windows.Forms.dll"
, "System.Drawing.dll" and paste them inside the Plugins folder in your unity Project directory !!
These 2 .dll files are vital and the asset will not work without them !

Conctact Info :
Email : DarknessBlade.Original@gmail.com
Youtube : https://www.youtube.com/DarknessBladeOrigin
Forum Thread: http://forum.unity3d.com/threads/242832-Official-Set-Cursor-Position?p=1606714#post1606714