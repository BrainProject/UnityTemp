using UnityEngine;
using System.Collections;

namespace MinigameMaze2
{
    public class FlameScript : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PlayerScript ps = other.GetComponent<PlayerScript>();
                ps.ResetPosition();
            }
        }
    }
}
