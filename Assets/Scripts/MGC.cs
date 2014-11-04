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
/// \author Milan Doležal
/// \author Jiri Chmelik
/// 
/// \date 07-2014
public class MGC : Singleton<MGC>
{
	protected MGC ()
	{
	}
	// guarantee this will be always a singleton only - can't use the constructor!

	
	//public Game.LoadLevelWithFade loadLevelWithFade;
	public BrainPartName currentBrainPart;
	public Vector3 currentCameraDefaultPosition;
	//public GameObject selectedMinigame;
	public string gameSelectionSceneName = "GameSelection";
	/// <summary>
	/// Logs players actions
	/// </summary>
	internal Logger logger;
	internal SceneLoader sceneLoader;
	internal MinigameStates minigameStates;
	internal GameObject kinectManager;
	internal GameObject mouseCursor;
	internal GameObject neuronHelp;

    internal GameObject minigamesGUIObject;
    internal MinigamesGUI minigamesGUI;
	internal bool fromMain;
	internal bool fromSelection;
	internal bool fromMinigame;
	internal Vector3 selectedMinigame;
	internal string inactivityScene = "SocialGame";

    /// <summary>
    /// TODO just a temporary solution, before mini-games statistics will be properly saved
    /// </summary>
    internal int hanoiTowersNumberOfDisks = 3;
	
	private float inactivityTimestamp;
	private float inactivityLenght = 60f;
	private int inactivityCounter = 0;
	private GameObject controlsGUI;

	void Awake ()
	{
		print ("Master Game Controller Awake()...");

		//Initiate Logger
		logger = this.gameObject.AddComponent<Logger> ();
		logger.Initialize ("Logs", "PlayerActions.txt");

		//initiate level loader
		sceneLoader = this.gameObject.AddComponent<SceneLoader> ();
			
		//initiate minigame states
		minigameStates = this.gameObject.AddComponent<MinigameStates> ();

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

		//due to unknown reason, I doesn't set the list
		//in the MinigameStates script correctly without this command.
		minigameStates.Start ();

		inactivityTimestamp = Time.time;
		#if UNITY_WEBPLAYER
		inactivityScene = "HanoiTowers";
		#endif
	}

	void Update()
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

		//Hidden menu possible to show with secret gesture
        if (Input.GetKeyDown(KeyCode.I))
        {
            print("Show hidden menu.");

            if (!minigamesGUIObject.activeSelf)
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

        if (Time.time - inactivityTimestamp > inactivityLenght)
        {
            InactivityReaction();
        }

		//Debug actions
		if (Input.GetKeyDown(KeyCode.F11))
		{
			Application.LoadLevel ("Main");
		}
		if (Input.GetKeyDown(KeyCode.F12))
		{
			Application.LoadLevel ("GameSelection");
		}
		if (Input.GetKeyDown(KeyCode.F5)) {
			SaveGame ();
		}
		if (Input.GetKeyDown(KeyCode.F8))
		{
			LoadGame ();
		}
		if (Input.GetKeyDown (KeyCode.F3))
		{
			ResetGameStatus ();
		}
	}

	void OnLevelWasLoaded (int level)
	{
		inactivityTimestamp = Time.time;
		inactivityCounter = 0;
		print ("[MGC] Scene: '" + Application.loadedLevelName + "' loaded");
		MGC.Instance.logger.addEntry ("Scene loaded: '" + Application.loadedLevelName + "'");
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);

		//DEV NOTE: Only temporary until we unify cursor styles.
		if (Application.loadedLevelName == "Coloring")
		{
			mouseCursor.SetActive (false);
			Screen.showCursor = true;
		}
		else if(mouseCursor)
		{
			mouseCursor.SetActive(true);
			Screen.showCursor = false;
		}

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

		if (Application.loadedLevelName == gameSelectionSceneName)
		{
			print ("this is game selection scene...");
			switch (currentBrainPart) {
			case BrainPartName.FrontalLobe: //Camera.main.transform.position = GameObject.Find ("GreenPos").transform.position;
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = GameObject.Find ("FrontalLobePos");
				break;
			case BrainPartName.ParietalLobe:
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = GameObject.Find ("ParietalLobePos");
				break;
			case BrainPartName.OccipitalLobe:
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = GameObject.Find ("OccipitalLobePos");
				break;
			case BrainPartName.TemporalLobe:
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = GameObject.Find ("TemporalLobePos");
				break;
			case BrainPartName.Cerebellum:
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = GameObject.Find ("CerebellumPos");
				break;
			case BrainPartName.BrainStem:
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = GameObject.Find ("BrainStemPos");
				break;
			}
			if (fromMain)
			{
				currentCameraDefaultPosition = Camera.main.GetComponent<CameraControl> ().currentWaypoint.transform.position;
			}

			//if player comes to selection scene from main, he can leave immediately by pressing Vertical key
			Camera.main.GetComponent<CameraControl> ().ReadyToLeave = fromMain;
			Camera.main.transform.position = Camera.main.GetComponent<CameraControl> ().currentWaypoint.transform.position;
			fromMain = false;
		}

				//if (level > 2)
				//{
				//    this.GetComponent<Game.MinigameStates>().SetPlayed(Application.loadedLevelName);
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
//			SaveGame ();
//		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 240, 110, 30), "Load")) {
//			LoadGame ();
//		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 280, 110, 30), "Reset status")) {
//			ResetGameStatus ();
//		}
//	}
	public void SaveGame()
	{
		#if !UNITY_WEBPLAYER
		//remove delete of the save file feature when finished, it will no longer be necessary
		if(File.Exists(Application.persistentDataPath + "/newron.sav"))
		{
			File.Delete(Application.persistentDataPath + "/newron.sav");
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/newron.sav");

		foreach(Game.Minigame minigameData in minigameStates.minigames)
		{
			bf.Serialize(file, minigameData);
		}
		print ("Game saved.");
		file.Close();
		#endif
	}

	public void LoadGame()
	{
		#if !UNITY_WEBPLAYER
		if(File.Exists(Application.persistentDataPath + "/newron.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/newron.sav", FileMode.Open);
			for(int i=0; i<minigameStates.minigames.Count; ++i)
			{
				minigameStates.minigames[i] = (Game.Minigame)bf.Deserialize(file);
			}

			print ("Saved game-data loaded.");
			file.Close();
		}

		if(Application.loadedLevelName == "GameSelection")
			sceneLoader.LoadScene("GameSelection");
		#endif
	}

	public void ShowCustomCursor()
	{
		mouseCursor = (GameObject)Instantiate(Resources.Load("MouseCursor") as GameObject);
		mouseCursor.guiTexture.enabled = false;
		mouseCursor.transform.parent = this.transform;
	}

	public void HideCustomCursor()
	{
		Destroy(mouseCursor);
	}

	void ResetGameStatus()
	{		
		foreach(Minigame minigameData in this.GetComponent<MinigameStates>().minigames)
		{
			minigameData.played = false;
			minigameData.initialShowHelpCounter = 0;
		}

		print ("Game statuses were reset to 'not yet played' (Minigame.played == false)");

		SaveGame ();
		if(Application.loadedLevelName == "GameSelection")
			sceneLoader.LoadScene("GameSelection");
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





		////// (optional) allow runtime registration of global objects
		//static public T RegisterComponent<T>()
		//{
		//    return Instance.GetOrAddComponent<T>();
		//}
}