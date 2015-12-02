using UnityEngine;
using System.Collections;
using Kinect;

namespace SocialGame{
public class KinectManagerSwitcher : MonoBehaviour {
		#if UNITY_STANDALONE
		public static GameObject thisLevelKManager;
		public static GameObject defaultKManager;
        public AvatarController player1;
        public AvatarController player2;

        // Use this for initialization
        void Awake ()
        {
            defaultKManager = MGC.Instance.kinectManagerObject;
            setThisLevelManager();
			
			activateThisLevelKManager();
		}

		/// <summary>
		/// Sets the this level manager.
		/// </summary>
		void setThisLevelManager()
		{
            /*GameObject kinectObj = GameObject.FindGameObjectWithTag("GameController");
			if(kinectObj)
			{
				thisLevelKManager = kinectObj;
				KinectManager kinectMan = kinectObj.GetComponentInChildren<KinectManager>();
				if(kinectMan)
				{
					thisLevelKManager = kinectMan;
				}
			}*/
            if(player1)
                MGC.Instance.kinectManagerInstance.avatarControllers.Add(player1);

            if (player2)
                MGC.Instance.kinectManagerInstance.avatarControllers.Add(player2);

            //MGC.Instance.kinectManagerInstance.Awake();
            bool bNeedRestart = false;
            KinectInterop.InitSensorInterfaces(false, ref bNeedRestart);
            MGC.Instance.kinectManagerInstance.StartKinect();
        }

		/// <summary>
		/// Activates the this level K manager.
		/// </summary>
		public static void activateThisLevelKManager()
        {
            MGC.Instance.ShowCustomCursor(false);
            InteractionManager im = MGC.Instance.kinectManagerInstance.GetComponent<InteractionManager>();
            im.controlMouseCursor = false;
            im.controlMouseDrag = false;
            im.allowHandClicks = false;
            /*setActiveMGC(false);
			
			if(thisLevelKManager)
			{
				thisLevelKManager.SetActive(true);
				KinectManager thisManager = thisLevelKManager.GetComponent<KinectManager>();
				thisManager.ClearKinectUsers();

				//thisManager.Start();
			}
			else
			{
				setActiveMGC(true);
			}*/
        }

		/// <summary>
		/// Sets the active MGC.
		/// </summary>
		/// <param name="active">If set to <c>true</c> active.</param>
		public static void setActiveMGC(bool active)
		{
            if(active)
            {
                MGC.Instance.ShowCustomCursor(true);
                MGC.Instance.kinectManagerInstance.ClearKinectUsers();
                MGC.Instance.kinectManagerInstance.StartKinect();
                MGC.Instance.kinectManagerInstance.avatarControllers.Clear();
            }
            else
            {
                if(MGC.Instance.mouseCursor)
				    MGC.Instance.ShowCustomCursor(false);
            }


            /*
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
                MGC.Instance.kinectManagerInstance.ClearKinectUsers();
                MGC.Instance.kinectManagerInstance.StartKinect();
                //MGC.Instance.kinectManagerInstance.Start();
                MGC.Instance.kinectManagerInstance.avatarControllers.Clear();
			}*/
		}


		/// <summary>
		/// Deactivates the this level K manager.
		/// </summary>
		public static void deactivateThisLevelKManager()
        {
            MGC.Instance.ShowCustomCursor(true);
            InteractionManager im = MGC.Instance.kinectManagerInstance.GetComponent<InteractionManager>();
            im.controlMouseCursor = true;
            im.controlMouseDrag = true;
            im.allowHandClicks = true;
            //if(thisLevelKManager)
            //{
            //	thisLevelKManager.SetActive(false);
            //}
            setActiveMGC(true);
		}
		#endif
	}
}
