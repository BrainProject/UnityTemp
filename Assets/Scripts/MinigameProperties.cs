

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{

    public struct  MinigameConfiguration
    {
        /// <summary>
        /// human-friendly name of mini-gameProps
        /// </summary>
        /// May contains spaces, apostrofs and other weird characters
        /// used only in debug prints and for logging
        public string readableName;

        /// <summary>
        /// if mini-gameProps has more than one scene, this one will be loaded first
        /// </summary>
        public string initialScene;

        /// <summary>
        /// When this scene is loaded, help for mini-gameProps will be shown
        /// </summary>
        public string sceneWithHelp;

        //maximum difficulty
        public int MaxDifficulty;

        /// <summary>
        /// Image symbolizing low difficulty
        /// </summary>
        public Sprite difficultyLowIcon;

        /// <summary>
        /// Image symbolizing high difficulty
        /// </summary>
        public Sprite difficultyHighIcon;
    }

    public struct MinigameStatistics
    {
        // on what difficulty was this minigame last played?
        // used as initial value for difficulty chooser slider value
        internal int DifficutlyLastPlayed;


        // how many times the gameProps was played on each difficulty
        internal int[] playedCount;

        // which difficulties has been succesfully played
        internal int[] finishedCount;



        //has been gameProps ever played - TODO remove completely?
        internal bool played;

        internal int initialShowHelpCounter;
    }

    /// <summary>
    /// Stores properties and statistics of one minigame
    /// </summary>
    /// 
    /// \author Milan Doležal
    /// \author Jiri Chmelik
    [System.Serializable]
    public class MinigameProperties : MonoBehaviour
    {

        public MinigameConfiguration conf;
        public MinigameStatistics stats;
    }



}