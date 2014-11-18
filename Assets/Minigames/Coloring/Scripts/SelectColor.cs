using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class SelectColor : MonoBehaviour {

		public GameObject Brush;
		public Texture cursorTexture;
		public LevelManagerColoring thisLevelManager;

		void OnMouseDown () {
			if(thisLevelManager.painting)
			{
				Color purple = new Color(0.5f, 0, 0.9f);
				Color orange = new Color(1.0f, 0.6f, 0);

				//Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

				switch(name)
				{
					case "White":
						Brush.renderer.material.color = Color.white;
						break;
					case "Black":
						Brush.renderer.material.color = Color.black;
						break;
					case "Red":
						Brush.renderer.material.color = Color.red;
						break;
					case "Green":
						Brush.renderer.material.color = Color.green;
						break;
					case "Blue":
						Brush.renderer.material.color = Color.blue;
						break;
					case "Yellow":
						Brush.renderer.material.color = Color.yellow;
						break;
					case "Purple":
						Brush.renderer.material.color = purple;
						break;
					case "Orange":
						Brush.renderer.material.color = orange;
						break;
				}
				thisLevelManager.brush = cursorTexture;
			}
		}
	}
}