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
		public FitCounter counter;

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
			temp = GameObject.FindWithTag("Board");
			if(temp != null)
			{
				counter = temp.GetComponent<FitCounter>();
			}
		}

	
		/// <summary>
		/// Check collison.
		/// </summary>
		IEnumerator Check() {
			//string name = gameObject.name;
			runningcorutine = true;
			while(runningcorutine)
			{
				bool complete = allChecked;
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
								break;
							}	
							else
							{
								script.UnCheck();
								if(allChecked)
								{
									complete = false;
								}
							}
						}
					}
				}
				if(complete)
				{
					runningcorutine =false;
					CompleteGest();
					yield return null;

				}
				else
				{
					yield return new WaitForSeconds(0.5f);
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
			if (cancle) 
			{
				cancle.deactivate();
			}
			if (counter)
			{
				counter.nextComplete();
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

		/// <summary>
		/// Destroies the checker.
		/// </summary>
		public void DestroyChecker()
		{
			runningcorutine = false;
			Destroy(gameObject);
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

