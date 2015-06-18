/**
 *@author Ján Bella
 */
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

        // in global
        const float MIN_Y = 0.93f;//0.83f;//-0.14f;
        const float MAX_Y = 1.15f;//1.05f;// 0.1f;

        // in local
        const float MAX_Y_BAR = -0.01f;
        const float MIN_Y_BAR = -0.29f;
        const float BAR_Z = -0.07f;

        // in degrees
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

                handle.transform.rotation = Quaternion.AngleAxis((MAX_ROT - MIN_ROT) * percentage + MIN_ROT, new Vector3(1, 0, 0));


                displayText.GetComponent<TextMesh>().text = Math.Round(255 * percentage, 0).ToString();

                stripe.transform.localScale = new Vector3(stripe.transform.localScale.x,
                                                          (MAX_Y_BAR - MIN_Y_BAR) / 2.0f * percentage,
                                                          stripe.transform.localScale.z);


                stripe.transform.localPosition = new Vector3(stripe.transform.localPosition.x,
                                                          (MAX_Y_BAR - MIN_Y_BAR) * percentage + MIN_Y_BAR,
                                                          BAR_Z);
            }
        }

        void OnMouseUp()
        {
            //Vector3 curScreenPoint = new Vector3(0, Input.mousePosition.y, screenPoint.z);
            //Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

            //Debug.Log("Mouse up.");

            // nothing
        }

    }
}