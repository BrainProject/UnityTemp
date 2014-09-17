using UnityEngine;
using System.Collections;

#if UNITY_STANDALONE
public class HelpListener : MonoBehaviour {
	public bool activated = false;

	//	public Game.BrainHelp neuronHelp;
	
	//	private bool hideHelp = true;

	void Start()
	{
		MGC.Instance.minigameStates.SetPlayed(Application.loadedLevelName);
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
				KinectManagerSwitcher.deactivateThisLevelKManager();
			else
				KinectManagerSwitcher.activateThisLevelKManager();
			activated = !activated;
		}
	}
}
#endif