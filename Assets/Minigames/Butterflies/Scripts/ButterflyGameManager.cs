using UnityEngine;
using System.Collections;

namespace Butterflies
{
    public class ButterflyGameManager : MonoBehaviour
    {
        public Kinect.AvatarController avatar;

        void Start()
        {
            if (!MGC.Instance)
                Debug.Log("Creating MGC " + MGC.Instance);

            if(MGC.Instance.kinectManagerObject.activeSelf)
            {
                MGC.Instance.ResetKinect();
                MGC.Instance.kinectManagerInstance.avatarControllers.Add(avatar);
                if (MGC.Instance.kinectManagerInstance.avatarControllers.Count < 1)
                {
                    MGC.Instance.kinectManagerInstance.avatarControllers.Add(avatar);
                }
                Debug.LogWarning("Avatars count: " + MGC.Instance.kinectManagerInstance.avatarControllers.Count);
                MGC.Instance.isKinectRestartRequired = true;
            }
        }
    }
}