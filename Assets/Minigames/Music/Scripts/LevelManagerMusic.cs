using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Music
{
    public class LevelManagerMusic : MonoBehaviour
    {
        // music variables
        public float DefaultPlayTime; // music is played for a "playTime"
        public float currentPlayTime; // PlayTime left

        public AudioSource wrongBuzz;
        public AudioSource mainMusic;

        public GameObject YellowNote;
        public GameObject BlueNote;
        public GameObject GreenNote;
        public GameObject PurpleNote;

        public List<GameObject> Blanks;


        private float counting;

        public List<GameObject> listOfVisible;


        void Awake()
        {
            // assigning main audiosource
            AudioSource[] players = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            foreach (AudioSource source in players)
            {
                if (source.name == "MusicPlayer")
                {
                    mainMusic = source;
                }
                if (source.name == "WrongBuzz")
                {
                    wrongBuzz = source;
                }
            }
        }

        void Start()
        {
            listOfVisible = new List<GameObject>();

            //DisplayButton(YellowNote);

            Debug.Log("Size of the list" + listOfVisible.Count);

            GameObject yellowNote = Instantiate(YellowNote, GetRandomPositionOnScreen(), Quaternion.identity) as GameObject;
            yellowNote.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            listOfVisible.Add(yellowNote);

            GameObject blueNote = Instantiate(BlueNote, GetRandomPositionOnScreen(), Quaternion.identity) as GameObject;
            blueNote.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            listOfVisible.Add(blueNote);
            
            GameObject greenNote = Instantiate(GreenNote, GetRandomPositionOnScreen(), Quaternion.identity) as GameObject;
            greenNote.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            listOfVisible.Add(greenNote);

        }

        void Update()
        {
            // Displaying blank button
            counting += Time.deltaTime;
            if (counting >= 6)
            {
                Debug.Log("Choosing whether to display blank!");
                counting = 0;
                DisplayBlank();
            }

            // fade in buttons
            foreach(GameObject button in listOfVisible)
            {
                if (button.GetComponent<SpriteRenderer>().enabled)
                {
                    float alpha = button.GetComponent<SpriteRenderer>().color.a;
                    if (alpha < 1)
                    {
                        alpha += (0.35f * Time.deltaTime);
                        // check if the value is not over the alpha value max
                        if (alpha > 1)
                        {
                            alpha = 1;
                        }
                        button.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
                    } 
                }
            }
            
        }

        public Vector2 GetRandomPositionOnScreen()
        {
            Debug.Log("Creating random position");
            Vector2 position;
            bool isPositionOk;

            // getting new coordinates until we have some which don't collide with the already visible buttons
            do
            {
                position = new Vector2(Random.Range(-1.75f, 1.75f), Random.Range(0.75f, 2.1f));
                isPositionOk = true;

                foreach (GameObject button in listOfVisible)
                {
                    if (Vector2.Distance(position, button.transform.position) < 1f)
                    {
                        isPositionOk = false;
                    }
                }
            } while (!isPositionOk);    

            return position;
        }

        void DisplayBlank()
        {
            // probability of displaying the blank button
            int random = Random.Range(0, 7);
            if (random == 3)
            {
                // randomly choose the sprite
                int colorOfBlank = Random.Range(0, Blanks.Count);

                GameObject blankNote = Instantiate(Blanks[colorOfBlank], GetRandomPositionOnScreen(), Quaternion.identity) as GameObject;
                blankNote.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                listOfVisible.Add(blankNote);
            }
            
        }

    }

}