using UnityEngine;
using System.Collections;
using System;
using Windows.Kinect;
using Kinect;

#if UNITY_STANDALONE
namespace Reddy
{
    public class ReddyKinect : MonoBehaviour, KinectGestures.GestureListenerInterface
    {
        //[Tooltip("GUI-Text to display gesture-listener messages and gesture information.")]
        //public GUIText gestureInfo;

        // internal variables to track if progress message has been displayed
        private bool progressDisplayed;
        //private float progressGestureTime;

        // whether the needed gesture has been detected or not
        private bool swipeLeft;
        private bool jump;
        private bool hiddenGesture;
        private bool squat;
        private bool leanLeft;
        private bool leanRight;


        // The singleton CubeGestureListener instance.
        public static ReddyKinect Instance { get; private set; }


        void Awake()
        {
            Instance = this;
            print("AWAKE listener + " + MGC.Instance);
        }

        void Start()
        {
            if (MGC.Instance)
            {
                if (MGC.Instance.kinectManagerInstance)
                {
                    //print("MGC.Instance.KMI = " + MGC.Instance.kinectManagerInstance);
                    MGC.Instance.kinectManagerInstance.gestureListeners.Add(this);
                    //MGC.Instance.kinectManagerInstance.ClearKinectUsers();
                    //MGC.Instance.ResetKinect();
                }
                else
                {
                    print("No MGC.Instance.KMI");
                }
            }
        }

        /// <summary>
        /// Determines whether swipe left is detected.
        /// </summary>
        /// <returns><c>true</c> if swipe left is detected; otherwise, <c>false</c>.</returns>
        public bool IsSwipeLeft()
        {
            if (swipeLeft)
            {

                //print("SWIPE gesture detected");
                swipeLeft = false;
                return true;
            }

            return false;
        }

        public bool IsJump()
        {
            if (jump)
            {
                print("JUMP gesture detected");
                jump = false;
                return true;
            }

            return false;
        }

        public bool IsHiddenGesture()
        {
            if (hiddenGesture)
            {
                print("HIDDEN gesture detected");
                hiddenGesture = false;
                return true;
            }

            return false;
        }

        public bool IsSquat()
        {
            if (squat)
            {
                print("SQUAT gesture detected");
                squat = false;
                return true;
            }

            return false;
        }

        public bool IsLeanLeft()
        {
            if (squat)
            {
                print("LeanL gesture detected");
                squat = false;
                return true;
            }

            return false;
        }

        public bool IsLeanRight()
        {
            if (squat)
            {
                print("LeanR gesture detected");
                squat = false;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        public void UserDetected(long userId, int userIndex)
        {
            
            // the gestures are allowed for the primary user only
            KinectManager manager = null;
            if (MGC.Instance.kinectManagerInstance)
            {
                manager = MGC.Instance.kinectManagerInstance;
                print("KinectManager instance");
            }
            else
            {
                manager = KinectManager.Instance;
            }

            if (!manager/* || (userId != manager.GetPrimaryUserID())*/)
                return;

            // detect these user specific gestures
            manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
            manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
            manager.DetectGesture(userId, KinectGestures.Gestures.HiddenGesture);
            manager.DetectGesture(userId, KinectGestures.Gestures.Squat);
            manager.DetectGesture(userId, KinectGestures.Gestures.LeanLeft);
            manager.DetectGesture(userId, KinectGestures.Gestures.LeanRight);

        }

        /// <summary>
        /// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        public void UserLost(long userId, int userIndex)
        {
            // the gestures are allowed for the primary user only
            KinectManager manager = KinectManager.Instance;
            if (!manager)
                return;
        }

        /// <summary>
        /// Invoked when a gesture is in progress.
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        /// <param name="gesture">Gesture type</param>
        /// <param name="progress">Gesture progress [0..1]</param>
        /// <param name="joint">Joint type</param>
        /// <param name="screenPos">Normalized viewport position</param>
        public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                                      float progress, KinectInterop.JointType joint, Vector3 screenPos)
        {
            // the gestures are allowed for the primary user only
            KinectManager manager = KinectManager.Instance;
            if (!manager || (userId != manager.GetPrimaryUserID()))
                return;
         

            if ((gesture == KinectGestures.Gestures.ZoomOut || gesture == KinectGestures.Gestures.ZoomIn) && progress > 0.5f)
            {
                /*if (gestureInfo != null)
                {
                    string sGestureText = string.Format("{0} - {1:F0}%", gesture, screenPos.z * 100f);
                    gestureInfo.GetComponent<GUIText>().text = sGestureText;

                    progressDisplayed = true;
                    //progressGestureTime = Time.realtimeSinceStartup;
                }*/
                //else
                //Debug.LogWarning(string.Format("{0} - {1:F0}%", gesture, screenPos.z * 100f));
            }
        }

        /// <summary>
        /// Invoked if a gesture is completed.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        /// <param name="gesture">Gesture type</param>
        /// <param name="joint">Joint type</param>
        /// <param name="screenPos">Normalized viewport position</param>
        public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                      KinectInterop.JointType joint, Vector3 screenPos)
        {
            // the gestures are allowed for the primary user only
            KinectManager manager = KinectManager.Instance;
            if (!manager || (userId != manager.GetPrimaryUserID()))
                return false;
            

            switch (gesture)
            {
                case KinectGestures.Gestures.SwipeLeft:
                    swipeLeft = true;
                    //print("SWIPE gesture completed");
                    break;

                case KinectGestures.Gestures.Jump:
                    jump = true;
                    print("JUMP gesture completed");
                    break;

                case KinectGestures.Gestures.HiddenGesture:
                    hiddenGesture = true;
                    print("HIDDEN gesture completed");
                    break;

                case KinectGestures.Gestures.Squat:
                    squat = true;
                    print("SQUAT gesture completed");
                    break;

                case KinectGestures.Gestures.LeanLeft:
                    squat = true;
                    print("LeanL gesture completed");
                    break;

                case KinectGestures.Gestures.LeanRight:
                    squat = true;
                    print("LeanR gesture completed");
                    break;
            }

            return true;
        }

        /// <summary>
        /// Invoked if a gesture is cancelled.
        /// </summary>
        /// <returns>true</returns>
        /// <c>false</c>
        /// <param name="userId">User ID</param>
        /// <param name="userIndex">User index</param>
        /// <param name="gesture">Gesture type</param>
        /// <param name="joint">Joint type</param>
        public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                      KinectInterop.JointType joint)
        {
            // the gestures are allowed for the primary user only
            KinectManager manager = KinectManager.Instance;
            if (!manager || (userId != manager.GetPrimaryUserID()))
                return false;

            if (progressDisplayed)
            {
                progressDisplayed = false;

                /*if (gestureInfo != null)
                {
                    gestureInfo.GetComponent<GUIText>().text = String.Empty;
                }*/
            }

            return true;
        }


        void OnDisable()
        {
            if (MGC.Instance)
            {
                if (MGC.Instance.kinectManagerInstance)
                {
                    MGC.Instance.kinectManagerInstance.gestureListeners.Remove(this);
                    print("Removing listener");
                }
            }
        }
    }
}
#endif
