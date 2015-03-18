#if UNITY_STANDALONE
using UnityEngine;
using System.Collections;
using Kinect;

public class ExtendsAvatar : AvatarController {
	public Transform handLeft;
	public Transform handRight;

	private Vector3 newPos;
	

	void Update()
	{
		if(KinectManager.Instance && (LastUserID != 0))
		{
			newPos = this.transform.position;
			newPos.x = Kinect.KinectManager.Instance.GetUserPosition(LastUserID).x;
			this.transform.position = newPos;
		}
	}
}
#endif