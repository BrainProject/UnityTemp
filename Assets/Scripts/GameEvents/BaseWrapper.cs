using UnityEngine;
using System.Collections;

public abstract class BaseWrapper : MonoBehaviour {

	// Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    protected void MoveForward(){}
    protected void MoveBack(){}
    protected void MoveLeft(){}
    protected void MoveRight(){}
    protected void TurnLeft(){}
    protected void TurnRight(){}
    protected void PickAnObject(){}
    protected void LookAround(){}
    protected void Jump(){}
}
