using UnityEngine;
using System.Collections;
using Kinect;

namespace SocialGame{
public class KinectManagerSwitcher : MonoBehaviour {
		#if UNITY_STANDALONE
		public static GameObject thisLevelKManager;
		public static GameObject defaultKManager;

		// Use this for initialization
		void Awake () {
			setThisLevelManager();
			defaultKManager = MGC.Instance.kinectManagerObject;
			activateThisLevelKManager();
		}

		/// <summary>
		/// Sets the this level manager.
		/// </summary>
		void setThisLevelManager()
		{
			GameObject kinectObj = GameObject.FindGameObjectWithTag("GameController");
			if(kinectObj)
			{
				thisLevelKManager = kinectObj;
				/*KinectManager kinectMan = kinectObj.GetComponentInChildren<KinectManager>();
				if(kinectMan)
				{
					thisLevelKManager = kinectMan;
				}*/
			}
		}

		/// <summary>
		/// Activates the this level K manager.
		/// </summary>
		public static void activateThisLevelKManager()
		{
			setActiveMGC(false);
			
			if(thisLevelKManager)
			{
				thisLevelKManager.SetActive(true);
			}
			else
			{
				setActiveMGC(true);
			}
		}

		/// <summary>
		/// Sets the active MGC.
		/// </summary>
		/// <param name="active">If set to <c>true</c> active.</param>
		public static void setActiveMGC(bool active)
		{
			if(defaultKManager)
			{
				MGC.Instance.checkInactivity = active;
				defaultKManager.SetActive(active);
			}
			if (!active && MGC.Instance.mouseCursor)
				MGC.Instance.ShowCustomCursor (false);
			else if (active) 
			{
				MGC.Instance.ShowCustomCursor (true);
				MGC.Instance.kinectManagerInstance.Start();
				MGC.Instance.kinectManagerInstance.avatarControllers.Clear();
			}
		}


		/// <summary>
		/// Deactivates the this level K manager.
		/// </summary>
		public static void deactivateThisLevelKManager()
		{
			if(thisLevelKManager)
			{
				thisLevelKManager.SetActive(false);
			}
			setActiveMGC(true);
		}
		#endif
	}
}
