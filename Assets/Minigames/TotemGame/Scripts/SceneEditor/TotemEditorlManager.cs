using UnityEngine;
using System.Collections;

namespace TotemGame
{
    public class TotemEditorlManager : MonoBehaviour
    {
        public static TotemEditorlManager Instance { get; private set; }
        public Transform CursorRoot;
        internal Transform heldItem;

        void Start()
        {
            Instance = this;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (heldItem)
                {
                    Destroy(heldItem.gameObject);
                    PickItem();
                }
            }
        }

        public void PickItem(Transform pickedItem = null)
        {
            if (heldItem)
            {
                heldItem.GetComponent<Rigidbody>().isKinematic = true;
                heldItem.GetComponent<Collider>().isTrigger = true;
                heldItem.SetParent(null);
                heldItem = null;
            }

            if (pickedItem)
            {
                heldItem = pickedItem;
                heldItem.GetComponent<Rigidbody>().isKinematic = false;
                heldItem.GetComponent<Collider>().isTrigger = false;
                pickedItem.SetParent(CursorRoot);
            }
        }

        public void DeleteAll()
        {
            GameObject[] GameObjects = GameObject.FindGameObjectsWithTag("scene");

            if (GameObjects.Length != 0)
            {
                for (int i = 0; i < GameObjects.Length; i++)
                {
                    Destroy(GameObjects[i]);
                }
            }
        }
    }
}
