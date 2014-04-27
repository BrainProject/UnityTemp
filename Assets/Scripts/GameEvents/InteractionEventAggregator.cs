using Assets.Scripts.Utils.GameEvents;
using UnityEngine;
using System.Collections;

public enum SubscriberIds : int
{
    Player1,
    Player2,
    Player3,
    Player4,
    MouseController
}

public enum MovementIds : int
{
    Forward,
    Back,
    Up,
    Down,
    Jump
}

public class InteractionEventAggregator : CommandMonoBehaviour
{
    /// <summary>
    /// Test event. DO NOT USE IT
    /// </summary>
    public static KeyboardGameEvent KeyPressed = new KeyboardGameEvent();

    public static RotationGameEvent OnRotate = new RotationGameEvent();
    public static MovingGameEvent OnMove = new MovingGameEvent();
    public static MouseGameEvent OnMouse = new MouseGameEvent();

//	// Use this for initialization
//	void Start () {
//	    
//	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
}
