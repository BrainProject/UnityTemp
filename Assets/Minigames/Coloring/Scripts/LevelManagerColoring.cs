using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class LevelManagerColoring : MonoBehaviour
	{
		public Texture brush;

		internal bool painting = false;

		private int x = (Screen.width / 10)/3;
		private int y = (Screen.height / 10)/2;
		private int w = Screen.width / 16;
		private int h = Screen.height / 9;

		public void CursorType (bool isPainting)
		{
			painting = isPainting;

			if(isPainting)
			{

				print ("show brush now");
			}
			MGC.Instance.ShowCustomCursor(!isPainting);
		}

		void OnGUI()
		{
			if(painting)
				GUI.DrawTexture (new Rect (Input.mousePosition.x - x, Screen.height - Input.mousePosition.y - y, w, h), brush);
		}
	}
}