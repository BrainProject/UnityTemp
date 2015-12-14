using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SocialGame{
	public class HelpListener : MonoBehaviour {
		#if UNITY_STANDALONE
		//public static HelpListener Instance;
		public  bool activatedGUI = false;
		public  GameObject[] ObjToPause;


		void Awake()
		{
			/*if (Instance) 
			{
				Destroy(this);
			}
			else
			{
				Instance = this;
			}*/
		}

		void OnEnable()
		{
			MGC.TakeControlForGUIEvent += GiveControl;
		}
		
		void OnDisable()
		{
			MGC.TakeControlForGUIEvent -= GiveControl;
			/*if(!activatedGUI)
			{
				KinectManagerSwitcher.deactivateThisLevelKManager();
			}*/
		}

		void GiveControl(bool taken)
		{
			Debug.Log ("Control taken: " + taken);
			activatedGUI = taken; 
			if(taken)
			{
				KinectManagerSwitcher.deactivateThisLevelKManager();
			}
			else
			{
				KinectManagerSwitcher.activateThisLevelKManager();
			}
			StopAll(taken);
		}

		void Start()
		{
			StartCoroutine (SetThisMinigamePlayed ());
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
			MGC.Instance.minigamesProperties.SetPlayed(SceneManager.GetActiveScene().name);
		}
		#endif
	}
}
