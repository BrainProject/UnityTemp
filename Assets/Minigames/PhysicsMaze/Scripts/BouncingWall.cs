using UnityEngine;
using System.Collections;

namespace MinigameMaze2
{
    public class BouncingWall : MonoBehaviour
    {
        [SerializeField]
        private float bounciness = 40.0f;
        
        void OnCollisionEnter(Collision collisionInfo)
        {
            Collider other = collisionInfo.contacts[0].otherCollider;
            if (other.tag == "Player")
            {
                Rigidbody player = collisionInfo.contacts[0].otherCollider.attachedRigidbody;
                player.velocity = Vector3.Reflect(player.velocity, collisionInfo.contacts[0].normal);
                player.AddForce(collisionInfo.contacts[0].normal * bounciness, ForceMode.VelocityChange);
            }
        }
    }
}
