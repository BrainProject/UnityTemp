using UnityEngine;
using System.Collections;

enum BallStates { Start, Game}

public class BallBehivour : MonoBehaviour {
    private BallStates state;
    private float countdown = 10f;

	// Use this for initialization
	void Start () {
        state = BallStates.Start;
        GetComponent<Rigidbody2D>().isKinematic = true;
	
	}
	
	// Update is called once per frame
	void Update () {
        switch(state)
        {
            case BallStates.Start:
                if (countdown <= 0)
                {
                    GetComponent<Rigidbody2D>().isKinematic = false;
                    state = BallStates.Game;
                }
                else
                {
                    countdown -= Time.deltaTime;
                }
                break;
            case BallStates.Game:

                break;
        }
	    
	}
}
