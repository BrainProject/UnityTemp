#if UNITY_STANDALONE
using UnityEngine;
using System.Collections;
using Kinect;

public class ExtendsAvatar : AvatarController {
	public Transform handLeft;
	public Transform handRight;

	private Vector3 newPos;
	private bool run = true;

	void Update()
	{
		if(KinectManager.Instance && (LastUserID != 0) && run)
		{
			newPos = this.transform.position;
			newPos.x = Kinect.KinectManager.Instance.GetUserPosition(LastUserID).x;
			this.transform.position = newPos;
		}
	}

	void Stop(bool stop)
	{
		run = !stop;
	}
}
#endif