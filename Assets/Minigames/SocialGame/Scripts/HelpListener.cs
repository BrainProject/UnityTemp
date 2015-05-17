using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class HelpListener : MonoBehaviour {
		#if UNITY_STANDALONE
		public static HelpListener Instance;
		public  bool activated = false;
		public  GameObject[] ObjToPause;

		//	public Game.BrainHelp neuronHelp;
		
		//	private bool hideHelp = true;
		void Awake()
		{
			if (Instance) 
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}
		}

		void Start()
		{
			StartCoroutine (SetThisMinigamePlayed ());
			//KinectManagerSwitcher.deactivateThisLevelKManager();
	//		if(MGC.Instance.neuronHelp)
	//			neuronHelp = MGC.Instance.neuronHelp.GetComponent<Game.BrainHelp>();
		}

		// Update is called once per frame
		void Update () {
	//		if(hideHelp)
	//		{
	//			if(neuronHelp)
	//			{
	//	 			if(!neuronHelp.helpExists)
	//				{
	//					KinectManagerSwitcher.activateThisLevelKManager();
	//					hideHelp = false;
	//				}
	//			}
	//			else
	//			{
	//				KinectManagerSwitcher.activateThisLevelKManager();
	//				hideHelp = false;
	//				Debug.Log("neco");
	//			}
	//		}

			if(Input.GetKeyDown(KeyCode.I))
			{
				if(!activated)
				{
					KinectManagerSwitcher.deactivateThisLevelKManager();
				}
				else
				{
					KinectManagerSwitcher.activateThisLevelKManager();
				}
				StopAll(!activated);
				activated = !activated;

			}
		}

		public	void StopAll(bool stop)
		{
			foreach(GameObject  obj in ObjToPause)
			{
				obj.SendMessage("Stop",stop);
			}
		}

		private IEnumerator SetThisMinigamePlayed()
		{
			while(!MGC.Instance)
			{
				Debug.Log(MGC.Instance);			
				yield return new WaitForSeconds (1);
			}
			MGC.Instance.minigamesProperties.SetPlayed(Application.loadedLevelName);
		}
		#endif
	}
}
