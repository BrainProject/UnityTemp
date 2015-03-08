#pragma warning disable 0414
using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class LevelManagerColoring : MonoBehaviour
	{
		public Texture brush;
		public GameObject backGUI;
		public GameObject savePictureGUI;

        public GameObject deleteColorGUI; // for colour deleting

		internal bool painting = false;
		internal bool hiddenGUIwhilePainting = false;
		internal float timestamp;

		private int x = (Screen.width / 10)/3;
		private int y = (Screen.height / 10)/2;
		private int w = Screen.width / 16;
		private int h = Screen.height / 9;

//		void Awake()
//		{
//			MGC.Instance.ShowCustomCursor (true);
//		}

		void Start()
		{
			timestamp = -2;
		}

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
//			print (isVisible);
			backGUI.SetActive (true);
			backGUI.GetComponent<BackGUI> ().IconVisible (isVisible);
			backGUI.guiTexture.texture = backGUI.GetComponent<BackGUI> ().normal;

			savePictureGUI.SetActive (true);
			savePictureGUI.GetComponent<SavePictureGUI> ().IconVisible (isVisible);
			savePictureGUI.guiTexture.texture = savePictureGUI.GetComponent<SavePictureGUI> ().normal;
		}

#if UNITY_STANDALONE
		void OnGUI()
		{
			if(painting && !hiddenGUIwhilePainting)
				GUI.DrawTexture (new Rect (Input.mousePosition.x - x*2, Screen.height - Input.mousePosition.y - y*2, w*2, h*2), brush);
		}
#endif
	}
}