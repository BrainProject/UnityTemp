using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace Game
{
    /// <summary>
    /// List of properties of all minigames. 
    /// </summary>
    public class Minigames : MonoBehaviour
    {
        /// <summary>
        /// List of properties of all minigames. 
        /// </summary>
        internal List<MinigameProperties> minigames = new List<MinigameProperties>();

        public void add(MinigameProperties newgame)
        {
            minigames.Add(newgame);
        }

        void Start()
        {
            print("Initializing minigame properties.");
        }

        /// <summary>
        /// Sets the minigame status to "played".
        /// Should be called from individual mini-games when gameProps itself starts...
        /// </summary>
        /// <param name="minigameName"> name of minigame</param>
        /// <param name="diff"> difficulty</param>
        public void SetPlayed(string minigameName, int diff = 0)
        {
            print("Now playing minigame: '" + minigameName + "', with diff: " + diff);

            try
            {
                foreach (MinigameProperties gameProps in minigames)
                {
                    if (gameProps.mainScene == minigameName || gameProps.initialScene == minigameName)
                    {
                        gameProps.stats.played = true;
                        gameProps.stats.playedCount[diff] += 1;
                        gameProps.stats.DifficutlyLastPlayed = diff;
                        break;
                    }
                }

                MGC.Instance.SaveMinigamesStatisticsToFile();
            }
            catch(NullReferenceException ex)
            {
                Debug.LogWarning("Played property of the current minigame is not set beacuse of the NullReferenceException.\n" + ex);
            }
        }

		/// <summary>
		/// Sets the "played" flag to true in order the minigame help doesn't show up on startup anymore.
		/// </summary>
		/// <param name="minigameName">Minigame name.</param>
		public void SetPlayedWithHelp(string minigameName)
		{
			foreach (MinigameProperties gameProps in minigames)
			{
				if (gameProps.mainScene == minigameName || gameProps.initialScene == minigameName)
				{
					gameProps.stats.played = true;
					break;
				}
			}
			
			MGC.Instance.SaveMinigamesStatisticsToFile();
		}
		
		public void SetSuccessfullyPlayed(string minigameName, int diff = 0)
		{
            try
            {
                foreach (MinigameProperties game in minigames)
                {
                    if (game.mainScene == minigameName || game.initialScene == minigameName)
                    {
                        game.stats.finishedCount[diff] += 1;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                Debug.LogWarning("Played successfully property of the current minigame is not set beacuse of the NullReferenceException.\n" + ex);
            }
        }

        public bool GetPlayed(string minigameName)
        {
            foreach (MinigameProperties game in minigames)
                if (game.mainScene == minigameName || game.initialScene == minigameName)
                    return game.stats.played;
            return false;
        }

        public MinigameProperties GetMinigame(string minigameName)
        {
            try
            {
                foreach (MinigameProperties game in minigames)
                {
                    //print("checking mini-game: '" + game.readableName + "'");
                    if (game.mainScene == minigameName || game.initialScene == minigameName)
                    {
                        return game;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                Debug.LogWarning("Minigame properties get failed.\n" + ex);
            }

            Debug.LogWarning("properties of mini-game: '" + minigameName + "' not found!"); 
            return null;
        }

        public List<string> GetMinigamesWithHelp()
        {
            List<string> minigamesWithHelp = new List<string>();
            foreach (MinigameProperties game in minigames)
            {
                minigamesWithHelp.Add(game.mainScene);
            }

            return minigamesWithHelp;
		}

		public bool IsWithHelp(string minigameName)
		{
			foreach (MinigameProperties game in minigames)
			{
				//print("checking mini-game: '" + game.readableName + "'");
				if (game.mainScene == minigameName)
				{
					return true;
				}
			}

			return false;
		}
		
		public void ResetStatistics()
		{
			foreach (MinigameProperties game in minigames)
			{
				for (int i = 0; i < game.stats.playedCount.Length; i++)
				{
					game.stats.playedCount[i] = 0;
				}
				game.stats.played = false;
			}
			MGC.Instance.SaveMinigamesStatisticsToFile();
		}
		
		public void printStatisticsToFile()
        {
            //TODO proper implementation

            //debug print to console
            #if UNITY_EDITOR
            
            foreach (MinigameProperties game in minigames)
            {
                print("Minigame: " + game.readableName);
                for (int i = 0; i <= game.MaxDifficulty; i++)
                {
                    print("   Diff: " + i + ":: played: " + game.stats.playedCount[i] + "; finished: " + game.stats.finishedCount[i]);
                }
            }

            #else
                Debug.LogError("Not implemented, yet");
            #endif
        }
		
		
		
		internal void loadConfigurationsfromFile()
		{
            try
            {
                print("Loading mini-games configurations from file...");
                //get instance of prefab
                GameObject mgParent = Instantiate(Resources.Load("mini-games-configuration")) as GameObject;

                mgParent.transform.parent = this.transform;

                //for each mini-game
                foreach (Transform child in mgParent.transform)
                {
                    //TODO checks...
                    // if there is scene with such name
                    MinigameProperties props = child.GetComponent<MinigameProperties>();
                    print("   loading mini-game: '" + props.readableName + "'");
                    minigames.Add(props);
                }

                print("   Mini-games configurations loaded: " + minigames.Count);

                //get rid of prefab instance
                //Destroy(mgParent);
            }
            catch (EndOfStreamException ex)
            {
                Debug.LogWarning("Minigame configurations not loaded because of EndOfStreamException!\n" + ex);
            }
        }
    }
}