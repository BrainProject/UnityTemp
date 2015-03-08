using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
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

        /// <summary>
        /// This script is only for Editor, not a run-time
        /// Start is therefore not called at all
        /// </summary>
        void Start()
        {
            
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

            SaveChanges();
        }



        public void SaveChanges()
        {
            #if UNITY_EDITOR
            print("Another Suxses - there will be.");

            minigamesParent = GameObject.Find("Mini-games");

            if (minigamesParent == null)
            {
                Debug.LogError("This should not happens...");
            }

            string fileName = "mini-games-conf";
            string fileLocation = "Assets/Prefabs/Resources/" + fileName + ".prefab";
            Object prefab = PrefabUtility.CreateEmptyPrefab(fileLocation);
            PrefabUtility.ReplacePrefab(minigamesParent, prefab, ReplacePrefabOptions.ConnectToPrefab);

//             /// <summary>
//            /// List of properties of all minigames. 
//            /// </summary>
//            List<MinigameProperties> minigames = new List<MinigameProperties>();

//            // gather all mini-games
//            foreach (Transform child in minigamesParent.transform)
//            {
//                if (child.gameObject != null)
//                {
//                    minigames.Add(child.gameObject.GetComponent<MinigameProperties>());
//                }
//            }

//            //MGC.Instance.SaveMinigamesStatisticsToFile();

//            // save configurations to single file
//#if !UNITY_WEBPLAYER
//            {
//                //delete old saved file 
//                if (File.Exists(Application.persistentDataPath + "/mini-games.conf"))
//                {
//                    File.Delete(Application.persistentDataPath + "/mini-games.conf");
//                }

//                BinaryFormatter bf = new BinaryFormatter();
//                FileStream file = File.Create(Application.persistentDataPath + "/mini-games.conf");

//                foreach (Game.MinigameProperties minigameData in minigames)
//                {
//                    print("trying to serialize...");
//                    bf.Serialize(file, minigameData);
//                    print("serialized");
//                }

//                print("Mini-games config saved to 'mini-games.conf' file.");
//                file.Close();
//            }
            //#endif

#endif

        }

    }

}