using UnityEngine;
using System.Collections;

namespace TotemGame
{
    public class DetectCollision : MonoBehaviour
    {
        private Color startcolor;
        private bool isCollision;

        void Start()
        {
            startcolor = GetComponent<Renderer>().material.color;
        }

        void OnTriggerEnter(Collider other)
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            isCollision = true;
        }

        void OnTriggerStay(Collider other)
        {
            other.gameObject.GetComponent<Renderer>().material.color = Color.red;
            isCollision = true;
        }

        void OnTriggerExit(Collider other)
        {
            other.gameObject.GetComponent<Renderer>().material.color = startcolor;
            isCollision = false;
        }

        void OnMouseDown()
        {
            if (!isCollision)
            {
                if (TotemEditorlManager.Instance)
                {
                    if (TotemEditorlManager.Instance.heldItem == this.transform)
                        TotemEditorlManager.Instance.PickItem();
                    else
                        TotemEditorlManager.Instance.PickItem(this.transform);
                }
            }
        }
    }
}
