/**
 * @mainpage Newron 
 *
 * @section About
 * 
 * The Newron is therapeutic software for children with autism spectrum disorder. The software is being developed on Faculty of Informatics, Masaryk University (Brno, Czech Republic).
 * 
 * It is based on Unity gameProps engine.
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
/// Singleton class for storing global variables and controlling main gameProps features. 
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
/// \author Milan Doležal
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
	internal Minigames minigamesProperties;
	internal GameObject kinectManager;
	internal GameObject mouseCursor;
	internal GameObject neuronHelp;

    internal GameObject minigamesGUIObject;
    internal MinigamesGUI minigamesGUI;
	internal bool fromMain;
	internal bool fromSelection;

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
			
		//initiate minigame properties
		minigamesProperties = this.gameObject.AddComponent<Minigames> ();
        minigamesProperties.loadConfigurationsfromFile();

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
            Debug.LogError("komponenta minigamesGUI nenalezena - špatný prefab?");
        }

        
		//initiate kinect manager
		kinectManager = (GameObject)Instantiate (Resources.Load ("_KinectManager") as GameObject);
		kinectManager.transform.parent = this.transform;
	}

	void Start()
	{
		print("Master Game Controller Start()...");

        //TODO ...
		//due to unknown reason, I doesn't set the list
		//in the Minigames script correctly without this command.
		minigamesProperties.Start ();

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
            minigamesProperties.printStatisticsToFile();
		}

        // reset 'gameProps satuts' - clear data saved in mini-games properties
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

		//perform fade in?
		if (MGC.Instance.sceneLoader.doFade)
		{
			MGC.Instance.sceneLoader.FadeIn ();
		}
		else
		{
			gameObject.guiTexture.enabled = false;
		}
	}

    /// <summary>
    /// Saves statistics of all mini-games to file
    /// </summary>
	public void SaveMinigamesStatisticsToFile()
    {
        #if !UNITY_WEBPLAYER
        {
            //delete old saved file 
            if (File.Exists(Application.persistentDataPath + "/mini-games.stats"))
            {
                File.Delete(Application.persistentDataPath + "/mini-games.stats");
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/mini-games.stats");

            foreach (Game.MinigameProperties minigameData in minigamesProperties.minigames)
            {
                bf.Serialize(file, minigameData.stats);
            }
            print("Mini-games statistics saved to 'mini-games.stats' file.");
            file.Close();
        }
        #endif
    }

	public void LoadMinigamesStatisticsFromFile()
	{
		#if !UNITY_WEBPLAYER
        if (File.Exists(Application.persistentDataPath + "/mini-games.stats"))
		{
			BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/mini-games.stats", FileMode.Open);
			FileInfo info = new FileInfo(file.Name);
			if(info.Length > 0)
			{
				for(int i=0; i<minigamesProperties.minigames.Count; ++i)
				{
					minigamesProperties.minigames[i].stats = (Game.MinigameStatistics)bf.Deserialize(file);
				}

				print ("Saved mini-games statistics was loaded.");
			}
			else
			{
                Debug.LogError("Saved mini-games statistics was NOT loaded.");
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
		foreach(MinigameProperties minigameProperties in this.GetComponent<Minigames>().minigames)
		{
			minigameProperties.stats.played = false;
			minigameProperties.stats.initialShowHelpCounter = 0;
		}

		print ("Game statuses were reset to 'not yet played' (MinigameProperties.played == false)");

		SaveMinigamesStatisticsToFile ();
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

    //returns properties of currently selected mini-gameProps, if there is such
    public MinigameProperties getSelectedMinigameProperties()
    {
        return minigamesProperties.GetMinigame(selectedMiniGameName);
    }

    // evoked by clicking on some mini-gameProps "sphere", or by clicking "replay" button...
    public void startMiniGame(string name)
    {
        //store the name of selected minigame
        selectedMiniGameName = name;

        // check, if difficulty is applicable for this mini-gameProps
        // if not, run it directly
        if (getSelectedMinigameProperties().conf.MaxDifficulty == 0)
        {
            sceneLoader.LoadScene(selectedMiniGameName);
        }

        // first, load difficulty chooser scene, mini-gameProps will be loaded from that scene
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

        minigamesProperties.SetSuccessfullyPlayed(selectedMiniGameName, selectedMiniGameDiff);
        SaveMinigamesStatisticsToFile();

        //global GUI
        MGC.Instance.minigamesGUI.show(true);
    }

    public Minigames getMinigameStates()
    {
        return minigamesProperties;
    }
}