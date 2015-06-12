/**
 *@author Ján Bella
 */

using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

namespace Coloring
{

    public class BlobMenu : MonoBehaviour
    {
        // constants determined from editor according to Tom Pouzar's scene model
        const float X_MAX_COORD = -0.2f;
        const float X_MIN_COORD = -2.3f;

        const float Y_COORD = 0.5f;
        const float Z_COORD = -4.1f;

        const float SCALE_X = 0.2f;
        const float SCALE_Y = 0.2f;
        const float SCALE_Z = 0.2f;

        const float X_SIZE = 0.25f;

        // attention! Quaternion nor Vector3 cannot be declared const, but should never change!
        static Quaternion ROTATION = Quaternion.Euler(90, 180, 0);
        static Vector3 SCALE = new Vector3(0.02f, 0.02f, 0.02f);

        public Object BlobPrefab;

        private List<GameObject> blobsList;

        public GameObject BlobsHolder;

        void CreateBasic()
        {
            Color[] pom = { new Color(0.5f, 0, 0.9f), 
                            new Color(1.0f, 0.6f, 0),
                            Color.white,
                            Color.black,
                            Color.red,
                            Color.green,
                            Color.blue,
                            Color.yellow };

            //Transform parent = gameObject.transform.FindChild("Blobs");
            GameObject parent = BlobsHolder;
            LevelManagerColoring lmc = GameObject.FindObjectOfType<LevelManagerColoring>();
            GameObject brush = GameObject.Find("Brush");

            if (parent == null || lmc == null || brush == null)
            {
                throw new MissingComponentException("Components missing! Unable to continue!");
            }

            blobsList = new List<GameObject>();

            for (int i = 0; i < pom.Length; i++)
            {
                blobsList.Add(CreateBlob(parent.transform, pom[i], X_MIN_COORD + i * X_SIZE, brush, lmc));
            }
            saveBlobs();
        }

        GameObject CreateBlob(Transform parent, Color colour, float xCoord, GameObject brush, LevelManagerColoring levelManager)
        {
            GameObject obj = GameObject.Instantiate(BlobPrefab) as GameObject;

            obj.name = "blob_" + colour.ToString();
            obj.transform.parent = parent;

            obj.transform.rotation = ROTATION;
            obj.transform.localScale = SCALE;
            obj.transform.localPosition = new Vector3(xCoord, Y_COORD, Z_COORD);

            obj.renderer.material.color = colour;

            SelectColor sc = obj.GetComponent<SelectColor>();
            sc.Brush = brush;
            sc.thisLevelManager = levelManager;

            obj.tag = "ColoringBlob";

            return obj;
        }

        void saveBlobs()
        {
            JSONClass file = new JSONClass();
            file.Add("count", new JSONData(blobsList.Count));
            JSONArray colours = new JSONArray();
            for(int i=0;i<blobsList.Count;i++)
            {
                JSONClass colour = new JSONClass();
                colour.Add("R", new JSONData(blobsList[i].renderer.material.color.r));
                colour.Add("G", new JSONData(blobsList[i].renderer.material.color.g));
                colour.Add("B", new JSONData(blobsList[i].renderer.material.color.b));
                colours.Add(colour);
            }
            file.Add("colours", colours);

            File.WriteAllText(Application.persistentDataPath + "/Coloring_Blobs.json", file.ToString());
        }

        bool loadBlobs()
        {
            string path = Application.persistentDataPath + "/Coloring_Blobs.json";

            // This text is added only once to the file. 
            if (File.Exists(path))
            {
                JSONNode root = JSON.Parse(File.ReadAllText(path));

                GameObject parent = BlobsHolder;
                LevelManagerColoring lmc = GameObject.FindObjectOfType<LevelManagerColoring>();
                GameObject brush = GameObject.Find("Brush");

                if (parent == null || lmc == null || brush == null)
                {
                    throw new MissingComponentException("Components missing! Unable to continue!");
                }

                blobsList = new List<GameObject>();


                for(int i=0;i< root["count"].AsInt;i++)
                {
                    //Debug.Log("R: " + root["colours"][i]["R"].AsFloat + ", G: " + root["colours"][i]["G"].AsFloat + ", B: " + root["colours"][i]["B"].AsFloat);
                    blobsList.Add(CreateBlob(parent.transform,
                        new Color(root["colours"][i]["R"].AsFloat,
                                  root["colours"][i]["G"].AsFloat,
                                  root["colours"][i]["B"].AsFloat),
                                  X_MIN_COORD + i * X_SIZE, brush, lmc));
                    Debug.Log("Loaded colour: " + blobsList[blobsList.Count-1].ToString());
                }
                return true;
            }
            return false;
        }


        // Use this for initialization
        void Start()
        {
            if (!loadBlobs() || blobsList.Count == 0)
            {
                CreateBasic();
            }

            float xMax = blobsList[blobsList.Count - 1].transform.localPosition.x;
            Debug.Log("XMax is " + xMax);

            if (xMax < X_MAX_COORD)
            {
                Debug.Log("Shifting blobs");
                BlobsHolder.transform.position = 
                    new Vector3(BlobsHolder.transform.position.x + (X_MAX_COORD - xMax) / 2.0f,
                                BlobsHolder.transform.position.y,
                                BlobsHolder.transform.position.z);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseOver()
        {
            Vector3 pos = Input.mousePosition;
            if(pos.x < Screen.width * 0.04)
            {
                Debug.Log("Move right");
            }
            else if(pos.x > Screen.width * 0.96)
            {
                Debug.Log("Move left");
            }
        }
    }
}