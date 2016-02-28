 using UnityEngine;
using System.Collections;

public class follow_mouse : MonoBehaviour {
	private new Transform transform;
	private Transform cameraTransform;

	public new Camera camera;

	private Vector3 idealForward;
	private Vector3 forward = Vector3.forward;
	private Vector3 forwardVelocity = Vector3.zero;

	private Vector3 idealUp = Vector3.up;
	private Vector3 up = Vector3.up;
	private Vector3 upVelocity = Vector3.zero;

	public float trackingTime = 0.1f;

	public float deadzone = 5f;

	public float reach = 3f;

	private Vector3 previousMousePosition = Vector3.zero;
	// Use this for initialization
	void Start () {
		transform = GetComponent<Transform> ();

		if (camera == null) {
			Debug.LogError ("no main camera");
			enabled = false;
		} else {
			cameraTransform = camera.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 mousePosition = Input.mousePosition;
		Vector3 falcon_position = new Vector3 ((float)falcon_statemachine.GetXPos (), (float)falcon_statemachine.GetYPos (), (float)falcon_statemachine.GetZPos ());
		Vector3 mousePosition = falcon_to_mouse.FalconToMouse (falcon_position);
		Vector3 mouseDelta = previousMousePosition - mousePosition;

		if (mouseDelta.magnitude > deadzone) {
			Vector3 cursorInThreeSpace = mousePosition;
			cursorInThreeSpace.z = reach;

			idealForward = (camera.ScreenToWorldPoint(cursorInThreeSpace) - transform.position).normalized;

			Matrix4x4 cameraLocalToWorld = cameraTransform.localToWorldMatrix;

			idealUp = cameraLocalToWorld.MultiplyVector(previousMousePosition - mousePosition).normalized;

			previousMousePosition = mousePosition;
		}

		forward = Vector3.SmoothDamp (forward, idealForward, ref forwardVelocity, trackingTime);
		up = Vector3.SmoothDamp (up, idealUp, ref upVelocity, trackingTime);
		transform.rotation = Quaternion.LookRotation (forward, up);
	}
}
