using UnityEngine;
using System.Collections;


namespace Game
{

    /// <summary>
    /// Defines content of MinigamesEditor
    /// </summary>
    public class MinigamesConfigurator : MonoBehaviour
    {

        //TODO parameters here are the same as in MinigameProperties
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
        public string initialScene;

        /// <summary>
        /// When this scene is loaded, help for mini-game will be shown
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


        GameObject minigamesParent;


        void Start()
    {
        minigamesParent = GameObject.Find("Mini-games");

        if(minigamesParent == null)
        {
            Debug.LogError("This should not happens...");
        }
    }

        public void AddMinigame()
        {
            //create new object representing mini-game
            //TODO get rid of resources
            GameObject newgameGO = (GameObject)Instantiate(Resources.Load("NewMiniGame") as GameObject);
            MinigameProperties props = newgameGO.GetComponent<MinigameProperties>();

            if(props == null)
            {
                throw new UnityException("Not a suxsess");
            }
            //todo add some tests here
                // empty name
                // the same name already exists
                // difficulty icons should be setted if difficulty is > 0
            
            //set parent and name of game object
            newgameGO.transform.SetParent(minigamesParent.transform);
            newgameGO.name = readableName;

            //set properties of mini-game
            props.readableName = readableName;
            props.sceneWithHelp = sceneWithHelp;
            props.initialScene = initialScene;
            props.MaxDifficulty = MaxDifficulty;
            props.difficultyLowIcon = difficultyLowIcon;
            props.difficultyHighIcon = difficultyHighIcon;

            //clear values in editor
            readableName = "";
            sceneWithHelp = "";
            initialScene = "";
            MaxDifficulty = 0;
            difficultyHighIcon = null;
            difficultyLowIcon = null;
        }

        public void SaveChanges()
        {
            print("Another Suxses - there will be.");
        }

    }

}