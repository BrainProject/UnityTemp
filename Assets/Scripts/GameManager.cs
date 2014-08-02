/*
 * Created by: Milan Doležal
 */ 

//This script exists across the whole game.

using UnityEngine;
using System.Collections;

namespace Game {
	public enum currentBrainPartEnum{none, FrontalLobe, ParietalLobe, OccipitalLobe};

	public class GameManager : MonoBehaviour {
		public currentBrainPartEnum selectedBrainPart;
		public Vector3 currentCameraDefaultPosition;
		//public GameObject selectedMinigame;
		internal bool fromMain;

		void Start()
		{
			//destroy this game object, if another game manager is present
			if(GameObject.Find ("_GameManager") != this.gameObject)
				Destroy(this.gameObject);
			fromMain = false;
			DontDestroyOnLoad (this.gameObject);
		}

		void OnGUI()
		{
			if(GUI.Button (new Rect(20,Screen.height - 40,100,30), "Brain"))
				Application.LoadLevel("NewMain");
			if(GUI.Button(new Rect(20,Screen.height - 80,100,30), "MirkaSelection"))
				Application.LoadLevel("MirkaSelection");
		}

		void OnLevelWasLoaded(int level)
		{
			if(level == 2)
			{
				//if(fromMain)
				//{
					print ("Selection scene");
					switch(selectedBrainPart)
					{
					case currentBrainPartEnum.FrontalLobe: //Camera.main.transform.position = GameObject.Find ("GreenPos").transform.position;
						Camera.main.GetComponent<MinigameSelection.CameraControl>().currentWaypoint = GameObject.Find ("GreenPos");
						break;
					case currentBrainPartEnum.ParietalLobe: //Camera.main.transform.position = GameObject.Find ("BluePos").transform.position;
						Camera.main.GetComponent<MinigameSelection.CameraControl>().currentWaypoint = GameObject.Find ("BluePos");
						break;
					case currentBrainPartEnum.OccipitalLobe: //Camera.main.transform.position = GameObject.Find ("OrangePos").transform.position;
						Camera.main.GetComponent<MinigameSelection.CameraControl>().currentWaypoint = GameObject.Find ("OrangePos");
						break;
					}
					if(fromMain)
						currentCameraDefaultPosition = Camera.main.GetComponent<MinigameSelection.CameraControl>().currentWaypoint.transform.position;
					if(!fromMain)
						Camera.main.GetComponent<MinigameSelection.CameraControl>().ReadyToLeave = false;
					Camera.main.transform.position = Camera.main.GetComponent<MinigameSelection.CameraControl>().currentWaypoint.transform.position;
					fromMain = false;
				//}
			}
		}
	}
}