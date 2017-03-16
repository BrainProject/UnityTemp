/// <summary>
/// Cursor size.
/// Use this on game object with GUITexture to set its size according to screen resolution.
/// \author: Milan Doležal
/// </summary>
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Game
{
	public class CursorUI : MonoBehaviour {
		public Sprite cursorNormal;
		public Sprite cursorDrag;
        public CursorCircle cursorCircleRight;
        public CursorCircle cursorCircleLeft;

        //private Sprite currentCursor;

        private RectTransform thisRectTransform;
        //private Kinect.InteractionManager interactionManager;
		
		void Start ()
        {
            SceneManager.sceneLoaded += OnLevelFinishedLoading;
#if !UNITY_ANDROID || UNITY_EDITOR
            Cursor.visible = false;
            thisRectTransform = GetComponent<RectTransform>();
            /*if(MGC.Instance.kinectManagerObject.activeSelf)
                interactionManager = MGC.Instance.kinectManagerInstance.GetComponent<Kinect.InteractionManager>();*/
#endif
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        void Update()
        {
            if (MGC.Instance.kinectManagerObject.activeSelf)
            {
#if UNITY_STANDALONE
                if (MGC.Instance.kinectManagerInstance && MGC.Instance.kinectManagerInstance.GetUsersCount() > 0)
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
        
        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
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