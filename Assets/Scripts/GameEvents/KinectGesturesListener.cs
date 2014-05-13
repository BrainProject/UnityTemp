using UnityEngine;
using System.Collections;

public class KinectGesturesListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    // GUI Text to display the gesture messages.
//    public GUIText GestureInfo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UserDetected(uint userId, int userIndex)
    {
        //detect these user specific gestures
        KinectManager manager = KinectManager.Instance;
        manager.DetectGesture(userId, KinectGestures.Gestures.Jump);
        manager.DetectGesture(userId, KinectGestures.Gestures.SwipeLeft);
        manager.DeleteGesture(userId, KinectGestures.Gestures.SwipeRight);
        manager.DetectGesture(userId, KinectGestures.Gestures.Push);
        manager.DetectGesture(userId, KinectGestures.Gestures.Pull);
        manager.DeleteGesture(userId, KinectGestures.Gestures.Click);
        manager.DeleteGesture(userId, KinectGestures.Gestures.RightHandCursor);
        manager.DeleteGesture(userId, KinectGestures.Gestures.LeftHandCursor);
    }

    public void UserLost(uint userId, int userIndex)
    {
        
    }

    public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectWrapper.SkeletonJoint joint,
        Vector3 screenPos)
    {
        
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.SkeletonJoint joint, Vector3 screenPos)
    {
//        string sGestureText = gesture + " detected";
        if (gesture == KinectGestures.Gestures.Click)
        {
            Debug.Log("Click Gesture");
            InteractionEventAggregator.OnMouse.Publish(new MouseGameEventArgs(MouseEmulator.MouseEventFlags.LeftUp, new MouseEmulator.MousePoint((uint)screenPos.x,(uint)screenPos.y)));
        }
            //sGestureText += string.Format(" at ({0:F1}, {1:F1})", screenPos.x, screenPos.y);
        if (gesture == KinectGestures.Gestures.RightHandCursor || gesture == KinectGestures.Gestures.LeftHandCursor)
        {
            Debug.Log("HandCursor Gesture");
            InteractionEventAggregator.OnRotate.Publish(new RotationEventArgs(screenPos));
        }

        if (gesture == KinectGestures.Gestures.Jump)
        {
            Debug.Log("Jump Gesture");
            InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Jump));
        }

        if (gesture == KinectGestures.Gestures.SwipeLeft)
        {
            Debug.Log("SwipeLeft Gesture");
            InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Left));
        }

        if (gesture == KinectGestures.Gestures.SwipeRight)
        {
            Debug.Log("SwipeRight Gesture");
            InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Right));
        }

        if (gesture == KinectGestures.Gestures.Pull)
        {
            Debug.Log("Pull Gesture");
            InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Back));
        }

        if (gesture == KinectGestures.Gestures.Push)
        {
            Debug.Log("Push Gesture");
            InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Forward));
        }

        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture, KinectWrapper.SkeletonJoint joint)
    {
        return true;
    }
}
