using UnityEngine;
using System.Collections;

namespace Builder
{
    public class BlockBehaviour : MonoBehaviour
    {

        public GameObject hand;
        public GameObject secondPlayer;
        private bool inHand;

        void Start()
        {
            inHand = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (hand.GetComponent<Transform>())
            {

                transform.position = hand.transform.position;
            }
            else
            {
                Debug.LogWarning("No transform component!");

            }
            
        }

        void OnTriggerEnter()
        {
           // TODO: predani objektu druhemu hraci (trigger funguje, akorat ho musis udelat tak, aby reagoval jenom na triggery ruky druheho hrace)
        }

        public bool IsInHand()
        {
            return this.inHand;
        }
    }
}

