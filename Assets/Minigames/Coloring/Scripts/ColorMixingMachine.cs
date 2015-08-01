/**
 * @file ColorMixingMachine.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;
using System;

namespace Coloring
{
    public class ColorMixingMachine : MonoBehaviour
    {

        public GameObject redHandle;
        public GameObject redBar;
        public GameObject redDisplayText;

        public GameObject greenHandle;
        public GameObject greenBar;
        public GameObject greenDisplayText;

        public GameObject blueHandle;
        public GameObject blueBar;
        public GameObject blueDisplayText;

        public GameObject colorPreview;

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

        public void SetToColor(Color color)
        {

            // RED

            redHandle.transform.position = new Vector3(redHandle.transform.position.x,
                                                       MIN_Y + color.r * (MAX_Y - MIN_Y),
                                                       redHandle.transform.position.z);

            redHandle.transform.rotation = Quaternion.AngleAxis((MAX_ROT - MIN_ROT) * color.r + MIN_ROT, new Vector3(1, 0, 0));


            redDisplayText.GetComponent<TextMesh>().text = Math.Round(color.r * 255.0f, 0).ToString();

            redBar.transform.localScale = new Vector3(redBar.transform.localScale.x,
                                                      (MAX_Y_BAR - MIN_Y_BAR) / 2.0f * color.r,
                                                      redBar.transform.localScale.z);

            redBar.transform.localPosition = new Vector3(redBar.transform.localPosition.x,
                                                         (MAX_Y_BAR - MIN_Y_BAR) * color.r + MIN_Y_BAR,
                                                         BAR_Z);

            // GREEN

            greenHandle.transform.position = new Vector3(greenHandle.transform.position.x,
                                                       MIN_Y + color.g * (MAX_Y - MIN_Y),
                                                       greenHandle.transform.position.z);

            greenHandle.transform.rotation = Quaternion.AngleAxis((MAX_ROT - MIN_ROT) * color.g + MIN_ROT, new Vector3(1, 0, 0));


            greenDisplayText.GetComponent<TextMesh>().text = Math.Round(color.g * 255.0f,0).ToString();

            greenBar.transform.localScale = new Vector3(greenBar.transform.localScale.x,
                                                      (MAX_Y_BAR - MIN_Y_BAR) / 2.0f * color.g,
                                                      greenBar.transform.localScale.z);

            greenBar.transform.localPosition = new Vector3(greenBar.transform.localPosition.x,
                                                         (MAX_Y_BAR - MIN_Y_BAR) * color.g + MIN_Y_BAR,
                                                         BAR_Z);

            // BLUE

            blueHandle.transform.position = new Vector3(blueHandle.transform.position.x,
                                                       MIN_Y + color.b * (MAX_Y - MIN_Y),
                                                       blueHandle.transform.position.z);

            blueHandle.transform.rotation = Quaternion.AngleAxis((MAX_ROT - MIN_ROT) * color.b + MIN_ROT, new Vector3(1, 0, 0));


            blueDisplayText.GetComponent<TextMesh>().text = Math.Round(color.b * 255.0f, 0).ToString();

            blueBar.transform.localScale = new Vector3(blueBar.transform.localScale.x,
                                                      (MAX_Y_BAR - MIN_Y_BAR) / 2.0f * color.b,
                                                      blueBar.transform.localScale.z);

            blueBar.transform.localPosition = new Vector3(blueBar.transform.localPosition.x,
                                                         (MAX_Y_BAR - MIN_Y_BAR) * color.b + MIN_Y_BAR,
                                                         BAR_Z);


            colorPreview.renderer.material.color = color;
        }

    }
}
