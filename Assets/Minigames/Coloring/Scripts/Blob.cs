/**
 *@author Tomáš Pouzar & Ján Bella
 */

using UnityEngine;
using System.Collections;
using System.Linq;

namespace Coloring
{
    public class Blob 
    {
        public GameObject blobGameObject;

        protected Blob()
        {

        }

        public Blob(GameObject gameObject, Color colour, GameObject brush, LevelManagerColoring levelManager)
        {
            blobGameObject = gameObject;

            blobGameObject.name = "blob_" + colour.ToString();

            blobGameObject.renderer.material.color = colour;

            blobGameObject.tag = "ColoringBlob";

            SelectColour b = blobGameObject.AddComponent<SelectColour>();
            b.Brush = brush;
            b.thisLevelManager = levelManager;
        }
    }

    public class SelectColour : MonoBehaviour
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

        void OnMouseUp()
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
                        Brush.renderer.material.color = selected.renderer.material.color;
                        thisLevelManager.brushColor = selected.renderer.material.color;
                    }
                }

#if UNITY_ANDROID
                neuronMaterial.color = Brush.renderer.material.color;
#endif
            }
        }

    }
}