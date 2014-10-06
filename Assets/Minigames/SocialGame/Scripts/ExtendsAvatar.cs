#if UNITY_STANDALONE
using UnityEngine;
using System.Collections;
using Kinect;

public class ExtendsAvatar : AvatarController {
	private Vector3 newPos;

	// Use this for initialization
	/*void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

	void Update()
	{
		newPos = this.transform.position;
		newPos.x = Kinect.KinectManager.Instance.GetUserPosition(LastUserID).x;
		this.transform.position = newPos;
	}
}
#endif