using UnityEngine;
using System.Collections;


namespace Reddy
{
    public class Rotator : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        }
    }
}


