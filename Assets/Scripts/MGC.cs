using UnityEngine;
using System.Collections;
using MainScene;
using MinigameSelection;


//NOTE: no namespace here as it should be accessible globally
//namespace Game
//{

public enum BrainPartName
{
    none,
    FrontalLobe,
    ParietalLobe,
    OccipitalLobe
};

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
        protected MGC() { } // guarantee this will be always a singleton only - can't use the constructor!

        /// <summary>
        /// Logs players actions
        /// </summary>
        public Logger logger;

        public Game.LevelLoader levelLoader;

        //public Game.LoadLevelWithFade loadLevelWithFade;

        public BrainPartName selectedBrainPart;
        public Vector3 currentCameraDefaultPosition;
        //public GameObject selectedMinigame;
        public string gameSelectionSceneName = "GameSelection";

        internal bool fromMain;
        internal bool fromSelection;
        internal bool fromMinigame;


        void Start()
        {
            print("Master Game Controller starts...");

            //Initiate Logger
            logger = this.gameObject.AddComponent<Logger>();
            logger.Initialize("Logs", "PlayerActions.txt");

            //initiate level loader
            levelLoader = this.gameObject.AddComponent<Game.LevelLoader>();
                        
        }

        void OnLevelWasLoaded(int level)
        {
            print("[MGC] Scene: '" + Application.loadedLevelName + "' loaded");
            MGC.Instance.logger.addEntry("Scene loaded: '" + Application.loadedLevelName + "'");

            //perform fade in?
            if (MGC.Instance.levelLoader.doFade)
            {
                MGC.Instance.levelLoader.FadeIn();
            }
            else
            {
                gameObject.guiTexture.enabled = false;
            }
            

            //loadLevelWithFade.LoadSeledctedLevelWithColorLerp()
            //print("calling object ID: " + this.GetInstanceID());

            if (Application.loadedLevelName == gameSelectionSceneName)
            {
                print("this is game selection scene...");
                switch (selectedBrainPart)
                {
                    case BrainPartName.FrontalLobe: //Camera.main.transform.position = GameObject.Find ("GreenPos").transform.position;
                        Camera.main.GetComponent<CameraControl>().currentWaypoint = GameObject.Find("GreenPos");
                        break;
                    case BrainPartName.ParietalLobe: //Camera.main.transform.position = GameObject.Find ("BluePos").transform.position;
                        Camera.main.GetComponent<CameraControl>().currentWaypoint = GameObject.Find("BluePos");
                        break;
                    case BrainPartName.OccipitalLobe: //Camera.main.transform.position = GameObject.Find ("OrangePos").transform.position;
                        Camera.main.GetComponent<CameraControl>().currentWaypoint = GameObject.Find("OrangePos");
                        break;
                }
                if (fromMain)
                {
                    currentCameraDefaultPosition = Camera.main.GetComponent<CameraControl>().currentWaypoint.transform.position;
                }

                //if player comes to selection scene from main, he can leave immediately by pressing Vertical key
                Camera.main.GetComponent<CameraControl>().ReadyToLeave = fromMain;
                Camera.main.transform.position = Camera.main.GetComponent<CameraControl>().currentWaypoint.transform.position;
                fromMain = false;
            }

            if (level > 2)
            {

                this.GetComponent<Game.MinigameStates>().SetPlayed(Application.loadedLevelName);
            }
        }

        //Only for debugging and testing purposes
        void OnGUI()
        {
            if (GUI.Button(new Rect(Screen.width - 130, Screen.height - 80, 110, 30), "Brain"))
            {
                Application.LoadLevel("Main");
            }
            if (GUI.Button(new Rect(Screen.width - 130, Screen.height - 120, 110, 30), "Game Selection"))
            {
                Application.LoadLevel("GameSelection");
            }
            if (GUI.Button(new Rect(Screen.width - 130, 20, 110, 30), "QUIT"))
            {
                Application.Quit();
            }
        }




        ////// (optional) allow runtime registration of global objects
        //static public T RegisterComponent<T>()
        //{
        //    return Instance.GetOrAddComponent<T>();
        //}
    }

//}