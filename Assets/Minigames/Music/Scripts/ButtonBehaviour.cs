using UnityEngine;
using System.Collections;
using UnityEngine.Audio;


namespace Music
{
    public class ButtonBehaviour : MonoBehaviour
    {
        public LevelManagerMusic levelManager;

        // notes displaying
        private float timeLeft;     // actual time
        public float DisabledTime; // time the note is disabled

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

            // music control
            if (levelManager.mainMusic.isPlaying)
            {               
                    

                    if(levelManager.mainMusic.GetComponent<AudioPlayer>().FadingOut)
                    {
                        if(levelManager.mainMusic.volume > 0.02f )
                        {
                            levelManager.mainMusic.volume -= (Time.deltaTime * 0.15f);
                        }
                        else
                        {
                            levelManager.mainMusic.Pause();
                            levelManager.mainMusic.GetComponent<AudioPlayer>().FadingOut = false;
                        }
                    }
                    else
                    {
                        if (levelManager.currentPlayTime <= 0)
                        {
                            levelManager.mainMusic.GetComponent<AudioPlayer>().FadingOut = true;
                            levelManager.currentPlayTime = levelManager.DefaultPlayTime;

                        }
                        else
                        {
                            levelManager.currentPlayTime -= Time.deltaTime;
                            
                        }
                    }
            }

        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.name == "HandCollider2D" && gameObject.GetComponent<SpriteRenderer>().enabled)
            {
                if (!levelManager.mainMusic.isPlaying)
                {
                    levelManager.mainMusic.Play();
                    levelManager.mainMusic.volume = 1f;
                    levelManager.currentPlayTime = levelManager.DefaultPlayTime;
                    
                }
                else
                {
                    levelManager.currentPlayTime = levelManager.DefaultPlayTime;
                    levelManager.mainMusic.volume = 1f;
                }         
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        /// <summary>
        /// method displays a buttun with a note on a random position
        /// </summary>
        void DisplayButton()
        {
            GetComponent<Transform>().position = levelManager.GetRandomPositionOnScreen();
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            GetComponent<SpriteRenderer>().enabled = true;
        }

       
    }

}