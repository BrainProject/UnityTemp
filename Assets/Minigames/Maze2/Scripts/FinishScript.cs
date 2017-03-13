using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace MinigameMaze2
{
    public class FinishScript : MonoBehaviour
    {
        //public static int difficutly = 0;
        //public static int level = 1;

        //private bool displayWin = false;

        /*void Start()
        {
            difficutly = 1;
        }*/

        void OnTriggerEnter(Collider other)
        {
            //player won the level
            if (other.tag == "Player")
            {
                Debug.Log("Win");
                //displayWin = true;
                GameObject.FindObjectOfType<MinigameController>().LoadNextLevel();
            }
        }

        /*void OnGUI()
        {
            if (displayWin)
            {
                GUI.Label(new Rect((Screen.width / 2), (Screen.height / 2), 200f, 200f), "WIN");
            }
        }*/

        private IEnumerator RestartGame()
        {
            yield return new WaitForSeconds(3f);
            PlayerScript ps = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
            //displayWin = false;
            ps.ResetPosition();
            yield return null;
        }

        /*private IEnumerator NextLevel()
        {
            //yield return new WaitForSeconds(3f);

            level++;
            //if next level exists, load it
            if (Application.CanStreamedLevelBeLoaded("Scenes/Maze2/Level" + difficutly + "_" + level))
            {
                SceneManager.LoadScene("Scenes/Maze2/Level" + difficutly + "_" + level);
            }
            //player won all levels of given difficulty, end game
            else
            {
                //END GAME HERE
            }
            yield return 0;
        }*/
    }
}
