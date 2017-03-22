using UnityEngine;
using System.Collections;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class ExplosionForce : MonoBehaviour
    {
        public Rigidbody thisRigidbody;
        public float radius = 100.0F;
        public float power = 100.0F;
        public GameObject bomb;
        private GameObject explosion;
        private Color startcolor;
        private Vector3 defaultPos;
        private float actualDistance = 6.0f;

        void Start()
        {
            if (bomb == null)
                bomb = TotemLevelManager.Instance.bomb;
            explosion = (GameObject)Resources.Load("RedExplosion");
            defaultPos = transform.position;

            if (!thisRigidbody)
            {
                Rigidbody tmp = GetComponent<Rigidbody>();
                thisRigidbody = tmp;
                if (!thisRigidbody)
                {
                    Debug.LogWarning(gameObject.name + " doesn't have any rigidbody!");
                    enabled = false;
                }
            }

            if (thisRigidbody)
            {
                thisRigidbody.isKinematic = true;
            }
        }

        void OnEnable()
        {
            TotemLevelManager.OnClicked += ActivatePhysics;
        }

        void OnDisable()
        {
            TotemLevelManager.OnClicked -= ActivatePhysics;
        }

        void ActivatePhysics()
        {
            if (thisRigidbody)
            {
                thisRigidbody.isKinematic = false;
            }
        }

        void Update()
        {
            if (bomb.activeInHierarchy)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = actualDistance;
                bomb.transform.position = new Vector3(mousePosition.x +65, mousePosition.y - 40, mousePosition.z);
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

