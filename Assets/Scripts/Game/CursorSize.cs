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

		// Use this for initialization
		void Start () {
			this.transform.position = Vector3.zero;
			this.transform.localScale = Vector3.zero;
			int x = Screen.width / 100;
			int y = Screen.height / 100;
			int w = 10;
			int h = 10;
			this.guiTexture.pixelInset = new Rect (x, y, w, h);
		}
	}
}