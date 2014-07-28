using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void StartGame()
    {
        //rotate to game view
        //create main object and attach script
    }

    /// <summary>
    /// COROUTINE. Called when selecting item in menu
    /// </summary>
    /// <returns></returns>
    /// <param name="first">First object</param>
    /// <param name="second">Second object</param>
    public IEnumerator SwitchView()
    {
        /*Mover mover1 = first.GetComponent("Mover") as Mover;
        Mover mover2 = second.GetComponent("Mover") as Mover;
        
        mover2.MoveUp();
        
        while (mover1.isMoving || mover2.isMoving)
        {
            yield return new WaitForSeconds(0.1f);
        }
        mover1.MoveDown();
        mover2.MoveDown();*/
        
        yield return 0;
    }
}
