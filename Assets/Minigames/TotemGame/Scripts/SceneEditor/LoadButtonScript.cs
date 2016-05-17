using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TotemGame
{
    public class LoadButtonScript : MonoBehaviour
    {
        private string filesPath;
        private string path;
        private string fieldText;
        private DirectoryInfo dir;
        private FileInfo[] info;
        public Dropdown loadDropdown;
        
        void Start()
        {
            filesPath = Application.dataPath + "/Minigames/TotemGame/XmlDocs/";
            dir = new DirectoryInfo(filesPath);
            info = dir.GetFiles("*.xml");

            for (int i = 0; i < info.Length; i++)
            {
                fieldText = info.GetValue(i).ToString();
                loadDropdown.options.Add(new Dropdown.OptionData(Path.GetFileName(fieldText)));
            }
        }

        void Update()
        {
            //AssetDatabase.Refresh();
            FileInfo[] info2 = dir.GetFiles("*.xml");

            if (info2.Length > info.Length)
                info = info2;
            {
                loadDropdown.options.Clear();
                for (int i = 0; i < info.Length; i++)
                {
                    fieldText = info.GetValue(i).ToString();
                    loadDropdown.options.Add(new Dropdown.OptionData(Path.GetFileName(fieldText)));
                }
            }
        }

        public void loadOnClick()
        {
            //AssetDatabase.Refresh();
            TotemEditorManager.Instance.DeleteAll();
            int val = loadDropdown.GetComponent<Dropdown>().value;
            string selectedFile = info.GetValue(val).ToString();

            path = selectedFile;

            XmlReader reader = XmlReader.Create(path);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(reader);
            XmlNodeList Data = xmlDoc.GetElementsByTagName("Data");
            for (int i = 0; i < Data.Count; i++)
            {
                // getting data
                XmlNode DataChilds = Data.Item(i);
                // getting all gameObjects stored inside data
                XmlNodeList allGameObjects = DataChilds.ChildNodes;


                for (int j = 0; j < allGameObjects.Count; j++)
                {
                    XmlNode game_Objects = allGameObjects.Item(j);

                    GameObject obj = TotemEditorManager.Instance.InstantiateObject(game_Objects.Name);
                    
                    if (obj)
                    {
                        XmlNodeList GameObjects_Position_Rotation = game_Objects.ChildNodes;
                        //First element have the position stored inside it 
                        XmlNode GameObjects_Position = GameObjects_Position_Rotation.Item(0);
                        string[] split_position = GameObjects_Position.InnerText.Split(',');
                        obj.transform.position = new Vector3(float.Parse(split_position[0]), 
                            float.Parse(split_position[1]), float.Parse(split_position[2]));
                        
                        //Second element have the rotation stored inside it 
                        XmlNode GameObjects_Rotation = GameObjects_Position_Rotation.Item(1);
                        string[] split_rotation = GameObjects_Rotation.InnerText.Split(',');
                        obj.transform.rotation = new Quaternion(float.Parse(split_rotation[0]), 
                            float.Parse(split_rotation[1]), float.Parse(split_rotation[2]), float.Parse(split_rotation[3]));

                        XmlNode GameObjects_Scale = GameObjects_Position_Rotation.Item(2);
                        string[] split_scale = GameObjects_Scale.InnerText.Split(',');
                        obj.transform.localScale = new Vector3(float.Parse(split_scale[0]),
                            float.Parse(split_scale[1]), float.Parse(split_scale[2]));
                    }
                }
            }
            reader.Close();
        }
    }
}