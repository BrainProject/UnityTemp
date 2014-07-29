/*
 * Created by: Milan Doležal
 */ 

//This script exists across the whole game.

using UnityEngine;
using System.Collections;

public enum currentBrainPartEnum{none, FrontalLobe, ParietalLobe, OccipitalLobe};

public class GameManager : MonoBehaviour {
	public currentBrainPartEnum selectedBrainPart;
	public Vector3 currentCameraDefaultPosition;


	void Start()
	{
		//destroy this game object, if another game manager is present
		if(GameObject.Find ("_GameManager") != this.gameObject)
			Destroy(this.gameObject);

		DontDestroyOnLoad (this.gameObject);
	}

	void OnGUI()
	{
		if(GUI.Button (new Rect(20,20,100,30), "Brain"))
			Application.LoadLevel("NewMain");
		if(GUI.Button(new Rect(20,60,100,30), "MirkaSelection"))
			Application.LoadLevel("MirkaSelection");
	}

	void OnLevelWasLoaded(int level)
	{
		if(level == 2)
		{
			print ("Selection scene");
			//IN DEV: default position - need to implement automatic search for waypoints
			switch(selectedBrainPart)
			{
			case currentBrainPartEnum.FrontalLobe: Camera.main.transform.position = GameObject.Find ("OrangePos").transform.position;
				break;
			case currentBrainPartEnum.ParietalLobe: Camera.main.transform.position = GameObject.Find ("BluePos").transform.position;
				break;
			case currentBrainPartEnum.OccipitalLobe: Camera.main.transform.position = GameObject.Find ("GreenPos").transform.position;
				break;
			default: Camera.main.transform.position = GameObject.Find ("BluePos").transform.position;
				print ("default");
				break;
			}
			currentCameraDefaultPosition = Camera.main.transform.position;
		}
	}
}
