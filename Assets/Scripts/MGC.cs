/**
 * @mainpage Newron 
 *
 * @section About
 * 
 * The Newron is therapeutic software for children with autism spectrum disorder. The software is being developed on Faculty of Informatics, Masaryk University (Brno, Czech Republic).
 * 
 * It is based on Unity game engine.
  * 
 * 
 * @section Manual
 * 
 * This pages serves mainly as a class and method reference. Detailed description of project can be found at official project web page:
 * 
 * http://www.newron.cz
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MainScene;
using MinigameSelection;
using Game;

public enum BrainPartName
{
	none,
	FrontalLobe,
	ParietalLobe,
	OccipitalLobe,
	TemporalLobe,
	Cerebellum,
	BrainStem
}
;

/// <summary>
/// Master Game Controller.
/// </summary>
/// <remarks>
/// Singleton class for storing global variables and controlling main game features. 
/// Accessible from any scene, any time, simply via call:
/// <example>
/// <c>
/// <code>
/// MGC.Instance
/// </code>
/// </c>
/// </example>
/// 
/// There is no need of prefab or GameObject in your scene (one will be created automatically)
/// </remarks>
/// 
/// \author Milan Dole�al
/// \author Jiri Chmelik
/// 
/// \date 07-2014
public class MGC : Singleton<MGC>
{
	protected MGC ()
	{
	}
	
	public BrainPartName currentBrainPart;
	public Vector3 currentCameraDefaultPosition;
	public string gameSelectionSceneName = "GameSelection";

    /// <summary>
	/// Logs players actions
	/// </summary>
	internal Logger logger;
	internal SceneLoader sceneLoader;
	internal Minigames minigameStates;
	internal GameObject kinectManager;
	internal GameObject mouseCursor;
	internal GameObject neuronHelp;

    internal GameObject minigamesGUIObject;
    internal MinigamesGUI minigamesGUI;
	internal bool fromMain;
	internal bool fromSelection;
	internal bool fromMinigame;

    internal Vector3 selectedMinigame;

    // name of MiniGame scene to be loaded
    internal string selectedMiniGameName;

    // minigame will be started with this difficulty
    internal int selectedMiniGameDiff { get; set; }

	internal string inactivityScene = "Figure";
	internal bool checkInactivity = true;
	
	private float inactivityTimestamp;
	private float inactivityLenght = 60f;
	private int inactivityCounter = 1;
	private GameObject controlsGUI;
#if UNITY_ANDROID
	private float touchBlockTimestamp;
#endif

	void Awake ()
	{
#if UNITY_EDITOR
        if (UnityEditorInternal.InternalEditorUtility.HasPro())
        {
            print("You are working with PRO version of Unity");
        }
        else
        {
            print("You are working with FREE version of Unity");
        }
#endif
		print ("Master Game Controller Awake()...");

		//Initiate Logger
		logger = this.gameObject.AddComponent<Logger> ();
        string pathString = Application.persistentDataPath + "/Logs";
		logger.Initialize ( pathString, "PlayerActions.txt");
        

		//initiate level loader
		sceneLoader = this.gameObject.AddComponent<SceneLoader> ();
			
		//initiate minigame states
		minigameStates = this.gameObject.AddComponent<Minigames> ();

        //initiate minigames GUI
        minigamesGUIObject = Instantiate(Resources.Load("MinigamesGUI")) as GameObject;
        if (minigamesGUIObject == null)
        {
            Debug.LogError("Nenelazen prefab pro MinigamesGUI");
        }

        //make GUI a child of MGC 
        minigamesGUIObject.transform.parent = this.transform;
        
        minigamesGUI = minigamesGUIObject.GetComponent<MinigamesGUI>();
        if (minigamesGUI == null)
        {
            Debug.LogError("komponenta minigamesGUI nenalezena - �patn� prefab?");
        }

        
		//initiate kinect manager
		kinectManager = (GameObject)Instantiate (Resources.Load ("_KinectManager") as GameObject);
		kinectManager.transform.parent = this.transform;
	}

	void Start()
	{
		print("Master Game Controller Start()...");

		//due to unknown reason, I doesn't set the list
		//in the Minigames script correctly without this command.
		minigameStates.Start ();

		inactivityTimestamp = Time.time;
		#if !UNITY_STANDALONE
		inactivityScene = "HanoiTowers";
		#endif
	}

	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

		//Hidden menu possible to show with secret gesture or with keyboard shortcut
#if UNITY_ANDROID
		if(Input.touchCount == 3 && ((Time.time - touchBlockTimestamp) > 2))
		{
			touchBlockTimestamp = Time.time;
#else
        if (Input.GetKeyDown(KeyCode.I))
        {
#endif
            print("Show hidden menu.");
            if (!minigamesGUI.visible)
            {
                minigamesGUI.show();
            }
            else
            {
                minigamesGUI.hide();
            }
        }

		//Inactivity detection
		if(Input.anyKeyDown)
		{
			inactivityTimestamp = Time.time;
			inactivityCounter = 0;
		}

		if(checkInactivity)
		{
			if (Time.time - inactivityTimestamp > inactivityLenght)
    	    {
        	    InactivityReaction();
        	}
		}

		//Debug actions
		if (Input.GetKeyDown(KeyCode.F11))
		{
			Application.LoadLevel ("Main");
		}
		if (Input.GetKeyDown(KeyCode.F12))
		{
			Application.LoadLevel ("GameSelection");

            //TODO test only...
            minigameStates.printStatisticsToFile();
		}

        // reset 'game satuts' - clear data saved in mini-games properties
#if UNITY_ANDROID
		if(Input.touchCount == 4 && ((Time.time - touchBlockTimestamp) > 2))
		{
			touchBlockTimestamp = Time.time;
#else
		if (Input.GetKeyDown (KeyCode.F3))
		{
#endif
			ResetGameStatus ();
		}
	}

	void OnLevelWasLoaded (int level)
	{
		inactivityTimestamp = Time.time;
		inactivityCounter = 0;
		print ("[MGC] Scene: '" + Application.loadedLevelName + "' loaded");
		MGC.Instance.logger.addEntry ("Scene loaded: '" + Application.loadedLevelName + "'");
		//Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);

		//DEV NOTE: Only temporary until we unify cursor styles.
//		if (Application.loadedLevelName == "Coloring")
//		{
//			mouseCursor.SetActive (false);
//			Screen.showCursor = true;
//		}
//		else if(mouseCursor)
//		{
//			mouseCursor.SetActive(true);
//			Screen.showCursor = false;
//		}
//
//		if (!mouseCursor && Application.loadedLevel > 0)
//		{
//			ShowCustomCursor ();
//		}

		//perform fade in?
		if (MGC.Instance.sceneLoader.doFade)
		{
			MGC.Instance.sceneLoader.FadeIn ();
		}
		else
		{
			gameObject.guiTexture.enabled = false;
		}
            

		//loadLevelWithFade.LoadSeledctedLevelWithColorLerp()
		//print("calling object ID: " + this.GetInstanceID());



		//if (level > 2)
		//{
		//    this.GetComponent<Game.Minigames>().SetPlayed(Application.loadedLevelName);
		//}
	}

	//Only for debugging and testing purposes
//	void OnGUI ()
//	{
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 80, 110, 30), "Brain")) {
//			Application.LoadLevel ("Main");
//		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 120, 110, 30), "Game Selection")) {
//			Application.LoadLevel ("GameSelection");
//		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 160, 110, 30), "QUIT")) {
//			Application.Quit ();
//		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 200, 110, 30), "Save")) {
//			SaveMinigamesPropertiesToFile ();
//		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 240, 110, 30), "Load")) {
//			LoadMinigamesPropertiesFromFile ();
//		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 280, 110, 30), "Reset status")) {
//			ResetGameStatus ();
//		}
//	}
	public void SaveMinigamesPropertiesToFile()
	{
		#if !UNITY_WEBPLAYER

		//delete old saved file 
		if(File.Exists(Application.persistentDataPath + "/newron.sav"))
		{
			File.Delete(Application.persistentDataPath + "/newron.sav");
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/newron.sav");

		foreach(Game.MinigameProperties minigameData in minigameStates.minigames)
		{
			bf.Serialize(file, minigameData);
		}
		print ("Game statistics saved.");
		file.Close();
		#endif
	}

	public void LoadMinigamesPropertiesFromFile()
	{
		#if !UNITY_WEBPLAYER
		if(File.Exists(Application.persistentDataPath + "/newron.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/newron.sav", FileMode.Open);
			FileInfo info = new FileInfo(file.Name);
			if(info.Length > 0)
			{
				for(int i=0; i<minigameStates.minigames.Count; ++i)
				{
					minigameStates.minigames[i] = (Game.MinigameProperties)bf.Deserialize(file);
				}

				print ("Saved game statistics loaded.");
			}
			else
			{
				print ("Game NOT loaded.");
			}
			file.Close();
		}

        if (Application.loadedLevelName == "GameSelection")
        {
            sceneLoader.LoadScene("GameSelection");
        }
		#endif
	}

	public void ShowCustomCursor(bool isShown)
	{
#if UNITY_ANDROID
		Debug.Log("No cursor on Android is needed.");
#else
		if(isShown)
		{
			if(!mouseCursor)
			{
				mouseCursor = (GameObject)Instantiate(Resources.Load("MouseCursor") as GameObject);
				mouseCursor.guiTexture.enabled = false;
				mouseCursor.transform.parent = this.transform;
			}
			else
			{
				mouseCursor.SetActive(true);
			}
		}
		else
		{
			if(!mouseCursor)
			{
				mouseCursor = (GameObject)Instantiate(Resources.Load("MouseCursor") as GameObject);
				mouseCursor.guiTexture.enabled = false;
				mouseCursor.transform.parent = this.transform;
			}
			else
			{
				mouseCursor.SetActive(false);
			}
		}
#endif
	}

	public void HideCustomCursor()
	{
#if UNITY_ANDROID
		Debug.Log("No cursor...");
#else
		Destroy(mouseCursor);
#endif
	}

	void ResetGameStatus()
	{		
		foreach(MinigameProperties minigameData in this.GetComponent<Minigames>().minigames)
		{
			minigameData.played = false;
			minigameData.initialShowHelpCounter = 0;
		}

		print ("Game statuses were reset to 'not yet played' (MinigameProperties.played == false)");

		SaveMinigamesPropertiesToFile ();
        if (Application.loadedLevelName == "GameSelection")
        {
            sceneLoader.LoadScene("GameSelection");
        }
	}

	public void ShowHelpBubble()
	{
		if(neuronHelp)
		{
			neuronHelp.GetComponent<BrainHelp>().ShowHelpBubble();
		}
	}


	void InactivityReaction()
	{
		print ("Inactive in " + Application.loadedLevelName + " for " + inactivityLenght * inactivityCounter + " seconds.");
		logger.addEntry("Inactive in " + Application.loadedLevelName + " for " + inactivityLenght * inactivityCounter + " seconds.");
		if(inactivityCounter == 5)
		{
			inactivityCounter = 0;
			print ("Load another scene. Im getting bored here.");
			if(Application.loadedLevelName != inactivityScene)
			{
				//load inactivityMinigame
				Application.LoadLevel(inactivityScene);
			}
			else
			{
				//load either previous scene or selection scene
				Application.LoadLevel(1);
			}
		}
		else
		{
			if(neuronHelp)
			{
				neuronHelp.GetComponent<BrainHelp>().ShowSmile(Resources.Load("Neuron/sadface") as Texture);
				++inactivityCounter;
			}
		}
		inactivityTimestamp = Time.time;
	}

    public string getSelectedMinigameName()
    {
        return selectedMiniGameName;
    }

    //returns properties of currently selected mini-game, if there is such
    public MinigameProperties getSelectedMinigameProps()
    {
        return minigameStates.GetMinigame(selectedMiniGameName);
    }

    // evoked by clicking on some mini-game "sphere", or by clicking "replay" button...
    public void startMiniGame(string name)
    {
        //store the name of selected minigame
        selectedMiniGameName = name;

        // check, if difficulty is applicable for this mini-game
        // if not, run it directly
        if (getSelectedMinigameProps().MaxDifficulty == 0)
        {
            sceneLoader.LoadScene(selectedMiniGameName);
        }

        // first, load difficulty chooser scene, mini-game will be loaded from that scene
        else 
        {
            sceneLoader.LoadScene("DifficultyChooser");
        }
    }



    // evoked when player successfuully finish minigame
    internal void FinishMinigame()
    {
        print("Minigame successffully finished");

        //TODO Add "Celebration phase" stuff here

        //animate Neuron character
        GameObject Neuron = MGC.Instance.neuronHelp;
        if (Neuron)
        {
            Neuron.GetComponent<Game.BrainHelp>().ShowSmile(Resources.Load("Neuron/smilyface") as Texture);
        }

        minigameStates.SetSuccessfullyPlayed(selectedMiniGameName, selectedMiniGameDiff);
        SaveMinigamesPropertiesToFile();

        //global GUI
        MGC.Instance.minigamesGUI.show(true);
    }
}