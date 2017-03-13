using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


namespace Butterflies
{
    public class ButterflyBehivour : MonoBehaviour
    {
        // how fast butterfly disappear
        public float disappearConst;

        // speed of butterflies flight
        public float speedConst;

        // range of randomness of butterfly direction
        public float directionRange;

        // current butterfly position
        //private float xPos;
        //private float yPos;

        // values determining the direction of butterfly flight
        private float directionX;
        private float directionY;

        // values for color
        private float red = 1;
        private float green = 1;
        private float blue = 1;
        private float alpha;

        private bool disappearing = false;

        private float angle;

        private Vector2 translateVector;

        private bool rotateRight;



        void Start()
        {
            directionX = ButterflySpawner.directionForButterfly.x; //Random.Range(min, max);
            directionY = ButterflySpawner.directionForButterfly.y; //Random.Range(min, max);
            // randomizing the direction
            directionX = Random.Range(directionX - directionRange, directionX + directionRange);
            directionY = Random.Range(directionY - directionRange, directionY + directionRange);

            angle = ButterflySpawner.angle;
            rotateRight = ButterflySpawner.rotateRight;


            // speed of butterfly depending on mouse speed
            //speedConst = speedConst * GameManager.deltaPosition;
            //disappearConst = disappearConst * GameManager.deltaPosition;

            alpha = 0.1f;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(red, green, blue, alpha + Random.Range(0, 0.4f));

            translateVector = new Vector2(directionX * speedConst * Time.deltaTime, directionY * speedConst * Time.deltaTime);
        }

        void Update()
        {

            // getting position of butterfly
            //xPos = gameObject.GetComponent<Transform>().position.x;
            //yPos = gameObject.GetComponent<Transform>().position.y;

            translateVector = this.rotateVector(translateVector, angle);
            //Debug.Log(translateVector.x + ", " + translateVector.y);

            transform.Translate(translateVector);


            alpha = gameObject.GetComponent<SpriteRenderer>().color.a;
            if (alpha >= 1)
            {
                disappearing = true;
            }
            if (alpha > 0)
            {
                if (disappearing)
                {
                    alpha -= disappearConst * Time.deltaTime;
                }
                else
                {
                    alpha += disappearConst * Time.deltaTime;
                }
                gameObject.GetComponent<SpriteRenderer>().color = new Color(red, green, blue, alpha);

            }
            else
            {
                Destroy(gameObject);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private Vector2 rotateVector(Vector2 vector, float angle)
        {
            float x;
            float y;
            if (rotateRight)
            {
                x = (vector.x * Mathf.Cos(angle / 120)) + (vector.y * Mathf.Sin(angle / 120));
                y = -(vector.x * Mathf.Sin(angle / 120)) + (vector.y * Mathf.Cos(angle / 120));
            }
            else
            {
                x = (vector.x * Mathf.Cos(angle / 120)) - (vector.y * Mathf.Sin(angle / 120));
                y = (vector.x * Mathf.Sin(angle / 120)) + (vector.y * Mathf.Cos(angle / 120));
            }

            //Debug.Log("angle: " + angle + ", " + x + ", " + y);
            return new Vector2(x, y);
        }


    }
}