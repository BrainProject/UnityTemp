using UnityEngine;
using System.Collections;

namespace MinigameMaze2
{
    public class MovingWall : MonoBehaviour
    {
        private Vector3 initialPos;
        [SerializeField]
        private Vector3 finalPos;
        [SerializeField]
        private float speed = 3f;

        void Start()
        {
            initialPos = transform.position;
        }

        void Update()
        {
            float distance = Vector3.Distance(initialPos, finalPos);
            transform.localPosition = Vector3.Lerp(initialPos, finalPos,
                Mathf.PingPong((Time.time * speed) / distance, 1.0f));
        }
    }
}
