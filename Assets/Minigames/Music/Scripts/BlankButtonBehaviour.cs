using UnityEngine;
using System.Collections;


namespace Music
{

    public class BlankButtonBehaviour : MonoBehaviour
    {
        public LevelManagerMusic levelManager;

        

        public float Expiration;


        void Awake()
        {
            // assigning levelmanager
            LevelManagerMusic[] managers = FindObjectsOfType(typeof(LevelManagerMusic)) as LevelManagerMusic[];
            levelManager = managers[0];
        }

        // Use this for initialization
        void Start()
        {
            if (Expiration == 0)
            {
                Expiration = 5;
            }
        }

        // Update is called once per frame
        void Update()
        {          
            if (Expiration <= 0)
            {
                float alpha = GetComponent<SpriteRenderer>().color.a;
                Debug.Log("Alpha is: " + alpha);
                if (GetComponent<SpriteRenderer>().color.a <= 0.1f)
                {
                    levelManager.listOfVisible.Remove(gameObject);
                    Destroy(gameObject);
                }
                alpha -= (Time.deltaTime * 0.7f);
                if (alpha < 0)
                {
                    alpha = 0;
                }
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);
            }
            else
            {
                Expiration -= Time.deltaTime;
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.name == "HandCollider2D")
            {
                levelManager.mainMusic.Stop();
                levelManager.wrongBuzz.Play();
                levelManager.listOfVisible.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    } 
}
