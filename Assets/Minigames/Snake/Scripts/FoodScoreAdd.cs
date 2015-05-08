using UnityEngine;
using System.Collections;
using Kinect;


namespace MinigameSnake
{
public class FoodScoreAdd : MonoBehaviour {
	static GameObject ob;
	static GameManager sc;
	public int score;
	public MGC controller;
	public int level;
	
	
	void Start () {
		ob = GameObject.Find("_Level Manager_");

		sc = (GameManager)ob.GetComponent(typeof(GameManager));

	}
	
	
	void OnDestroy () {
		sc.AddPoints ();
		
		if (ob != null) {
			score = ob.GetComponent<GameManager> ().LastScore ();
		
				if (score == 5 )
				{
				GameObject.Find ("_Level Manager_").GetComponent<GameManager> ().game = false;
				GameObject.Find("snake1").GetComponent<MoveSnake>().StopWon();
				Screen.showCursor = true;
				GameObject.Find ("_Level Manager_").GetComponent<GameManager> ().Winning ();
				}
		}
		}
	}
}
