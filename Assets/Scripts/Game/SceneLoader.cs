using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game 
{
    /// <summary>
    /// For loading scenes and fading
    /// </summary>
    /// One instance of this class is a member of the MGC. Therefore, you can access sceneLoader simply by: <code>MGC.Instance.sceneLoader</code>.
	public class SceneLoader : MonoBehaviour 
    {
        public bool doFade = false;
        public float fadeSpeed = 1f;

        private float speed;
        private Color transparentColor;
		private Color opaqueColor;
		private float startTime;
		internal string levelName;

        private Image fadePanel;

        private bool fadeInProgress = false;

        /// <summary>
        /// Set it up, mainly UI panel used for fading
        /// </summary>
		void Start()
		{
            //print("SceneLoader::Start()...");

            transparentColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            opaqueColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            //instatiate prefab of canvas with fadepanel
            GameObject fadeCanvas = Instantiate(Resources.Load("FadeCanvas")) as GameObject;

            if (fadeCanvas == null)
            {
                Debug.LogError("Missing 'FadeCanvas' prefab");
            }
            else
            {
                //print("Fade Canvas instantiated");
                fadeCanvas.name = "Fade Canvas";

                //set proper parent to canvas
                fadeCanvas.transform.SetParent(gameObject.transform);

                //find 'Image' component
                fadePanel = fadeCanvas.GetComponentInChildren<Image>();

                if (fadePanel == null)
                {
                    Debug.LogError("There should be <Image> component in 'FadeCanvas' prefab");
                }
                else
                {
                    //print("fadePanel founded.");
                    fadePanel.enabled = false;
                }
            }
		}

        public void LoadScene(string levelName, bool doFadeInOut = true)
        {
            print("Loading scene: '" + levelName + "'");

            doFade = doFadeInOut;

            if (doFade)
            {
                if(!fadeInProgress)
                    StartCoroutine(LoadWithFadeOut(levelName));
            }
            else
            {
                SceneManager.LoadScene(levelName);
            }
        }

		public void LoadScene(int levelIndex, bool doFadeInOut = true)
		{
			print("Loading scene with index: " + levelIndex + "");
			
			doFade = doFadeInOut;
			
			if (doFade)
            {
                if (!fadeInProgress)
                    StartCoroutine(LoadByIndexWithFadeOut(levelIndex));
			}
			else
			{
                SceneManager.LoadScene(levelIndex);
			}
		}

        public void FadeIn()
        {
            StartCoroutine(FadeInCoroutine());
        }

        private IEnumerator FadeInCoroutine()
        {
			//print("fading in...");
			GameObject blockBorderClone = (GameObject)Instantiate (Resources.Load ("BlockBorder"));
			
            float startTime = Time.time;
            fadePanel.enabled = true;
            
            while (fadePanel.color.a > 0.01f)
            {
                fadePanel.color = Color.Lerp(opaqueColor, transparentColor, (Time.time - startTime) * fadeSpeed);
                yield return null;
            }

            fadePanel.enabled = false;
			Destroy (blockBorderClone);
        }

		/// <summary>
		/// Coroutine for fading out and loading level
		/// </summary>
		/// <param name="levelName"></param>
		/// <returns></returns>
		private IEnumerator LoadWithFadeOut(string levelName)
		{
            fadeInProgress = true;
			Instantiate (Resources.Load ("BlockBorder"));

			//print("fading out coroutine...");
			if (levelName == "")
			{
				Debug.LogError("Level name not defined.");
				//this.gameObject.guiTexture.enabled = false;
			}
			else
			{
				float startTime = Time.time;              
                fadePanel.enabled = true;

                while (fadePanel.color.a < 1)
                {
                    fadePanel.color = Color.Lerp(transparentColor, opaqueColor, (Time.time - startTime) * fadeSpeed);
                    //print("barva: " + fadePanel.color);
                    yield return null;
                }
                
                fadeInProgress = false;
                SceneManager.LoadScene(levelName);
            }
        }
		/// <summary>
		/// Coroutine for fading out and loading level by index
		/// </summary>
		/// <param index="levelIndex"></param>
		/// <returns></returns>
		private IEnumerator LoadByIndexWithFadeOut(int levelIndex)
        {
            fadeInProgress = true;
            Instantiate (Resources.Load ("BlockBorder"));
			print("fading out coroutine...");

			float startTime = Time.time;
            fadePanel.enabled = true;

            while (fadePanel.color.a < 1)
            {
                fadePanel.color = Color.Lerp(transparentColor, opaqueColor, (Time.time - startTime) * fadeSpeed);
                yield return null;
            }

            fadeInProgress = false;
            SceneManager.LoadScene(levelIndex);
        }
	}
}