using UnityEngine;
using System.Collections;

public enum MenuType
{
	None,
	Brain,
	Tiles,
	GSI
}


public class MenuCrossroad : MonoBehaviour {
	public GameObject errorPanel;
	public MenuType menuType;
	public string brainMenuScene;
    public string tilesMenuScene;
    public string GSIMenuScene;

    // Use this for initialization
    void Start ()
	{
		// override menuType if MGC has already set some (from splash screen)
		// if returning from some other scene, the menuType will just get rewritten
		// with the same value (if it's not changed, which is now also possible for better testing)
		if(MGC.Instance.menuType != MenuType.None)
		{
			menuType = MGC.Instance.menuType;
		}
		else
		{
			MGC.Instance.menuType = menuType;
		}

#if UNITY_STANDALONE
        // clear KinectManager
        if (MGC.Instance.isKinectUsed && !MGC.Instance.kinectManagerObject.activeSelf)
        {
            bool bNeedRestart = false;
            Kinect.KinectInterop.InitSensorInterfaces(false, ref bNeedRestart);
            MGC.Instance.kinectManagerObject.SetActive(true);
            MGC.Instance.kinectManagerInstance.ClearKinectUsers();
            MGC.Instance.kinectManagerInstance.avatarControllers.Clear();
            MGC.Instance.kinectManagerInstance.StartKinect();
        }
#endif
        MGC.Instance.sceneLoader.doFade = true;

        switch (menuType)
        {
            case MenuType.Brain:
                {
                    MGC.Instance.mainSceneName = brainMenuScene;
                    Application.LoadLevel(brainMenuScene);
                    break;
                }
            case MenuType.Tiles:
                {
                    MGC.Instance.mainSceneName = tilesMenuScene;
                    Application.LoadLevel(tilesMenuScene);
                    break;
                }
            case MenuType.GSI:
                {
                    MGC.Instance.mainSceneName = GSIMenuScene;
                    Application.LoadLevel(GSIMenuScene);
                    break;
                }
            default:
                {
                    errorPanel.gameObject.SetActive(true);
                    break;
                }
        }
	}


	void Update()
	{
		if(Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			if(MGC.Instance.menuType != MenuType.None)
			{
				MGC.Instance.sceneLoader.LoadScene(Application.loadedLevelName);
			}
		}
	}
}
