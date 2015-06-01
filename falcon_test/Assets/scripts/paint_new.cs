using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class paint_new : MonoBehaviour {

	public float radius;
	public float pull= 10.0f;
	private MeshFilter unappliedMesh;

	public enum FallOff { Gauss, Linear, Needle }
	private int fallOff= (int)FallOff.Gauss;
	private float sqrMagnitude;
	private float distance;
	private float falloff;
	private Ray ray;
	private RaycastHit hit;
	public Text debug;
	public Text mouse_debug;
	public Text map_debug;
	private Vector3 falcon_position;
	private Vector3 current_position;
	public Text pushPull;
	private bool isSculptable;

	static float  LinearFalloff ( float distance  ,   float inRadius  ){
		return Mathf.Clamp01(1.0f - distance / inRadius);
	}
	
	static float  GaussFalloff ( float distance  ,   float inRadius  ){
		return Mathf.Clamp01 (Mathf.Pow (360.0f, -Mathf.Pow (distance / inRadius, 2.5f) - 0.01f));
	}
	
	static float NeedleFalloff ( float dist ,   float inRadius  ){
		return -(dist*dist) / (inRadius * inRadius) + 1.0f;
	}
	
	private void  DeformMesh ( Mesh mesh ,   Vector3 position ,   float power ,   float inRadius  ){
		Vector3[] vertices= mesh.vertices;
		Vector3[] normals= mesh.normals;
		float sqrRadius= inRadius * inRadius;
		
		// Calculate averaged normal of all surrounding vertices	
		Vector3 averageNormal= Vector3.zero;
		for (int i=0;i<vertices.Length;i++)
		{
			sqrMagnitude= (vertices[i] - position).sqrMagnitude;
			// Early out if too far away
			if (sqrMagnitude > sqrRadius)
				continue;
			
			distance= Mathf.Sqrt(sqrMagnitude);
			falloff= LinearFalloff(distance, inRadius);
			averageNormal += falloff * normals[i];
		}
		averageNormal = averageNormal.normalized;
		
		// Deform vertices along averaged normal
		for (int i=0;i<vertices.Length;i++)
		{
			sqrMagnitude = (vertices[i] - position).sqrMagnitude;
			// Early out if too far away
			if (sqrMagnitude > sqrRadius)
				continue;
			
			distance = Mathf.Sqrt(sqrMagnitude);
			switch (fallOff)
			{
			case (int)FallOff.Gauss:
				falloff = GaussFalloff(distance, inRadius);
				break;
			case (int)FallOff.Needle:
				falloff = NeedleFalloff(distance, inRadius);
				break;
			default:
				falloff = LinearFalloff(distance, inRadius);
				break;
			}
			
			//vertices[i] += averageNormal * falloff * power;
			vertices[i] += averageNormal * power * falloff ;
		}
		
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	void  Update (){
		//get the mapped position of falcon
		falcon_position = FalconToMouse (GetServoPos());

		debug.text = "falcon position is " + GetServoPos ().ToString ();
		//map_debug.text = "mapped falcon position is " + falcon_position.ToString(); 
		//mouse_debug.text = Input.mousePosition.ToString ();



		//-------- pull and push --------------
		if (statemachine.pull) {
//			if(pull>0) //do nothing 
//				;
//			else
//				pull = -pull;
			pull = 1;
		}
		if (statemachine.push) {
			if(pull>0) pull = -1;
				//pull = -pull;
				//pull=-1f;
			//radius=1;
		}
		pushPull.text = pull.ToString ();

		if (!statemachine.isSelected) {
			ApplyMeshCollider();
			return;
		}
		//---------------------------------------

		if (statemachine.isSelected) {

			MeshFilter filter = statemachine.hit.GetComponent<MeshFilter>();	
			if (filter) 
			{
					// Don't update mesh collider every frame since physX
					// does some heavy processing to optimize the collision mesh.
					// So this is not fast enough for real time updating every frame
					if (filter != unappliedMesh)
					{
						ApplyMeshCollider();
						unappliedMesh = filter;
					}
					
					// Deform mesh
					//Vector3 relativePoint= filter.transform.InverseTransformPoint(hit.point);
					current_position = new Vector3((float)falcon_statemachine.GetXPos(), (float)falcon_statemachine.GetYPos(), (float)falcon_statemachine.GetZPos());
//					Ray ray = new Ray(current_position,GetDirection(current_position, statemachine.hit.gameObject.GetComponent<Renderer>().bounds.center));
					Ray ray = new Ray(current_position,GetDirection(current_position, statemachine.start_point));

					RaycastHit hitObj;
					if(Physics.Raycast(ray, out hitObj, Mathf.Infinity))
					{
						Debug.Log ("hit!!!!!!!!!!");
						mouse_debug.text = "hitobj hit point is " + hitObj.point.ToString();
						map_debug.text = "start point is " + statemachine.start_point.ToString();
						MeshCollider temp = (MeshCollider)hitObj.collider;
						pushPull.text = "material is " + temp.material.name;
						if(temp.material.name.Contains("clay")) 
						{
							radius = 0.5f;//dis = dis/10f;
							falcon_statemachine.stiffness = 3f;
						}
						else if(temp.material.name.Contains("stone")) 
						{
							radius = 0.2f;//dis = dis/15f;
						falcon_statemachine.stiffness = 5f;
						}
					}
					//Vector3 relativePoint= filter.transform.InverseTransformPoint(current_position);
					//Vector3 relativePoint= filter.transform.InverseTransformPoint(falcon_position);
					//Vector3 relativePoint= filter.transform.InverseTransformPoint(hitObj.point);
				Vector3 relativePoint= filter.transform.InverseTransformPoint(statemachine.start_point);
				//DeformMesh(filter.mesh, relativePoint, pull * Time.deltaTime, radius);
				float dis = Vector3.Distance(statemachine.start_point, current_position);
				debug.text = "r is " + radius.ToString();


				dis = dis*pull/10f;
				DeformMesh(filter.mesh, relativePoint, dis, radius);
				float r = Vector3.Distance(statemachine.start_point,current_position);
				r = Mathf.Sqrt(r);
					//DeformMesh(filter.mesh, relativePoint, r, radius);
					//DeformMesh(filter.mesh, current_position, pull * Time.deltaTime, radius);
			}
			//}
		}
		

	}
	
	void  ApplyMeshCollider (){
		if (unappliedMesh && unappliedMesh.GetComponent<MeshCollider>()) {
			unappliedMesh.GetComponent<MeshCollider>().sharedMesh = null;
			unappliedMesh.GetComponent<MeshCollider>().sharedMesh = unappliedMesh.mesh;
		}
		unappliedMesh = null;
	}
	//void  Test (){}

	Vector3 GetServoPos() {
		return new Vector3((float)new_falcon_test.GetXPos(), (float)new_falcon_test.GetYPos(), -(float)new_falcon_test.GetZPos());
	}

	Vector3 FalconToMouse(Vector3 position)
	{
		float height = (float)Screen.height;
		float width = (float)Screen.width;

		float min = -2f;
		float max = 2f;

		float old_range = max - min;
		float new_x = ((position.x - min) * width) / old_range;
		float new_y = ((position.y - min) * height) / old_range;
		float new_z = ((position.z - min) * 8f) / old_range;

		//float new_x = Mathf.Lerp (0f, width, position.x);

		return new Vector3 (new_x, new_y, new_z);

	}

	Vector3 GetDirection(Vector3 from, Vector3 to){
		Vector3 dir;
		Vector3 temp = to - from;
		float distance = temp.magnitude;
		dir = temp / distance;
		//dir = (to - from).normalized;

		return dir;
	}
}