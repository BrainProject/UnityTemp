using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SocialGame
{
	public class GestCheckerFigure : MonoBehaviour {
#if UNITY_STANDALONE
		public float distance;
		public GameObject next;
		public Check[] nextCheck;
		public CheckCancleFigure cancle;
		public string clipBone;

		public bool player1;
		public bool player2;
		public bool destroy = true;
		public bool allChecked = false;

		private Vector3 temp;
		//private List<Transform> Targets = new List<Transform>();
		private bool runningcorutine;
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
				findTartgetByCheckName();
				StartCoroutine(Check());
			}
			else
			{
				Debug.Log("Kinect Manager not founded");
			}
		}

	
		// Update is called once per frame
		IEnumerator Check() {
			//string name = gameObject.name;
			runningcorutine = true;
			while(runningcorutine)
			{
				//Debug.Log(name + " is chenking");
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
					runningcorutine =false;
					CompleteGest();
					//Debug.Log( name + "finish checking");
					//StopCoroutine("Check");
					yield return null;

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
			if (cancle) 
			{
				cancle.deactivate();
			}
			foreach(Check check in nextCheck)
			{
				check.activate();
			}
			if(destroy)
			{
				DestroyChecker();
			}
		}

		public void DestroyChecker()
		{
			runningcorutine = false;
			Destroy(gameObject);
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
			findTartgetByCheckName(check);
			
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

