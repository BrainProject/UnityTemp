using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TotemGame
{
    public class TotemLevelManager : MonoBehaviour
    {
        public static TotemLevelManager Instance { get; private set; }

        public Animator anim;
        public GameObject player;

        void Awake()
        {
            Instance = this;
            /*switch (MGC.Instance.selectedMiniGameDiff)
            {
                case 0:
                    MGC.Instance.sceneLoader.LoadScene("TotemGameTutorial");
                    //MGC.Instance.getMinigameStates().SetPlayed("TotemGameTutorial", MGC.Instance.selectedMiniGameDiff);
                    break;
                case 1:
                    MGC.Instance.sceneLoader.LoadScene("TotemGameTutorialExplosions");
                    //MGC.Instance.getMinigameStates().SetPlayed("SecondScene", MGC.Instance.selectedMiniGameDiff);
                    break;
            }*/
            }

        void Start()
        {

                    /*case 2:
                        //nacitani nahodne sceny ze slozky Difficulty2 v XmlDocs
                        MGC.Instance.sceneLoader.LoadScene("");
                        break;
                    case 3:
                        //nacitani nahodne sceny ze slozky Difficulty3 v XmlDocs
                        MGC.Instance.sceneLoader.LoadScene("");
                        break;*/
                    
            //}
            //MGC.Instance.getMinigameStates().SetPlayed("TotemGame", MGC.Instance.selectedMiniGameDiff);
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                player.GetComponent<Rigidbody>().useGravity = true;
                MovingEye();
            }
        }

        public void RestartScene()
        {
            //MGC.Instance.sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
           Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void MovingEye()
        {
            anim.Play("eyeanimation", -1, 0f);
        }
    }
}
