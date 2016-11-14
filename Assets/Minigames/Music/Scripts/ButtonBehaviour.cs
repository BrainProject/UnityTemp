using UnityEngine;
using System.Collections;


namespace Music
{
    public class ButtonBehaviour : MonoBehaviour
    {

        public LevelManagerMusic levelManager;

        void Start()
        {
            // assigning levelmanager
            LevelManagerMusic[] managers = FindObjectsOfType(typeof(LevelManagerMusic)) as LevelManagerMusic[];
            levelManager = managers[0];
        }

        void Update()
        {
            
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.name == "HandCollider2D" && gameObject.GetComponent<SpriteRenderer>().enabled)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

}