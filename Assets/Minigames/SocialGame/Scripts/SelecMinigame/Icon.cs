using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Icon : MonoBehaviour {

		public static float Size;
		public static float Expands;
		public static Camera MainCamera;

		public string LevelName;
		public Texture icon;

		private GUITexture GUITex;
		private bool selected;

		// Use this for initialization
		void Start () {
			MGC.Instance.logger.addEntry("start Stand Alone Social Game");
			GUITex = gameObject.GetComponent<GUITexture>();
			GUITex.texture = icon;
			Resize();
		}

		/// <summary>
		/// Resize this icons.
		/// </summary>
		void Resize()
		{
			float spaceForOne = Screen.height / Size;
			if(selected)
				spaceForOne =spaceForOne * Expands;
			GUITex.pixelInset = new Rect(-spaceForOne/2,-spaceForOne/2,spaceForOne,spaceForOne);
		}

		/// <summary>
		/// Raises the mouse over event.
		/// </summary>
		void OnMouseOver()
		{
			selected = true;
			float spaceForOne = Screen.height / Size;
			spaceForOne =spaceForOne * Expands;
			GUITex.pixelInset = new Rect(-spaceForOne/2,-spaceForOne/2,spaceForOne,spaceForOne);
		}

		/// <summary>
		/// Raises the mouse exit event.
		/// </summary>
		void OnMouseExit()
		{
			selected = false;
			float spaceForOne = Screen.height / Size;
			GUITex.pixelInset = new Rect(-spaceForOne/2,-spaceForOne/2,spaceForOne,spaceForOne);
		}

		/// <summary>
		/// Raises the mouse down event.
		/// </summary>
		void OnMouseDown()
		{
			MainCamera.cullingMask &= ~32;
			MGC.Instance.sceneLoader.LoadScene(LevelName);
		}
	}
}
