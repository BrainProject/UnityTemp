using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SocialGame
{
    public class FitCounter : MonoBehaviour
    {
    #if UNITY_STANDALONE
        public int max;

        public Slider progressBar;
        public float space;
        public GameObject line;
        public SpriteRenderer[] lines;
        public Vector3 startGUI;

        public int selectedMovements;

        public GameObject[] movements;
        public int[] numOfPose;

        //actual count of finished exercises
        private int count;

        // Use this for initialization
        void Start()
        {
            selectedMovements = Random.Range(0, movements.Length);
            max = numOfPose[selectedMovements] * max;
            progressBar.value = 0;
            progressBar.maxValue = max;
            GameObject clone = GameObject.Instantiate(movements[selectedMovements]) as GameObject;
            //Debug.Log(clone + " created");
            clone.GetComponent<GestCheckerFigure>().counter = this;
            drawCount();
            redraw();
        }

        /// <summary>
        /// Nexts is complete.
        /// </summary>
        public void nextComplete()
        {
            count++;
            if (count >= max)
            {
                redraw();
                GameObject[] playerItem = GameObject.FindGameObjectsWithTag("Player1");
                foreach (GameObject item in playerItem)
                {
                    GestCheckerFigure gest = item.GetComponent<GestCheckerFigure>();
                    if (gest)
                    {
                        Destroy(item);
                    }
                }

                LevelManager.win();
            }
            else
            {
                redraw();
            }
        }

        /// <summary>
        /// Draws initial state of the counter.
        /// </summary>
        void drawCount()
        {
            //old visual
            /*
            lines = new SpriteRenderer[max];
            for (int i = 0; i < max; i++)
            {
                if (line)
                {
                    Vector3 position = startGUI + Vector3.right * space * i;
                    GameObject clone = (GameObject)GameObject.Instantiate(line, position, Quaternion.identity);
                    clone.transform.parent = transform;
                    SpriteRenderer cloneRender = clone.GetComponent<SpriteRenderer>();
                    if (cloneRender)
                        lines[i] = cloneRender;
                    else
                    {
                        Debug.LogWarning("On " + gameObject.name + " atribut line must be sprite");
                        lines = null;
                        break;
                    }
                }
            }
             */
        }

        /// <summary>
        /// Redraw the counter
        /// </summary>
        void redraw()
        {
            progressBar.value = count;

            //Old visual
            /*
            for (int i = 0; i < lines.Length; i++)
            {
                if (i < max - count)
                {
                    lines[i].enabled = true;
                }
                else
                {
                    lines[i].enabled = false;
                }
            }
             */
        }
#endif
    }
}
