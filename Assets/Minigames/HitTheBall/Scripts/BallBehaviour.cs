using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace HitTheBall
{
    

    public class BallBehaviour : MonoBehaviour
    {
        public LevelManagerBall levelManager;

        public float HitPower;
        
        private float countdown = 3.4f;

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
                    
                    if (countdown <= 1)
                    {
                        GetComponent<Rigidbody2D>().isKinematic = false;
                        levelManager.countdownText.enabled = false;
                        levelManager.state = GameStates.Game;
                    }
                    else
                    {
                        countdown -= Time.deltaTime;
                        levelManager.countdownText.text = string.Format(countdown.ToString("F0"));
                    }
                    break;
                case GameStates.Game:
                    // everything is OnCollision
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
        }

        /// <summary>
        /// Gives movement to the hit ball
        /// </summary>
        /// <param name="col">collision with hand</param>
        private void hitBall(Collision2D col)
        {
            // angle between the collision point and the ball
            Vector2 direction = col.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
            // opposite (-Vector3) and normalize it
            direction = -direction.normalized;

            // combination of vector.up and the direction of the hit
            gameObject.GetComponent<Rigidbody2D>().AddForce((new Vector2(Vector2.up.x + direction.x, Vector2.up.y + direction.y) * HitPower), ForceMode2D.Force);
        }
    }
}
