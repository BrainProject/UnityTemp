using UnityEngine;
using System.Collections;

namespace TotemGame
{
    public class DetectLoss : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player")
            {
                MGC.Instance.LoseMinigame();
            }
        }
    }
}
