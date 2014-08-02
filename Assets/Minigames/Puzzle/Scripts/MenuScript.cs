using UnityEngine;
using System.Collections;
using System;

namespace Puzzle
{
    public class MenuScript : MonoBehaviour
    {
        private double angle = 0;
        // Use this for initialization
        void Start()
        {
            UnityEngine.Object[] images = Resources.LoadAll("Pictures");

            angle = 2.0 * Math.PI / images.LongLength;

            Debug.Log("Number of images: " + images.LongLength);

            float x_size = 0;
            double radius = 0;
            bool variables_set = false;

            for (long i = 0; i < images.LongLength; i++)
            {
                Debug.Log(images[i].ToString());

                GameObject image_plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                image_plane.AddComponent("ChoosePictureScript");

                if (!variables_set)
                {
                    x_size = image_plane.renderer.bounds.size.x;
                    radius = x_size / Math.Tan(angle / 2.0);
                    variables_set = true;
                }

                image_plane.transform.position = new Vector3(
                    (float)(radius * Math.Sin(angle * i)),
                    (float)(0),
                    (float)(radius * Math.Cos(angle * i)));

                image_plane.renderer.material.mainTexture = images[i] as Texture;

                image_plane.transform.rotation = Quaternion.Euler(
                                                        90.0f,
                                                        (float)(Mathf.Rad2Deg * angle * i),
                                                        0.0f);

                //if (i == 3) break;
            }

            Camera.main.transform.position = new Vector3(0, 0, (float)radius + 10);
            Camera.main.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // lower, the faster
        const float velocity = 200;

        // Update is called once per frame
        void Update()
        {
            if (Input.mousePosition.x < 50)
            {
                Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, (float)angle * Mathf.Rad2Deg / velocity);
            }
            else if (Input.mousePosition.x > Screen.width - 50)
            {
                Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, -(float)angle * Mathf.Rad2Deg / velocity);
            }
        }
    }
}
