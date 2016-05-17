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
                //MovingPlayerRandom();
            }

            /*if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }*/
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

        public void MovingPlayerRandom()
        {
            //string moveState = string.Format("move{0}", Random.Range(0, 4));
            //anim.Play(moveState, -1, 0f);
            //anim.Play("rotate", -1, 0f);
        }
    }
}
