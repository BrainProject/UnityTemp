using UnityEngine;
using System.Collections;


namespace GSIv2
{
    public class ActionHelp : CollisionLoading
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
            if (MGC.Instance.neuronHelp)
            {
                MGC.Instance.neuronHelp.GetComponent<Game.NEWBrainHelp>().helpObject.ShowHelpAnimation();
            }
            else
            {
                Debug.Log("Help is not available");
            }
        }
    } 
}
