using UnityEngine;
using System.Collections;
//using GSIv2;

namespace Builder
{
    
    [RequireComponent(typeof(BoxCollider2D))]
    public class ActionTakeBlock : CollisionLoading
    {

        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Action runs when loading is done.
        /// It shows the help.
        /// </summary>
        public override void Action()
        {
            Debug.Log("I am working");
        }
    }

}