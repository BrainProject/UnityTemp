using UnityEngine;
using System.Collections;

namespace Frogger
{
    public class FrogGoal : MonoBehaviour
    {
        public Transform mesh;

        internal bool occupied = false;

        private float waveOffset;

        void Start()
        {
            mesh.transform.localEulerAngles += (Vector3.up * Random.Range(0, 360));
        }

        void Update()
        {
            mesh.transform.Translate(Vector3.forward * Mathf.Cos(Time.time + transform.position.x) / 200);
        }
    }
}