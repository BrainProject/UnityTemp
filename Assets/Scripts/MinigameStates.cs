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
		internal int initialShowHelpCounter;
		//internal List<string> scenes = new List<string>();

		public Minigame(string minigameName)
		{
			this.initialScene = minigameName;
			this.sceneWithHelp = minigameName;
			played = false;
			initialShowHelpCounter = 0;
		}

		public Minigame(string minigameName, string sceneWithHelp)
		{
			this.initialScene = minigameName;
			this.sceneWithHelp = sceneWithHelp;
			played = false;
			initialShowHelpCounter = 0;
		}
	}

	public class MinigameStates : MonoBehaviour 
    {
		/// <summary>
        /// Status of each minigame. True if minigame was played.
        /// Minigames needs to be set in Start() function manualy in this script.
		/// </summary>
		internal List<Minigame> minigames = new List<Minigame>();

		public void Start ()
		{
			Minigame main = new Minigame ("Main");
			minigames.Add (main);
			Minigame selection = new Minigame ("GameSelection");
			selection.initialShowHelpCounter = 2;
			minigames.Add (selection);
			//Set your minigame here (don't forget to add it into collection too):
			Minigame hanoi = new Minigame ("HanoiTowers");
			minigames.Add (hanoi);
			Minigame pexeso = new Minigame ("Pexeso");
			minigames.Add (pexeso);
			Minigame similarities = new Minigame ("Similarities");
			minigames.Add (similarities);
			Minigame silhouette = new Minigame ("Silhouettes");
			minigames.Add (silhouette);
			Minigame puzzle = new Minigame ("PuzzleChoosePicture", "PuzzleGame");
			minigames.Add (puzzle);
			Minigame coloring = new Minigame ("Coloring");
			minigames.Add (coloring);
			Minigame findIt = new Minigame ("FindIt", "FindItGame");
			minigames.Add (findIt);
			Minigame socialGame = new Minigame ("_XSocialGame");
			minigames.Add (socialGame);
			Minigame dodge = new Minigame("Dodge");
			minigames.Add (dodge);
			Minigame figure = new Minigame ("Figure");
			minigames.Add (figure);
			Minigame interaction = new Minigame ("Interaction");
			minigames.Add (interaction);
			Minigame repeat = new Minigame ("Repeat");
			minigames.Add (repeat);
			Minigame cooperative = new Minigame ("Cooperative");
			minigames.Add (cooperative);

			MGC.Instance.LoadGame ();
		}

		/// <summary>
		/// Sets the minigame status to "played".
		/// </summary>
		/// <param name="minigameName">Minigame name.</param>
		public void SetPlayed(string minigameName)
		{
			foreach(Minigame game in minigames)
			{
				if(game.sceneWithHelp == minigameName)
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
				if(game.sceneWithHelp == minigameName || game.initialScene == minigameName)
					return game.played;
			return false;
		}

		public Minigame GetMinigame(string minigameName)
		{
			foreach(Minigame game in minigames)
				if(game.sceneWithHelp == minigameName || game.initialScene == minigameName)
					return game;
			return null;
		}

		public List<string> GetMinigamesWithHelp()
		{
			List <string> minigamesWithHelp = new List<string> ();
			foreach(Minigame game in minigames)
				minigamesWithHelp.Add(game.sceneWithHelp);
			return minigamesWithHelp;
		}

//		public void IncreaseHelpShowCounter(string minigameName)
//		{
//			foreach(Minigame game in minigames)
//			{
//				if(game.sceneWithHelp == minigameName)
//				{
//					++game.initialShowHelpCounter;
//					break;
//				}
//			}
//		}
	}
}