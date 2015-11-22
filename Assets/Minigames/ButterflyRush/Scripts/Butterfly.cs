using UnityEngine;
using System.Collections;

namespace ButterflyRush
{
    public class Butterfly : MonoBehaviour
    {
        public Animator thisAnimator;

        private int direction;
        private float speed = 1.5f;

        void Start()
        {
            direction = Random.Range(1, 3);
            thisAnimator.SetInteger("Direction", direction);
        }

        // Update is called once per frame
        void Update()
        {
            switch (direction)
            {
                case 1: //top left
                    {
                        transform.Translate(new Vector2(-speed * Time.deltaTime, speed * Time.deltaTime));
                        break;
                    }
                case 2: //top right
                    {
                        transform.Translate(new Vector2(speed * Time.deltaTime, speed * Time.deltaTime));
                        break;
                    }
            }

            if (Vector2.Distance(transform.position, Camera.main.transform.position) > 15)
            {
                Destroy(gameObject);
            }
        }
    }
}