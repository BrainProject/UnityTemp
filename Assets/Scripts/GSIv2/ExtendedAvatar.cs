
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Kinect;

namespace GSIv2
{
#if UNITY_STANDALONE
    public class ExtendedAvatar : AvatarController
    {
        public bool useRealPosition = true;

        private bool canControl = false;
        private Vector3 newPos;
        private int currentUserId;

        void Start()
        {
            if (!MGC.Instance)
                Debug.Log("Creating MGC " + MGC.Instance);

            canControl = true;
        }

        /// <summary>
        /// Update the avatar each frame.
        /// </summary>
        public override void UpdateAvatar(Int64 UserID)
        {
            if (enabled)
            {
                if (AvatarSwitcher.Instance.players.Count == 1)
                {
                    base.UpdateAvatar(KinectManager.Instance.GetPrimaryUserID());   // detect the closest user, if only one is available
                }
                else
                {
                    base.UpdateAvatar(UserID);  // detect players with assigned ID
                }

                if (useRealPosition && kinectManager && (UserID != 0) && canControl)
                {
                    newPos = transform.position;
                    newPos.x = kinectManager.GetUserPosition(playerId).x;
                    transform.position = newPos;
                }
            }
        }
    }
#endif
}