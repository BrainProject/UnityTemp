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
        const float X_MIN_COORD = -2.25f; // -2.3

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

        public GameObject brush;

        public LevelManagerColoring levelManagerController;

        public GameObject colorPreview;
        
        void Start()
        {
            if (BlobsHolder == null || levelManagerController == null || brush == null)
            {
                throw new MissingComponentException("Components missing! Unable to continue!");
            }

            Reset();
        }

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
           
            if(blobsList != null)
            {
                blobsList.Clear();
            }
            else blobsList = new List<Blob>();

            AddBlobAdd(ref blobsList, X_MIN_COORD, BlobsHolder.transform);

            for (int i = 1; i <= pom.Length; i++)
            {
                GameObject go = buildBlobGameObjectWithTransform(X_MIN_COORD + i * X_SIZE, BlobsHolder.transform);
                blobsList.Add(new Blob(go, pom[i-1], brush, levelManagerController, this));
            }
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
            for(int i=1;i<blobsList.Count;i++)
            {
                JSONClass colour = new JSONClass();
                colour.Add("R", new JSONData(blobsList[i].blobGameObject.renderer.material.color.r));
                colour.Add("G", new JSONData(blobsList[i].blobGameObject.renderer.material.color.g));
                colour.Add("B", new JSONData(blobsList[i].blobGameObject.renderer.material.color.b));
                colours.Add(colour);
            }
            file.Add("colours", colours);
            File.WriteAllText(filePath, file.ToString());
        }

        bool loadBlobs()
        {
            string filePath = Application.persistentDataPath + filePathSuffix;
            //Debug.Log(filePath);

            // This text is added only once to the file. 
            if (File.Exists(filePath))
            {
                JSONNode root = JSON.Parse(File.ReadAllText(filePath));

                if (blobsList != null)
                {
                    blobsList.Clear();
                }
                else blobsList = new List<Blob>();

                AddBlobAdd(ref blobsList, X_MIN_COORD, BlobsHolder.transform);

                for(int i=1;i<= root["count"].AsInt;i++)
                {
                    GameObject go = buildBlobGameObjectWithTransform(X_MIN_COORD + i * X_SIZE, BlobsHolder.transform);
                    blobsList.Add(new Blob(go, 
                        new Color(root["colours"][i - 1]["R"].AsFloat,
                            root["colours"][i - 1]["G"].AsFloat,
                            root["colours"][i - 1]["B"].AsFloat),
                        brush, 
                        levelManagerController, 
                        this));
                    Debug.Log("Loaded colour: " + blobsList[blobsList.Count - 1].ToString());
                }
                return true;
            }
            else return false;
        }

        private void AddBlobAdd(ref List<Blob> blobList, float xCoord, Transform parent)
        {
            GameObject gameObject = GameObject.Instantiate(BlobAddPrefab) as GameObject;

            gameObject.transform.parent = parent.transform;
            gameObject.transform.rotation = ROTATION;
            gameObject.transform.localScale = SCALE;
            gameObject.transform.localPosition = new Vector3(xCoord, Y_COORD, Z_COORD);
            blobsList.Add(new BlobAdd(gameObject, ref brush, ref levelManagerController));
        }


        public void Reset()
        {
            if (!loadBlobs() || blobsList.Count <= 1)
            {
                CreateBasic();
                saveBlobs();
            }

            float xMax = blobsList[blobsList.Count - 1].blobGameObject.transform.localPosition.x;
            float xMin = blobsList[0].blobGameObject.transform.position.x;

            if (xMin < X_MIN_COORD)
            {
                Debug.Log("Shifting blobs");
                BlobsHolder.transform.position =
                    new Vector3(X_MIN_COORD,
                                BlobsHolder.transform.localPosition.y,
                                BlobsHolder.transform.localPosition.z);
            }
        }

        public void RemoveBlob(ref Blob blob)
        {
            for (int i = 0; i < blobsList.Count;i++ )
            {
                if(blobsList[i].blobGameObject.name == blob.blobGameObject.name)
                {
                    blobsList.RemoveAt(i);
                    break;
                }
            }

            Destroy(blob.blobGameObject);
            saveBlobs();
            destroyBlobs();
            Reset();
        }

        public void AddColor(Color color)
        {
            GameObject gameObject = GameObject.Instantiate(BlobPrefab) as GameObject;

            blobsList.Add(new Blob(gameObject, color, null, null, null));
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

        void OnMouseOver()
        {
            Vector3 firstBlobPosition = blobsList[0].blobGameObject.transform.localPosition;
            Vector3 lastBlobPosition = blobsList[blobsList.Count - 1].blobGameObject.transform.localPosition;
            Vector3 pos = Input.mousePosition;
            if (pos.x < Screen.width * 0.04 
                && BlobsHolder.transform.localPosition.x < 0.422f)
            {
                BlobsHolder.transform.localPosition =
                    new Vector3(BlobsHolder.transform.localPosition.x + 0.005f,
                        BlobsHolder.transform.localPosition.y,
                        BlobsHolder.transform.localPosition.z);
                Debug.Log("Move right, camera moves left");
            }
            else if (pos.x > Screen.width * 0.96 
                     && BlobsHolder.transform.localPosition.x > 0.422f - System.Math.Max(0, blobsList.Count-9) * 0.1f)
            {
                BlobsHolder.transform.localPosition =
                    new Vector3(BlobsHolder.transform.localPosition.x - 0.005f,
                        BlobsHolder.transform.localPosition.y,
                        BlobsHolder.transform.localPosition.z);
                Debug.Log("Move left, camera moves right");
            }
        }

        public void AddMixedColor()
        {
            if(colorPreview)
            {
                Debug.LogError(colorPreview.renderer.material.color);
                AddColor(colorPreview.renderer.material.color);
            }
        }
    }
}