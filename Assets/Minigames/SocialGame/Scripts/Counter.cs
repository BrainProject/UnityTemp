using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SocialGame
{
    public class Counter : MonoBehaviour
    {
#if UNITY_STANDALONE
        public int max;
        public CounterPointControlDodge[] pointsGraf;
        public Slider progressBar;
        public int points = 0;
        public Vector2 fieldOnSreen;
        public GameObject PointObj;
        public float step;

        // Use this for initialization
        void Start()
        {
            //CreatePoints();

            progressBar.maxValue = max;
            progressBar.value = 0;

            setPoints(0);
            redraw();
        }

        /// <summary>
        /// Adds the points.
        /// </summary>
        /// <param name="point">Point.</param>
        public void addPoints(int point)
        {
            if ((point + points) < 0)
                points = 0;
            else
                points += point;
            redraw();
        }

        /// <summary>
        /// Sets the points.
        /// </summary>
        /// <param name="point">Point.</param>
        public void setPoints(int point)
        {
            if (point < 0)
                points = 0;
            else
                points = point;
            redraw();
        }

        /// <summary>
        /// Creates the points.
        /// </summary>
        void CreatePoints()
        {
            pointsGraf = new CounterPointControlDodge[max];
            int leftHalf = max / 2;
            float center = fieldOnSreen.x + (fieldOnSreen.y - fieldOnSreen.x) / 2;
            for (int i = 0; i < max; i++)
            {
                Vector3 position = transform.position;
                position.x = center + (i - leftHalf) * step;
                GameObject obj = (GameObject)Instantiate(PointObj, position, Quaternion.identity);
                obj.name = "Point_" + i;
                obj.transform.SetParent(transform);
                CounterPointControlDodge scr = obj.GetComponent<CounterPointControlDodge>();
                if (scr)
                {
                    pointsGraf[i] = scr;
                }
            }


        }

        [ContextMenu("Redraw")]
        /// <summary>
        /// Redraw this instance.
        /// </summary>
        void redraw()
        {
            progressBar.value = points;

            //old visual
            /*
            for (int i = 0; i < max; i++)
            {
                pointsGraf[i].AddThis(i < points);
            }
            */

            if (points >= max)
            {

                LevelManager.win();
                checkShooter check = (checkShooter)FindObjectOfType(typeof(checkShooter));
                if (check)
                {
                    check.Stop(true);
                }
            }
        }
#endif
    }
}
