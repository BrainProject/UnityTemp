using UnityEngine;
using System.Collections;
using System.Diagnostics;

namespace Game
{


    public class X_MinigamesGUIIconsActions : MonoBehaviour
    {
		public string action;
		public Texture2D texture_normal;
		public Texture2D texture_hover;
		public Texture2D texture_normalGSI;
		public Texture2D texture_hoverGSI;
		
		internal Color startColor;
		internal Color targetColor;

		void Start()
		{
			startColor = this.GetComponent<Renderer>().material.color;
			targetColor = this.GetComponent<Renderer>().material.color;
		}
        
		public void resetState()
        {
			if(transform.parent.GetComponent<MinigamesGUI>().gsiStandalone)
				GetComponent<Renderer>().material.mainTexture = texture_normalGSI;
			else
	            GetComponent<Renderer>().material.mainTexture = texture_normal;
        }

        void OnMouseEnter()
		{
			if(transform.parent.GetComponent<MinigamesGUI>().gsiStandalone)
          		GetComponent<Renderer>().material.mainTexture = texture_hoverGSI;
			else
				GetComponent<Renderer>().material.mainTexture = texture_hover;
        }

        void OnMouseExit()
        {
            resetState();
        }

        void OnMouseUp()
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

                    MGC.Instance.startMiniGame(MGC.Instance.getSelectedMinigameName());
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
			GetComponent<Collider>().enabled = true;
			startColor = this.GetComponent<Renderer>().material.color;
			targetColor = this.GetComponent<Renderer>().material.color;
			targetColor.a = 1;
			
			while(this.GetComponent<Renderer>().material.color.a < 0.99f)
			{
				this.GetComponent<Renderer>().material.color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}
		}
		
		IEnumerator FadeOutGUI()
		{
			float startTime = Time.time;
			StopCoroutine ("FadeInGUI");
			GetComponent<Collider>().enabled = false;
			startColor = this.GetComponent<Renderer>().material.color;
			targetColor = this.GetComponent<Renderer>().material.color;
			targetColor.a = 0;
			
			while(this.GetComponent<Renderer>().material.color.a > 0.001f)
			{
				this.GetComponent<Renderer>().material.color = Color.Lerp (startColor, targetColor, Time.time - startTime);
				yield return null;
			}

			this.GetComponent<Renderer>().material.color = targetColor;
			this.gameObject.SetActive (false);
		}
    }
}