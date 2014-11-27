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
		
		internal Color startColor;
		internal Color targetColor;

		private bool restartDifferentScene = false;
		private string differentSceneName;

		void Start()
		{
			startColor = this.renderer.material.color;
			targetColor = this.renderer.material.color;
		}
        
		public void resetState()
        {
            renderer.material.mainTexture = texture_normal;
        }

        void OnMouseOver()
        {
            renderer.material.mainTexture = texture_hover;
        }

        void OnMouseExit()
        {
            resetState();
        }

        void OnMouseDown()
        {
			transform.parent.GetComponent<MinigamesGUI> ().hide ();

            //resolve action
            switch(action)
			{
				case "Restart":
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

					break;
	            }

				case "GameSelection":
	            {
	                //hide GUI
	                MGC.Instance.minigamesGUI.hide();

	                //return to game selection scene
	                MGC.Instance.sceneLoader.LoadScene(2);

					break;
	            }

				case "Reward":
	            {
	                
	                //run external application with reward
	                //TODO solve things like controlling and closing external application and mainly - how to return to Unity

	                string path = @"-k http://musee.louvre.fr/visite-louvre/index.html?defaultView=rdc.s46.p01&lang=ENG";
	                Process foo = new Process();
	                foo.StartInfo.FileName = "iexplore.exe";
	                foo.StartInfo.Arguments = path;
	                foo.Start();

					break;
	            }

				case "Brain":
				{
					//hide GUI
					MGC.Instance.minigamesGUI.hide();
					
					//return to game selection scene
					MGC.Instance.sceneLoader.LoadScene(1);

					break;
				}
			}
            
        }

		public void SetRestartDifferentScene(bool shouldRestartDifferentScene,string differentRestartSceneName)
		{
			this.restartDifferentScene = shouldRestartDifferentScene;
			this.differentSceneName = differentRestartSceneName;
		}

		public void show()
		{
			StartCoroutine ("FadeInGUI");
		}

		public void hide()
		{
			StartCoroutine ("FadeOutGUI");
		}

		IEnumerator FadeInGUI()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeOutGUI");
			collider.enabled = true;
			startColor = this.renderer.material.color;
			targetColor = this.renderer.material.color;
			targetColor.a = 1;
			
			while(this.renderer.material.color.a < 0.99f)
			{
				this.renderer.material.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}
			//Time.timeScale = 0;
		}
		
		IEnumerator FadeOutGUI()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeInGUI");
			collider.enabled = false;
			startColor = this.renderer.material.color;
			targetColor = this.renderer.material.color;
			targetColor.a = 0;
			
			while(this.renderer.material.color.a > 0.001f)
			{
				this.renderer.material.color = Color.Lerp (startColor, targetColor, Time.time - startTime);
				//Time.timeScale = state;
				yield return null;
			}
		}

    }

}