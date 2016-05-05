using UnityEngine;
using System.Collections;

namespace TotemGame
{
    public class ExplosionForce : MonoBehaviour
    {
        public float radius = 5.0F;
        public float power = 10.0F;
        private GameObject explosion;
        private Color startcolor;
        public GameObject bomb;
        private Vector3 defaultPos;
        private float actualDistance = 6.0f;

        void Start()
        {
            //bomb = (GameObject)Instantiate(Resources.Load("Bomb"));
            explosion = (GameObject)Resources.Load("BlueExplosion");
            defaultPos = transform.position;
            
        }
        void Update()
        {
            if (bomb.activeInHierarchy)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = actualDistance;
                bomb.transform.position = new Vector3(mousePosition.x + 15, mousePosition.y - 20, mousePosition.z);
            }
        }
        private void OnMouseEnter()
        {
            bomb.SetActive(true);
            startcolor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.color = Color.red;
        }

        private void OnMouseExit()
        {
            GetComponent<Renderer>().material.color = startcolor;
            bomb.SetActive(false);
            bomb.transform.position = defaultPos;
        }

        void OnMouseDown()
        {
            bomb.SetActive(false);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);

            Vector3 explosionPos = transform.position;
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
            }
        }
    }
}

