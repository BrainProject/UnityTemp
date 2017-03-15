using UnityEngine;
using System.Collections;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class DestroyObject : MonoBehaviour
    {
        public Rigidbody thisRigidbody;
        private Color startcolor;
        private GameObject particlesObj;
        private ParticleSystem particlesSystem;

        private void Start()
        {
            if(!thisRigidbody)
            {
                Rigidbody tmp = GetComponent<Rigidbody>();
                thisRigidbody = tmp;
                if(!thisRigidbody)
                {
                    Debug.LogWarning(gameObject.name + " doesn't have any rigidbody!");
                    enabled = false;
                }
            }

            if(thisRigidbody)
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

        private void OnMouseEnter()
        {
            startcolor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material.color = Color.green;
        }

        private void OnMouseExit()
        {
            GetComponent<Renderer>().material.color = startcolor;
        }

        private void OnMouseDown()
        {
            if (enabled)
            {
                Destroy(gameObject);
                DestroyEffect(gameObject.transform.position);
                Explosion(gameObject.transform.position);
            }
        }

        private void Explosion(Vector3 position)
        {
            particlesObj = (GameObject)Resources.Load("ParticleSystemEffect");
            particlesSystem = particlesObj.GetComponent<ParticleSystem>();
            instantiate(particlesSystem, position);
        }

        private void DestroyEffect(Vector3 position)
        {
            Instantiate(Resources.Load("destroyEffect", typeof(GameObject)), transform.position, transform.rotation);
        }

        private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
        {
            ParticleSystem newParticleSystem = Instantiate(
              prefab,
              position,
              Quaternion.identity
            ) as ParticleSystem;

            // Make sure it will be destroyed
            Destroy(
              newParticleSystem.gameObject,
              newParticleSystem.startLifetime
            );

            return newParticleSystem;
        }
    }
}
