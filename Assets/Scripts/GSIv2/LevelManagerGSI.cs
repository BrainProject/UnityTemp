using UnityEngine;
using System.Collections;

namespace GSIv
{
    public class LevelManagerGSI : MonoBehaviour
    {
        public static LevelManagerGSI Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }
    }
}