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
        public BlobAdd(GameObject gameObject, LevelManagerColoring levelManager)
        {
            blobGameObject = gameObject;

            blobGameObject.name = "blob_AddNewColor";

            //blobGameObject.renderer.material.color = colour;

            blobGameObject.tag = "ColoringBlob";

            TransformToLab b = blobGameObject.AddComponent<TransformToLab>();
        }
    }

    public class TransformToLab : MonoBehaviour
    {
        public GameObject Brush;
        public LevelManagerColoring thisLevelManager;

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
                Transform selected;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    selected = hit.transform;
                    if (selected.tag == "ColoringBlob")
                    {
                        // spustit animaciu
                    }
                }

#if UNITY_ANDROID
                neuronMaterial.color = Brush.renderer.material.color;
#endif
            }
        }

    }
}