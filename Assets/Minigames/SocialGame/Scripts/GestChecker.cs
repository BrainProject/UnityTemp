using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SocialGame
{
public class GestChecker : MonoBehaviour {
	public float distance;
	public GameObject next;
	public string clipBone;
	public bool handMode = true;
	public bool player1;
	public bool player2;
	private Vector3 temp;
	public Kinect.KinectManager KManager;
	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.FindWithTag("GameController");
		if(temp != null)
		{
			KManager = temp.GetComponent<Kinect.KinectManager>();
		}
		if(handMode)
		{
				List<Transform> Targets = new List<Transform>();
				if(player1 && KManager)
				{
					foreach(GameObject avatar in KManager.Player1Avatars)
					{
						Kinect.AvatarController avatarControler = avatar.GetComponent<Kinect.AvatarController>();
						if(avatarControler)
						{
							Targets.Add(avatarControler.LeftHand);
							Targets.Add(avatarControler.RightHand);
						}
					}
				}
				if(player2)
				{
					foreach(GameObject avatar in KManager.Player2Avatars)
					{
						Kinect.AvatarController avatarControler = avatar.GetComponent<Kinect.AvatarController>();
						if(avatarControler)
						{
							Targets.Add(avatarControler.LeftHand);
							Targets.Add(avatarControler.RightHand);
						}
					}
				}
				for(int i =0; i <transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					Check che =child.GetComponent<Check>();
					if (che != null)
					{
						Transform[] targ = Targets.ToArray();
						che.target = targ;
					}
				}

		}
		else
		{
			for(int i =0; i <transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				string nameGest = child.name;
				string[] names =nameGest.Split('-');
				GameObject obj = GameObjectEx.findGameObjectWithNameTag(names[0],gameObject.tag);
				Check che =child.GetComponent<Check>();
				if (che != null)
				{
						Transform[] targ = new Transform[] {obj.transform};
						che.target = targ;
				}
			}
			if(clipBone != null)
			{
			MoveParentOnBone(clipBone);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool complete = false;
		for(int i = 0; i< transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			Check script = child.GetComponent<Check>();
			if(script  && script.active)
			{
				Transform[] targets = script.target;
				foreach(Transform target in targets)
				{
					bool next = Vector2.Distance(child.position,target.position) < distance;
					if(next)
					{
						complete = script.Checked();
						Debug.DrawRay(target.position,child.position - target.position,Color.green);
						break;
					}
					else
					{
						Debug.DrawRay(target.position,child.position - target.position,Color.red);
					}
				}
			}
		}
		if(complete)
		{
			if(next)
			{
				GameObject.Instantiate(this.next);
			}
			Destroy(gameObject);
		}
	}

	void MoveParentOnBone(string boneName)
	{
		GameObject bone = GameObjectEx.findGameObjectWithNameTag(boneName,gameObject.tag);
		if(bone != null)
		{
			Vector3 pos =transform.position;
			Quaternion rot = transform.rotation;
			gameObject.transform.parent= bone.transform;
			transform.localPosition = Vector3.zero + pos;
			transform.localRotation = rot;
		}
	}
}
}
