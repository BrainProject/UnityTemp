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
			if(Input.GetAxis ("Vertical") > 0 || Input.GetButtonDown ("Fire1"))
			{
				if(!cameraAnimation.GetBool("start"))
				{
					cameraAnimation.SetBool("start", true);
					cameraAnimation.speed = 1;
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
			this.transform.parent.gameObject.GetComponent<RotateAroundBrain>().CanRotate = true;
			GameObject.Find("Left").GetComponent<RotateAroundBrainBorder>().CanRotate = true;
			GameObject.Find("Right").GetComponent<RotateAroundBrainBorder>().CanRotate = true;
			GameObject.Find("Up").GetComponent<RotateAroundBrainBorder>().CanRotate = true;
			GameObject.Find("Down").GetComponent<RotateAroundBrainBorder>().CanRotate = true;
			//this.transform.GetChild(0).GetComponent<RotateAroundBrainBorder>().enabled = true;
			//this.transform.GetChild(1).GetComponent<RotateAroundBrainBorder>().enabled = true;
			//this.transform.GetChild(2).GetComponent<RotateAroundBrainBorder>().enabled = true;
			//this.transform.GetChild(3).GetComponent<RotateAroundBrainBorder>().enabled = true;
            GameObject.Find("FrontalLobe").GetComponent<SelectBrainPart>().CanSelect = true;
            GameObject.Find("ParietalLobe").GetComponent<SelectBrainPart>().CanSelect = true;
            GameObject.Find("OccipitalLobe").GetComponent<SelectBrainPart>().CanSelect = true;
		}
	}
}