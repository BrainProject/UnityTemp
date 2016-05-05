using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TotemGame
{
    public class InstantiatePrefabs : MonoBehaviour
    {
        private string filesPath;
        private string fieldText;
        private GameObject newObject;
        private DirectoryInfo dir;
        private FileInfo[] info;
        private Vector3 objScale;
        private Quaternion objRotation;

        public List<GameObject> prefabs = new List<GameObject>();
        public Dropdown loadDropdown;
        public Slider scaleXSlider;
        public Slider scaleYSlider;
        public Slider scaleXYSlider;
        public Slider rotationSlider;

        void Start()
        {
            Debug.Log("Tips: left mouse click for place the object, right mouse click for destroy the object");
            //filesPath = Application.dataPath + "/Prefabs/PrefabsObjects/Resources/";
            //dir = new DirectoryInfo(filesPath);
            //info = dir.GetFiles("*.prefab");

            for (int i = 0; i < prefabs.Count; i++)
            {
                fieldText = prefabs[i].name;
                loadDropdown.options.Add(new Dropdown.OptionData(fieldText));
            }
        }

        public void InstantiateObject()
        {
            int val = loadDropdown.GetComponent<Dropdown>().value;
            string selectedFile = prefabs[val].name;
            newObject = (GameObject)Instantiate(Resources.Load(selectedFile));
            newObject.name = Path.GetFileNameWithoutExtension(selectedFile);
            newObject.GetComponent<DestroyObject>().enabled = false;
            newObject.AddComponent<DetectCollision>();
            Rigidbody rb = newObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.freezeRotation = true;
            }

            objScale = newObject.transform.localScale;
            objRotation = newObject.transform.localRotation;

            TotemEditorlManager.Instance.PickItem(newObject.transform);
            newObject.transform.localPosition = Vector3.zero;
        }

        /*private void PlaceOnLeftClick()
        {
            if (Input.GetMouseButtonDown(0) && !isPlaced && !newObject.GetComponent<DetectCollision>().IsCollision)
            {
                newObject.GetComponent<Rigidbody>().useGravity = false;
                newObject.GetComponent<Collider>().isTrigger = true;
                newObject.GetComponent<Rigidbody>().isKinematic = true;
                newObject.GetComponent<FollowMouse>().enabled = false;
                isPlaced = true;
            } 
        }*/

        private void DeleteOnRightClick()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(newObject);
            }
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
    }
}
