using UnityEngine;
using System.Collections;

public class LeapDebugCamera : MonoBehaviour
{
	public float smooth = 3f;
	
	
	void FixedUpdate ()
	{
		LeapManager leapManager = LeapManager.Instance;
		
		if(leapManager && leapManager.GetHandID() != 0)
		{
			Vector3 targetPos = leapManager.GetHandPos() * leapManager.DisplayFingerScale + leapManager.DisplayFingerPos;
			//targetPos.y += 1f;
			targetPos.z -= 4f;
			
			transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smooth);
		}
		
	}
}
