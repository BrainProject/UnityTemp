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

        }

        // Update is called once per frame
        void Update()
        {
            Expiration -= Time.deltaTime;
            if (Expiration <= 0)
            {
                
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.name == "HandCollider2D")
            {
                levelManager.listOfVisible.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    } 
}
