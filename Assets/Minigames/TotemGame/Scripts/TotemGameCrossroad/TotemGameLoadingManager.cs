using UnityEngine;
using System.IO;
using System.Xml;

namespace TotemGame
{
    public class TotemGameLoadingManager : MonoBehaviour
    {
        private string fieldText;
        private GameObject obj;
        public string xmlName;

        void Start()
        {
                LoadNewLevel(TotemGameCrossroadManager.Instance.filesPath);
                //LoadNewLevel(Application.dataPath + "/Minigames/TotemGame/XmlDocs/" + xmlName + ".xml");
        }

        public void LoadNewLevel(string fpath)
        {
            DirectoryInfo dir = new DirectoryInfo(fpath);
            FileInfo[] info = dir.GetFiles("*.xml");
            int x = info.Length;
            int rndm = Random.Range(0, x);
            fieldText = info.GetValue(rndm).ToString();
            
            XmlReader reader = XmlReader.Create(fieldText);
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
                    
                    if (game_Objects.Name == TotemLevelManager.Instance.player.name)
                    {
                        obj = TotemLevelManager.Instance.player;
                    }
                    else {
                        obj = Instantiate(Resources.Load(game_Objects.Name,
                        typeof(GameObject))) as GameObject;
                        obj.name = game_Objects.Name;
                    }

                    if (obj.name == "GoalCube")
                    {
                        obj.AddComponent<DetectWin>();
                    }

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

                        //Third element have the scale stored inside it 
                        XmlNode GameObjects_Scale = GameObjects_Position_Rotation.Item(2);
                        string[] split_scale = GameObjects_Scale.InnerText.Split(',');
                        obj.transform.localScale = new Vector3(float.Parse(split_scale[0]),
                            float.Parse(split_scale[1]), float.Parse(split_scale[2]));

                        if (GameObjects_Position_Rotation.Count >3)
                        {
                            //XmlNode GameObjects_Explosion = GameObjects_Position_Rotation.Item(3);
                            if (obj.GetComponent<DestroyObject>())
                                Destroy(obj.GetComponent<DestroyObject>());
                            obj.AddComponent<ExplosionForce>();
                        }
                    }
                }
            }
            reader.Close();
        }
    }
}
