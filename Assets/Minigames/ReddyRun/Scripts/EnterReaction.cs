using UnityEngine;
using System.Collections;


namespace Reddy
{
    public class EnterReaction : MonoBehaviour
    {
        public Transform rotatingRoot;
        public Transform enteringTransform;


        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {

                ReddyController playerReference = ReddyLevelManager.Instance.playerReference;
                playerReference.transform.SetParent(rotatingRoot);
                playerReference.runningSpeed = 0;

                enteringTransform = playerReference.transform;
            }
        }
    }

}
