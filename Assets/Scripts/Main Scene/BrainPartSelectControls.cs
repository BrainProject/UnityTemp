/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;

namespace MainScene 
{
	public class BrainPartSelectControls : MonoBehaviour 
    {
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
		}
		
		void Update()
		{
			if(Input.GetAxis ("Vertical") > 0 || Input.GetButtonDown ("Fire1") || Mathf.RoundToInt(Time.timeSinceLevelLoad) == 2)
			{
				if(!cameraAnimation.GetBool("start"))
				{
					cameraAnimation.SetBool("start", true);
					cameraAnimation.speed = 1;
					MGC.Instance.LoadGame();
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
			GameObject.Find("Left").GetComponent<RotateAroundBrainBorder>().CanRotate = true;
			GameObject.Find("Right").GetComponent<RotateAroundBrainBorder>().CanRotate = true;
			GameObject.Find("Up").GetComponent<RotateAroundBrainBorder>().CanRotate = true;
			GameObject.Find("Down").GetComponent<RotateAroundBrainBorder>().CanRotate = true;
			GameObject.Find("FrontalLobe").GetComponent<SelectBrainPart>().CanSelect = true;
			GameObject.Find("ParietalLobe").GetComponent<SelectBrainPart>().CanSelect = true;
			GameObject.Find("OccipitalLobe").GetComponent<SelectBrainPart>().CanSelect = true;
			GameObject.Find("Cerebellum").GetComponent<SelectBrainPart>().CanSelect = true;
			GameObject.Find("TemporalLobe").GetComponent<SelectBrainPart>().CanSelect = true;
		}
	}
}