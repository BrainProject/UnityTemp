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
    // guarantee this will be always a singleton only - can't use the constructor!
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
	
    /// <summary>
    /// For more simple loading of scenes with fade effect, logger entry, ...
    /// </summary>
    internal SceneLoader sceneLoader;

    /// <summary>
    /// GUI for minigames - "replay, back to selection, reward, ... icons"
    /// </summary>
    private GameObject minigamesGUIObject;
    internal MinigamesGUI minigamesGUI;

	internal MinigameStates minigameStates;
	internal GameObject kinectManager;

	internal bool fromMain;
	internal bool fromSelection;
	internal bool fromMinigame;
	internal Vector3 selectedMinigame;

	void Awake ()
	{
		print ("Master Game Controller starts...");

		//Initiate Logger
		logger = this.gameObject.AddComponent<Logger> ();
		logger.Initialize ("Logs", "PlayerActions.txt");

		//initiate level loader
		sceneLoader = this.gameObject.AddComponent<SceneLoader> ();
			
		//initiate minigame states
		minigameStates = this.gameObject.AddComponent<MinigameStates> ();

        //initiate minigames GUI
        minigamesGUIObject = Instantiate(Resources.Load("MinigamesGUI")) as GameObject;
        minigamesGUI = minigamesGUIObject.GetComponent<MinigamesGUI>();
        
        //make GUI a child of MGC, to be part of it and don't be destroyed during scene switching
        minigamesGUIObject.transform.parent = this.transform;

        //minigamesGUI = this.gameObject.AddComponent<MinigamesGUI>();

		//initiate kinect manager
		kinectManager = (GameObject)Instantiate (Resources.Load ("_KinectManager") as GameObject);
		kinectManager.transform.parent = this.transform;
	}

	void Update()
	{
		if(Input.GetKeyDown (KeyCode.Escape))
		   Application.Quit();

		if(Input.GetKeyDown (KeyCode.O))
			print ("GOOOOOOOOOOOOOOD");
	}

	void OnLevelWasLoaded (int level)
	{
		print ("[MGC] Scene: '" + Application.loadedLevelName + "' loaded");
		MGC.Instance.logger.addEntry ("Scene loaded: '" + Application.loadedLevelName + "'");
		Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);

		//perform fade in?
		if (MGC.Instance.sceneLoader.doFade)
		{
			MGC.Instance.sceneLoader.FadeIn ();
		}
		else
		{
			gameObject.guiTexture.enabled = false;
		}
            

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
	void OnGUI ()
	{
		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 80, 110, 30), "Brain")) {
			Application.LoadLevel ("Main");
		}
		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 120, 110, 30), "Game Selection")) {
			Application.LoadLevel ("GameSelection");
		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 160, 110, 30), "QUIT")) {
//			Application.Quit ();
//		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 200, 110, 30), "Save")) {
//			SaveGame ();
//		}
//		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 240, 110, 30), "Load")) {
//			LoadGame ();
//		}
		if (GUI.Button (new Rect (Screen.width - 130, Screen.height - 280, 110, 30), "Reset status")) {
			ResetGameStatus ();
		}
	}

	public void SaveGame()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/newron.sav");

		foreach(Game.Minigame minigameData in minigameStates.minigames)
		{
			bf.Serialize(file, minigameData);
		}

		print ("Game saved.");
		file.Close();
	}

	public void LoadGame()
	{
		if(File.Exists(Application.persistentDataPath + "/newron.sav"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/newron.sav", FileMode.Open);
			for(int i=0; i<minigameStates.minigames.Count; ++i)
			{
				minigameStates.minigames[i] = (Game.Minigame)bf.Deserialize(file);
			}

			print ("Game loaded.");
			file.Close();
		}

        if (Application.loadedLevelName == "GameSelection")
        {
            sceneLoader.LoadScene("GameSelection");
        }
	}

	void ResetGameStatus()
	{		
		foreach(Minigame minigameData in this.GetComponent<MinigameStates>().minigames)
		{
			minigameData.played = false;
		}

		print ("Game statuses were reset to 'not yet played' (Minigame.played == false)");

        if (Application.loadedLevelName == "GameSelection")
        {
            sceneLoader.LoadScene("GameSelection");
        }
	}




		////// (optional) allow runtime registration of global objects
		//static public T RegisterComponent<T>()
		//{
		//    return Instance.GetOrAddComponent<T>();
		//}
}