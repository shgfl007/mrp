using UnityEngine;
using System.Collections;

public class drawLine : MonoBehaviour {
	public float offsetX = 0.01f;
	public float offsetY = 0.01f;
	public Texture texture;
	private Vector2 temp;
	private Vector2 temp_trace1;
	private Vector2 temp_trace2;
	private Vector2 temp_trace3;
	private Renderer renderer;
	private Renderer temp_rend;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if (!Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit))
			return;
		
		renderer = hit.transform.GetComponent<Renderer> ();
		print (hit.ToString());
		MeshCollider col = hit.collider as MeshCollider;
		if (renderer == null || renderer.sharedMaterial == null || renderer.sharedMaterial.mainTexture == null || col == null) {
			if(renderer == null) print("renderer null");
			if(renderer.sharedMaterial == null) print("shared material null");
			if(renderer.sharedMaterial.mainTexture == null) print("main texture null");
			if(col == null) print("mesh collider null");

			print("null stuff");
			return;
		}
		
		Texture2D tex = (Texture2D)renderer.material.mainTexture;
		Vector2 pixelUV = hit.textureCoord;
		Vector2 trace1 = pixelUV;
		Vector2 trace2 = pixelUV;
		Vector2 trace3 = pixelUV;
		trace1.x += offsetX; trace1.y += offsetY;
		trace2.x -= offsetX; trace2.y -= offsetY;
		trace3.x -= offsetX; trace3.y -= offsetY;trace3.x -= offsetX; trace3.y -= offsetY;
		Vector2 pUV = hit.textureCoord2;
		Debug.Log ("coord is " + pixelUV.ToString());

		pixelUV.x *= tex.width;
		pixelUV.y *= tex.height;
		trace1.x *= tex.width; trace1.y *= tex.height;
		trace2.x *= tex.width; trace2.y *= tex.height;
		trace3.x *= tex.width; trace3.y *= tex.height;
		pUV.x *= tex.width;
		pUV.y *= tex.height;
		Debug.Log ("UV cord is " + pixelUV.ToString());

//		if (count == 0)
//			temp = pixelUV;
//		else if (count == 5)
//			count = 0;

		DrawLine (tex, (int)temp.x, (int)temp.y, (int)pixelUV.x, (int)pixelUV.y, Color.black);
		DrawLine (tex, (int)temp_trace1.x, (int)temp_trace1.y, (int)trace1.x, (int)trace1.y, Color.black);
		DrawLine (tex, (int)temp_trace2.x, (int)temp_trace2.y, (int)trace2.x, (int)trace2.y, Color.black);
		DrawLine (tex, (int)temp_trace3.x, (int)temp_trace3.y, (int)trace3.x, (int)trace3.y, Color.black);
		//tex.SetPixel ((int)pixelUV.x, (int)pixelUV.y, Color.black);
		//tex.SetPixel ((int)pUV.x, (int)pUV.y, Color.black);
		//tex.SetPixel ((int)trace1.x, (int)trace1.y, Color.black);
		//tex.SetPixel ((int)trace2.x, (int)trace2.y, Color.black);
		tex.Apply ();
		//count++;
		temp = pixelUV; temp_trace1 = trace1; temp_trace2 = trace2;temp_trace3 = trace3;
		temp_rend = renderer;
	}
	
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
	void OnApplicationQuit()
	{
		//temp_rend.material.mainTexture = texture;
	}
} 
