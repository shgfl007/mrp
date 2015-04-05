using UnityEngine;
using System.Collections;

// Require these components when using this script
[RequireComponent(typeof (Animator))]
[RequireComponent(typeof (CapsuleCollider))]
[RequireComponent(typeof (Rigidbody))]
public class BotControlScript : MonoBehaviour
{
	[System.NonSerialized]					
	public float lookWeight;					// the amount to transition when using head look
	
	[System.NonSerialized]
	public Transform enemy;						// a transform to Lerp the camera to during head look
	
	public float animSpeed = 1.5f;				// a public setting for overall animator animation speed
	public float lookSmoother = 3f;				// a smoothing setting for camera motion
	public bool useCurves;						// a setting for teaching purposes to show use of curves
	
	private Animator anim;							// a reference to the animator on the character
	private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
	private AnimatorStateInfo layer2CurrentState;	// a reference to the current state of the animator, used for layer 2
	private CapsuleCollider col;					// a reference to the capsule collider of the character
	
	private LeapManager leapManager;
	private LeapGuiButton btnJump;
	private LeapGuiButton btnWave;
	
	private float walkSpeed;
	private float walkDirection;
	private bool runNow;
	private bool jumpNow;
	private bool waveNow;

	static int idleState = Animator.StringToHash("Base Layer.Idle");	
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
	static int jumpState = Animator.StringToHash("Base Layer.Jump");				// and are used to check state for various actions to occur
	//static int jumpDownState = Animator.StringToHash("Base Layer.JumpDown");		// within our FixedUpdate() function below
	//static int fallState = Animator.StringToHash("Base Layer.Fall");
	//static int rollState = Animator.StringToHash("Base Layer.Roll");
	static int waveState = Animator.StringToHash("Layer2.Wave");
	
	private GameObject debugText;
	

	void Start ()
	{
		// initialising reference variables
		anim = GetComponent<Animator>();					  
		col = GetComponent<CapsuleCollider>();				
		//enemy = GameObject.Find("Enemy").transform;	
		
		if(anim.layerCount == 2)
			anim.SetLayerWeight(1, 1);
		
		leapManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LeapManager>();
		debugText = GameObject.Find("DebugText");

		GameObject objButton = GameObject.Find("BtnJump");
		btnJump = objButton ? objButton.GetComponent<LeapGuiButton>() : null;
		
		objButton = GameObject.Find("BtnWave");
		btnWave = objButton ? objButton.GetComponent<LeapGuiButton>() : null;
	}
	
	
	void FixedUpdate ()
	{
		if(leapManager != null && leapManager.IsLeapInitialized())
		{
			if(leapManager.IsPointableValid())
			{
				// get the direction around y
				float fLeapDir = leapManager.GetPointableQuat().eulerAngles.y;
				
				// walk or run
				walkSpeed = !runNow ? 0.2f : 0.5f;
				
				if(fLeapDir >= 0f && fLeapDir <= 180f)
					walkDirection = fLeapDir / 180f;
				else if(fLeapDir > 180f && fLeapDir <= 360f)
					walkDirection = -(360f - fLeapDir) / 180f;
				
				if(leapManager.IsGestureScreentapDetected())
				{
					// run
					runNow = true;
					Debug.Log(/**leapManager.GetLeapFrameCounter() + " " + */
						leapManager.GetGestureScreentapID() + " Screentap detected - run.");
				}
				else if(leapManager.IsGestureKeytapDetected())
				{
					// don´t run any more 
					runNow = false;
					Debug.Log(/**leapManager.GetLeapFrameCounter() + " " + */
						leapManager.GetGestureKeytapID() + " Keytap detected - stop running.");
				}
				else if(leapManager.IsGestureCircleDetected() ||
						(btnJump && btnJump.IsButtonPressed()))
				{
					// jump
					jumpNow = true;
					runNow = true;
					
					Debug.Log(/**leapManager.GetLeapFrameCounter() + " " + */
						leapManager.GetGestureCircleID() + " Circle detected - jump.");
				}
				else if((leapManager.IsGestureSwipeDetected() && leapManager.GetSwipeDirection() == LeapManager.SwipeDirection.Right) ||
					(btnWave && btnWave.IsButtonPressed()))
				{
					// wave
					waveNow = true;
					Debug.Log(/**leapManager.GetLeapFrameCounter() + " " + */
						leapManager.GetGestureSwipeID() + " Swipe detected - wave.");
				}
				else
				{
					string sMoveMsg = !runNow ? "Walking..." : "Running...";
					//sMoveMsg = "(" + leapManager.GetPointableID().ToString() + ") " + sMoveMsg;
					
					if(debugText && (debugText.GetComponent<GUIText>().text != sMoveMsg))
						debugText.GetComponent<GUIText>().text = sMoveMsg;
				}
				
			}
			else
			{
				if(debugText)
					debugText.GetComponent<GUIText>().text = "Stopped.";
				
				// stop
				walkSpeed = 0f;
				walkDirection = 0f;
			}
			
		}
		else
		{
			walkDirection = Input.GetAxis("Horizontal");				// setup h variable as our horizontal input axis
			walkSpeed = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
			jumpNow = Input.GetButtonDown("Jump");
			waveNow = Input.GetButtonDown("Jump");
		}
		
		anim.SetFloat("Speed", walkSpeed);					// set our animator's float parameter 'Speed' equal to the vertical input axis				
		anim.SetFloat("Direction", walkDirection); 			// set our animator's float parameter 'Direction' equal to the horizontal input axis		
		anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'
		anim.SetLookAtWeight(lookWeight);					// set the Look At Weight - amount to use look at IK vs using the head's animation
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation
		
		if(anim.layerCount == 2)		
			layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);	// set our layer2CurrentState variable to the current state of the second Layer (1) of animation
		
		
		// STANDARD JUMPING
		
		// if we are currently in a state called Locomotion (see line 25), then allow Jump input (Space) to set the Jump bool parameter in the Animator to true
		if (currentBaseState.nameHash == locoState)
		{
			if(jumpNow)
			{
				jumpNow = false;
				anim.SetBool("Jump", true);
			}
		}
		
		// if we are in the jumping state... 
		else if(currentBaseState.nameHash == jumpState)
		{
			//  ..and not still in transition..
			if(!anim.IsInTransition(0))
			{
				if(useCurves)
					// ..set the collider height to a float curve in the clip called ColliderHeight
					col.height = anim.GetFloat("ColliderHeight");
				
				// reset the Jump bool so we can jump again, and so that the state does not loop 
				anim.SetBool("Jump", false);
			}
			
			// Raycast down from the center of the character.. 
			Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
			RaycastHit hitInfo = new RaycastHit();
			
			if (Physics.Raycast(ray, out hitInfo))
			{
				// ..if distance to the ground is more than 1.75, use Match Target
				if (hitInfo.distance > 1.75f)
				{
					
					// MatchTarget allows us to take over animation and smoothly transition our character towards a location - the hit point from the ray.
					// Here we're telling the Root of the character to only be influenced on the Y axis (MatchTargetWeightMask) and only occur between 0.35 and 0.5
					// of the timeline of our animation clip
					anim.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
				}
			}
		}
		
		
		// IDLE
		
		// check if we are at idle, if so, let us Wave!
		//else if (currentBaseState.nameHash == idleState)
		{
			if(waveNow)
			{
				waveNow = false;
				anim.SetBool("Wave", true);
			}
		}
		
		// if we enter the waving state, reset the bool to let us wave again in future
		if(layer2CurrentState.nameHash == waveState)
		{
			anim.SetBool("Wave", false);
		}
	}
}
