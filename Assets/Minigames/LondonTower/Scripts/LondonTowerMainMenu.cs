using UnityEngine;
using System.Collections;

/// <summary>
/// Main menu gui for london tower
/// </summary>
public class LondonTowerMainMenu : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;


    public void SelectLevel()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void Level(int level)
    {
        //LondonTowerGameManager.dataSet = level;
        MGC.Instance.sceneLoader.LoadScene("LondonTowerGame");
    }

    public void BackToMenu()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
    }

    public void BackToGameSelect()
    {
        //edit to load main scene level
        Application.LoadLevel("Main");
        //Application.Quit();
    }

}
