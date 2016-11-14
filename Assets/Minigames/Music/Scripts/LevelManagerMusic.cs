using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Music
{
    public class LevelManagerMusic : MonoBehaviour
    {

        public GameObject YellowNote;
        public GameObject BlueNote;
        public GameObject GreenNote;
        public GameObject PurpleNote;

        public List<GameObject> Blanks;


        private float counting;
        public float speedOfDisplayingButtons;

        public List<GameObject> listOfVisible;


        void Start()
        {
            listOfVisible = new List<GameObject>();

            //DisplayButton(YellowNote);

            Debug.Log("Size of the list" + listOfVisible.Count);

            GameObject yellowNote = Instantiate(YellowNote, GetRandomPositionOnScreen(), Quaternion.identity) as GameObject; 
            listOfVisible.Add(yellowNote);

            GameObject blueNote = Instantiate(BlueNote, GetRandomPositionOnScreen(), Quaternion.identity) as GameObject;
            listOfVisible.Add(blueNote);
            
            GameObject greenNote = Instantiate(GreenNote, GetRandomPositionOnScreen(), Quaternion.identity) as GameObject;
            listOfVisible.Add(greenNote);

        }

        void Update()
        {
            counting += Time.deltaTime;
            if (counting >= 5)
            {
                Debug.Log("Choosing whether to display blank!");
                counting = 0;
                DisplayBlank();
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
            int random = Random.Range(0, 8);
            if (random == 3)
            {
                // randomly choose the sprite
                int colorOfBlank = Random.Range(0, Blanks.Count);

                GameObject blankNote = Instantiate(Blanks[colorOfBlank], GetRandomPositionOnScreen(), Quaternion.identity) as GameObject;
                listOfVisible.Add(blankNote);
            }
            
        }

    }

}