/**
 *@author Ján Bella
 */

using UnityEngine;
using System.Collections;
using System.Linq;
using System;


namespace Coloring
{
    public class Blob
    {
        public GameObject blobGameObject;

        protected BlobMenu menu;

        protected Blob()
        {

        }

        public Blob(GameObject gameObject, Color colour, GameObject brush, LevelManagerColoring levelManager, BlobMenu menu)
        {
            blobGameObject = gameObject;

            blobGameObject.name = "blob_" + colour.ToString();

            blobGameObject.renderer.material.color = colour;

            blobGameObject.tag = "ColoringBlob";

            SelectColour b = blobGameObject.AddComponent<SelectColour>();
            b.Brush = brush;
            b.thisLevelManager = levelManager;
            b.color = blobGameObject.renderer.material.color;

            DeleteColour d = blobGameObject.AddComponent<DeleteColour>();
            d.thisLevelManager = levelManager;

            this.menu = menu;
            d.menu = menu;
            d.blob = this;
        } 
    }

    public class SelectColour : MonoBehaviour
    {
        public GameObject Brush;
        public LevelManagerColoring thisLevelManager;
        public Color color;

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
                Brush.renderer.material.color = color;
                thisLevelManager.brushColor = color;

#if UNITY_ANDROID
                neuronMaterial.color = Brush.renderer.material.color;
#endif
            }
            else if(thisLevelManager.mixing)
            {
                GameObject machine = GameObject.Find("ColorMixingMachine");
                ColorMixingMachine script = machine.GetComponent<ColorMixingMachine>();

                script.SetToColor(color);
            }
        }

    }

    public class DeleteColour : MonoBehaviour
    {
        public LevelManagerColoring thisLevelManager;
        public BlobMenu menu;
        public Blob blob;

        private Vector3 originalObjPosition = Vector3.zero;
        private Vector3 mouseDownPosition = Vector3.zero;

        private bool moving = false;

#if UNITY_ANDROID
        private Material neuronMaterial;

        void Start()
        {
            neuronMaterial = GameObject.Find("Neuron_body").renderer.material;
        }
#endif

        Vector3 screenPoint;
        Vector3 offset;

        void OnMouseDown()
        {
            if (thisLevelManager.painting)
            {
                originalObjPosition = transform.position;
                screenPoint = Camera.main.WorldToScreenPoint(originalObjPosition);
                offset = originalObjPosition - Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                moving = true;
            }
        }

        void OnMouseDrag()
        {
            if (moving && thisLevelManager.painting)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

                transform.position = curPosition;
                //Debug.Log(curPosition - originalObjPosition);
            }
        }

        void OnMouseUp()
        {
            if (moving && thisLevelManager.painting)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

                if (Math.Abs(curPosition.y - originalObjPosition.y) > 0.2)
                {
                    Debug.Log("delete");

                    menu.RemoveBlob(ref blob);
                }
                else
                {
                    Debug.Log("Position restored. Tag: ");
                    transform.position = originalObjPosition;
                }
            }
            moving = false;
        }
    }
}