using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class HelpListener : MonoBehaviour {
		#if UNITY_STANDALONE
		public static HelpListener Instance;
		public  bool activated = false;
		public  GameObject[] ObjToPause;


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
		}

		/// <summary>
		/// Wait to acvtivate secret menu
		/// </summary>
		void Update () {
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

		/// <summary>
		/// Stops all.
		/// </summary>
		/// <param name="stop">If set to <c>true</c> stop.</param>
		public	void StopAll(bool stop)
		{
			foreach(GameObject  obj in ObjToPause)
			{
				obj.SendMessage("Stop",stop);
			}
		}

		/// <summary>
		/// Sets the this minigame played.
		/// </summary>
		/// <returns>The this minigame played.</returns>
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
