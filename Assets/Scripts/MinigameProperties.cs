

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{

    /// <summary>
    /// Just a structure to distinguish between "configuration" and "statistics" of mini-game
    /// </summary>
    /// 
    /// \author Jiri Chmelik
    [System.Serializable]
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

        //TODO parameters here are the same as in MinigamesConfigurator
        // how to re-use the code?

        /// <summary>
        /// human-friendly name of mini-game
        /// </summary>
        /// May contains spaces, apostrofs and other weird characters
        /// used only in debug prints and for logging
        public string readableName;

        /// <summary>
        /// if mini-game has more than one scene, this one will be loaded first
        /// </summary>
        /// Usualy this is some settings scene, like choosing image-set for Pexeso mini-game
        public string initialScene;

        /// <summary>
        /// Main scene of the mini-game
        /// </summary>
        /// When this scene is loaded, mini-game is considered as "played"
        /// Also, mini-game help will appear (if required) after loading of this scene
        public string mainScene;

        /// <summary>
        /// Maximal difficulty
        /// </summary>
        /// Range of diff is: {0, MaxDifficulty}. So if You set MaxDifficulty to value 2, there will be three different difficulties
        /// If mini-game has no difficulty setting (e. g. Coloring mini-game), set value to 0.
		public int MaxDifficulty;
		
		/// <summary>
		/// Icon of this minigame.
		/// </summary>
		//public Texture menuIcon;
		
		/// <summary>
		/// Is Kinect required for this minigame?
		/// </summary>
		public bool isKinectRequired = false;
		
		/// <summary>
		/// mini-game help. So far, simple texture
		/// </summary>
		//public Texture helpTexture;

        /// <summary>
        /// Image symbolizing low difficulty
        /// </summary>
        public Sprite IconDifficultyLow;

        /// <summary>
        /// Image symbolizing high difficulty
        /// </summary>
        public Sprite IconDifficultyHigh;

		/// <summary>
		/// The prefab with help.
		/// </summary>
		public GameObject helpPrefab;

		/// <summary>
		/// The duration of the help animation.
		/// </summary>
		public float helpDuration = 3;


        internal MinigameStatistics stats;
    }



}