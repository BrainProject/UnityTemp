/*
 * Created by: Milan Doležal
 */

using UnityEngine;
using System.Collections;

namespace MainScene {
	public class MakeTransparrent : MonoBehaviour {
		private bool clicked;
		private bool fading;
		private float startTime;
		private Color originalColor;
		private Color targetColor;

		// Use this for initialization
		void Start () {
			clicked = false;
			fading = false;
			originalColor = this.renderer.material.color;
			targetColor = this.renderer.material.color;
			targetColor.a = 0.05f;
		}
		
		// Update is called once per frame
		void Update () {
			//if(Input.GetButtonDown ("Fire1") && !clicked)
			if((Input.GetButtonDown ("Vertical") || Input.GetButtonDown ("Fire1")) && !clicked)
			{
				fading = true;
				clicked = true;
				startTime = Time.time;
			}

			if(fading)
			{
				this.renderer.material.color = Color.Lerp (originalColor, targetColor, (Time.time - startTime)/4);
			}
		}
	}
}