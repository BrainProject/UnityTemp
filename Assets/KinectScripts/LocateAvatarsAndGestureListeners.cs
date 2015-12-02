using UnityEngine;
using System.Collections;

namespace Kinect
{
    public class LocateAvatarsAndGestureListeners : MonoBehaviour
    {

        void Start()
        {
            Debug.Log(MGC.Instance);
            KinectManager manager = KinectManager.Instance;

            if (manager)
            {
                // remove all users, filters and avatar controllers
                manager.avatarControllers.Clear();
                manager.ClearKinectUsers();

                // get the mono scripts. avatar controllers and gesture listeners are among them
                MonoBehaviour[] monoScripts = FindObjectsOfType(typeof(MonoBehaviour)) as MonoBehaviour[];

                // add available avatar controllers
                foreach (MonoBehaviour monoScript in monoScripts)
                {
                    if (typeof(AvatarController).IsAssignableFrom(monoScript.GetType()) &&
                       monoScript.enabled)
                    {
                        AvatarController avatar = (AvatarController)monoScript;
                        manager.avatarControllers.Add(avatar);
                    }
                }

                // add available gesture listeners
                manager.gestureListeners.Clear();

                foreach (MonoBehaviour monoScript in monoScripts)
                {
                    if (typeof(KinectGestures.GestureListenerInterface).IsAssignableFrom(monoScript.GetType()) &&
                       monoScript.enabled)
                    {
                        //KinectGestures.GestureListenerInterface gl = (KinectGestures.GestureListenerInterface)monoScript;
                        manager.gestureListeners.Add(monoScript);
                    }
                }

            }
        }

        //void Update()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Start();
        //    }
        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        KinectManager m = KinectManager.Instance;
        //        m.avatarControllers.Clear();
        //        m.ClearKinectUsers();
        //    }
        //}
    }
}