using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Game 
{
    /// <summary>
    /// For loading scenes and fading
    /// </summary>
    /// instance of this class is member of MGC. You can access sceneLoader by: <code>MGC.Instance.sceneLoader</code>.
	public class SceneLoader : MonoBehaviour 
    {
        public bool doFade = false;
        public float fadeSpeed = 5f;

        private float speed;
		private Color originalColor;
		private Color targetColor;
		private float startTime;
		private string levelName;

        private Image fadePanel;

		void Start()
		{
            print("SceneLoader::Start()...");

            //instatiate prefab of canvas with fadepanel
            GameObject fadeCanvas = Instantiate(Resources.Load("FadeCanvas")) as GameObject;

            if (fadeCanvas == null)
            {
                print("Missing 'FadeCanvas' prefab");
            }
            else
            {
                fadeCanvas.name = "Fade Canvas";

                //set proper parent to canvas
                fadeCanvas.transform.SetParent(gameObject.transform);

                //find 'Image' component
                fadePanel = fadeCanvas.GetComponentInChildren<Image>();
                if (fadePanel == null)
                {
                    print("There should be <Image> component in 'FadeCanvas' prefab");
                }
                else
                {
                    print("fadePanel founded.");
                    fadePanel.enabled = false;
                }

                
            }

            speed = 0.1f * fadeSpeed;
			
		}

        public void LoadScene(string levelName, bool doFadeInOut = true)
        {
            print("Loading scene: '" + levelName + "'");

            doFade = doFadeInOut;

            if (doFade)
            {
                StartCoroutine(LoadWithFadeOut(levelName));
            }
            else
            {
                Application.LoadLevel(levelName);
            }
        }

		public void LoadScene(int levelIndex, bool doFadeInOut = true)
		{
			print("Loading scene: '" + levelIndex + "'");
			
			doFade = doFadeInOut;
			
			if (doFade)
			{
				StartCoroutine(LoadByIndexWithFadeOut(levelIndex));
			}
			else
			{
				Application.LoadLevel(levelIndex);
			}
		}

        public void FadeIn()
        {
            StartCoroutine(FadeInCoroutine());
        }

        private IEnumerator FadeInCoroutine()
        {
			print("fading in...");
			GameObject blockBorderClone = (GameObject)Instantiate (Resources.Load ("BlockBorder"));
			
            fadePanel.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

            float startTime = Time.time;
            originalColor = fadePanel.color;
            targetColor.a = 0;
            fadePanel.enabled = true;
            
            while (fadePanel.color.a > 0.01f)
            {
                fadePanel.color = Color.Lerp(originalColor, targetColor, (Time.time - startTime) * speed);
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
			Instantiate (Resources.Load ("BlockBorder"));
			print("fading out coroutine...");
			if (levelName == "")
			{
				Debug.LogError("Level name not defined.");
				this.gameObject.guiTexture.enabled = false;
			}
			else
			{
				
				float startTime = Time.time;
				originalColor.a = 0.0f;
				targetColor.a = 1.0f;  
              
                fadePanel.enabled = true;

                while (fadePanel.color.a < 0.51f)
                {
                    fadePanel.color = Color.Lerp(originalColor, targetColor, (Time.time - startTime) * speed);
                    yield return null;
                }
                
				
				Application.LoadLevel(levelName);
			}
		}
		/// <summary>
		/// Coroutine for fading out and loading level by index
		/// </summary>
		/// <param index="levelIndex"></param>
		/// <returns></returns>
		private IEnumerator LoadByIndexWithFadeOut(int levelIndex)
		{
			Instantiate (Resources.Load ("BlockBorder"));
			print("fading out coroutine...");

			float startTime = Time.time;
			originalColor.a = 0.0f;
			targetColor.a = 1.0f;

            fadePanel.enabled = true;

            while (fadePanel.color.a < 0.51f)
            {
                fadePanel.color = Color.Lerp(originalColor, targetColor, (Time.time - startTime) * speed);
                yield return null;
            }
				
			Application.LoadLevel(levelIndex);
		}
	}
}