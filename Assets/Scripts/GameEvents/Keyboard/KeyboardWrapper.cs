using UnityEngine;
using System.Collections;

public class KeyboardWrapper : MonoBehaviour
{
	// Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
	    {
            InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Jump));
	    }
        if (Input.GetKeyDown(KeyCode.W))
	    {
	        InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Forward));
	    }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Back));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Left));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            InteractionEventAggregator.OnMove.Publish(new MovingGameEventArgs(MovementIds.Right));
        }
	}

//    protected void MoveForward(){}
//    protected void MoveBack(){}
//    protected void MoveLeft(){}
//    protected void MoveRight(){}
//    protected void TurnLeft(){}
//    protected void TurnRight(){}
}
