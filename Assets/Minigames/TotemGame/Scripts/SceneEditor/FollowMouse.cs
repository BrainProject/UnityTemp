using UnityEngine;
using UnityEngine.UI;

/**
 * Newron minigame - TotemGame
 *
 * @author Petra Ambrozkova
 */
namespace TotemGame
{
    public class FollowMouse : MonoBehaviour
    {
        public float distance = 1.0f;
        public bool useInitalCameraDistance = false;
        public InputField actPos;
        private float actualDistance;

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
