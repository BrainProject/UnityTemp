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
    public string gameSelectionSceneName = "GameSelection";
	public GameObject confetti;
    /// <summary>
    /// Logs players actions
    /// </summary>
    internal Logger logger;
    internal SceneLoader sceneLoader;
    internal Minigames minigamesProperties;
    internal GameObject kinectManagerObject;
    internal Kinect.KinectManager kinectManagerInstance;
    internal GameObject mouseCursor;
    internal GameObject neuronHelp;

    internal GameObject minigamesGUIObject;
    internal MinigamesGUI minigamesGUI;
    internal bool fromMain;
    internal bool fromSelection;

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
    internal bool checkInactivity = true;

    private float inactivityTimestamp;
    private float inactivityLenght = 60f;
    private int inactivityCounter = 1;
    private GameObject controlsGUI;
#if UNITY_ANDROID
	private float touchBlockTimestamp;
#endif

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
    }

    void Update()
    {

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

        if(Input.touchCount == 4 && ((Time.time - touchBlockTimestamp) > 2))
		{
			touchBlockTimestamp = Time.time;
            ResetMinigamesStatistics();
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
            Application.LoadLevel("GameSelection");

            //TODO test only...
            minigamesProperties.printStatisticsToFile();
        }

    #endif
        
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
        else
        {
//            gameObject.guiTexture.enabled = false;
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
        }

        kinectManagerInstance = KinectManager.Instance;
        int sensorsCount = 0;

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
            }
            // sensorsCount == 0 means no sensor is currently connected
        }

        if ((Kinect.KinectInterop.GetSensorType() == "Kinect1Interface" && sensorsCount == 0) || (Kinect.KinectInterop.GetSensorType() == "Kinect2Interface" && sensorsCount == 1))
        {
            kinectManagerObject.SetActive(false);
            Debug.Log("Something with Kinect initialization went terribly wrong! Thus disabling the KinectManager.");
        }

#else
		yield return null;
#endif

    }


}
