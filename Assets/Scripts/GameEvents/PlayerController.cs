using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public static int SubscriberId = (int)SubscriberIds.Player1;

    private CharacterMotor motor; 
    // Use this for initialization
    public void Awake()
    {
        motor = GetComponent<CharacterMotor>();
//        InteractionEventAggregator.KeyPressed.Subscribe(SubscriberId, foo);
        InteractionEventAggregator.OnRotate.Subscribe(SubscriberId,Rotate);
        InteractionEventAggregator.OnMove.Subscribe(SubscriberId,Move);
        InteractionEventAggregator.OnMouse.Subscribe(SubscriberId,MouseEvent);
    }

    private void MouseEvent(MouseGameEventArgs mEva)
    {
        MouseEmulator.MouseEvent(mEva.Flags);
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
            case MovementIds.Left:
                MoveLeft();
                break;
            case MovementIds.Right:
                MoveRight();
                break;
            case MovementIds.Jump:
                MoveJump();
                break;
        }
    }

    private void MoveForward()
    {
        Debug.Log("MoveForward");
        var directionVector = new Vector3(0, 0, 1);
        InputController(directionVector);
    }

    private void InputController(Vector3 v3)
    {
        if (v3 != Vector3.zero)
        {
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            var directionLength = v3.magnitude;
            v3 = v3 / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength * directionLength;

            // Multiply the normalized direction vector by the modified length
            v3 = v3 * directionLength;
        }

        // Apply the direction to the CharacterMotor
        motor.inputMoveDirection = transform.rotation * v3;
    }

    private void MoveBack()
    {
        Debug.Log("MoveBack");
        var directionVector = new Vector3(0, 0, -1);
        InputController(directionVector);
    }

    private void MoveLeft()
    {
        Debug.Log("MoveLeft");
        var directionVector = new Vector3(-1, 0, 0);
        InputController(directionVector);
    }

    private void MoveRight()
    {
        Debug.Log("MoveRight");
        var directionVector = new Vector3(1, 0, 0);
        InputController(directionVector);
    }

    private void MoveJump()
    {
        Debug.Log("MoveJump");
        motor.inputJump = Input.GetButton("Jump");
    }
//    private void foo(KeyboardEventArgs ka)
//    {
//        Debug.Log("event");
//    }

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
