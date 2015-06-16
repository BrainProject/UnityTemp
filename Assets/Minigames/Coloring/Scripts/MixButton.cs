/**
 * @file MixButton.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;

namespace Coloring
{
    public class MixButton : MonoBehaviour
    {
        public ParticleSystem particles;

        public GameObject redDisplayNumber;
        public GameObject greenDisplayNumber;
        public GameObject blueDisplayNumber;


        void OnMouseDown()
        {
            Debug.Log("Mouse down.");
            int red = int.Parse(redDisplayNumber.GetComponent<TextMesh>().text);
            int green = int.Parse(greenDisplayNumber.GetComponent<TextMesh>().text);
            int blue = int.Parse(blueDisplayNumber.GetComponent<TextMesh>().text);

            Color particlesColor = new Color(red / 255.0f, green / 255.0f, blue / 255.0f);
            Debug.Log("Particles color is " + particlesColor);

            particles.startColor = particlesColor;

            particles.Play();
        }

        void OnParticleCollision(GameObject other)
        {
            Debug.Log(other.ToString());
        }
    }
}