using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;

namespace TotemGame
{
    public class TotemGameCrossroadManager : MonoBehaviour
    {
        public static TotemGameCrossroadManager Instance { get; private set; }
        public string filesPath;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
                switch (MGC.Instance.selectedMiniGameDiff)
                {
                    case 0:
                        MGC.Instance.sceneLoader.LoadScene("TotemGameTutorial");
                        MGC.Instance.getMinigameStates().SetPlayed("TotemGameTutorial", MGC.Instance.selectedMiniGameDiff);
                        break;
                    case 1:
                        MGC.Instance.sceneLoader.LoadScene("TotemGameTutorialExplosion");
                        MGC.Instance.getMinigameStates().SetPlayed("TotemGameTutorialExplosion", MGC.Instance.selectedMiniGameDiff);
                        break;
                    case 2:
                        filesPath = Application.dataPath + "/Minigames/TotemGame/XmlDocs/Difficulty1";
                        MGC.Instance.sceneLoader.LoadScene("TotemGameCrossroad");
                        //MGC.Instance.getMinigameStates().SetPlayed("TotemGameCrossroad", MGC.Instance.selectedMiniGameDiff);
                        break;
                    
                    case 3:
                        filesPath = Application.dataPath + "/Minigames/TotemGame/XmlDocs/Difficulty2";
                        MGC.Instance.sceneLoader.LoadScene("TotemGameCrossroad");
                        //MGC.Instance.getMinigameStates().SetPlayed("TotemGameCrossroad", MGC.Instance.selectedMiniGameDiff);
                        break;
                    case 4:
                        MGC.Instance.sceneLoader.LoadScene("TotemGameFirstLevel");
                        MGC.Instance.getMinigameStates().SetPlayed("TotemGameFirstLevel", MGC.Instance.selectedMiniGameDiff);
                        break;
                    /*
                    case 4:
                        filesPath = Application.dataPath + "/Minigames/TotemGame/XmlDocs/Difficulty3";
                        MGC.Instance.sceneLoader.LoadScene("TotemGameCrossroad");
                        //MGC.Instance.getMinigameStates().SetPlayed("TotemGameCrossroad", MGC.Instance.selectedMiniGameDiff);
                        break;     
                    */
            }
        }
    }
}
