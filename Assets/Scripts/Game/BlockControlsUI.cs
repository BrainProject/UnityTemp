using UnityEngine;
using System.Collections;

namespace Game
{
    public class BlockControlsUI : MonoBehaviour
    {
        public Transform block2DCollider;
        public Transform block3DCollider;

        //void Start()
        //{
        //    if(MGC.Instance)
        //    {
        //        block2DCollider.parent = MGC.Instance.gameObject.transform;
        //        block3DCollider.parent = MGC.Instance.gameObject.transform;
        //    }
        //}

        public void MoveToCamera()
        {
            if (Camera.main)
            {
                Camera mainCamera = Camera.main;
                Vector3 tmp = mainCamera.transform.position;
                block2DCollider.rotation = mainCamera.transform.rotation;
                block2DCollider.position = tmp + mainCamera.transform.forward * 0.5f;
                block3DCollider.rotation = mainCamera.transform.rotation;
                block3DCollider.position = tmp + mainCamera.transform.forward * 0.5f;
            }
        }
    }
}