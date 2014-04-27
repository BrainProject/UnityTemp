using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public static int SubscriberId = (int)SubscriberIds.Player1;
    // Use this for initialization
    public void Awake()
    {
        InteractionEventAggregator.KeyPressed.Subscribe(SubscriberId, foo);
        InteractionEventAggregator.OnRotate.Subscribe(SubscriberId,Rotate);
        InteractionEventAggregator.OnMove.Subscribe(SubscriberId,Move);
    }

    private void Rotate(RotationEventArgs rEvA)
    {
        transform.localEulerAngles = rEvA.V3Angles;
    }

    private void Move(MovingGameEventArgs moveArgs)
    {
        switch (moveArgs.MovementId)
        {
            case MovementIds.Forward:
                MoveForward();
                break;
            case MovementIds.Back:
                MoveBack();
                break;
        }
    }

    private void MoveForward()
    {
        Debug.Log("MoveForward");
    }

    private void MoveBack()
    {
        Debug.Log("MoveBack");
    }

    private void foo(KeyboardEventArgs ka)
    {
        Debug.Log("event");
    }

    void Start()
    {
        //It came from MouseLook script
        // Make the rigid body not change rotation
        if (rigidbody)
            rigidbody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

//    void OnDisable()
//    {
//        KeyboardEventCommader.KeyPressed.UnSubscribe(SubscriberId);
//    }
}
