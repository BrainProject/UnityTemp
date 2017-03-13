using UnityEngine;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class DetectCollision : MonoBehaviour
    {
        public bool isExplosive = false;
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
                if (TotemEditorManager.Instance)
                {
                    if (TotemEditorManager.Instance.heldItem == this.transform)
                        TotemEditorManager.Instance.PickItem();
                    else
                        TotemEditorManager.Instance.PickItem(this.transform);
                }
            }
        }  
    }
}
