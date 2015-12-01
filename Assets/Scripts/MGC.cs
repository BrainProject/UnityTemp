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
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MainScene;
using MinigameSelection;
using Game;
using Kinect;

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
    protected MGC()
    {
    }

    public BrainPartName currentBrainPart;
	public Vector3 currentCameraDefaultPosition;
	public string mainSceneName = "Main";
	public string gameSelectionSceneName = "GameSelection";
	public GameObject confetti;

    /// <summary>
    /// Logs players actions
    /// </summary>
    internal Logger logger;
    internal SceneLoader sceneLoader;

    internal Minigames minigamesProperties;
    internal GameObject kinectManagerObject;
    internal KinectManager kinectManagerInstance;
    internal GameObject mouseCursor;
	internal GameObject neuronHelp;
	internal MenuType menuType = MenuType.None;

    internal GameObject minigamesGUIObject;
    internal MinigamesGUI minigamesGUI;
    internal bool fromMain;
    internal bool fromSelection;
	internal bool isControlTakenForGUI;

    // name of MiniGame scene to be loaded
    internal string selectedMiniGameName;

    // minigame will be started with this difficulty
    internal int selectedMiniGameDiff { get; set; }

    /// <summary>
    /// Path, where painting created by players will be stored
    /// </summary>
    internal string pathtoPaintings;

    /// <summary>
    /// Path, where users shloud put their custom image sets...
    /// </summary>
    internal string pathtoCustomImageSets;

    internal string inactivityScene = "Figure";
	internal string alternativeInteractionScene = "HanoiTowers";
    internal bool checkInactivity = true;

    private float inactivityTimestamp;
    private float inactivityLenght = 60f;
    private int inactivityCounter = 1;
    private GameObject controlsGUI;
#if UNITY_STANDALONE
    private bool isKinectUsed = false; // to disable all future checks if Kinect is not initialized in the beginning
#elif UNITY_ANDROID
    private float touchBlockTimestamp;
