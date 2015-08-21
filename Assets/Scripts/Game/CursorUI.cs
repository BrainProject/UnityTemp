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
		public CursorCircle cursorCircle;

//		private Sprite currentCursor;
		
#if !UNITY_ANDROID
		void Start ()
		{
			Screen.showCursor = false;
		}
#endif

		void Update()
		{
			GetComponent<RectTransform> ().position = Input.mousePosition;

			if (Input.GetMouseButtonDown (0))
				CursorToDrag ();
			
			if (Input.GetMouseButtonUp (0))
				CursorToNormal ();
		}
		
		void OnLevelWasLoaded(int level)
		{
			GetComponent<Image>().sprite = cursorNormal;
		}

		public void CursorToDrag()
		{
			GetComponent<Image>().sprite = cursorDrag;
		}

		public void CursorToNormal()
		{
			GetComponent<Image>().sprite = cursorNormal;
		}
	}
}