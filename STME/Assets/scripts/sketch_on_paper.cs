using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sketch_on_paper : MonoBehaviour {
	public Texture texture;
	private Renderer renderer;
	private bool Draw = false;
	private Vector2 temp;
	public Text draw;
	private Camera main;
	// Use this for initialization
	void Start () {
		main = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Draw = sketch_test.isDraw;
		//Debug.Log ("draw? " + Draw.ToString());
		draw.text = "draw? " + Draw.ToString ();
		if (!Draw) {
			temp = Vector2.zero; 
			return;
		}
//		if (!Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit))
//			return;
		//Vector3 falcon_position = new Vector3 ((float)falcon_sketch.GetXPos (), (float)falcon_sketch.GetYPos (), -(float)falcon_sketch.GetZPos ());
		//Vector3 mapped = Falcon_Control.Falcon_To_Mouse();
		Vector3 mapped = sketch_test.FalconToMouse (Falcon_Control.GetServoPos());
		//Debug.Log ("maped is " + mapped.ToString ());
		Vector3 mouse = Input.mousePosition;
		if (!Physics.Raycast (Camera.main.ScreenPointToRay (mapped), out hit))
			return;

		renderer = hit.transform.GetComponent<Renderer> ();
		MeshCollider col = hit.collider as MeshCollider;
		if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || col == null) {
			//Debug.Log("draw return");
			return;
		}
		
		Texture2D tex = (Texture2D)renderer.material.mainTexture;
		Vector2 pixelUV = hit.textureCoord;
		Vector2 pUV = hit.textureCoord2;
	
		//Debug.Log ("coord is " + pixelUV.ToString());
		//Debug.Log ("coord2 is " + pUV.ToString());
		
		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;

		//Debug.Log ("UV cord is " + pixelUV.ToString());
		//		tex.SetPixel ((int)pixelUV.x, (int)pixelUV.y, Color.red);
		//		tex.SetPixel ((int)trace1.x, (int)trace1.y, Color.red);
		//		tex.SetPixel ((int)trace2.x, (int)trace2.y, Color.red);
		if (temp.Equals(Vector2.zero)) {
			temp = pixelUV;
			draw.text = "temp is " + temp.ToString ();
		} else
			;

		DrawLine (tex, (int)temp.x, (int)temp.y, (int)pixelUV.x, (int)pixelUV.y, Color.black);
		//draw.text = "temp is " + temp.ToString ();
		//tex.SetPixel ((int)pixelUV.x, (int)pixelUV.y, Color.black);
		//tex.SetPixel ((int)pUV.x, (int)pUV.y, Color.black);
		//tex.SetPixel ((int)trace1.x, (int)trace1.y, Color.black);
		//tex.SetPixel ((int)trace2.x, (int)trace2.y, Color.black);
		tex.Apply ();
		//count++;
		temp = pixelUV;

	}

//	void OnCollisionEnter(Collision col)
//	{
//		if (col.gameObject.tag == "Controller") {
//			Draw = true;
//		}
//	}
//
//	void OnCollisionStay(Collision col)
//	{
//		if (col.gameObject.tag == "Controller" && !Draw)
//			Draw = true;
//	}
//
//	void OnCollisionExit(Collision col)
//	{
//		if (col.gameObject.tag == "Controller" && Draw)
//			Draw = false;
//	}

	void DrawLine (Texture2D text, int x1, int y1, int x2, int y2, Color col)
	{
		int dy = (int)(y2 - y1);
		int dx = (int)(x2 - x1);
		int stepx, stepy;
		
		if (dy < 0) {
			dy = -dy;
			stepy = -1;
		} else
			stepy = 1;
		
		if (dx < 0) {
			dx = -dx;
			stepx = -1;
		} else
			stepx = 1;
		dx <<= 1; dy <<= 1;
		
		float fraction = 0;
		
		text.SetPixel (x1, y1, col);
		if (dx > dy) {
			fraction = dy - (dx >> 1);
			while (Mathf.Abs(x1-x2)>1) {
				if (fraction >= 0) {
					y1 += stepy;
					fraction -= dx; 
				}
				x1 += stepx;
				fraction += dy;
				text.SetPixel (x1, y1, col);
			}
		} else {
			fraction = dx - (dy >> 1);
			while (Mathf.Abs(y1-y2) >1) {
				if (fraction >= 0)
				{
					x1+= stepx;
					fraction -= dy;
				}
				y1 += stepy;
				fraction += dx;
				text.SetPixel(x1,y1,col);
			}
		}
		
		
	}
	
} 
