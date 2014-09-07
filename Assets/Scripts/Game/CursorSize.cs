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
		public Texture2D cur;
		// Use this for initialization
		int x = Screen.width / 10;
		int y = Screen.height / 10;
		int w = Screen.width / 16;
		int h = Screen.height / 9;
		void Start ()
		{
			this.transform.position = Vector3.zero;
			this.transform.localScale = Vector3.zero;
			this.guiTexture.pixelInset = new Rect (x, y, w, h);
			//Cursor.SetCursor(cur, Vector2.zero, CursorMode.Auto);
			//Screen.showCursor = false;
		}

//		void OnGUI()
//		{	
//			if(true)
//			{
//				GUI.DrawTextureWithTexCoords (new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, x, y),cur,new Rect(0,0,0,0));
//			}
//		}
	}
}