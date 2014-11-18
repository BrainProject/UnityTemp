using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class LevelManagerColoring : MonoBehaviour
	{
		public Texture brush;
		public GameObject backGUI;

		internal bool painting = false;
		internal bool hiddenGUIwhilePainting = false;

		private int x = (Screen.width / 10)/3;
		private int y = (Screen.height / 10)/2;
		private int w = Screen.width / 16;
		private int h = Screen.height / 9;


		void Update()
		{
			if(Input.GetKeyDown(KeyCode.I))
			{
				hiddenGUIwhilePainting = !hiddenGUIwhilePainting;

				if(painting && hiddenGUIwhilePainting)
					MGC.Instance.ShowCustomCursor(true);
				if(painting && !hiddenGUIwhilePainting)
					MGC.Instance.ShowCustomCursor(false);
			}
		}


		public void ShowColoringGUI(bool isVisible)
		{
			backGUI.SetActive (isVisible);
			backGUI.guiTexture.texture = backGUI.GetComponent<BackGUI> ().normal;
		}

		void OnGUI()
		{
			if(painting && !hiddenGUIwhilePainting)
				GUI.DrawTexture (new Rect (Input.mousePosition.x - x*2, Screen.height - Input.mousePosition.y - y*2, w*2, h*2), brush);
		}
	}
}