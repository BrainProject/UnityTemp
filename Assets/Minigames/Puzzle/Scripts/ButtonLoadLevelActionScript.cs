using UnityEngine;
using System.Collections;

namespace Puzzle
{
    public class ButtonLoadLevelActionScript : MonoBehaviour
    {
        public bool shouldExitApplication = false;

        public string targetLevel = "ChoosePicture";

        private Color noActionColor = new Color32(0x33, 0x33, 0x33, 0xFF);

        private Color pointerOverColor = new Color32(0xFF, 0x77, 0x44, 0xFF);



        void Start()
        {
            GetComponent<SpriteRenderer>().color = noActionColor;
            //noActionColor.
        }

        void OnMouseEnter()
        {
            GetComponent<SpriteRenderer>().color = pointerOverColor;
        }

        void OnMouseExit()
        {
            GetComponent<SpriteRenderer>().color = noActionColor;
        }

        void OnMouseDown()
        {
            if (shouldExitApplication)
            {
                Application.Quit();
            }
            else
            {
                Application.LoadLevel(targetLevel);
                //AutoFade.LoadLevel(targetLevel, 3, 1, Color.white);
                //LevelLoadFade.FadeAndLoadLevel(targetLevel, Color.white, 0.5);
                //SceneLoader loader = new SceneLoader();
                //loader.LoadScene(targetLevel);
            }

        }
    }
}
