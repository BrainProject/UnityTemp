using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
        MGC.Instance.ResetKinect();
#endif
        MGC.Instance.sceneLoader.doFade = true;

        switch (menuType)
        {
            case MenuType.Brain:
                {
                    MGC.Instance.mainSceneName = brainMenuScene;
                    SceneManager.LoadScene(brainMenuScene);
                    break;
                }
            case MenuType.Tiles:
                {
                    MGC.Instance.mainSceneName = tilesMenuScene;
                    SceneManager.LoadScene(tilesMenuScene);
                    break;
                }
            case MenuType.GSI:
                {
                    MGC.Instance.mainSceneName = GSIMenuScene;
                    SceneManager.LoadScene(GSIMenuScene);
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
				MGC.Instance.sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}
}
