using UnityEngine;
using System.Collections;
using Kinect;

public class KinectManagerSwitcher : MonoBehaviour {
	public GameObject thisLevelKManager;
	GameObject defaultKManager;
	// Use this for initialization
	void Awake () {
		//defaultKManager = MGC.Instance.kinectManager;
		activateThisLevelKManager();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void activateThisLevelKManager()
	{ 
		if(defaultKManager)
		{
			defaultKManager.SetActive(false);
		}
		if(thisLevelKManager)
		{
			thisLevelKManager.SetActive(true);
		}
		else
		{
			if(defaultKManager)
			{
				defaultKManager.SetActive(true);
			}
		}
	}

	public void deactivateThisLevelKManager()
	{
		if(thisLevelKManager)
		{
			thisLevelKManager.SetActive(false);
		}
		if(defaultKManager)
		{
			defaultKManager.SetActive(true);
		}
	}
}
