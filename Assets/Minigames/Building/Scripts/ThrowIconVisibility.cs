using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ThrowIconVisibility : MonoBehaviour {

    public LevelManagerBuilding levelManager;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (levelManager.gameState == GameState.Player1Gives ||
            levelManager.gameState == GameState.Player2Puts ||
            levelManager.gameState == GameState.Player2Gives ||
            levelManager.gameState == GameState.Player1Puts)
        {
            if (!gameObject.GetComponent<Image>().enabled)
            {
                gameObject.GetComponent<Image>().enabled = true;
            }
        }
        else
        {
            if (gameObject.GetComponent<Image>().enabled)
            {
                gameObject.GetComponent<Image>().enabled = false;
            }
        }
	}
}
