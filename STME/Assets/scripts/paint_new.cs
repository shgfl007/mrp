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

	//tool set index number
	private int toolNum;
	private int hand;
	private int spatulas;
	private int cutter;
	private int hand_mode2;

	private GameObject controller;
	private GameObject targetObj;
	private float strength;
	public static RaycastHit hitObj;
	public static bool paint;
	public static bool isSculpting;
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
		//debug.text = "r is " + inRadius.ToString();
		
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	//this function is use delta time to calculate the power of manipulating the mesh
	private void  DeformMeshDelta ( Mesh mesh ,   Vector3 position ,   float power ,   float inRadius  ){
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
			
			vertices[i] += averageNormal * falloff * power;
		}
		
		mesh.vertices = vertices;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}


	void Start(){

		//assign tool index
		hand = 0;
		spatulas = 1;
		cutter = 2;
		hand_mode2 = 3;
		controller = GameObject.Find ("controller");
		isSculpting = false;
	}


	void  Update (){
		//get the falcon position --> may need to add a remapping function
		falcon_position = FalconToMouse (Falcon_Control.GetServoPos());


		//debug.text = "falcon position is " + GetServoPos ().ToString ();
		//map_debug.text = "mapped falcon position is " + falcon_position.ToString(); 
		//mouse_debug.text = Input.mousePosition.ToString ();

		toolNum = change_tools.toolNum;

		//only worry about push and pull when tool is hand
		if (toolNum == hand) {
			//-------- pull and push --------------
			if (statemachine.pull) {
				if (pull > 0) //do nothing 
					;
				else
					pull = -pull;
				//pull = 1;
			}
			if (statemachine.push) {
				if (pull > 0)
					//pull = -1;
					pull = -pull;
				//pull=-1f;
				//radius=1;
			}
			pushPull.text = pull.ToString ();
			paint = false;
		} else if (toolNum == spatulas) {
			//using spatulas can only allow users to push
			if (pull > 0)
				pull = -pull;
			//enable paint function to leave a texture mark on the obj
			//paint = true;
			pushPull.text = "spatulas!";


		} else if (toolNum == cutter) {
			//no sculpting... pass it to cut
			pushPull.text = "cutter!";
			paint = false;
			return;
		} else if (toolNum == hand_mode2) {
			pushPull.text = "hand 2!";
			// call the moving function 
			paint = false;
			if(statemachine.isSelected && !combine_mesh_click.isCombined)
			{
				GameObject head = GameObject.Find("clay_head");
				GameObject tempobj = statemachine.hit.gameObject;
				if(head == null){ pushPull.text = "clay_head not found"; return;}
				//head.transform.position = Falcon_Control.GetServoPos();
				tempobj.transform.position = Falcon_Control.GetServoPos();
				Falcon_Control.Strength = 3f;
				double[] temp = new double[3];
				temp[0] = (double)Vector3.down.x; temp[1] = (double)Vector3.down.y; temp[2] = (double)Vector3.down.z;
				Falcon_Control.norm = temp;

			}
			if(statemachine.isSelected && combine_mesh_click.isCombined)
			{
				GameObject combined = GameObject.Find("Combined");
				if(combined == null) {pushPull.text = "combined not found"; return;}
				pushPull.text = "select combined mesh";
				combined.transform.position = Falcon_Control.GetServoPos();
				Falcon_Control.Strength = 3f;
				double[] temp = new double[3];
				temp[0] = (double)Vector3.down.x; temp[1] = (double)Vector3.down.y; temp[2] = (double)Vector3.down.z;
				Falcon_Control.norm = temp;
			}
			//Falcon_Control._feedback();
			//Falcon_Control.SetForce(Falcon_Control.norm, Falcon_Control.Strength);
		}
		if (!statemachine.isSelected) {
			ApplyMeshCollider();
			paint = false;
			double[] temp  = new double[3];
			temp[0] = (double)Falcon_Control.GetXPos(); temp[1] = (double)Falcon_Control.GetYPos();
			temp[2] = (double)Falcon_Control.GetZPos();
			Falcon_Control.Strength = 0f;
			//Falcon_Control._feedback();
			return;
		}
		//---------------------------------------

		//mesh editing starts when an obj is selected
		if (statemachine.isSelected) {

			//store hit obj
			targetObj = statemachine.hit.gameObject;

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
				current_position = Falcon_Control.GetServoPos();
//					Ray ray = new Ray(current_position,GetDirection(current_position, statemachine.hit.gameObject.GetComponent<Renderer>().bounds.center));
				Ray ray = new Ray(current_position,GetDirection(current_position, statemachine.start_point));

					//RaycastHit hitObj;
				if(Physics.Raycast(ray, out hitObj, Mathf.Infinity))
				{
					isSculpting = true;
					if(toolNum == spatulas) 
					{
						paint = true; 
						strength = 1.5f;
					}else paint = false;

					mouse_debug.text = "hitobj hit point is " + hitObj.point.ToString();
					map_debug.text = "start point is " + statemachine.start_point.ToString();
					//MeshCollider temp = (MeshCollider)hitObj.collider;
					MeshCollider temp = hitObj.collider as MeshCollider;
					Vector3 tempN = hitObj.normal;
					double[] normal = new double[3];
					normal[0] = (double)tempN.x; normal[1] = (double)tempN.y; normal[2] = (double)tempN.z;
					Falcon_Control.norm = normal;
					Falcon_Control.Strength = strength;
					//Falcon_Control.SetForce(normal, (double)strength);
				}else
					isSculpting = false;

				if(toolNum == hand)
				{
					Vector3 relativePoint= filter.transform.InverseTransformPoint(statemachine.start_point);
					//DeformMesh(filter.mesh, relativePoint, pull * Time.deltaTime, radius);
					float dis = Vector3.Distance(statemachine.start_point, current_position);
					//debug.text = "r is " + radius.ToString();

					if(dis > 0.5f) 
					{
						dis = 0.5f;
						return;
					} 
					Debug.Log("distance is " + dis.ToString());
					dis = dis*pull/10f;

					radius = 0.5f;
					DeformMesh(filter.mesh, relativePoint, dis, radius);
					float r = Vector3.Distance(statemachine.start_point,current_position);
					Vector3 dir = GetDirection(statemachine.start_point, current_position);
					r = Mathf.Sqrt(r);
				}else if (toolNum == spatulas)
				{
					//sculpting mode 2: using spatulas tool --> 
					pull = -0.02f;
					//Vector3 relativePoint= filter.transform.InverseTransformPoint(statemachine.start_point);
					Vector3 relativePoint= filter.transform.InverseTransformPoint(hitObj.point);
					//DeformMesh(filter.mesh, relativePoint, pull * Time.deltaTime, radius);
					radius = 0.002f;
					//DeformMesh(filter.mesh, relativePoint, dis, radius);
					DeformMesh(filter.mesh, relativePoint,pull * Time.deltaTime ,radius);
					float r = Vector3.Distance(statemachine.start_point,current_position);
					r = Mathf.Sqrt(r);
				}
			}
			//}
		}
		//Falcon_Control._feedback();

	}
	
	void  ApplyMeshCollider (){
		if (unappliedMesh && unappliedMesh.GetComponent<MeshCollider>()) {
			unappliedMesh.GetComponent<MeshCollider>().sharedMesh = null;
			unappliedMesh.GetComponent<MeshCollider>().sharedMesh = unappliedMesh.mesh;
		}
		unappliedMesh = null;
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