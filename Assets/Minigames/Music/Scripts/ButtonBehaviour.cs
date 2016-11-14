using UnityEngine;
using System.Collections;


namespace Music
{
    public class ButtonBehaviour : MonoBehaviour
    {

        public LevelManagerMusic levelManager;

        private float timeLeft;     // actual time
        public float DisabledTime; // time the button is disabled

        void Awake()
        {
            // assigning levelmanager
            LevelManagerMusic[] managers = FindObjectsOfType(typeof(LevelManagerMusic)) as LevelManagerMusic[];
            levelManager = managers[0];
        }

        void Start()
        {
            timeLeft = 0;
        }

        void Update()
        {
            if (gameObject.GetComponent<SpriteRenderer>().enabled)
            {
                // the button is visible
            }
            else
            {
                //the button is not visible
                timeLeft += Time.deltaTime;
                if (timeLeft >= DisabledTime)
                {
                    DisplayButton();
                    timeLeft = 0;
                }
            }

        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.name == "HandCollider2D" && gameObject.GetComponent<SpriteRenderer>().enabled)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        /// <summary>
        /// method displays a buttun with a note on a random position
        /// </summary>
        void DisplayButton()
        {
            GetComponent<Transform>().position = levelManager.GetRandomPositionOnScreen();
            GetComponent<SpriteRenderer>().enabled = true;
        }

       
    }

}