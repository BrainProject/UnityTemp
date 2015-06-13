/**
 *@author Tomáš Pouzar & Ján Bella
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

    public class DeleteColour : MonoBehaviour
    {
        public LevelManagerColoring thisLevelManager;
        public BlobMenu menu;
        public Blob blob;

        private GameObject selectedColour = null;
        private Vector3 originalObjPosition = Vector3.zero;
        private Vector3 mouseDownPosition = Vector3.zero;

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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Transform selected = hit.transform;
                    if (selected.tag == "ColoringBlob")
                    {
                        selectedColour = selected.gameObject;
                        originalObjPosition = selected.position;

                        screenPoint = Camera.main.WorldToScreenPoint(originalObjPosition);
                        offset = originalObjPosition - Camera.main.ScreenToWorldPoint(
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

                    }
                }
            }
        }

        void OnMouseDrag()
        {
            if (selectedColour != null && thisLevelManager.painting)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

                transform.position = curPosition;
                Debug.Log(curPosition - originalObjPosition);
            }
        }

        void OnMouseUp()
        {
            /*if (selectedColour != null && thisLevelManager.painting)
            {
                Ray ray = Camera.main.ScreenPointToRay(mouseDownPosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit) )
                {
                    Transform selected = hit.transform;
                    if ((selected.tag == "ColoringBlob" && selected != gameObject) || selected.tag == "ColoringPallete")
                    {
                        Debug.Log("Position restored. Tag: " + selected.tag);
                        selected.position = originalObjPosition;
                    }
                    else
                    {
                        Debug.Log("Deleted. Tag: " + selected.tag);
                    }
                }
                else
                {
                    Debug.Log("Deleted.");
                }
            }*/

            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            if (Math.Abs(curPosition.y - originalObjPosition.y) > 0.2)
            {
                Debug.Log("delete");

                menu.RemoveBlob(blob);
            }
            else
            {
                Debug.Log("Position restored. Tag: ");
                selectedColour.transform.position = originalObjPosition;
            }

            selectedColour = null;

        }

    }
}