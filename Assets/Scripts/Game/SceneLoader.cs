using UnityEngine;
using System.Collections;

namespace Game 
{
    /// <summary>
    /// For loading scenes and fading
    /// </summary>
	public class SceneLoader : MonoBehaviour 
    {
        public bool doFade = false;
        public float fadeSpeed = 4f;

        private float speed;
		private Color originalColor;
		private Color targetColor;
		private float startTime;
		private string levelName;

        

		void Start()
		{
            print("SceneLoader start...");

            gameObject.AddComponent<GUITexture>();
            guiTexture.texture = Resources.Load("Textures/white") as Texture;
            guiTexture.pixelInset = new Rect(Screen.width / 2, Screen.height / 2, 1, 1);

            originalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            targetColor = this.guiTexture.color;
            guiTexture.color = originalColor;
            guiTexture.enabled = false;

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

        public void FadeIn()
        {
            StartCoroutine(FadeInCoroutine());
        }

        private IEnumerator FadeInCoroutine()
        {
            print("fading in...");
            guiTexture.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            //print("initial alpha = " + guiTexture.color.a);
            float startTime = Time.time;
            originalColor = guiTexture.color;
            targetColor.a = 0.0f;
            
            guiTexture.enabled = true;
            
            while (guiTexture.color.a > 0.05f)
            {
                //guiTexture.enabled = true;
                this.guiTexture.color = Color.Lerp(originalColor, targetColor, (Time.time - startTime) * speed);
                //print("alpha = " + this.guiTexture.color.a);

                yield return null;
            }
            guiTexture.enabled = false;
        }

        /// <summary>
        /// Coroutine for fading out and loading level
        /// </summary>
        /// <param name="levelName"></param>
        /// <returns></returns>
        private IEnumerator LoadWithFadeOut(string levelName)
        {
            print("fading out coroutine...");
            if (levelName == "")
            {
                Debug.LogError("Level name not defined.");
                this.gameObject.guiTexture.enabled = false;
            }
            else
            {
                
                //GameObject kinectObject = GameObject.Find("KinectControls");
                //if (kinectObject)
                //    kinectObject.SetActive(false);

                float startTime = Time.time;
                originalColor.a = 0.0f;
                targetColor.a = 1.0f;                
                
                this.gameObject.guiTexture.enabled = true;
                while (this.guiTexture.color.a < 0.9f)
                {
                    this.guiTexture.color = Color.Lerp(originalColor, targetColor, (Time.time - startTime) * speed);
                    //print("alpha = " + this.guiTexture.color.a);
                    yield return null;
                }

                Application.LoadLevel(levelName);
            }
        }
	}
}