/**
 *@author  Ján Bella
 */
using UnityEngine;
using System.Collections;


namespace Coloring
{
    public class NewColorBlobInteraction : MonoBehaviour
    {
        public GameObject system;

        public LevelManagerColoring levelManager;

        void OnParticleCollision(GameObject other)
        {
            if(levelManager.mixing)
            {
                levelManager.mixing = false;
                levelManager.painting = true;
                gameObject.renderer.material.color = system.particleSystem.startColor;
                Animator anim = gameObject.GetComponent<Animator>();
                anim.SetTrigger("animate");

                GameObject pallete = GameObject.Find("Pallete");
                anim = pallete.GetComponent<Animator>();
                anim.SetBool("mixing", false);
                anim.SetTrigger("animateLab");

                GameObject cam = GameObject.Find("MainCamera");
                anim = cam.GetComponent<Animator>();
                anim.SetBool("mixing", false);
                anim.SetTrigger("animate");
        }
        }
    }
}