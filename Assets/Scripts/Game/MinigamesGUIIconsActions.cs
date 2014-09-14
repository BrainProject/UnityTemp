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

		private bool restartDifferentScene = false;
		private string differentSceneName;


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
            

            //resolve action
            if (action == "Restart")
            {
                //hide GUI
                MGC.Instance.minigamesGUI.hide();

                //load proper scene
				if(restartDifferentScene)
				{
					restartDifferentScene = false;
					MGC.Instance.sceneLoader.LoadScene(differentSceneName);
				}
				else MGC.Instance.sceneLoader.LoadScene(Application.loadedLevelName);
            }

            else if (action == "GameSelection")
            {
                //hide GUI
                MGC.Instance.minigamesGUI.hide();

                //return to game selection scene
                MGC.Instance.sceneLoader.LoadScene("GameSelection");
            }

            else if (action == "Reward")
            {
                
                //run external application with reward
                //TODO solve things like controlling and closing external application and mainly - how to return to Unity

                string path = @"-k http://musee.louvre.fr/visite-louvre/index.html?defaultView=rdc.s46.p01&lang=ENG";
                Process foo = new Process();
                foo.StartInfo.FileName = "iexplore.exe";
                foo.StartInfo.Arguments = path;
                foo.Start();
            }
            
        }

		public void SetRestartDifferentScene(bool shouldRestartDifferentScene,string differentRestartSceneName)
		{
			this.restartDifferentScene = shouldRestartDifferentScene;
			this.differentSceneName = differentRestartSceneName;
		}

    }

}