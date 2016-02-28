using UnityEngine;
using System.Collections;

public class combine_mesh_click : MonoBehaviour {

	private bool callPopupWindow = false;
	private Camera main;
	public Rect windowRect = new Rect(500,300,120,50);
	private bool ifCombine = false;
	public static bool isCombined = false;
	public Material newMaterialRef;
	// Use this for initialization
	void Start () {
		main = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		windowRect.center = new Vector2 (main.pixelWidth/2, main.pixelHeight/2); 
	}

	void OnGUI(){
		if (callPopupWindow && !isCombined) {
			windowRect = GUI.Window(0, windowRect, DoMyWindow, "Do you want to combine?");
			Debug.Log("GUI window");
		}
	}
	
	void DoMyWindow(int windowID){
		if (GUI.Button (new Rect (40, 50, 50, 20), "Yes") || Input.GetKeyDown(KeyCode.Return)) {
			callPopupWindow = false;
			ifCombine = true;
		}
		if (GUI.Button (new Rect (120, 50, 50, 20), "NO") || Input.GetKeyDown(KeyCode.Escape)) {
			callPopupWindow = false;
			ifCombine = false;
		}
	}

	// Update is called once per frame
	void Update () {

		if (change_tools.toolNum != 3)
			return;

		//mouse control is for non-VR settings
		//-----------------------------------------------------------------------------------------------
			//press left button on the mouse to confirm combine meshes
//			if (Input.GetMouseButtonDown (0)) {
//				Debug.Log ("mouse button down");
//				callPopupWindow = true;
//			}
		//-----------------------------------------------------------------------------------------------

		//press enter/space in vr mode to combine meshes
		if (!isCombined) {
			if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
			{
				ifCombine = true;
			}
		}

		//if combining mesh is comfirmed by the user, combine meshes
		if(ifCombine) {
			Vector3 tempPosition = GameObject.Find("clay_body").transform.position;

			GameObject[] temp = GameObject.FindGameObjectsWithTag("Sculpt_obj");
			Debug.Log("There are " + temp.Length.ToString() + " game objs with the tag 'Sculpt_obj'");
			int count = 0;
			for(int i = 0; i < temp.Length; i++)
			{
				if(temp[i].name.Contains("clay"))
					count++;
			}
			Debug.Log("count is " + count.ToString());
			if(count == 0 ) return;
			//MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(); 
			MeshFilter[] meshFilters = new MeshFilter[count];
			int p = 0;
			for(int i = 0; i < temp.Length; i++)
			{
				if(temp[i].name.Contains("clay"))
				{
					meshFilters[p] = temp[i].GetComponent<MeshFilter>();
					Debug.Log("name is " + temp[i].name);
					Debug.Log("p is " + p.ToString() + " i is " + i.ToString());
					p++;
				}
			}

			Debug.Log("there are " + meshFilters.Length.ToString() + " in meshFilters");

			int amount = meshFilters.Length;
			CombineInstance[] combine = new CombineInstance[amount];

			for (int i = 0; i < amount; i++)
			{
				if(meshFilters[i] == null) Debug.Log(i.ToString() + " is null");

				combine[i].mesh = meshFilters[i].sharedMesh;
				combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			}

			GameObject obj = new GameObject("clay_combined", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
			obj.GetComponent<MeshFilter>().mesh = new Mesh();
			obj.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine);
			obj.GetComponent<MeshRenderer>().material = newMaterialRef;
			obj.GetComponent<MeshRenderer>().sharedMaterial = new Material(meshFilters[0].gameObject.GetComponent<MeshRenderer>().sharedMaterial);
			obj.GetComponent<MeshCollider>().sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;
			obj.AddComponent<texture_test>();
			obj.tag = "Sculpt_obj";
			foreach (MeshFilter m in meshFilters)
			{
				DestroyImmediate(m.gameObject);
			}
			ifCombine = false;
			isCombined = true;
			GameObject group = GameObject.Find("Combined");
			group.transform.position = tempPosition;
			obj.transform.parent = group.transform;
		}
	}
}
