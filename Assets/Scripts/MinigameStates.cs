

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// Stores properties and statistics of one minigame
    /// </summary>
    /// 
    /// \author Milan Doležal
    /// \author Jiri Chmelik
    [System.Serializable]
    public class MinigameProperties
    {
        public string initialScene;
        public string sceneWithHelp;

        //human readable name of mini-game
        internal string readableName;

        // on what difficulty was this minigame last played?
        // used as initial value for difficulty chooser slider value
        internal int DifficutlyLastPlayed = 0;

        //maximum difficulty
        internal int MaxDifficulty;

        // how many times the game was played on each difficulty
        internal int[] playedCount;

        // which difficulties has been succesfully played
        internal int[] finishedCount;



        //has been game ever played - TODO remove completely?
        internal bool played;

        internal int initialShowHelpCounter;

        public MinigameProperties(string minigameName, int MaxDiff = 0, string sceneWithHelp = "")
        {
            this.initialScene = minigameName;
            this.sceneWithHelp = sceneWithHelp;
            played = false;
            initialShowHelpCounter = 0;

            MaxDifficulty = MaxDiff;

            playedCount = new int[MaxDiff+1];
            finishedCount = new int[MaxDiff+1];

            //set readable name
            if(sceneWithHelp == "")
            {
                readableName = minigameName;
            }
            else 
            {
                readableName = sceneWithHelp;
            }
        }

        //public void played()

    }

    /// <summary>
    /// List of properties of all minigames. 
    /// </summary>
    public class Minigames : MonoBehaviour
    {
        /// <summary>
        /// List of properties of all minigames. 
        /// Minigames needs to be set in Start() function manualy in this script.
        /// </summary>
        internal List<MinigameProperties> minigames = new List<MinigameProperties>();

        public void Start()
        {
            MinigameProperties main = new MinigameProperties("Main");
            minigames.Add(main);
            MinigameProperties selection = new MinigameProperties("GameSelection");
            selection.initialShowHelpCounter = 2;
            minigames.Add(selection);

            // Set your minigame here (don't forget to add it into collection too):
            // If You change anything in following collection, be sure to delete 'newron.sav' file in '...\AppData\LocalLow\HCI\Newron'

            MinigameProperties hanoi = new MinigameProperties("HanoiTowers", 4);
            minigames.Add(hanoi);
            MinigameProperties london = new MinigameProperties("LondonTowerGUIMenu", 3, "LondonTowerGame");
            minigames.Add(london);
            MinigameProperties pexeso = new MinigameProperties("Pexeso", 4);
            minigames.Add(pexeso);
            MinigameProperties similarities = new MinigameProperties("Similarities", 4);
            minigames.Add(similarities);
            MinigameProperties silhouette = new MinigameProperties("Silhouettes", 4);
            minigames.Add(silhouette);
            MinigameProperties puzzle = new MinigameProperties("PuzzleChoosePicture", 2, "PuzzleGame");
            minigames.Add(puzzle);
            MinigameProperties coloring = new MinigameProperties("Coloring");
            minigames.Add(coloring);
            MinigameProperties findIt = new MinigameProperties("FindIt", 2, "FindItGame");
            minigames.Add(findIt);
            MinigameProperties socialGame = new MinigameProperties("_XSocialGame");
            minigames.Add(socialGame);
            MinigameProperties dodge = new MinigameProperties("Dodge");
            minigames.Add(dodge);
            MinigameProperties figure = new MinigameProperties("Figure");
            minigames.Add(figure);
            MinigameProperties interaction = new MinigameProperties("Interaction");
            minigames.Add(interaction);
            MinigameProperties repeat = new MinigameProperties("Repeat");
            minigames.Add(repeat);
            MinigameProperties cooperative = new MinigameProperties("Cooperative");
            minigames.Add(cooperative);

            MGC.Instance.LoadMinigamesPropertiesFromFile();
        }

        /// <summary>
        /// Sets the minigame status to "played".
        /// </summary>
        /// <param name="minigameName">Minigame scene-name.</param>
        public void SetPlayed(string minigameName, int diff = 0)
        {
            print("Now playing minigame: '" + minigameName + "', with diff: " + diff);

            foreach (MinigameProperties game in minigames)
            {
                if (game.sceneWithHelp == minigameName || game.initialScene == minigameName)
                {
                    game.played = true;
                    game.playedCount[diff] += 1;
                    game.DifficutlyLastPlayed = diff;
                    break;
                }
            }

            MGC.Instance.SaveMinigamesPropertiesToFile();
        }

        public void SetSuccessfullyPlayed(string minigameName, int diff = 0)
        {
            foreach (MinigameProperties game in minigames)
            {
                if (game.sceneWithHelp == minigameName || game.initialScene == minigameName)
                {
                    game.finishedCount[diff] += 1;
                }
            }
        }

        public bool GetPlayed(string minigameName)
        {
            foreach (MinigameProperties game in minigames)
                if (game.sceneWithHelp == minigameName || game.initialScene == minigameName)
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
            List<string> minigamesWithHelp = new List<string>();
            foreach (MinigameProperties game in minigames)
            {
                minigamesWithHelp.Add(game.sceneWithHelp);
            }

            return minigamesWithHelp;
        }

        public void printStatisticsToFile()
        {
            //TODO
            Debug.LogError("Not implemented, yet");

            //debug only
            foreach (MinigameProperties game in minigames)
            {
                print("Minigame: " + game.readableName);
                for( int i = 0; i <= game.MaxDifficulty; i++)
                {
                    print("   Diff: " + i + ":: played: " + game.playedCount[i] + "; finished: " + game.finishedCount[i]);
                }
            }

        }


    }
}