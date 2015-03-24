﻿

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        public void Start()
        {
            //TODO following comments are used just as reference for poor-memory humans, to see the names and difficulties...
            //MinigameProperties hanoi = new MinigameProperties("HanoiTowers", 4);
            //minigames.Add(hanoi);
            //MinigameProperties london = new MinigameProperties("LondonTowerGUIMenu", 3, "LondonTowerGame");
            //minigames.Add(london);
            //MinigameProperties pexeso = new MinigameProperties("Pexeso", 3);
            //minigames.Add(pexeso);
            //MinigameProperties similarities = new MinigameProperties("Similarities", 3);
            //minigames.Add(similarities);
            //MinigameProperties silhouette = new MinigameProperties("Silhouettes", 3);
            //minigames.Add(silhouette);
            //MinigameProperties puzzle = new MinigameProperties("PuzzleChoosePicture", 2, "PuzzleGame");
            //minigames.Add(puzzle);
            //MinigameProperties coloring = new MinigameProperties("Coloring");
            //minigames.Add(coloring);
            //MinigameProperties findIt = new MinigameProperties("FindIt", 2, "FindItGame");
            //minigames.Add(findIt);
            //MinigameProperties socialGame = new MinigameProperties("_XSocialGame");
            //minigames.Add(socialGame);
            //MinigameProperties dodge = new MinigameProperties("Dodge");
            //minigames.Add(dodge);
            //MinigameProperties figure = new MinigameProperties("Figure");
            //minigames.Add(figure);
            //MinigameProperties interaction = new MinigameProperties("Interaction");
            //minigames.Add(interaction);
            //MinigameProperties repeat = new MinigameProperties("Repeat");
            //minigames.Add(repeat);
            //MinigameProperties cooperative = new MinigameProperties("Cooperative");
            //minigames.Add(cooperative);
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

        public void SetSuccessfullyPlayed(string minigameName, int diff = 0)
        {
            foreach (MinigameProperties game in minigames)
            {
                if (game.mainScene == minigameName || game.initialScene == minigameName)
                {
                    game.stats.finishedCount[diff] += 1;
                }
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
            foreach (MinigameProperties game in minigames)
            {
                //print("checking mini-game: '" + game.readableName + "'");
                if (game.mainScene == minigameName || game.initialScene == minigameName)
                {
                    return game;
                }
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
    }
}