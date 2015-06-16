using UnityEngine;
using System.Collections;

public class NewColorBlobInteraction : MonoBehaviour 
{
    public GameObject system;

    void OnParticleCollision(GameObject other )
    {
        gameObject.renderer.material.color = system.particleSystem.startColor;
    }
}
