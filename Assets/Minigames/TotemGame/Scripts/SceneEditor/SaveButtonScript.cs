using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine.UI;
//using UnityEditor;
using UnityEngine.SceneManagement;
using System.Collections;

namespace TotemGame
{
    public class SaveButtonScript : MonoBehaviour
    {

        private List<GameObject> Objects = new List<GameObject>();
        private GameObject[] allObjects = new GameObject[0];
        private string path;
        public GameObject inputFieldGo;
        private string fieldText;
        private string filesPath;
        private DirectoryInfo dir;
        private FileInfo[] info;
        private string fileName;
        private bool isExists;
        /*
        public void saveOnClick()
        {
            //GameObject[] allObjects = new GameObject[GameObject.FindGameObjectsWithTag("scene").Length];
            allObjects = GameObject.FindGameObjectsWithTag("scene");
            for (int i = 0; i < allObjects.Length; i++)
            {
                if (allObjects[i] != null)
                {
                    //getting all objects that needs to be stored in xml
                    Objects.Add(allObjects[i]);
                }
            }
            fieldText = inputFieldGo.GetComponent<InputField>().text;


            filesPath = Application.dataPath + "/Minigames/TotemGame/XmlDocs/";
            dir = new DirectoryInfo(filesPath);
            info = dir.GetFiles("*.xml");

            for (int i = 0; i == info.Length; i++)
            {
                fileName = Path.GetFileNameWithoutExtension(info.GetValue(i).ToString());
                if (fileName == Path.GetFileName(fieldText).ToString())
                {
                    isExists = true;
                }
            }


            if (fieldText.Length == 0)
            {
                Debug.Log("You must type 1 or more characters");
            }
            else if (isExists)
            {
                Debug.Log("You must change the name of file. File with name " + fileName + " has already exists.");
                isExists = false;
            }
            else
            {
                path = Application.dataPath + "/Minigames/TotemGame/XmlDocs/" + fieldText + ".xml";
                XmlDocument xmlDoc = new XmlDocument();

                XmlElement elmRoot = xmlDoc.CreateElement("Data");
                xmlDoc.AppendChild(elmRoot);

                for (int i = 0; i < Objects.Count; i++)
                {
                    if (Objects[i] != null)
                    {
                        //Creating an xml element with object name
                        XmlElement Object = xmlDoc.CreateElement(Objects[i].name);
                        //Creating an xml element for saving object position
                        XmlElement Object_Position = xmlDoc.CreateElement("Position");
                        //Combining object position x,y and z value into a single string separeted by commas
                        Object_Position.InnerText = Objects[i].transform.position.x + ","
                            + Objects[i].transform.position.y + "," + Objects[i].transform.position.z;
                        //Creating an xml element for saving object rotation
                        XmlElement Object_Rotation = xmlDoc.CreateElement("Rotation");
                        //Combining object rotation x,y,z and w value into a single string separeted by commas
                        Object_Rotation.InnerText = Objects[i].transform.rotation.x + ","
                            + Objects[i].transform.rotation.y + "," + Objects[i].transform.rotation.z + "," + Objects[i].transform.rotation.w;

                        XmlElement Object_Scale = xmlDoc.CreateElement("Scale");
                        Object_Scale.InnerText = Objects[i].transform.localScale.x + ","
                            + Objects[i].transform.localScale.y + "," + Objects[i].transform.localScale.z;

                        Object.AppendChild(Object_Position);
                        Object.AppendChild(Object_Rotation);
                        Object.AppendChild(Object_Scale);

                        if (Objects[i].GetComponent<DetectCollision>())
                        {
                            if (Objects[i].GetComponent<DetectCollision>().isExplosive == enabled)
                            {
                                XmlElement Object_Explosion = xmlDoc.CreateElement("Explosion");
                                Object.AppendChild(Object_Explosion);
                            }
                        }
                        elmRoot.AppendChild(Object);
                    }
                }

                StreamWriter outStream = File.CreateText(path);

                xmlDoc.Save(outStream);
                outStream.Close();
            }
            //AssetDatabase.Refresh();
        }*/
    }
}