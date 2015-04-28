using UnityEngine;
using System.Collections;

public class HelpListener : MonoBehaviour {
	#if UNITY_STANDALONE
	public bool activated = false;
	public GameObject[] ObjToPause;

	//	public Game.BrainHelp neuronHelp;
	
	//	private bool hideHelp = true;

	void Start()
	{
        MGC.Instance.minigamesProperties.SetPlayed(Application.loadedLevelName);
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

	void StopAll(bool stop)
	{
		foreach(GameObject  obj in ObjToPause)
		{
			obj.SendMessage("Stop",stop);
		}
	}
	#endif
}