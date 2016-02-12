using UnityEngine;
using System.Collections;

namespace Frogger
{
    public class VehicleControls : MonoBehaviour
    {
        //public int roadLine;
        //public int speed = 5;

        //private FrogLevelManager thisLevelManager;

        // Use this for initialization
        //void Start () {
        //    thisLevelManager = FrogLevelManager.Instance;
        //}

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.name == "Deadzone")
            {
                //transform.position = thisLevelManager.carSpawns[roadLine].position;
                //GetComponent<Rigidbody>().AddForce(transform.right * speed, ForceMode.Impulse);
                Destroy(gameObject);
            }
        }
    }
}