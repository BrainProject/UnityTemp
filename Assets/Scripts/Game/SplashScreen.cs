﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Game 
{
    /// <summary>
    /// Only for transition into main scene...
    /// No important stuff should be created or initialized here
    /// GMC is initialized here, but only for `fade-in` effect in main.
    /// </summary>
	public class SplashScreen: MonoBehaviour 
    {
        public float timeBeforeFade = 2.5f;
		
        private Color originalColor;
		private Color targetColor;
        
        private float startTime;

		void Awake()
		{
			this.GetComponent<GUITexture>().pixelInset = new Rect(Screen.width/2, Screen.height/2, 1, 1);
		}

		void Start()
		{
			originalColor = this.GetComponent<GUITexture>().color;
			targetColor = this.GetComponent<GUITexture>().color;
			Cursor.visible = false;
			StartCoroutine (LoadMainLevel());

            //following line not only print something, but also create instance of MGC (if this is the first call...)
            print("Initialization of master game controller: " + MGC.Instance);
            MGC.Instance.ShowCustomCursor(false);
        }

		void Update()
		{
            //Load next level immediately if player press the button
			if(Input.GetMouseButtonDown(0))
			{
				//Screen.showCursor = true;
                MGC.Instance.sceneLoader.LoadScene("Crossroad");
			}
		}

        public IEnumerator LoadMainLevel()
        {
            yield return new WaitForSeconds(timeBeforeFade);
            startTime = Time.time;

            originalColor.a = 0;
            targetColor.a = 1.0f;
            while (this.GetComponent<GUITexture>().color.a < 0.99f)
            {
                this.GetComponent<GUITexture>().color = Color.Lerp(originalColor, targetColor, (Time.time - startTime) / 2);
                yield return null;
            }

            //Screen.showCursor = true;
            //MGC.Instance.ShowCustomCursor();

            //we want 'fade-in' effect for main scene
            MGC.Instance.sceneLoader.doFade = true;

            //load main scene
            SceneManager.LoadScene("Crossroad");
		}
	}
}