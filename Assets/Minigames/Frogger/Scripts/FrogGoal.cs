using UnityEngine;
using System.Collections;

namespace Frogger
{
    public class FrogGoal : MonoBehaviour
    {
        public Transform mesh;
        public float waveOffset = 0;
        public Animator meshAnimator;

        internal bool occupied = false;

        void Start()
        {
            mesh.transform.localEulerAngles += (Vector3.up * Random.Range(0, 360));
            StartCoroutine(WaveOffset());
        }


        IEnumerator WaveOffset()
        {
            yield return new WaitForSeconds(waveOffset);
            meshAnimator.SetTrigger("CanPlay");
        }
        //void Update()
        //{
            //mesh.transform.Translate(Vector3.forward * Mathf.Cos(Time.time + transform.position.x * Time.deltaTime + offset) / 800);
        //}
    }
}