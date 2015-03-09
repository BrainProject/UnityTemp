/**
 *@author Ján Bella
 */

using UnityEngine;
using System.Collections.Generic;

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

            Transform parent = gameObject.transform.FindChild("Blobs");
            LevelManagerColoring lmc = GameObject.FindObjectOfType<LevelManagerColoring>();
            GameObject brush = GameObject.Find("Brush");

            if (parent == null || lmc == null || brush == null)
            {
                if(lmc == null)
                {
                    Debug.Log("LMC null");
                }
                if(brush == null)
                {
                    Debug.Log("Brush null");
                }
                throw new MissingComponentException("Components missing! Unable to continue!");
            }

            blobsList = new List<GameObject>();

            for (int i = 0; i < pom.Length; i++)
            {
                blobsList.Add(CreateBlob(parent, pom[i], X_MIN_COORD + i * X_SIZE,brush,lmc));
            }
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

        // Use this for initialization
        void Start()
        {
            CreateBasic();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseOver()
        {
            Debug.Log("Mouse over");
        }
    }
}