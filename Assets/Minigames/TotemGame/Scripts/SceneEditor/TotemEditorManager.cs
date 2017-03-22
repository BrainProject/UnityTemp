using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class TotemEditorManager : MonoBehaviour
    {
        public static TotemEditorManager Instance { get; private set; }
        public Transform CursorRoot;
        public Toggle toggleExplosions;
        public List<GameObject> prefabs = new List<GameObject>();
        public Dropdown loadDropdown;
        public Slider scaleXSlider;
        public Slider scaleYSlider;
        public Slider scaleXYSlider;
        public Slider rotationSlider;

        internal Transform heldItem;

        private string filesPath;
        private string fieldText;
        private GameObject newObject;
        private DirectoryInfo dir;
        private FileInfo[] info;
        private Vector3 objScale;
        private Quaternion objRotation;

        void Start()
        {
            Instance = this;
            Debug.Log("Tips: left mouse click for place the object, right mouse click for destroy the object");

            for (int i = 0; i < prefabs.Count; i++)
            {
                fieldText = prefabs[i].name;
                loadDropdown.options.Add(new Dropdown.OptionData(fieldText));
            }
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

        public void GetNameOfObject()
        {
            int val = loadDropdown.GetComponent<Dropdown>().value;
            string selectedFile = prefabs[val].name;
            InstantiateObject(selectedFile);
        }

        public GameObject InstantiateObject(string selectedFile)
        {
            newObject = (GameObject)Instantiate(Resources.Load(selectedFile));
            newObject.name = selectedFile;
            if (newObject.GetComponent<DestroyObject>())
                newObject.GetComponent<DestroyObject>().enabled = false;
            if (newObject.GetComponent<Rigidbody>())
                newObject.GetComponent<Rigidbody>().useGravity = false;
            newObject.AddComponent<DetectCollision>();

            objScale = newObject.transform.localScale;
            objRotation = newObject.transform.localRotation;

            PickItem(newObject.transform);
            newObject.transform.localPosition = Vector3.zero;

            if (AddExplosionForce())
            {
                newObject.GetComponent<DetectCollision>().isExplosive = true;
                toggleExplosions.isOn = false;
            }
            return newObject;
        }

        public void ScaleObjectX()
        {
            newObject.transform.localScale = new Vector3(objScale.x + scaleXSlider.value,
                    newObject.transform.localScale.y, objScale.z);
        }

        public void ScaleObjectY()
        {
            newObject.transform.localScale = new Vector3(newObject.transform.localScale.x,
                    objScale.y + scaleYSlider.value, objScale.z);
        }

        public void ScaleObjectXY()
        {
            newObject.transform.localScale = new Vector3(objScale.x + scaleXYSlider.value,
                    objScale.y + scaleXYSlider.value, objScale.z);
        }

        public void ScaleObjectToDefault()
        {
            newObject.transform.localScale = new Vector3(objScale.x, objScale.y, objScale.y);
            scaleXSlider.value = 0;
            scaleYSlider.value = 0;
            scaleXYSlider.value = 0;
        }

        public void RotateObject()
        {
            newObject.transform.localRotation = new Quaternion(objRotation.x,
                     objRotation.y, objRotation.z + rotationSlider.value, objRotation.w);
        }

        public void RotateToDefault()
        {
            newObject.transform.localRotation = new Quaternion(objRotation.x, objRotation.y, objRotation.z, objRotation.w);
            rotationSlider.value = 0;
        }

        public void PickItem(Transform pickedItem = null)
        {
            if (heldItem)
            {
                if (heldItem.GetComponent<Rigidbody>())
                    heldItem.GetComponent<Rigidbody>().isKinematic = true;
                heldItem.GetComponent<Collider>().isTrigger = true;
                heldItem.SetParent(null);
                heldItem = null;
            }

            if (pickedItem)
            {
                heldItem = pickedItem;
                if (heldItem.GetComponent<Rigidbody>())
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

        public bool AddExplosionForce()
        {
            if (toggleExplosions.isOn == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