#endif




    internal Vector2 initialTouchPosition = Vector2.zero;
    internal Vector2 swipeDistance = Vector2.zero;

    void Awake()
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
        print("Master Game Controller Awake()...");

		//Show custom cursor
		ShowCustomCursor (true);

        //Initiate Logger
        logger = this.gameObject.AddComponent<Logger>();
        logger.Initialize(Application.persistentDataPath + "/Logs", "PlayerActions.txt");

        //initiate level loader
        sceneLoader = this.gameObject.AddComponent<SceneLoader>();

        //initiate minigame properties
        minigamesProperties = this.gameObject.AddComponent<Minigames>();
        minigamesProperties.loadConfigurationsfromFile();
        LoadMinigamesStatisticsFromFile();

        //initiate minigames GUI
        minigamesGUIObject = Instantiate(Resources.Load("MinigamesUI")) as GameObject;
        if (minigamesGUIObject == null)
        {
            Debug.LogError("Nenelazen prefab pro MinigamesGUI");
        }

        //make GUI a child of MGC 
		minigamesGUIObject.transform.SetParent(this.transform);

        minigamesGUI = minigamesGUIObject.GetComponent<MinigamesGUI>();
        if (minigamesGUI == null)
        {
            Debug.LogError("komponenta minigamesGUI nenalezena - špatný prefab?");
        }

        //set up path, check or create directories
        pathtoPaintings = Application.persistentDataPath + "/Pictures/";
        pathtoCustomImageSets = Application.persistentDataPath + "/CustomImages/";

        Directory.CreateDirectory(pathtoPaintings);
        Directory.CreateDirectory(pathtoCustomImageSets);
        

        //initiate kinect manager
        Debug.Log("Trying to create KinectManager.");
        kinectManagerObject = (GameObject)Instantiate(Resources.Load("_KinectManager") as GameObject);
        kinectManagerObject.transform.parent = this.transform;

		//create UI event system
		GameObject eventSystem = (GameObject)Instantiate (Resources.Load ("EventSystem") as GameObject);
		eventSystem.transform.parent = this.transform;

		
		isControlTakenForGUI = false;
    }

    void Start()
    {
        print("Master Game Controller Start()...");

        //TODO ...
        //due to unknown reason, I doesn't set the list
        //in the Minigames script correctly without this command.
        minigamesProperties.Start();

        inactivityTimestamp = Time.time;
#if !UNITY_STANDALONE
		inactivityScene = "HanoiTowers";
#endif



#if UNITY_STANDALONE
        kinectManagerInstance = Kinect.KinectManager.Instance;

        //should the KinectManager be active?
        //Debug.Log ("Windows version: " + Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor);


        StartCoroutine(CheckKinect());


        /*if(Environment.OSVersion.Version.Major <= 6 && Environment.OSVersion.Version.Minor < 2)	//is Windows version is lower than Windows 8?
        {
            if(!kinectManager.transform.GetChild(0).GetComponent<Kinect.KinectManager>().IsInitialized())
            {

                print(kinectManager.transform.GetChild(0).GetComponent<Kinect.KinectManager>().IsInitialized());
                kinectManager.SetActive(false);
            }
        }*/
#else
		kinectManagerObject.SetActive(false);
#endif
        swipeDistance = new Vector2(Screen.width / 4, Screen.width / 3);
    }

    void Update()
    {


        /*
        if (Input.GetMouseButtonDown(0))
        {
            initialTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && (initialTouchPosition.x != 0))
        {
            if (((initialTouchPosition.x - Input.mousePosition.x) < -swipeDistance.x))
            {
                Kinect.Win32.MouseKeySimulator.SendKeyDown(Kinect.Win32.KeyCode.KEY_D);
                Kinect.Win32.MouseKeySimulator.SendKeyUp(Kinect.Win32.KeyCode.KEY_D);
            }
            else if (((initialTouchPosition.x - Input.mousePosition.x) > swipeDistance.x))
            {
                Kinect.Win32.MouseKeySimulator.SendKeyDown(Kinect.Win32.KeyCode.KEY_A);
                Kinect.Win32.MouseKeySimulator.SendKeyUp(Kinect.Win32.KeyCode.KEY_A);
            }
            initialTouchPosition.x = 0;
        }
        if (Input.GetMouseButtonUp(0) && (initialTouchPosition.y != 0))
        {
            if (((initialTouchPosition.y - Input.mousePosition.y) < -swipeDistance.y))
            {
                Kinect.Win32.MouseKeySimulator.SendKeyDown(Kinect.Win32.KeyCode.KEY_W);
                Kinect.Win32.MouseKeySimulator.SendKeyUp(Kinect.Win32.KeyCode.KEY_W);
            }
            else if (((initialTouchPosition.y - Input.mousePosition.y) > swipeDistance.y))
            {
                Kinect.Win32.MouseKeySimulator.SendKeyDown(Kinect.Win32.KeyCode.KEY_S);
                Kinect.Win32.MouseKeySimulator.SendKeyUp(Kinect.Win32.KeyCode.KEY_S);
            }
            initialTouchPosition.y = 0;
        }*/


#if UNITY_ANDROID
        //if(Input.touchCount == 3 && ((Time.time - touchBlockTimestamp) > 2) || Input.GetKeyDown (KeyCode.I))
        //{
        //    touchBlockTimestamp = Time.time;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
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

        if(Input.touchCount == 10 && ((Time.time - touchBlockTimestamp) > 2))
		{
			touchBlockTimestamp = Time.time;
            ResetMinigamesStatistics();
        }


        if (Input.touchCount == 4 && ((Time.time - touchBlockTimestamp) > 2))
        {
            menuType = MenuType.Brain;
            touchBlockTimestamp = Time.time;
        }
        if (Input.touchCount == 5 && ((Time.time - touchBlockTimestamp) > 2))
        {
            menuType = MenuType.Tiles;
            touchBlockTimestamp = Time.time;
        }


        //PC ...
#else
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
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

        //Debug actions
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Application.LoadLevel("Main");
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Application.LoadLevel(gameSelectionSceneName);

            //TODO test only...
            minigamesProperties.printStatisticsToFile();
        }
		if(Input.GetKeyDown(KeyCode.F8))
		{
			ResetMinigamesStatistics();
			if(Application.loadedLevelName == gameSelectionSceneName)
				Application.LoadLevel(gameSelectionSceneName);
		}
    
