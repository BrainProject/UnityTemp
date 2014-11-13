using UnityEngine;
using System.Collections;
#if UNITY_STANDALONE
using Kinect;


public class KinectManagerSwitcher : MonoBehaviour {
	public static GameObject thisLevelKManager;
	public static GameObject defaultKManager;
	// Use this for initialization
	void Awake () {
		setThisLevelManager();
		defaultKManager = MGC.Instance.kinectManager;
		activateThisLevelKManager();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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

	public static void setActiveMGC(bool active)
	{
		if(defaultKManager)
		{
			MGC.Instance.checkInactivity = active;
			defaultKManager.SetActive(active);
		}
		if(!active && MGC.Instance.mouseCursor)
			MGC.Instance.HideCustomCursor();
		else if(active)
			MGC.Instance.ShowCustomCursor();
			//MGC.Instance.mouseCursor.SetActive(active);*/
	}



	public static void deactivateThisLevelKManager()
	{
		if(thisLevelKManager)
		{
			thisLevelKManager.SetActive(false);
		}
		setActiveMGC(true);
	}

	/*public void OnDestroy()
	{
		deactivateThisLevelKManager();
	}*/
}
#endif
