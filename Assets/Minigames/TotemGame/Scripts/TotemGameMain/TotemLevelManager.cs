using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class TotemLevelManager : MonoBehaviour
    {
        public static TotemLevelManager Instance { get; private set; }

        public GameObject player;
        public Animator anim;
        public GameObject goalCube;
        public GameObject bomb;

        public delegate void ClickAction();
        public static event ClickAction OnClicked;

        private bool isGameInitiated = false;

        void Awake()
        {
            Instance = this;
            player.GetComponent<Rigidbody>().useGravity = false;
            isGameInitiated = false;
        }

        void Update()
        {
            if (!isGameInitiated)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    player.GetComponent<Rigidbody>().useGravity = true;
                    isGameInitiated = true;

                    if (OnClicked != null)
                        OnClicked();
                }
            }
        }

        public void RestartScene()
        {
            MGC.Instance.sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void NextLevel()
        {
            MGC.Instance.sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
