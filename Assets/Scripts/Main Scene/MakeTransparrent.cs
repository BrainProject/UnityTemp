/*
 * Created by: Milan Doležal
 */

using UnityEngine;
using System.Collections;

namespace MainScene 
{
	public class MakeTransparrent : MonoBehaviour 
    {
        //float transparencyDelayTime = 0f;
        public float alpha = 0.3f;

		private bool clicked;
		private bool fading;
		private float startTime;
		private Color originalColor;
		private Color targetColor;

		void Start () 
        {
#if UNITY_ANDROID
			this.renderer.material.shader = Shader.Find("Transparent/Diffuse");
#endif
			clicked = false;
			fading = false;
			originalColor = this.GetComponent<Renderer>().material.color;
			targetColor = this.GetComponent<Renderer>().material.color;
			targetColor.a = alpha;
            if (MGC.Instance.fromSelection)
			{
				this.GetComponent<Renderer>().material.color = targetColor;
				originalColor = targetColor;
			}
			else
			{
				originalColor.a = 1.0f;
				this.GetComponent<Renderer>().material.color = originalColor;
			}
		}
		
		void Update () 
        {
			if((Input.GetButtonDown ("Vertical") || Input.GetButtonDown ("Fire1") || Mathf.RoundToInt(Time.timeSinceLevelLoad) == 2) && !clicked)
			{
				//fading = true;
				clicked = true;
				startTime = Time.time;
			}

            if (clicked && !fading)
            {
                fading = true;
                startTime = Time.time;

                //renderer.material.shader = Shader.Find("Transparent/Diffuse");
            }

            if (fading)
            {
                this.GetComponent<Renderer>().material.color = Color.Lerp(originalColor, targetColor, (Time.time - startTime) / 4);
            }
		}
	}
}