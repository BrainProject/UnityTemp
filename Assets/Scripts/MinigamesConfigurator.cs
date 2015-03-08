using UnityEngine;
using System.Collections;

/// <summary>
/// Defines content of MinigamesEditor
/// </summary>
public class MinigamesConfigurator : MonoBehaviour 
{

    //TODO parameters here are the same as in MinigameProperties
        // how to re-use the code?

    /// <summary>
    /// human-friendly name of mini-game
    /// </summary>
    /// May contains spaces, apostrofs and other weird characters
    /// used only in debug prints and for logging
    public string readableName;

    /// <summary>
    /// if mini-game has more than one scene, this one will be loaded first
    /// </summary>
    public string initialScene;

    /// <summary>
    /// When this scene is loaded, help for mini-game will be shown
    /// </summary>
    public string sceneWithHelp;

    //maximum difficulty
    public int MaxDifficulty;

    /// <summary>
    /// Image symbolizing low difficulty
    /// </summary>
    public Sprite difficultyLow;

    /// <summary>
    /// Image symbolizing high difficulty
    /// </summary>
    public Sprite difficultyHigh;


    GameObject minigamesParent;


    void Start()
    {
        minigamesParent = GameObject.Find("Mini-games");

        if(minigamesParent == null)
        {
            Debug.LogError("This should not happens...")
        }
    }
	
	public void AddMinigame () 
    {
        //create new object representing mini-game
        //TODO get rid of resources
        GameObject newgame = (GameObject)Instantiate(Resources.Load("NewMiniGame") as GameObject);

        //set parent
        newgame.transform.SetParent(minigamesParent.transform);
	}

    public void SaveChanges()
    {
        print("Another Suxses");
    }

}
