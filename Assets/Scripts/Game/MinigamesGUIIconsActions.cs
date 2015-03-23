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
		public Texture2D texture_normalGSI;
		public Texture2D texture_hoverGSI;
		
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
			if(transform.parent.GetComponent<MinigamesGUI>().gsiStandalone)
				renderer.material.mainTexture = texture_normalGSI;
			else
	            renderer.material.mainTexture = texture_normal;
        }

        void OnMouseEnter()
		{
			if(transform.parent.GetComponent<MinigamesGUI>().gsiStandalone)
          		renderer.material.mainTexture = texture_hoverGSI;
			else
				renderer.material.mainTexture = texture_hover;
        }

        void OnMouseExit()
        {
            resetState();
        }

        void OnMouseDown()
        {
			MinigamesGUI parent = transform.parent.GetComponent<MinigamesGUI> ();
			parent.hide ();
			parent.clicked = true;

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

		void OnMouseUp()
		{
			transform.parent.GetComponent<MinigamesGUI> ().clicked = false;
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
				yield return null;
			}

			this.renderer.material.color = targetColor;
			this.gameObject.SetActive (false);
		}
    }
}