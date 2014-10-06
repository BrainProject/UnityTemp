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
		public bool destroy = true;
		public bool finish = true;
		public bool allChecked = false;
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
					findTartgetByCheckName();
					if(clipBone != null)
					{
						MoveParentOnBone(clipBone);
					}
				}
		}

	
		// Update is called once per frame
		void Update () {
			bool complete = allChecked;//need start true for sai is not all checked but need false if to say no
			for(int i = 0; i< transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				Check script = child.GetComponent<Check>();
				if(script  && script.activated)
				{
					Transform[] targets = script.target;
					foreach(Transform target in targets)
					{
						bool next = Vector2.Distance(child.position,target.position) < distance;
						if(next)
						{
							if(!allChecked)
							{
								complete = script.Checked(target);
							}
							else
							{
								complete = script.Checked(target) && complete;
							}
							Debug.DrawRay(target.position,child.position - target.position,Color.green);
							break;
						}	
						else
						{
							if(allChecked)
							{
								complete = false;
							}
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
				if(finish && (next== null))
				{
					Debug.Log("blbnu " + gameObject.name);
					finishHim();
				}
				if(destroy)
				{
					Destroy(gameObject);
				}
			}
		}


		void finishHim()
		{
			GameObject root = gameObject.transform.root.gameObject;
			if(!transform.parent)
			{
				FinalCount script = root.GetComponent<FinalCount>();
				if(script)
				{
					script.next();
					return;
				}
			}
			LevelManager.finish();

		}
	

		public void MoveParentOnBone(string boneName)
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

		public void findTartgetByCheckName()
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
		}
	}
}
