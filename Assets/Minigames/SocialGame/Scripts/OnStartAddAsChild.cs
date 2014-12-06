using UnityEngine;
using System.Collections;


public class OnStartAddAsChild : MonoBehaviour {

	public bool player1;
	public Vector3 position;
	public Vector3 rotation;

	// Use this for initialization
	void Start () {
	
		GameObject playerObj;
		string tag;
		Kinect.KinectManager kinect = Kinect.KinectManager.Instance;
		if(player1)
		{
			
			tag="Player1";
			if(kinect)
			{
				playerObj = GameObjectEx.FindByTagFromList(kinect.Player1Avatars,tag);
			}
			else
			{
				playerObj = GameObjectEx.FindGameObjectWithNameTag("Root",tag);
			}
		}
		else
		{
			tag="Player2";
			if(kinect)
			{
				playerObj = GameObjectEx.FindByTagFromList(kinect.Player2Avatars,tag);
			}
			else
			{
				playerObj = GameObjectEx.FindGameObjectWithNameTag("Root",tag);
			}
		}
		if(playerObj)
		{
			transform.parent = playerObj.transform;
			transform.localPosition = position;
			transform.localRotation = Quaternion.Euler(rotation);
		}
		else
			Debug.LogWarning("Not Found player");

	}
}
