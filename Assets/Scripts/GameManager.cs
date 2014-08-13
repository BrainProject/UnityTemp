/*
 * Created by: Milan Doležal
 */ 

//This script exists across the whole game.

using UnityEngine;
using System.Collections;
using MainScene;
using MinigameSelection;

namespace Game 
{
	public enum currentBrainPartEnum
    {
        none, 
        FrontalLobe, 
        ParietalLobe, 
        OccipitalLobe
    };

	public class GameManager : MonoBehaviour 
    {
		public currentBrainPartEnum selectedBrainPart;
		public Vector3 currentCameraDefaultPosition;
		//public GameObject selectedMinigame;
        public string gameSelectionSceneName = "GameSelection";

		internal bool fromMain;
		internal bool fromSelection;
		internal bool fromMinigame;

        bool toBeDestroyed = false;
		void Awake()
		{
			//destroy this game manager, if another one is present
            if (GameObject.Find("_GameManager") != this.gameObject)
            {
                print("Destroying redundant game manager... instance ID: " + this.GetInstanceID());

                toBeDestroyed = true;
                Destroy(this.gameObject);
            }
		}

		void Start()
		{
            if (!toBeDestroyed)
            {
                fromMain = false;
                fromSelection = false;
                DontDestroyOnLoad(this.gameObject);
            }
		}

		void OnLevelWasLoaded(int level)
        {
            if (!toBeDestroyed)
            {
                print("Scene: '" + Application.loadedLevelName + "' successfully loaded");
                //print("calling object ID: " + this.GetInstanceID());
                if (Application.loadedLevelName == gameSelectionSceneName)
                {
                    print("this is game selection scene...");
                    switch (selectedBrainPart)
                    {
                        case currentBrainPartEnum.FrontalLobe: //Camera.main.transform.position = GameObject.Find ("GreenPos").transform.position;
                            Camera.main.GetComponent<CameraControl>().currentWaypoint = GameObject.Find("GreenPos");
                            break;
                        case currentBrainPartEnum.ParietalLobe: //Camera.main.transform.position = GameObject.Find ("BluePos").transform.position;
                            Camera.main.GetComponent<CameraControl>().currentWaypoint = GameObject.Find("BluePos");
                            break;
                        case currentBrainPartEnum.OccipitalLobe: //Camera.main.transform.position = GameObject.Find ("OrangePos").transform.position;
                            Camera.main.GetComponent<CameraControl>().currentWaypoint = GameObject.Find("OrangePos");
                            break;
                    }
                    if (fromMain)
                        currentCameraDefaultPosition = Camera.main.GetComponent<CameraControl>().currentWaypoint.transform.position;

                    //if player comes to selection scene from main, he can leave immediately by pressing Vertical key
                    Camera.main.GetComponent<CameraControl>().ReadyToLeave = fromMain;
                    Camera.main.transform.position = Camera.main.GetComponent<CameraControl>().currentWaypoint.transform.position;
                    fromMain = false;
                }

                if (level > 3)
                {

                    this.GetComponent<MinigameStates>().SetPlayed(Application.loadedLevelName);
                }
            }
		}

		//Only for debugging and testing purposes
		void OnGUI()
		{
			if(GUI.Button (new Rect(Screen.width - 120,Screen.height - 80,100,30), "Brain"))
				Application.LoadLevel("Main");
			if(GUI.Button(new Rect(Screen.width - 120,Screen.height - 120,100,30), "Game Selection"))
				Application.LoadLevel("GameSelection");
			if(GUI.Button(new Rect(Screen.width - 120,20,100,30), "QUIT"))
				Application.Quit();
		}
	}
}