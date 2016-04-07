using UnityEngine;
using System.Collections;
using Image = UnityEngine.UI.Image;

namespace GSIv2
{
    public class ActionGoBack : CollisionLoading 
    {
        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Action runs when loading is done.
        /// It goes back to the menu.
        /// </summary>
        public override void Action()
        {
            MGC.Instance.minigamesGUI.backIcon.GUIAction();           
        }

        
    }
}