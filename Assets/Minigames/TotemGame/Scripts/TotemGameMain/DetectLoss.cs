using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace TotemGame
{
    public class DetectLoss : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == TotemLevelManager.Instance.player)
            {
                //MGC.Instance.LoseMinigame();

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
