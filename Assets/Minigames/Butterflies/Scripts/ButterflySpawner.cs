using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace Butterflies
{
    public class ButterflySpawner : MonoBehaviour
    {
        public GameObject[] butterflies;
        public Camera mainCamera;
        public float printButterflyValue;
        //public int createButterflyDelay;
        public float appearRange; //radius of range where butterflies randomly appears
        public int rangeOfCurve; //length of buffer for last mouse positions
        public int numberOfButterflies; //number of butterfiles printed in one update

        private float durationBetweenSpawn = 0.02f;

        // values for directions and mouse posiotions
        // private Vector3 deltaMousePosition;
        public static Vector3 directionForButterfly;
        private Vector3 mousePosition;
        //public static float deltaPosition; // determinig the speed of butterfly
        //public static float lastDeltaPosition;

        public static float angle;
        public static bool rotateRight;

        private List<Vector3> mousePositionBuffer = new List<Vector3>();

        private float timestamp;

        //private float timestampPrint;



        void Start()
        {

            mousePosition = transform.position;//mainCamera.WorldToScreenPoint(transform.position); //transform.position;
            //deltaMousePosition = Vector3.zero;
            timestamp = Time.time;
            //timestampPrint = Time.time;
        }

        void Update()
        {
            if (Time.time - timestamp > durationBetweenSpawn)
            {
                //Vector3 tmp = mainCamera.WorldToScreenPoint(transform.position);

              //  if (Time.time - timestampPrint > 0.1f)
            //{
                mousePosition = transform.position;
                mousePositionBuffer.Add(mousePosition);

                if (mousePositionBuffer.Count >= rangeOfCurve)
                {
                    mousePositionBuffer.RemoveAt(0);
                }

                float deltaFirstLast = this.countPointDistance(mousePositionBuffer[0], mousePositionBuffer[mousePositionBuffer.Count - 1]);
                //Mathf.Sqrt(Mathf.Pow(mousePositionBuffer[0].x - mousePositionBuffer[mousePositionBuffer.Count-1].x, 2) + Mathf.Pow(mousePositionBuffer[0].y - mousePositionBuffer[mousePositionBuffer.Count-1].y, 2));
                directionForButterfly = mousePositionBuffer[mousePositionBuffer.Count - 1] - mousePositionBuffer[0];

                angle = this.countAngle(mousePositionBuffer[0], mousePositionBuffer[mousePositionBuffer.Count - 1], mousePositionBuffer[(mousePositionBuffer.Count - 1) / 2]);
                if (float.IsNaN(angle))
                {
                    angle = 0;
                }
                //timestampPrint = Time.time;
            //}

            /*
            Vector3 lastMousePositionDelta = deltaMousePosition;
            // counting difference between last and current mouse position
            directionForButterfly = Input.mousePosition - mousePosition;
            */
            /*
            // makes direction absolute, saves value to deltaMouseVariable (directionForButterfly is the first delta for determinig direction of butterfly)
            if (directionForButterfly.x < 0)
            {
                deltaMousePosition.x = (-directionForButterfly.x);
            }
            else
            {
                deltaMousePosition.x = directionForButterfly.x;
            }
            if (directionForButterfly.y < 0)
            {
                deltaMousePosition.y = (-directionForButterfly.y);
            }
            else
            {
                deltaMousePosition.y = directionForButterfly.y;
            }
            */
            /*
            mousePosition = Input.mousePosition;
            mousePosition.z = 5;
            */

            Vector3 objectPosition = mousePosition; //mainCamera.ScreenToWorldPoint(tmp);
            objectPosition.z = 5;


            //GameObject instance = Instantiate(butterflies[Random.Range(0, butterflies.Length)], objectPosition, Quaternion.identity) as GameObject;
            /*
            deltaPosition = Mathf.Sqrt(Mathf.Pow(deltaMousePosition.x, 2) + Mathf.Pow(deltaMousePosition.y, 2));
            lastDeltaPosition = Mathf.Sqrt(Mathf.Pow(lastMousePositionDelta.x, 2) + Mathf.Pow(lastMousePositionDelta.y, 2));
            */
                if (deltaFirstLast > printButterflyValue)
                {
                    this.isRightRotation(mousePositionBuffer[(mousePositionBuffer.Count - 1) / 2], mousePositionBuffer[0], mousePositionBuffer[mousePositionBuffer.Count - 1]);
                    for (int i = 0; i < numberOfButterflies; i++)
                    {
                        objectPosition = new Vector3(Random.Range(objectPosition.x - appearRange, objectPosition.x + appearRange), Random.Range(objectPosition.y - appearRange, objectPosition.y + appearRange), objectPosition.z);
                        Instantiate(butterflies[Random.Range(0, butterflies.Length)], objectPosition, Quaternion.identity);
                    }
                    /*   
                    objectPosition = new Vector3(Random.Range(objectPosition.x - appearRange, objectPosition.x + appearRange), Random.Range(objectPosition.y - appearRange, objectPosition.y + appearRange), objectPosition.z);
                    GameObject instance2 = Instantiate(butterflies[Random.Range(0, butterflies.Length)], objectPosition, Quaternion.identity) as GameObject;
                    objectPosition = new Vector3(Random.Range(objectPosition.x - appearRange, objectPosition.x + appearRange), Random.Range(objectPosition.y - appearRange, objectPosition.y + appearRange), objectPosition.z);
                    GameObject instance3 = Instantiate(butterflies[Random.Range(0, butterflies.Length)], objectPosition, Quaternion.identity) as GameObject;*/
                }
                timestamp = Time.time;
            }



        }

        /// <summary>
        /// counts angle for rotating butterfly in radians
        /// </summary>
        /// <param name="a">length of a</param>
        /// <param name="b">length of b</param>
        /// <param name="c">length of c</param>
        /// <returns></returns>
        private float countAngle(Vector3 A, Vector3 B, Vector3 C)
        {
            float a = this.countPointDistance(B, C);
            float b = this.countPointDistance(A, C);
            float c = this.countPointDistance(B, A);

            return Mathf.Acos((Mathf.Pow(a, 2) - Mathf.Pow(b, 2) - Mathf.Pow(c, 2)) / -(2 * a * b));
        }

        private float countPointDistance(Vector3 A, Vector3 B)
        {
            return Mathf.Sqrt(Mathf.Pow(A.x - B.x, 2) + Mathf.Pow(A.y - B.y, 2));
        }

        private void isRightRotation(Vector2 a, Vector2 b, Vector2 c)
        {
            if (((b.x - a.x) * (c.y - a.y) - (b.y - a.y) * (c.x - a.x)) > 0)
            {
                rotateRight = true;
            }
            else
            {
                rotateRight = false;
            }
        }



    }
}