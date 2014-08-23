/*
 * Created by: Milan Doležal
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game 
{
	[System.Serializable]
	public class Minigame
	{
		internal string minigameName;
		internal bool played;

		public Minigame(string minigameName)
		{
			this.minigameName = minigameName;
			played = false;
		}
	}

	public class MinigameStates : MonoBehaviour 
    {
		/// <summary>
        /// Status of each minigame. True if minigame was played.
        /// Minigames needs to be set in Start() function manualy in this script.
		/// </summary>
		public List<Minigame> minigames = new List<Minigame>();

		void Start()
		{
			//Set your minigame here (don't forget to add it into collection too):
			Minigame hanoi = new Minigame ("HanoiTowers");
			minigames.Add (hanoi);
			Minigame pexeso = new Minigame ("Pexeso");
			minigames.Add (pexeso);
			Minigame silhouette = new Minigame ("Silhouette");
			minigames.Add (silhouette);
//			Minigame splashScreen = new Minigame ("SplashScreen");
//			minigames.Add (splashScreen);
			Minigame puzzle = new Minigame ("Puzzle");
			minigames.Add (puzzle);
		}
		
		public void SetPlayed(string minigameName)
		{
			foreach(Minigame game in minigames)
			{
				if(game.minigameName == minigameName)
				{
					game.played = true;
					break;
				}
			}
		}

		public bool GetPlayed(string minigameName)
		{
			foreach(Minigame game in minigames)
				if(game.minigameName == minigameName)
			{
				print (game.played);
					return game.played;
			}
			return false;
		}
	}
}