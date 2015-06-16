using UnityEngine;
using System.Collections;
using System;

namespace Coloring
{
    public class AdjustColor : MonoBehaviour
    {

        public GameObject handle;
        public GameObject displayText;
        public GameObject stripe;

        const float MIN_Y = 0.83f;//-0.14f;
        const float MAX_Y = 1.05f;// 0.1f;

        private const float MIN_ROT = 240;
        private const float MAX_ROT = 300;

        private Vector3 screenPoint;
        private Vector3 offset;

        public LevelManagerColoring levelManager;

        void Start()
        {
            if (!levelManager)
            {
                this.levelManager = GameObject.FindObjectOfType<LevelManagerColoring>();
            }
            if (!levelManager)
            {
                Debug.LogError("LevelManagerColoring in one of AdjustColor.cs is not set!");
            }
        }

        void OnMouseDown()
        {
            Debug.LogWarning("OnMouseDown called.");
            if (levelManager.mixing)
            {
                screenPoint = Camera.main.WorldToScreenPoint(handle.transform.position);
                offset = handle.transform.position - Camera.main.ScreenToWorldPoint(
                        new Vector3(0, Input.mousePosition.y, screenPoint.z));

            }
        }

        void OnMouseDrag()
        {
            if (levelManager.mixing)
            {
                Vector3 curScreenPoint = new Vector3(0, Input.mousePosition.y, screenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

                handle.transform.position = new Vector3(curPosition.x,
                                                        Math.Min(Math.Max(curPosition.y, MIN_Y), MAX_Y),
                                                        curPosition.z);

                float percentage = (handle.transform.position.y - MIN_Y) / (MAX_Y - MIN_Y);
                
                handle.transform.rotation = Quaternion.AngleAxis((MAX_ROT - MIN_ROT) * percentage + MIN_ROT,new Vector3(1,0,0));


                displayText.GetComponent<TextMesh>().text = Math.Round(255 * percentage, 0).ToString();

                float oldScale = stripe.transform.localScale.y;

                stripe.transform.localScale = new Vector3(stripe.transform.localScale.x,
                                                          0.14f * percentage,
                                                          stripe.transform.localScale.z);
                

                // -0.29... -0.11

                stripe.transform.localPosition = new Vector3(stripe.transform.localPosition.x,
                                                             (-0.01f+0.29f) * percentage - 0.29f,
                                                             -0.07f);
            }
        }

        void OnMouseUp()
        {
            Vector3 curScreenPoint = new Vector3(0, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            Debug.Log("Mouse up.");
        }

    }
}