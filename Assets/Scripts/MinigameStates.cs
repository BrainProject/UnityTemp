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
		public string initialScene;
		public string sceneWithHelp;
		internal bool played;
		//internal List<string> scenes = new List<string>();

		public Minigame(string minigameName)
		{
			this.initialScene = minigameName;
			this.sceneWithHelp = minigameName;
			played = false;
		}

		public Minigame(string minigameName, string sceneWithHelp)
		{
			this.initialScene = minigameName;
			this.sceneWithHelp = sceneWithHelp;
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
			Minigame similarities = new Minigame ("Similaries");
			minigames.Add (similarities);
			Minigame silhouette = new Minigame ("Silhouettes");
			minigames.Add (silhouette);
			Minigame puzzle = new Minigame ("Puzzle", "PuzzleGame");
			minigames.Add (puzzle);
			Minigame coloring = new Minigame ("Coloring");
			minigames.Add (coloring);
			Minigame findIt = new Minigame ("FindIt", "FindItGame");
			minigames.Add (findIt);

			MGC.Instance.LoadGame ();
		}
		
		public void SetPlayed(string minigameName)
		{
			foreach(Minigame game in minigames)
			{
				if(game.initialScene == minigameName)
				{
					game.played = true;
					break;
				}
			}

			MGC.Instance.SaveGame ();
		}

		public bool GetPlayed(string minigameName)
		{
			foreach(Minigame game in minigames)
			if(game.initialScene == minigameName)
			{
				return game.played;
			}
			return false;
		}

		public List<string> GetMinigamesWithHelp()
		{
			List <string> minigamesWithHelp = new List<string> ();
			foreach(Minigame game in minigames)
				minigamesWithHelp.Add(game.sceneWithHelp);
			return minigamesWithHelp;
		}
	}
}