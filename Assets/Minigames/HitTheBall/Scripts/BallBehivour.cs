using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace HitTheBall
{
    

    public class BallBehivour : MonoBehaviour
    {
        public LevelManagerBall levelManager;
        
        private float countdown = 10f;

        // Use this for initialization
        void Start()
        {
            levelManager.state = GameStates.Start;
            GetComponent<Rigidbody2D>().isKinematic = true;
            
        }

        // Update is called once per frame
        void Update()
        {
            switch (levelManager.state)
            {
                case GameStates.Start:
                    
                    if (countdown <= 0)
                    {
                        GetComponent<Rigidbody2D>().isKinematic = false;
                        levelManager.state = GameStates.Game;
                    }
                    else
                    {
                        countdown -= Time.deltaTime;
                    }
                    break;
                case GameStates.Game:

                    break;
            }

        }

        void OnCollisionEnter2D(Collision2D col)
        {

            switch (levelManager.state)
            {
                case GameStates.Start:

                    if (countdown <= 0)
                    {
                        GetComponent<Rigidbody2D>().isKinematic = false;
                        levelManager.state = GameStates.Game;
                    }
                    else
                    {
                        countdown -= Time.deltaTime;
                    }
                    break;
                case GameStates.Game:
                    // 2Dcolliders are on the hands of active player
                    if (col.gameObject.name == "2Dcollider")
                    {
                        hitBall(col);
                        levelManager.score++;
                        levelManager.scoreText.text = string.Format(levelManager.score.ToString());
                        if (col.gameObject.tag == "Player1")
                        {
                            
                            levelManager.player1turn = false;
                        }
                        else
                        {
                            levelManager.player1turn = true;
                        }

                    }
                    break;
            }



            /*if (state == BallStates.Start)
            {
                if (col.transform.root.tag == "Player1")
                {
                    levelManager.player1turn = true;
                }
                else
                {
                    levelManager.player1turn = false;
                }
                Debug.Log("Player1 is playing? " + levelManager.player1turn);
                GetComponent<Rigidbody2D>().isKinematic = false;
                state = BallStates.Game;
            }*/

            
        }

        // give movement to the hit ball
        private void hitBall(Collision2D col)
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
