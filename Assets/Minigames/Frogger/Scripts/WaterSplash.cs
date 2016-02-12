using UnityEngine;
using System.Collections;

namespace Frogger
{
    public class WaterSplash : MonoBehaviour
    {

        public void DestroyThisObject()
        {
            Destroy(gameObject);
        }
    }
}