#endif

        //Change of menu types
        if (Input.GetKeyDown(KeyCode.Keypad0))
		{
			menuType = MenuType.None;
			Debug.Log("Changed menu type to default.");
		}
		if(Input.GetKeyDown(KeyCode.Keypad1))
		{
			menuType = MenuType.Brain;
			Debug.Log("Changed menu type to brain.");
		}
		if(Input.GetKeyDown(KeyCode.Keypad2))
		{
			menuType = MenuType.Tiles;
			Debug.Log("Changed menu type to tiles.");
		}
		if(Input.GetKeyDown(KeyCode.Keypad3))
		{
			menuType = MenuType.GSI;
			Debug.Log("Changed menu type to gsi.");
		}

        
        //Inactivity detection
        if (Input.anyKeyDown)
        {
            inactivityTimestamp = Time.time;
            inactivityCounter = 0;
        }

        if (checkInactivity)
        {
            if (Time.time - inactivityTimestamp > inactivityLenght)
            {
                InactivityReaction();
            }
        }
    }

    void OnLevelWasLoaded(int level)
    {
        inactivityTimestamp = Time.time;
        inactivityCounter = 0;
        print("[MGC] Scene: '" + Application.loadedLevelName + "' loaded");
        MGC.Instance.logger.addEntry("Scene loaded: '" + Application.loadedLevelName + "'");

        //perform fade in?
        if (MGC.Instance.sceneLoader.doFade)
        {
            MGC.Instance.sceneLoader.FadeIn();
        }

		if (minigamesProperties.GetPlayed (Application.loadedLevelName))
		{
			Debug.Log ("The minigame was already visited, don't show help.");
		}
		else
		{
			if(level > 4)
			{
				if(minigamesProperties.GetMinigame(Application.loadedLevelName))
				{
					if(minigamesProperties.IsWithHelp(Application.loadedLevelName))
					{
		//				Debug.Log(minigamesProperties.GetMinigame(Application.loadedLevelName));
						neuronHelp.GetComponent<NEWBrainHelp>().helpObject.ShowHelpAnimation();
						minigamesProperties.SetPlayedWithHelp(Application.loadedLevelName);
					}
				}
			}
		}
//        else
//        {
//            gameObject.guiTexture.enabled = false;
//        }

		if(Application.loadedLevelName == "GameSelection" || Application.loadedLevelName == "Main")
		{
#if UNITY_STANDALONE
            if(isKinectUsed && !kinectManagerObject.activeSelf)
			{
				kinectManagerObject.SetActive(true);
				kinectManagerInstance.ClearKinectUsers();
				//kinectManagerInstance.StartKinect();
				kinectManagerInstance.avatarControllers.Clear();
			}
#endif


            if (!mouseCursor.activeSelf)
			{
				ShowCustomCursor(true);
			}

			isControlTakenForGUI = false;
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
        try
        {
            if (File.Exists(Application.persistentDataPath + "/mini-games.stats"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/mini-games.stats", FileMode.Open);
                FileInfo info = new FileInfo(file.Name);
                if (info.Length > 0)
                {
                    for (int i = 0; i < minigamesProperties.minigames.Count; ++i)
                    {
                        minigamesProperties.minigames[i].stats = (Game.MinigameStatistics)bf.Deserialize(file);
                    }

                    print("Saved mini-games statistics was loaded.");
                }
                else
                {
                    Debug.LogError("Saved mini-games statistics was NOT loaded.");
                }
                file.Close();
            }

            // if file doesn't exists, allocate new statistics and save initial version of file
            else
            {
                ResetMinigamesStatistics();
            }

		    if (Application.loadedLevelName == gameSelectionSceneName)
		    {
			    sceneLoader.LoadScene(gameSelectionSceneName);
		    }
        }
        catch (EndOfStreamException ex)
        {
            Debug.LogWarning("Minigame statistics not loaded because of EndOfStreamException!\n" + ex);
        }
#endif
    }


    public void ShowCustomCursor(bool isShown)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		Debug.Log("No cursor on Android is needed.");
#else
        if (isShown)
        {
            if (!mouseCursor)
            {
                mouseCursor = (GameObject)Instantiate(Resources.Load("CursorUI") as GameObject);
				mouseCursor.transform.GetChild(0).GetComponent<Image>().enabled = true;
                mouseCursor.transform.SetParent(this.transform);
            }
            else
            {
                mouseCursor.SetActive(true);
				mouseCursor.GetComponent<CursorReference>().cursorReference.CursorToNormal();
            }
        }
        else
        {
            if (!mouseCursor)
            {
                mouseCursor = (GameObject)Instantiate(Resources.Load("CursorUI") as GameObject);
				mouseCursor.transform.GetChild(0).GetComponent<Image>().enabled = false;
				mouseCursor.transform.SetParent(this.transform);
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

    /// <summary>
    /// resets statistics of all mini-games
    /// </summary>
    /// creates new data-structure and fills it for each mini-game
    /// saves freshly created data to file
    public void ResetMinigamesStatistics()
    {
        print("Reset...");
        Minigames minigames = this.GetComponent<Minigames>();
        if (minigames != null)
        {


            foreach (MinigameProperties minigameProperties in this.GetComponent<Minigames>().minigames)
            {
                print("   reseting minigame: " + minigameProperties.readableName);
                Game.MinigameStatistics newstats = new MinigameStatistics();
                newstats.played = false;
                newstats.initialShowHelpCounter = 0;
                newstats.DifficutlyLastPlayed = 0;
                newstats.playedCount = new int[minigameProperties.MaxDifficulty + 1];
                newstats.finishedCount = new int[minigameProperties.MaxDifficulty + 1];

                minigameProperties.stats = newstats;
            }

            SaveMinigamesStatisticsToFile();

            print("Mini-games statistics were reseted");
        }

        else
        {
            Debug.LogError("Missing component in MGC...");
        }


        //TODO ?question by jch? what is purpose of this code?
        //if (Application.loadedLevelName == "GameSelection")
        //{
        //    sceneLoader.LoadScene("GameSelection");
        //}
    }

    public void ShowHelpBubble()
    {
        if (neuronHelp)
        {
			neuronHelp.GetComponent<NEWBrainHelp>().helpObject.ShowHelpAnimation();
        }
    }

	public delegate void TakeControlForGUI(bool isShown);

	public static event TakeControlForGUI TakeControlForGUIEvent;

	public void TakeControlForGUIAction(bool isShown)
	{
		if(!isShown)
		{
			isControlTakenForGUI = false;
			if(TakeControlForGUIEvent != null)
				TakeControlForGUIEvent(isShown);
		}
		else
		{
			if(!isControlTakenForGUI)
			{
				isControlTakenForGUI = true;
				if(TakeControlForGUIEvent != null)
					TakeControlForGUIEvent(isShown);
			}
		}
    }

    void InactivityReaction()
    {
        print("Inactive in " + Application.loadedLevelName + " for " + inactivityLenght * inactivityCounter + " seconds.");
        logger.addEntry("Inactive in " + Application.loadedLevelName + " for " + inactivityLenght * inactivityCounter + " seconds.");
        if (inactivityCounter == 5)
        {
            inactivityCounter = 0;
			ShowCustomCursor(true);
            print("Load another scene. Im getting bored here.");
            if (Application.loadedLevelName != inactivityScene)
            {
                //load inactivityMinigame
				if(kinectManagerObject.activeSelf)
	                Application.LoadLevel(inactivityScene);
				else
					Application.LoadLevel(alternativeInteractionScene);
            }
            else
            {
                //load either previous scene or selection scene
                Application.LoadLevel(1);
            }
        }
        else
        {
            if (neuronHelp)
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
    public MinigameProperties getSelectedMinigameProperties()
    {
        return minigamesProperties.GetMinigame(selectedMiniGameName);
    }

    public void startMiniGame(string name)
    {
        try
        {
            //store the name of selected minigame
            selectedMiniGameName = name;

            // check, if difficulty is applicable for this mini-game
            // if not, run it directly
            if (getSelectedMinigameProperties().MaxDifficulty == 0)
            {
                sceneLoader.LoadScene(selectedMiniGameName);
            }

            // first, load difficulty chooser scene, mini-game will be loaded from that scene
            else
            {
                sceneLoader.LoadScene("DifficultyChooser");
            }
        }
        catch(NullReferenceException ex)
        {
            Debug.LogWarning("Minigame not did not start correctly.\n" + ex);
        }
    }



    // evoked when player successfuully finish minigame
    public void WinMinigame()
    {
        print("Minigame successffully finished");

        //TODO Add "Celebration phase" stuff here
        //animate Neuron character
        if (neuronHelp)
        {
			Game.BrainHelp neuron = neuronHelp.GetComponent<Game.BrainHelp>();
			neuron.ShowSmile(Resources.Load("Neuron/smilyface") as Texture);
			neuron.LaunchConfetties();
        }

        minigamesProperties.SetSuccessfullyPlayed(selectedMiniGameName, selectedMiniGameDiff);
        SaveMinigamesStatisticsToFile();

        //global GUI
        MGC.Instance.minigamesGUI.show(true);
    }

	public void LoseMinigame()
	{
        print("Minigame successffully unfinished");

		//animate Neuron character
		if (neuronHelp)
		{
			neuronHelp.GetComponent<Game.BrainHelp>().ShowSmile(Resources.Load("Neuron/sadface") as Texture);
		}
				
		//global GUI
		MGC.Instance.minigamesGUI.show(true);
	}


    public Minigames getMinigameStates()
    {
        return minigamesProperties;
    }

    public string getPathtoPaintings()
    {
        return pathtoPaintings;
    }

    public string getPathtoCustomImageSets()
    {
        return pathtoCustomImageSets;
    }


    /// <summary>
    /// Checks the Kinect connection.
    /// </summary>
    /// 
    /// //Please don't look at this function. It's nasty, but it works...
    private IEnumerator CheckKinect()
    {
#if UNITY_STANDALONE
        kinectManagerInstance = KinectManager.Instance;
        // Wait until all is initialized.
        yield return new WaitForSeconds(1);

        // Set more aggressive smoothing for cursor if Kinect1 is connected.
        if (Kinect.KinectInterop.GetSensorType() == "Kinect1Interface")
        {
            Kinect.InteractionManager im = Kinect.InteractionManager.Instance;
			im.smoothFactor = 5;
			isKinectUsed = true;
        }

        kinectManagerInstance = KinectManager.Instance;

        if(!kinectManagerInstance.IsInitialized())
        {
            kinectManagerObject.SetActive(false);
        }
        /*int sensorsCount = 0;

        
        try
        {
            Debug.Log("Is kinect initialized: " + KinectManager.Instance.IsInitialized());
        }
        catch (NullReferenceException)
        {
            kinectManagerObject.SetActive(false);
            isKinectUsed = false;
            yield break;
        }


        if (KinectManager.Instance.IsInitialized())
        {
            KinectInterop.SensorData sensorData = kinectManagerInstance.GetSensorData();
            sensorsCount = (sensorData != null && sensorData.sensorInterface != null) ? sensorData.sensorInterface.GetSensorsCount() : 0;
            //Debug.Log("Connected sensors: " + sensorsCount);

            if (Kinect.KinectInterop.GetSensorType() == "Kinect2Interface" && sensorsCount < 2)
            {
                Destroy(kinectManagerObject);
                yield return new WaitForSeconds(1);
                Debug.Log("First Kinect 2 initialization failed. Trying to recreate KinectManager again.");
                kinectManagerObject = (GameObject)Instantiate(Resources.Load("_KinectManager") as GameObject);
				kinectManagerObject.transform.parent = this.transform;
				isKinectUsed = false;
            }
			else
			{
				isKinectUsed = true;
				yield return null;
			}
            // sensorsCount == 0 means no sensor is currently connected
        }

        if ((Kinect.KinectInterop.GetSensorType() == "Kinect1Interface" && sensorsCount == 0) || (Kinect.KinectInterop.GetSensorType() == "Kinect2Interface" && sensorsCount == 1))
        {
            kinectManagerObject.SetActive(false);
			Debug.Log("Something with Kinect initialization went terribly wrong! Thus disabling the KinectManager.");
			isKinectUsed = false;
        }*/
#else
		yield return null;
#endif

    }


}
