/// <summary>
/// Cursor size.
/// Use this on game object with GUITexture to set its size according to screen resolution.
/// \author: Milan Doležal
/// </summary>
using UnityEngine;
using System.Collections;

namespace Game
{
	public class CursorSize : MonoBehaviour {
		public Texture2D cursorNormal;
		public Texture2D cursorDrag;

		private Texture2D currentCursor;
		// Use this for initialization
		private int x = (Screen.width / 10)/3;
		private int y = (Screen.height / 10)/2;
		private int w = Screen.width / 16;
		private int h = Screen.height / 9;
		void Start ()
		{
			if(GameObject.Find("MouseCursor(Clone)") != this.gameObject)
				Destroy(this.gameObject);
			this.transform.position = new Vector3 (0, 0, 1000);
			//this.transform.localScale = Vector3.zero;
			//this.guiTexture.pixelInset = new Rect (x, y, w, h);
			currentCursor = cursorNormal;
#if !UNITY_ANDROID
			Screen.showCursor = false;
#endif
			//Cursor.SetCursor(cur, Vector2.zero, CursorMode.Auto);
			//Screen.showCursor = false;
		}

		void Update()
		{
			//this.guiTexture.pixelInset = new Rect(Input.mousePosition.x-x,Input.mousePosition.y-y,w,h);
			
			if(Input.GetMouseButtonDown(0))
				currentCursor = cursorDrag;
			
			if(Input.GetMouseButtonUp(0))
				currentCursor = cursorNormal;
		}

//		void OnGUI()
//		{	
//			if(true)
//			{
//				GUI.DrawTextureWithTexCoords (new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, x, y),cur,new Rect(0,0,0,0));
//			}
//		}

		void OnGUI()
		{
			if(currentCursor)
				GUI.DrawTexture (new Rect (Input.mousePosition.x - x, Screen.height - Input.mousePosition.y - y, w, h), currentCursor);
		}
		
		void OnLevelWasLoaded(int level)
		{
			currentCursor = cursorNormal;
		}
	}
}