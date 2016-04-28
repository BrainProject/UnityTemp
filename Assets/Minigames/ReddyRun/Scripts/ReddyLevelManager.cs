using UnityEngine;
using System.Collections;


namespace Reddy
{
    public class ReddyLevelManager : MonoBehaviour
    {
        public static ReddyLevelManager Instance { get; private set; }
        public const float HEIGHT_HEXAGON = 20.78461f;
        public ReddyController playerReference;
        public Camera cameraReference;

        void Awake()
        {
            Instance = this;
        }
    }

}
