using UnityEngine;
using System.Collections;


namespace Reddy
{
    public class RotatingRoot : MonoBehaviour
    {

        public int direction = 1;

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(direction * Vector3.up * Time.deltaTime * 50);
        }
    }

}
