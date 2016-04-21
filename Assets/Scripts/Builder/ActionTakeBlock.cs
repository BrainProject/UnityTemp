using UnityEngine;
using System.Collections;


namespace Builder
{
    
    [RequireComponent(typeof(BoxCollider2D))]
    public class ActionTakeBlock : CollisionLoading
    {

        public GameObject block;
        public GameObject hand;

        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Action runs when loading is done.
        /// The object appers in avatar's hand.
        /// </summary>
        public override void Action()
        {
            if (true) //TODO: overovani jestli je kostka zrovna v ruce
            {
                Vector3 startPosition = hand.transform.position;
                block = Instantiate(block, startPosition, Quaternion.identity) as GameObject;
                block.GetComponent<BlockBehaviour>().hand = hand;
            }     
        }
    }

}