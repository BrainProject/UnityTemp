using UnityEngine;
using System.Collections;

public enum MenuType
{
	Brain,
	Tiles,
	GSI
}


public class MenuCrossroad : MonoBehaviour {
	public MenuType menuType;
	public string brainMenuScene;
	public string tilesMenuScene;

	// Use this for initialization
	void Start ()
	{
		MGC.Instance.menuType = menuType;
		switch (menuType) 
		{
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
		}
	}
}
