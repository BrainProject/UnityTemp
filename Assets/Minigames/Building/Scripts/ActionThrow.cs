using UnityEngine;
using System.Collections;
using GSIv2;

namespace Building
{

    [RequireComponent(typeof(BoxCollider2D))]
    public class ActionThrow : CollisionLoading
    {
        public LevelManagerBuilding levelManager;



        protected override void Start()
        {
            base.Start();

        }

        /// <summary>
        /// Action runs when loading is done.
        /// Action depends on the GameState.
        /// </summary>
        public override void Action()
        {
            Debug.Log("Action activated.");
            levelManager.throwObject = true;
        }
    }

}
