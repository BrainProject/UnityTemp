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
                        MGC.Instance.sceneLoader.LoadScene("TGTutorial");
                        MGC.Instance.getMinigameStates().SetPlayed("TGTutorial", MGC.Instance.selectedMiniGameDiff);
                        break;
                    case 1:
                        MGC.Instance.sceneLoader.LoadScene("TGTutorialExplosion");
                        MGC.Instance.getMinigameStates().SetPlayed("TGTutorialExplosion", MGC.Instance.selectedMiniGameDiff);
                        break;
                    case 2:
                        filesPath = Application.dataPath + "/Minigames/TotemGame/XmlDocs/Difficulty1/Resources";
                        MGC.Instance.sceneLoader.LoadScene("TGCrossroad");
                        MGC.Instance.getMinigameStates().SetPlayed("TGCrossroad", MGC.Instance.selectedMiniGameDiff);
                        break;
                    case 3:
                        filesPath = Application.dataPath + "/Minigames/TotemGame/XmlDocs/Difficulty2/Resources";
                        MGC.Instance.sceneLoader.LoadScene("TGCrossroad");
                        MGC.Instance.getMinigameStates().SetPlayed("TmGCrossroad", MGC.Instance.selectedMiniGameDiff);
                        break;
                    case 4:
                        filesPath = Application.dataPath + "/Minigames/TotemGame/XmlDocs/Difficulty3/Resources";
                        MGC.Instance.sceneLoader.LoadScene("TGCrossroad");
                    MGC.Instance.getMinigameStates().SetPlayed("TmGCrossroad", MGC.Instance.selectedMiniGameDiff);
                    break;
                    /*case 2:
                        MGC.Instance.sceneLoader.LoadScene("TotemGameThirdLevel");
                        MGC.Instance.getMinigameStates().SetPlayed("TotemGameThirdLevel", MGC.Instance.selectedMiniGameDiff);
                    break;
                    case 3:
                        MGC.Instance.sceneLoader.LoadScene("TotemGameFirstLevel");
                        MGC.Instance.getMinigameStates().SetPlayed("TotemGameFirstLevel", MGC.Instance.selectedMiniGameDiff);
                        break;  
                    */
            }
        }
    }
}
