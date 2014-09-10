using UnityEngine;
using System.Collections;
using System.Diagnostics;

namespace Game
{


    public class MinigamesGUIIconsActions : MonoBehaviour
    {
        public string action;
        public Texture2D texture_normal;
        public Texture2D texture_hover;


        void OnMouseOver()
        {
            renderer.material.mainTexture = texture_hover;
        }

        void OnMouseExit()
        {
            renderer.material.mainTexture = texture_normal;
        }

        void OnMouseDown()
        {
            if (action == "Restart")
            {
                MGC.Instance.sceneLoader.LoadScene(Application.loadedLevelName);
            }

            if (action == "GameSelection")
            {
                MGC.Instance.sceneLoader.LoadScene("GameSelection");
            }

            if (action == "Reward")
            {
                //System.Diagnostics.Process.Start("iexplore.exe");

                string path = @"-k http://musee.louvre.fr/visite-louvre/index.html?defaultView=rdc.s46.p01&lang=ENG";
                Process foo = new Process();
                foo.StartInfo.FileName = "iexplore.exe";
                foo.StartInfo.Arguments = path;
                foo.Start();
            }
            
        }

    }

}