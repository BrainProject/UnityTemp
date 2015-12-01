/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;

namespace MainScene 
{
	public class BrainPartSelectControls : MonoBehaviour 
    {
		public RotateAroundBrainBorder rotateLeft;
		public RotateAroundBrainBorder rotateRight;
		public RotateAroundBrainBorder rotateUp;
		public RotateAroundBrainBorder rotateDown;
		public SelectBrainPart frontalLobe;
		public SelectBrainPart parietalLobe;
		public SelectBrainPart occipitalLobe;
		public SelectBrainPart cerebellum;
		public SelectBrainPart temporalLobe;



		internal Animator cameraAnimation;

		void Start()
		{
			cameraAnimation = this.GetComponent<Animator>();
			cameraAnimation.speed = 0;
            if (MGC.Instance.fromSelection)
			{
				cameraAnimation.SetBool("start", true);
				cameraAnimation.speed = 100;
			}
			MGC.Instance.minigamesGUI.backIcon.gameObject.SetActive (false);
			Color tmp = MGC.Instance.minigamesGUI.backIcon.thisImage.color;
			tmp.a = 0;
			MGC.Instance.minigamesGUI.backIcon.thisImage.color = tmp;
		}
		
		void Update()
		{
			if(Input.GetAxis ("Vertical") > 0 || Input.GetButtonDown ("Fire1") || Mathf.RoundToInt(Time.timeSinceLevelLoad) == 2)
			{
				if(!cameraAnimation.GetBool("start"))
				{
					cameraAnimation.SetBool("start", true);
					cameraAnimation.speed = 1;
					//MGC.Instance.LoadMinigamesPropertiesFromFile();
				}
			}
    
            if(cameraAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				cameraAnimation.SetBool("done", true);
				cameraAnimation.speed = 0;
				EnableInteraction();
			}
		}

		public void EnableInteraction()
		{
			rotateLeft.CanRotate = true;
			rotateRight.CanRotate = true;
			rotateUp.CanRotate = true;
			rotateDown.CanRotate = true;
			frontalLobe.CanSelect = true;
			parietalLobe.CanSelect = true;
			occipitalLobe.CanSelect = true;
			cerebellum.CanSelect = true;
			temporalLobe.CanSelect = true;
#if UNITY_ANDROID
			frontalLobe.ShowIcon();
			parietalLobe.ShowIcon();
			occipitalLobe.ShowIcon();
			cerebellum.ShowIcon();
			temporalLobe.ShowIcon();
#endif
		}
	}
}