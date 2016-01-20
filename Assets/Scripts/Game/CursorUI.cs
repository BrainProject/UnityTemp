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

        //private Sprite currentCursor;

        private RectTransform thisRectTransform;
        //private Kinect.InteractionManager interactionManager;
		
		void Start ()
        {
#if !UNITY_ANDROID || UNITY_EDITOR
			Cursor.visible = false;
            thisRectTransform = GetComponent<RectTransform>();
            /*if(MGC.Instance.kinectManagerObject.activeSelf)
                interactionManager = MGC.Instance.kinectManagerInstance.GetComponent<Kinect.InteractionManager>();*/
#endif
        }

        void Update()
        {
            if (MGC.Instance.kinectManagerObject.activeSelf)
            {
#if UNITY_STANDALONE
                if (MGC.Instance.kinectManagerInstance.GetUsersCount() > 0)
                {
                    //thisRectTransform.position = new Vector2(
                    //    interactionManager.GetCursorPosition().x * Screen.width,
                    //    interactionManager.GetCursorPosition().y * Screen.height);

                    //Kinect.Win32.MouseKeySimulator.CursorPos(interactionManager.GetCursorPosition());
                    thisRectTransform.position = Input.mousePosition;
                }
                else
                {
                    thisRectTransform.position = Input.mousePosition;
                }
#endif
            }
            else
            {
                thisRectTransform.position = Input.mousePosition;
            }

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

        public void CursorPosition(Vector2 newPos)
        {
            thisRectTransform.position = newPos;
        }
	}
}