/*
 * Created by: Milan Doležal
 */ 

//This script exists across the whole game.

using UnityEngine;
using System.Collections;
using MainScene;
using MinigameSelection;

namespace Game {
	public enum currentBrainPartEnum{none, FrontalLobe, ParietalLobe, OccipitalLobe};

	public class GameManager : MonoBehaviour {
		public currentBrainPartEnum selectedBrainPart;
		public Vector3 currentCameraDefaultPosition;
		//public GameObject selectedMinigame;
		internal bool fromMain;
		internal bool fromSelection;
		internal bool fromMinigame;

		void Awake()
		{
			//destroy this game manager, if another one is present
			if(GameObject.Find ("_GameManager") != this.gameObject)
				Destroy(this.gameObject);
		}

		void Start()
		{
			fromMain = false;
			fromSelection = false;
			DontDestroyOnLoad (this.gameObject);
		}

		void OnGUI()
		{
			if(GUI.Button (new Rect(20,Screen.height - 40,100,30), "Brain"))
				Application.LoadLevel("Main");
			if(GUI.Button(new Rect(20,Screen.height - 80,100,30), "Game Selection"))
				Application.LoadLevel("GameSelection");
		}

		void OnLevelWasLoaded(int level)
		{
			if(level == 2)
			{
				print ("Selection scene");
				switch(selectedBrainPart)
				{
				case currentBrainPartEnum.FrontalLobe: //Camera.main.transform.position = GameObject.Find ("GreenPos").transform.position;
					Camera.main.GetComponent<CameraControl>().currentWaypoint = GameObject.Find ("GreenPos");
					break;
				case currentBrainPartEnum.ParietalLobe: //Camera.main.transform.position = GameObject.Find ("BluePos").transform.position;
					Camera.main.GetComponent<CameraControl>().currentWaypoint = GameObject.Find ("BluePos");
					break;
				case currentBrainPartEnum.OccipitalLobe: //Camera.main.transform.position = GameObject.Find ("OrangePos").transform.position;
					Camera.main.GetComponent<CameraControl>().currentWaypoint = GameObject.Find ("OrangePos");
					break;
				}
				if(fromMain)
					currentCameraDefaultPosition = Camera.main.GetComponent<CameraControl>().currentWaypoint.transform.position;
				//if player comes to selection scene from main, he can leave immidietely by pressing Vertical key
				Camera.main.GetComponent<CameraControl>().ReadyToLeave = fromMain;
				Camera.main.transform.position = Camera.main.GetComponent<CameraControl>().currentWaypoint.transform.position;
				fromMain = false;
			}

			if(level > 2)
			{
				print (Application.loadedLevelName);
				this.GetComponent<MinigameStates> ().SetPlayed(Application.loadedLevelName);
			}
		}
	}
}