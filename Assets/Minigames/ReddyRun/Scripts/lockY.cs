using UnityEngine;
using System.Collections;


namespace Reddy
{
    public class lockY : MonoBehaviour
    {


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void LateUpdate()
        {
            Vector3 newPosition = transform.position;
            //newPosition.z += anim.GetFloat("Runspeed") * Time.deltaTime;

            newPosition.y = 7.0f;

            transform.position = newPosition;
        }
    }
}

