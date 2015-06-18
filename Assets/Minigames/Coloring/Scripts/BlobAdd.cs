/**
 *@author Ján Bella
 */

using UnityEngine;
using System.Collections;
using System.Linq;

namespace Coloring
{
    public class BlobAdd : Blob
    {
        public BlobAdd(GameObject gameObject, ref GameObject brush, ref LevelManagerColoring levelManagerr)
        {
            blobGameObject = gameObject;

            blobGameObject.name = "blob_AddNewColor";

            //blobGameObject.renderer.material.color = colour;

            blobGameObject.tag = "ColoringBlob";

            TransformToLab b = blobGameObject.AddComponent<TransformToLab>();
            b.thisLevelManager = levelManagerr;
            b.Brush = brush;
        }
    }

    public class TransformToLab : MonoBehaviour
    {
        public GameObject Brush;
        public LevelManagerColoring thisLevelManager;

        public Animator mixingAnimator;

#if UNITY_ANDROID
        private Material neuronMaterial;

        void Start()
        {
            neuronMaterial = GameObject.Find("Neuron_body").renderer.material;
        }
#endif

        void OnMouseDown()
        {
            if (thisLevelManager.painting)
            {
                GameObject machine = GameObject.Find("ColorMixingMachine");
                ColorMixingMachine script = machine.GetComponent<ColorMixingMachine>();

                script.SetToColor(Brush.renderer.material.color);

                GameObject camera = GameObject.Find("MainCamera");
                Animator cameraAnimator = camera.GetComponent<Animator>();

                GameObject pallete = GameObject.Find("Pallete");
                Animator palleteAnimator = pallete.GetComponent<Animator>();

                Transform selected;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    selected = hit.transform;
                    if (selected.tag == "ColoringBlob")
                    {
                        cameraAnimator.SetBool("mixing", true);
                        cameraAnimator.SetTrigger("animate");

                        palleteAnimator.SetBool("visible", true);
                        palleteAnimator.SetBool("mixing", true);
                        palleteAnimator.SetTrigger("animateLab");

                        thisLevelManager.painting = false;
                        thisLevelManager.mixing = true;
                    }
                }

#if UNITY_ANDROID
                neuronMaterial.color = Brush.renderer.material.color;
#endif
            }
        }

    }
}