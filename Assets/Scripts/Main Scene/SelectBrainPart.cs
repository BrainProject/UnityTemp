﻿/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;
using Game;

namespace MainScene {
	public class SelectBrainPart : MonoBehaviour {
		public string descriptionText;
		public string iconName;
		public currentBrainPartEnum brainPartToLoad;
		public bool CanSelect{ get; set; }

		private string levelName;
		private bool initialMouseOver;
		private Color selectionColor;
		private Color originalColor;
		private GameManager gameManager;
		private GameObject Icon { get; set; }
		private GameObject Description{ get; set; }

		void Start()
		{
			CanSelect = false;
			Icon = GameObject.Find ("Brain Part Icon");
			Icon.renderer.material.color = new Color(Icon.renderer.material.color.r, Icon.renderer.material.color.g, Icon.renderer.material.color.b, 0);
			Description = GameObject.Find ("Description");
			gameManager = GameObject.Find ("_GameManager").GetComponent<GameManager>();
			originalColor = this.renderer.material.color;
            levelName = "GameSelection";
			initialMouseOver = true;
		}


		void OnMouseEnter()
		{
			if(CanSelect)
			{
				Texture tmp = (Texture)Resources.Load ("Main/" + iconName, typeof(Texture));
				if(tmp != null)
				{
					Icon.renderer.material.mainTexture = tmp;
					Icon.renderer.material.color = new Color(Icon.renderer.material.color.r, Icon.renderer.material.color.g, Icon.renderer.material.color.b, 1);
					Icon.transform.position = this.transform.parent.transform.position;
				}
				
                //this.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

                //set description
				Description.GetComponent<TextMesh> ().text = descriptionText;
				Description.transform.position = this.transform.parent.transform.position - (new Vector3(0, 0.05f, 0));
				
                //highlight material
                this.renderer.material.color = new Color(originalColor.r + 0.4f, originalColor.g + 0.4f, originalColor.b + 0.4f);
				initialMouseOver = false;
			}
		}

		void OnMouseExit()
		{
			if(CanSelect)
			{
				Icon.renderer.material.mainTexture = null;
				Icon.renderer.material.color = new Color(Icon.renderer.material.color.r, Icon.renderer.material.color.g, Icon.renderer.material.color.b, 0);
				//this.transform.localScale = new Vector3(1, 1, 1);
				Description.GetComponent<TextMesh> ().text = "";
			}
			this.renderer.material.color = originalColor;
		}

		void OnMouseOver()
		{
			if(CanSelect)
			{
				if(Input.GetButtonDown ("Fire1"))
				{
                    //useless
                    //switch(descriptionText)
                    //{
                    //case "Frontal Lobe": 
                    //    gameManager.currentCameraDefaultPosition = new Vector3(0,0,0);
                    //    break;
                    //case "Pariental Lobe": 
                    //    gameManager.currentCameraDefaultPosition = new Vector3(0,0,0);
                    //    break;
                    //case "Occipital Lobe": 
                    //    gameManager.currentCameraDefaultPosition = new Vector3(0,0,0);
                    //    break;
                    //}

					gameManager.selectedBrainPart = brainPartToLoad;
					gameManager.fromMain = true;
					gameManager.fromSelection = false;
					StopAllCoroutines();
					StartCoroutine(GameObject.Find("LoadLevelWithFade").GetComponent<LoadLevelWithFade>().LoadSeledctedLevelWithColorLerp(false, levelName));
				}
			}
			if(CanSelect && initialMouseOver)
			{
				OnMouseEnter();
				print ("Mouse over from initial animation.");
			}
		}

	//	void Fade(Color currentColor, Color nextColor)
	//	{
	//		duration += Time.deltaTime;
	//		this.renderer.material.color = Color.Lerp(currentColor, nextColor, duration);
	//	}
	}
}