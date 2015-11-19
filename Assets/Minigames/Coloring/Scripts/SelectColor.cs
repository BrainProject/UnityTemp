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
						Brush.GetComponent<Renderer>().material.color = Color.white;
						break;
					case "Black":
						Brush.GetComponent<Renderer>().material.color = Color.black;
						break;
					case "Red":
						Brush.GetComponent<Renderer>().material.color = Color.red;
						break;
					case "Green":
						Brush.GetComponent<Renderer>().material.color = Color.green;
						break;
					case "Blue":
						Brush.GetComponent<Renderer>().material.color = Color.blue;
						break;
					case "Yellow":
						Brush.GetComponent<Renderer>().material.color = Color.yellow;
						break;
					case "Purple":
						Brush.GetComponent<Renderer>().material.color = purple;
						break;
					case "Orange":
						Brush.GetComponent<Renderer>().material.color = orange;
						break;
				}

				
#if UNITY_ANDROID
				thisLevelManager.neuronMaterial.color = Brush.renderer.material.color;
#endif
				thisLevelManager.brush = cursorTexture;
			}
		}
	}
}