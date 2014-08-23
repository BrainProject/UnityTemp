/*
 * Created by: Milan Doležal
 */

using UnityEngine;
using System.Collections;

namespace MainScene 
{
	public class MakeTransparrent : MonoBehaviour 
    {
        float transparencyDelayTime = 0f;

		private bool clicked;
		private bool fading;
		private float startTime;
		private Color originalColor;
		private Color targetColor;

		void Start () 
        {
			clicked = false;
			fading = false;
			originalColor = this.renderer.material.color;
			targetColor = this.renderer.material.color;
			targetColor.a = 0.05f;
            if (MGC.Instance.fromSelection)
			{
				this.renderer.material.color = targetColor;
				originalColor = targetColor;
			}
			else
			{
				originalColor.a = 1.0f;
				this.renderer.material.color = originalColor;
			}
		}
		
		void Update () 
        {
			if((Input.GetButtonDown ("Vertical") || Input.GetButtonDown ("Fire1")) && !clicked)
			{
				//fading = true;
				clicked = true;
				startTime = Time.time;
			}

            if (clicked && !fading && ((Time.time - startTime) > transparencyDelayTime) )
            {
                fading = true;
                startTime = Time.time;

                //renderer.material.shader = Shader.Find("Transparent/Diffuse");
            }

            if (fading)
            {
                this.renderer.material.color = Color.Lerp(originalColor, targetColor, (Time.time - startTime) / 4);
            }
		}
	}
}