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
        const string filePathSuffix = "/Coloring_Blobs.json";

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

        // prefab for ordinary color blobs
        public Object BlobPrefab;

        // prefab for adding new colors blob
        public Object BlobAddPrefab;

        private List<Blob> blobsList;

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

            GameObject parent = BlobsHolder;
            LevelManagerColoring lmc = GameObject.FindObjectOfType<LevelManagerColoring>();
            GameObject brush = GameObject.Find("Brush");

            if (parent == null || lmc == null || brush == null)
            {
                throw new MissingComponentException("Components missing! Unable to continue!");
            }

            if(blobsList != null)
            {
                blobsList.Clear();
            }
            else blobsList = new List<Blob>();

            for (int i = 0; i < pom.Length; i++)
            {
                GameObject go = buildBlobGameObjectWithTransform(X_MIN_COORD + i * X_SIZE, parent.transform);
                blobsList.Add(new Blob(go, pom[i], brush, lmc, this));
            }
            saveBlobs();
            AddBlobAdd(ref blobsList, X_MIN_COORD + pom.Length * X_SIZE, parent.transform, ref lmc);

        }

        private GameObject buildBlobGameObjectWithTransform(float x, Transform parent)
        {
            GameObject gameObject = GameObject.Instantiate(BlobPrefab) as GameObject;

            gameObject.transform.parent = parent;
            gameObject.transform.rotation = ROTATION;
            gameObject.transform.localScale = SCALE;
            gameObject.transform.localPosition = new Vector3(x, Y_COORD, Z_COORD);
            return gameObject;
        }

        void saveBlobs()
        {
            string filePath = Application.persistentDataPath + filePathSuffix;

            JSONClass file = new JSONClass();
            file.Add("count", new JSONData(blobsList.Count-1));
            JSONArray colours = new JSONArray();
            for(int i=0;i<blobsList.Count;i++)
            {
                BlobAdd form = blobsList[i] as BlobAdd;
                if (form != null)
                   
                //if (blobsList[i].GetType() == typeof(BlobAdd))
                {
                    Debug.Log("BlobAdd.");
                    continue;
                }

                JSONClass colour = new JSONClass();
                colour.Add("R", new JSONData(blobsList[i].blobGameObject.renderer.material.color.r));
                colour.Add("G", new JSONData(blobsList[i].blobGameObject.renderer.material.color.g));
                colour.Add("B", new JSONData(blobsList[i].blobGameObject.renderer.material.color.b));
                colours.Add(colour);
            }
            file.Add("colours", colours);
            Debug.Log(file.ToString());
            File.WriteAllText(filePath, file.ToString());
        }

        bool loadBlobs()
        {
            string filePath = Application.persistentDataPath + filePathSuffix;
            Debug.Log(filePath);

            // This text is added only once to the file. 
            if (File.Exists(filePath))
            {
                JSONNode root = JSON.Parse(File.ReadAllText(filePath));

                GameObject parent = BlobsHolder;
                LevelManagerColoring lmc = GameObject.FindObjectOfType<LevelManagerColoring>();
                GameObject brush = GameObject.Find("Brush");

                if (parent == null || lmc == null || brush == null)
                {
                    throw new MissingComponentException("Components missing! Unable to continue!");
                }

                if (blobsList != null)
                {
                    blobsList.Clear();
                }
                else blobsList = new List<Blob>();

                for(int i=0;i< root["count"].AsInt;i++)
                {
                    GameObject go = buildBlobGameObjectWithTransform(X_MIN_COORD + i * X_SIZE, parent.transform);
                    blobsList.Add(new Blob(go, 
                        new Color(root["colours"][i]["R"].AsFloat,
                            root["colours"][i]["G"].AsFloat,
                            root["colours"][i]["B"].AsFloat),
                        brush, 
                        lmc, 
                        this));
                    Debug.Log("Loaded colour: " + blobsList[blobsList.Count - 1].ToString());
                }
                AddBlobAdd(ref blobsList, X_MIN_COORD + root["count"].AsInt * X_SIZE, parent.transform, ref lmc);
                return true;
            }
            return false;
        }

        private void AddBlobAdd(ref List<Blob> blobList, float xCoord, Transform parent, ref LevelManagerColoring lmc)
        {
            GameObject gameObject = GameObject.Instantiate(BlobAddPrefab) as GameObject;

            gameObject.transform.parent = parent.transform;
            gameObject.transform.rotation = ROTATION;
            gameObject.transform.localScale = SCALE;
            gameObject.transform.localPosition = new Vector3(xCoord, Y_COORD, Z_COORD);
            blobsList.Add(new BlobAdd(gameObject, lmc));
        }


        public void Reset()
        {
            if (!loadBlobs() || blobsList.Count == 0)
            {
                CreateBasic();
            }

            float xMax = blobsList[blobsList.Count - 1].blobGameObject.transform.localPosition.x;
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

        public void RemoveBlob(Blob blob)
        {
            bool b = blobsList.Remove(blob);
            if (b) Debug.Log("REMOVE TRUE. Count: " + blobsList.Count);
            else Debug.Log("REMOVE FALSE.  Count: " + blobsList.Count);
            Destroy(blob.blobGameObject);
            saveBlobs();
            destroyBlobs();
            Reset();
        }

        public void AddColor(Color color)
        {
            GameObject gameObject = GameObject.Instantiate(BlobPrefab) as GameObject;

            blobsList.Add(new Blob(gameObject,color,null, null,null));
            saveBlobs();
            destroyBlobs();
            Reset();

        }

        private void destroyBlobs()
        {
            for (int i = 0; i < blobsList.Count; i++)
            {
                Destroy(blobsList[i].blobGameObject);
            }
        }

        void Start()
        {
            Reset();
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