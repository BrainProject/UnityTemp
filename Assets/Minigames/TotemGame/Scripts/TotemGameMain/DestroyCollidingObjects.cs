using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class DestroyCollidingObjects : MonoBehaviour
    {
        public float radius = 50.0F;
        public float power = 30.0F;
        public List<GameObject> colObj = new List<GameObject>();
        public GameObject bomb;
        private GameObject explosion;
        private Color startcolor;
        private Vector3 defaultPos;
        private float actualDistance = 6.0f;

        void Start()
        {
            if (bomb == null)
                bomb = (GameObject)Instantiate(Resources.Load("Bomb"));
            explosion = (GameObject) Resources.Load("RedExplosion");
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

        private void OnCollisionEnter(Collision col)
        {
            if ((col.gameObject.Equals(TotemLevelManager.Instance.goalCube)) || (col.gameObject.Equals(TotemLevelManager.Instance.player)))
            {
            }
            else
            { 
                colObj.Add(col.gameObject);
            }
        }

        private void OnCollisionStay(Collision col)
        {
            colObj.RemoveAll((o) => o == null);
        }

        private void OnCollisionExit(Collision col)
        {
                colObj.Remove(col.gameObject);
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

            for (int i = 0; i < colObj.Count; i++)
            {
                Destroy(colObj[i]); 
            }

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
