/*
 * Created by: Milan Doležal
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game 
{
	[System.Serializable]
	public class MinigameProperties
	{
		public string initialScene;
		public string sceneWithHelp;

        //minimim difficulty
        internal int DifficultyMin;

        //maximum difficulty
        internal int DifficultyMax;

        internal bool played;
		internal int initialShowHelpCounter;

		public MinigameProperties(string minigameName, int DiffMin = 1, int DiffMax = 1, string sceneWithHelp = "")
		{
			this.initialScene = minigameName;
			this.sceneWithHelp = sceneWithHelp;
			played = false;
			initialShowHelpCounter = 0;

            DifficultyMin = DiffMin;
            DifficultyMax = DiffMax;
		}
	}

	public class MinigameStates : MonoBehaviour 
    {
		/// <summary>
        /// Status of each minigame. True if minigame was played.
        /// Minigames needs to be set in Start() function manualy in this script.
		/// </summary>
		internal List<MinigameProperties> minigames = new List<MinigameProperties>();

		public void Start ()
		{
			MinigameProperties main = new MinigameProperties ("Main");
			minigames.Add (main);
			MinigameProperties selection = new MinigameProperties ("GameSelection");
			selection.initialShowHelpCounter = 2;
			minigames.Add (selection);
			
            //Set your minigame here (don't forget to add it into collection too):
			
            MinigameProperties hanoi = new MinigameProperties ("HanoiTowers", 2, 8);
			minigames.Add (hanoi);
			MinigameProperties london = new MinigameProperties ("LondonTowerGUIMenu", 1, 5, "LondonTowerGame");
			minigames.Add (london);
			MinigameProperties pexeso = new MinigameProperties ("Pexeso", 1, 4);
			minigames.Add (pexeso);
			MinigameProperties similarities = new MinigameProperties ("Similarities");
			minigames.Add (similarities);
			MinigameProperties silhouette = new MinigameProperties ("Silhouettes");
			minigames.Add (silhouette);
			MinigameProperties puzzle = new MinigameProperties ("PuzzleChoosePicture", 1, 3, "PuzzleGame");
			minigames.Add (puzzle);
			MinigameProperties coloring = new MinigameProperties ("Coloring");
			minigames.Add (coloring);
			MinigameProperties findIt = new MinigameProperties ("FindIt", 1, 2, "FindItGame");
			minigames.Add (findIt);
			MinigameProperties socialGame = new MinigameProperties ("_XSocialGame");
			minigames.Add (socialGame);
			MinigameProperties dodge = new MinigameProperties("Dodge");
			minigames.Add (dodge);
			MinigameProperties figure = new MinigameProperties ("Figure");
			minigames.Add (figure);
			MinigameProperties interaction = new MinigameProperties ("Interaction");
			minigames.Add (interaction);
			MinigameProperties repeat = new MinigameProperties ("Repeat");
			minigames.Add (repeat);
			MinigameProperties cooperative = new MinigameProperties ("Cooperative");
			minigames.Add (cooperative);

			MGC.Instance.LoadMinigameStatesFromFile ();
		}

		/// <summary>
		/// Sets the minigame status to "played".
		/// </summary>
		/// <param name="minigameName">MinigameProperties scene name.</param>
		public void SetPlayed(string minigameName)
		{
			foreach(MinigameProperties game in minigames)
			{
				if(game.sceneWithHelp == minigameName || game.initialScene == minigameName)
				{
					game.played = true;
					break;
				}
			}

			MGC.Instance.SaveMinigameStatesToFile ();
		}

		public bool GetPlayed(string minigameName)
		{
			foreach(MinigameProperties game in minigames)
				if(game.sceneWithHelp == minigameName || game.initialScene == minigameName)
					return game.played;
			return false;
		}

		public MinigameProperties GetMinigame(string minigameName)
		{
            foreach (MinigameProperties game in minigames)
            {
                if (game.sceneWithHelp == minigameName || game.initialScene == minigameName)
                {
                    return game;
                }
            }
			return null;
		}

		public List<string> GetMinigamesWithHelp()
		{
			List <string> minigamesWithHelp = new List<string> ();
			foreach(MinigameProperties game in minigames)
				minigamesWithHelp.Add(game.sceneWithHelp);
			return minigamesWithHelp;
		}

//		public void IncreaseHelpShowCounter(string minigameName)
//		{
//			foreach(MinigameProperties game in minigames)
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