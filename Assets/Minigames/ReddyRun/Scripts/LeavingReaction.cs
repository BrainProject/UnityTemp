using UnityEngine;
using System.Collections;
using System;


namespace Reddy
{
    public class LeavingReaction : MonoBehaviour
    {

        public Transform rotatingRoot;
        private ReddyController player;
        private Tile thisTile;


        void Start()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                player = ReddyLevelManager.Instance.playerReference;
                player.transform.SetParent(null);
                player.runningSpeed = 100; //player speed

                player.transform.position = this.transform.position;
                player.transform.rotation = this.transform.rotation;

                switch (player.pathCount)
                {
                    case 0:
                        player.transform.Translate(Vector3.right * -4);
                        break;
                    case 1:
                        break;
                    case 2:
                        player.transform.Translate(Vector3.right * 4);
                        break;
                }

                // Right or Left turn

                // LEFT


            }
        }
    }

}
