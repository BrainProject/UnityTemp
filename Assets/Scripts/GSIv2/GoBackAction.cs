using UnityEngine;
using System.Collections;
using Image = UnityEngine.UI.Image;

namespace GSIv2
{
    public class GoBackAction : CollisionLoading 
    {
        /*private bool activated;         //collision has been detected
        private bool done;              //image is whole drawn
        private Image img;*/

 
        protected override void Start()
        {
            base.Start();
           /* img = GetComponent<Image>();
            img.fillAmount = 0;
            activated = false;
            done = false;*/

        }

        /* void Update()
         {
             if (activated)
             {
                 if (img.fillAmount < 1)
                 {
                     img.fillAmount += (0.6f * Time.deltaTime);
                 }
                 else
                 {
                     done = true;
                 }
             }
             else
             {
                 img.fillAmount = 0;
             }

             if(done)
             {
                 MGC.Instance.minigamesGUI.backIcon.GUIAction();
                 done = false;
                 img.fillAmount = 0;
                 this.gameObject.SetActive(false);

             }

         }*/

        public override void Action()
        {
            MGC.Instance.minigamesGUI.backIcon.GUIAction();           
        }

        
    }
}