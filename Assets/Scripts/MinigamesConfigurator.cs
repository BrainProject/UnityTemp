
#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;


namespace Game
{

    /// <summary>
    /// Defines content of MinigamesConfiguratorEditor
    /// </summary>
    /// 
    /// This script is "editor-only" 
    /// 
    /// \author Jiri Chmelik
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
        public string mainScene;

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

        /// <summary>
        /// This script is only for Editor, not a run-time
        /// Start is therefore not called at all
        /// </summary>
        void Start()
        {

        }

        public void AddMinigame()
        {

            print("Adding mini-game...");
            //create new object representing mini-game
            //TODO get rid of resources?
            GameObject newgameGO = (GameObject)Instantiate(Resources.Load("NewMiniGame") as GameObject);
            MinigameProperties minigameProperties = newgameGO.GetComponent<MinigameProperties>();

            //TODO add some tests here
            // valid pointer
            // empty name
            // the same name already exists
            // difficulty icons should be setted if difficulty is > 0

            // find parent object
            minigamesParent = GameObject.Find("Mini-games");

            //set parent and name of gameProps object
            newgameGO.transform.SetParent(minigamesParent.transform);
            newgameGO.name = readableName;

            //set properties of mini-game
            minigameProperties.readableName = readableName;
            minigameProperties.mainScene = mainScene;
            minigameProperties.initialScene = initialScene;
            minigameProperties.MaxDifficulty = MaxDifficulty;
            minigameProperties.IconDifficultyLow = difficultyLowIcon;
            minigameProperties.IconDifficultyHigh = difficultyHighIcon;

            //clear values in editor
            readableName = "";
            mainScene = "";
            initialScene = "";
            MaxDifficulty = 0;
            difficultyHighIcon = null;
            difficultyLowIcon = null;

            SaveConfigurationstoFile();
        }



        public void SaveConfigurationstoFile()
        {
            print("Saving configuration of mini-games into prefab");

            minigamesParent = GameObject.Find("Mini-games");

            if (minigamesParent == null)
            {
                Debug.LogError("This should not happens...");
            }

            string fileName = "mini-games-configuration";
            string fileLocation = "Assets/Prefabs/Resources/" + fileName + ".prefab";

            //save mini-games configuration as prefab object
            Object prefab = PrefabUtility.CreateEmptyPrefab(fileLocation);
            PrefabUtility.ReplacePrefab(minigamesParent, prefab, ReplacePrefabOptions.ConnectToPrefab);

            //clear statistics of all mini-games as they depend on configurations...
            if (File.Exists(Application.persistentDataPath + "/mini-games.stats"))
            {
                File.Delete(Application.persistentDataPath + "/mini-games.stats");
            }

        }

    }
}
#endif