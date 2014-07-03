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
		if(Input.GetMouseButtonDown(0))
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
			}
		}
	}
}
