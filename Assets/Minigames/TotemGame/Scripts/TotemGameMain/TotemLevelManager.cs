using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TotemGame
{
    public class TotemLevelManager : MonoBehaviour
    {
        public static TotemLevelManager Instance { get; private set; }

        public GameObject player;
        public Animator anim;
        public GameObject goalCube;
        public GameObject bomb;

        void Awake()
        {
            Instance = this;
            player.GetComponent<Rigidbody>().useGravity = false;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                player.GetComponent<Rigidbody>().useGravity = true;
                MovingEye();
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        public void RestartScene()
        {
            //MGC.Instance.sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log(SceneManager.GetActiveScene().name);
            Scene scene = SceneManager.GetActiveScene();
           SceneManager.LoadScene(scene.name);
        }

        public void NextLevel()
        {
            //MGC.Instance.sceneLoader.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log(SceneManager.GetActiveScene().name);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex + 1);

        }

        public void MovingEye()
        {
            anim.Play("eyeanimation", -1, 0f);
        }
    }
}
