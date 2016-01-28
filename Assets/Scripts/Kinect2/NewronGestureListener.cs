using UnityEngine;
using System.Collections;
using System;
//using Windows.Kinect;

#if UNITY_STANDALONE
namespace Kinect
{
    public class NewronGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
    {
        //[Tooltip("GUI-Text to display gesture-listener messages and gesture information.")]
        //public GUIText gestureInfo;

        // singleton instance of the class
        private static NewronGestureListener instance = null;

        // internal variables to track if progress message has been displayed
        private bool progressDisplayed;
        //private float progressGestureTime;

        // whether the needed gesture has been detected or not
        private bool swipeLeft;
        private bool swipeRight;
        private bool swipeUp;
        //private bool click;
        private bool hiddenGesture;


        /// <summary>
        /// Gets the singleton CubeGestureListener instance.
        /// </summary>
        /// <value>The CubeGestureListener instance.</value>
        public static NewronGestureListener Instance
        {
            get
            {
                return instance;
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
                swipeLeft = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether swipe right is detected.
        /// </summary>
        /// <returns><c>true</c> if swipe right is detected; otherwise, <c>false</c>.</returns>
        public bool IsSwipeRight()
        {
            if (swipeRight)
            {
                swipeRight = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether swipe up is detected.
        /// </summary>
        /// <returns><c>true</c> if swipe up is detected; otherwise, <c>false</c>.</returns>
        public bool IsSwipeUp()
        {
            if (swipeUp)
            {
                swipeUp = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether click is detected.
        /// </summary>
        /// <returns><c>true</c> if click is detected; otherwise, <c>false</c>.</returns>
        /*public bool IsClick()
        {
            if (click)
            {
                click = false;
                return true;
            }

            return false;
        }*/

        /// <summary>
        /// Determines whether hidden gesture is detected.
        /// </summary>
        /// <returns><c>true</c> if hidden gesture is detected; otherwise, <c>false</c>.</returns>
        public bool IsHiddenGesture()
        {
            if (hiddenGesture)
            {
                hiddenGesture = false;
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
            KinectManager manager = KinectManager.Instance;
            if (!manager/* || (userId != manager.GetPrimaryUserID())*/)
                return;

            // detect these user specific gestures
            manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
            manager.DetectGesture(userId, KinectGestures.Gestures.SwipeRight);
            manager.DetectGesture(userId, KinectGestures.Gestures.SwipeUp);
            //manager.DetectGesture(userId, KinectGestures.Gestures.Click);
            manager.DetectGesture(userId, KinectGestures.Gestures.HiddenGesture);

            //if (gestureInfo != null)
            //{
                //gestureInfo.GetComponent<GUIText>().text = "Swipe left, right or up to change the slides.";
            //}
            //else
                //Debug.LogWarning("Swipe left, right or up to change the slides.");
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
            if (!manager/* || (userId != manager.GetPrimaryUserID())*/)
                return;

            //if (gestureInfo != null)
            //{
                //gestureInfo.GetComponent<GUIText>().text = string.Empty;
            //}
            //else
                //Debug.LogWarning("User " + userId + " lost");
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

            // this function is currently needed only to display gesture progress, skip it otherwise
            //if (gestureInfo == null)
                //return;

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
            else if ((gesture == KinectGestures.Gestures.Wheel || gesture == KinectGestures.Gestures.LeanLeft ||
                     gesture == KinectGestures.Gestures.LeanRight) && progress > 0.5f)
            {
                /*if (gestureInfo != null)
                {
                    string sGestureText = string.Format("{0} - {1:F0} degrees", gesture, screenPos.z);
                    gestureInfo.GetComponent<GUIText>().text = sGestureText;

                    progressDisplayed = true;
                    //progressGestureTime = Time.realtimeSinceStartup;
                }*/
                //else
                    //Debug.LogWarning(string.Format("{0} - {1:F0} degrees", gesture, screenPos.z));
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

            /*if (gestureInfo != null)
            {
                string sGestureText = gesture + " detected";
                gestureInfo.GetComponent<GUIText>().text = sGestureText;
            }*/
            //else
                //Debug.LogWarning(gesture + " detected");

            switch (gesture)
            {
                case KinectGestures.Gestures.SwipeLeft:
                    swipeLeft = true;
                    break;
                case KinectGestures.Gestures.SwipeRight:
                    swipeRight = true;
                    break;
                case KinectGestures.Gestures.SwipeUp:
                    swipeUp = true;
                    break;
                /*case KinectGestures.Gestures.Click:
                    click = true;
                    break;*/
                case KinectGestures.Gestures.HiddenGesture:
                    hiddenGesture = true;
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


        void Awake()
        {
            instance = this;
        }

        /*void Update()
        {
            if (progressDisplayed && ((Time.realtimeSinceStartup - progressGestureTime) > 2f))
            {
                progressDisplayed = false;
                gestureInfo.GetComponent<GUIText>().text = String.Empty;

                Debug.Log("Forced progress to end.");
            }
        }*/
    }
}
#endif