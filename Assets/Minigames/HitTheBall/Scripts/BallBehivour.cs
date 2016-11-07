using UnityEngine;
using System.Collections;

namespace HitTheBall
{
    enum BallStates { Start, Game}

    public class BallBehivour : MonoBehaviour
    {

        private BallStates state;
        private float countdown = 10f;

        // Use this for initialization
        void Start()
        {
            state = BallStates.Start;
            GetComponent<Rigidbody2D>().isKinematic = true;
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case BallStates.Start:
                    if (countdown <= 0)
                    {
                        GetComponent<Rigidbody2D>().isKinematic = false;
                        state = BallStates.Game;
                    }
                    else
                    {
                        countdown -= Time.deltaTime;
                    }
                    break;
                case BallStates.Game:

                    break;
            }

        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.name == "2Dcollider")
            {

                // angle between the collision point and the ball
                Vector2 direction = col.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
                // opposite (-Vector3) and normalize it
                direction = -direction.normalized;

                // combination of vector.up and the direction of the hit
                gameObject.GetComponent<Rigidbody2D>().AddForce((new Vector2(Vector2.up.x * direction.x, Vector2.up.y * direction.y) * 800), ForceMode2D.Force);
            }
        }
    }
}
