using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SocialGame
{
	public class GestCheckerFigure : MonoBehaviour {
#if !UNITY_WEBPLAYER
		public float distance;
		public GameObject next;
		public string clipBone;
		public bool handMode = true;
		public bool player1;
		public bool player2;
		public bool destroy = true;
		public bool allChecked = false;

		private Vector3 temp;
		private List<Transform> Targets = new List<Transform>();

		public Kinect.KinectManager KManager;
		// Use this for initialization
		void Start () {
			GameObject temp = GameObject.FindWithTag("GameController");
			if(temp != null)
			{
				KManager = temp.GetComponent<Kinect.KinectManager>();
			}
			if(KManager)
			{
				if(handMode)
				{
					if(player1)
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
					if(player2 && KManager.TwoUsers)
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
					}
				StartCoroutine(Check());
			}
			else
			{
				Debug.Log("Kinect Manager not founded");
			}
		}

	
		// Update is called once per frame
		IEnumerator Check() {
			while(true)
			{
				bool complete = allChecked;//need start true for sai is not all checked but need false if to say no
				for(int i = 0; i< transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					CheckFigure script = child.GetComponent<CheckFigure>();
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
								//Debug.DrawRay(target.position,child.position - target.position,Color.green);
								break;
							}	
							else
							{
								script.UnCheck();
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
					Debug.Log("end");
					StopCoroutine(Check());
				}
				else
				{
					yield return new WaitForSeconds(0.5f);
				}
			}
		}

		protected virtual void CompleteGest()
		{
			if(next)
			{
				GameObject.Instantiate(this.next);
			}
			if(destroy)
			{
				Destroy(gameObject);
			}
		}

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

