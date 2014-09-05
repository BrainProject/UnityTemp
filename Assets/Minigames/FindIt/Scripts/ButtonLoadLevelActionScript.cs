using UnityEngine;
using System.Collections;

namespace FindIt
{
    public class ButtonLoadLevelActionScript : MonoBehaviour
    {
        public bool shouldExitApplication = false;

        public string targetLevel = "ChoosePicture";

        private Color noActionColor = new Color32(0x33, 0x33, 0x33, 0xFF);

        private Color pointerOverColor = new Color32(0xFF, 0x77, 0x44, 0xFF);



        void Start()
        {
            guiTexture.color = noActionColor;
            //noActionColor.
        }

        void OnMouseEnter()
        {
            guiTexture.color = pointerOverColor;
        }

        void OnMouseExit()
        {
            guiTexture.color = noActionColor;
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
            }

        }
    }
}
