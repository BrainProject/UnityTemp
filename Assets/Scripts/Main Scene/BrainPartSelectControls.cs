/*
 * Created by: Milan Doležal
 */ 


using UnityEngine;
using System.Collections;

public class BrainPartSelectControls : MonoBehaviour {
	private Animator cameraAnimation;

	// Use this for initialization
	void Start()
	{
		cameraAnimation = this.GetComponent<Animator>();
		cameraAnimation.speed = 0;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(Input.GetButtonDown ("Vertical") || Input.GetButtonDown ("Fire1"))
		{
			if(!cameraAnimation.GetBool("start"))
			{
				cameraAnimation.SetBool("start", true);
				cameraAnimation.speed = 1;
			}
		}
		if(cameraAnimation.GetCurrentAnimatorStateInfo(0).IsName("BeginningCamera"))
		{
			if(cameraAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
			{
				cameraAnimation.SetBool("done", true);
				cameraAnimation.speed = 0;
				this.transform.parent.gameObject.GetComponent<RotateAroundBrain>().CanRotate = true;
				this.transform.GetChild(0).GetComponent<RotateAroundBrainBorder>().CanRotate = true;
				this.transform.GetChild(1).GetComponent<RotateAroundBrainBorder>().CanRotate = true;
				this.transform.GetChild(2).GetComponent<RotateAroundBrainBorder>().CanRotate = true;
				this.transform.GetChild(3).GetComponent<RotateAroundBrainBorder>().CanRotate = true;
				this.transform.GetChild(0).GetComponent<RotateAroundBrainBorder>().enabled = true;
				this.transform.GetChild(1).GetComponent<RotateAroundBrainBorder>().enabled = true;
				this.transform.GetChild(2).GetComponent<RotateAroundBrainBorder>().enabled = true;
				this.transform.GetChild(3).GetComponent<RotateAroundBrainBorder>().enabled = true;
				GameObject.Find("Frt_FrontalLobe").GetComponent<SelectBrainPart>().CanRotate = true;
				GameObject.Find("Mid_ParietalLobe").GetComponent<SelectBrainPart>().CanRotate = true;
				GameObject.Find("Bck_OccipitalLobe").GetComponent<SelectBrainPart>().CanRotate = true;
			}
		}
	}
}
