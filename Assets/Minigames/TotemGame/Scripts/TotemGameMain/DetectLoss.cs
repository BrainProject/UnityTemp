using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class DetectLoss : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == TotemLevelManager.Instance.player)
            {
                MGC.Instance.LoseMinigame();
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
