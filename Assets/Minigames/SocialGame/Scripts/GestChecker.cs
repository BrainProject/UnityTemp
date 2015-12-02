using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SocialGame
{
	public class GestChecker : MonoBehaviour {
#if UNITY_STANDALONE
		public bool activeChecking = true;
		public float distance;
		public GameObject next;
		public string clipBone;
		public bool handMode = true;
		public bool player1;
		public bool player2;
		public bool destroy = true;
		public bool finish = true;
		public bool allChecked = false;
		//private Vector3 temp;
		public List<Transform> Targets = new List<Transform>();

		public Kinect.KinectManager KManager;
		// Use this for initialization
		void Start () {

			//GameObject temp = GameObject.FindWithTag("GameController");
			KManager = MGC.Instance.kinectManagerInstance;
            //Debug.Log(MGC.Instance.kinectManagerInstance.avatarControllers.Count);
            if (KManager)
			{
				if(handMode)
				{
                    
                    foreach (Kinect.AvatarController avatar in KManager.avatarControllers)
					{
                        
						if((player1 && avatar.playerIndex == 0)||(player2 && avatar.playerIndex == 1))
						{
							ExtendsAvatar avatarEx = avatar as ExtendsAvatar;
							if(avatarEx)
							{
								Targets.Add(avatarEx.handLeft);
								Targets.Add(avatarEx.handRight);
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
			else
			{
				Debug.LogWarning("Kinect Manager not founded");
			}
		}

	
		/// <summary>
		/// Checking collision
		/// </summary>
		void Update () {
			if(activeChecking)
			{
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
							bool next = Vector2.Distance(child.position,target.position) < (script.multiDist * distance);
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
								//Debug.DrawRay(target.position,child.position - target.position,Color.green);
								break;
							}	
							else
							{
								if(allChecked)
								{
									complete = false;
								}
								//Debug.DrawRay(target.position,child.position - target.position,Color.red);
								//Debug.Log(child.name +" "+ target.name + "ve vzdalenosti: " + Vector2.Distance(child.position,target.position).ToString());
							}
						}
					}
				}
				if(complete)
				{
					CompleteGest();
				}
			}
		}

		/// <summary>
		/// Completes the gest.
		/// </summary>
		protected virtual void CompleteGest()
		{
			if(next)
			{
				GameObject.Instantiate(this.next);
			}
			if(finish && (next== null))
			{
				finishHim();
			}
			if(destroy)
			{
				Destroy(gameObject);
			}
		}


		/// <summary>
		/// Finishs the him.
		/// </summary>
		void finishHim()
		{
			GameObject root = gameObject.transform.root.gameObject;
			if(!transform.parent)
			{
				FinalCount script = root.GetComponent<FinalCount>();
				if(script)
				{
					script.Next();
					return;
				}
			}
			LevelManager.win();

		}
	
		/// <summary>
		/// Moves the parent on bone.
		/// </summary>
		/// <param name="boneName">Bone name.</param>
		public void MoveParentOnBone(string boneName)
		{
			GameObject bone = GameObjectEx.FindGameObjectWithNameTag(boneName,gameObject.tag);
			if(bone != null)
			{
				Vector3 pos =transform.position;
				Quaternion rot = transform.rotation;
				gameObject.transform.parent= bone.transform;
				transform.localPosition = Vector3.zero + pos;
				transform.localRotation = rot;
			}
		}

		/// <summary>
		/// Finds the name of the tartget by check.
		/// </summary>
		public void findTartgetByCheckName()
		{
			for(int i =0; i <transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				string nameGest = child.name;
				string[] names =nameGest.Split('-');
				GameObject obj = GameObjectEx.FindGameObjectWithNameTag(names[0],gameObject.tag);
				Check che =child.GetComponent<Check>();
				if (che != null)
				{
					Transform[] targ = new Transform[] {obj.transform};
					che.target = targ;
				}
			}
		}

		/// <summary>
		/// Finds the name of the tartget by check.
		/// </summary>
		/// <param name="child">Child.</param>
		public void findTartgetByCheckName(Transform child)
		{
			string nameGest = child.name;
			string[] names =nameGest.Split('-');
			GameObject obj = GameObjectEx.FindGameObjectWithNameTag(names[0],gameObject.tag);
			Check che =child.GetComponent<Check>();
			if (che != null)
			{
				Transform[] targ = new Transform[] {obj.transform};
				che.target = targ;
			}										
		}

		/// <summary>
		/// Adds the check.
		/// </summary>
		/// <param name="check">Check.</param>
		public void addCheck(Transform check)
		{
			check.parent = transform;

			if(handMode)
			{
				Check che =check.GetComponent<Check>();
				if (che != null)
				{
					Transform[] targ = Targets.ToArray();
					che.target = targ;
				}
			}
			else
			{
				findTartgetByCheckName(check);
			}
		}

		/// <summary>
		/// Activates the checking.
		/// </summary>
		/// <param name="active">If set to <c>true</c> active.</param>
		public void ActivateChecking(bool active)
		{

			activeChecking = true;//active;
			for(int i = 0; i< transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				CheckClip script = child.GetComponent<CheckClip>();
				if(script)
				{
					script.Halo(active);
				}
			}
		}

		void OnDrawGizmos() {
			for(int i = 0; i< transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				Check script = child.GetComponent<Check>();
				if(script)
				{
					Gizmos.DrawWireSphere(script.transform.position,distance * script.multiDist);
				}
			}
		}
		
		#else
		public void findTartgetByCheckName()
		{

		}

		public void MoveParentOnBone(string boneName)
		{

		}

		public void addCheck(Transform check)
		{

		}
#endif
	}
}

