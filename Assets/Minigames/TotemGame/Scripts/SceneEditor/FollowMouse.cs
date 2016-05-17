using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TotemGame
{
    public class FollowMouse : MonoBehaviour
    {
        public float distance = 1.0f;
        public bool useInitalCameraDistance = false;
        private float actualDistance;
        public InputField actPos;

        void Start()
        {
            if (useInitalCameraDistance)
            {
                Vector3 toObjectVector = transform.position - Camera.main.transform.position;
                Vector3 linearDistanceVector = Vector3.Project(toObjectVector, Camera.main.transform.forward);
                actualDistance = linearDistanceVector.magnitude; 
            }
            else
            {
                actualDistance = distance;
            }
        }

        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = actualDistance;
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            actPos.text = transform.position.ToString("F4");
        }
    }
}
