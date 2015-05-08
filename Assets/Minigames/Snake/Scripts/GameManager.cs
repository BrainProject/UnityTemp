using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect;


namespace MinigameSnake
{
public class GameManager : MonoBehaviour {
	public int score;
	public int currentLevel;
	public bool death;
	public bool winning;
	public bool game;
	public AudioClip eating;

	void Start()
	{

		death = false;
		winning = false;
		game = false;
		score = 0;
		
		
		//Sets the position of GUI Text with score
		this.guiText.pixelOffset = new Vector2 (Screen.width/6,Screen.height/2);
	}

	void Update()
	{
			this.guiText.text = "Score: " + score;
	}

	public void AddPoints(){
		score++;
		print (score);
	}

	public int LastScore(){
		return score;
	}
	public void EraseScore(){
		score = 0;
	}

	public void TakeOffPointsAddedAfterSnakeDead(int i){
		score = score - 3;
	}
	public string NextLevel()
	{
		currentLevel++;
		return "scene" + currentLevel;
		}
	public bool Alive()
	{
		death = false;
		return death;
	}
	public bool Dead()
	{
		death = true;
		return death;
	}
	public void Winning()
	{
		winning = true;
	}
	public void NotWinning()
	{
		winning = false;
	}
	public void SetLevel(int number)
	{
		currentLevel = number;
	}
	
}
}