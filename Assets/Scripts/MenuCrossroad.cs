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


		

		switch (menuType) 
		{
		case MenuType.None:
		{
			errorPanel.gameObject.SetActive(true);
			break;
		}
		case MenuType.Brain:
		{
			Application.LoadLevel (brainMenuScene);
			MGC.Instance.mainSceneName = brainMenuScene;
			break;
		}
		case MenuType.Tiles:
		{
			Application.LoadLevel (tilesMenuScene);
			MGC.Instance.mainSceneName = tilesMenuScene;
			break;
		}
		case MenuType.GSI:
		{
			Application.LoadLevel (brainMenuScene);	// GSI specific menu is missing, used brain menu instead
			MGC.Instance.mainSceneName = brainMenuScene;
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
