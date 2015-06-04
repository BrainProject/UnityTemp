/// <summary>
/// Cursor size.
/// Use this on game object with GUITexture to set its size according to screen resolution.
/// \author: Milan Doležal
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
	public class CursorUI : MonoBehaviour {
		public Sprite cursorNormal;
		public Sprite cursorDrag;

		private Sprite currentCursor;
		
#if !UNITY_ANDROID
		void Start ()
		{
			Screen.showCursor = false;
		}
#endif

		void Update()
		{
			GetComponent<RectTransform> ().position = Input.mousePosition;

			if(Input.GetMouseButtonDown(0))
				GetComponent<Image>().sprite = cursorDrag;
			
			if(Input.GetMouseButtonUp(0))
				GetComponent<Image>().sprite = cursorNormal;
		}
		
		void OnLevelWasLoaded(int level)
		{
			currentCursor = cursorNormal;
		}
	}
}