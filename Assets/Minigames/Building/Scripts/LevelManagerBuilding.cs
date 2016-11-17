using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState { Player1Takes, Player2Takes, Player1Gives, Player2Gives, Player1Puts, Player2Puts};  

public class LevelManagerBuilding : MonoBehaviour {

    public int Floor;
    public List<GameObject> listOfContructions;
    private int difficulty;
    private GameObject construction;

    public GameState gameState;

	void Start () {
        chooseLevel();
        Floor = 0;
        // instantiate the building construction based on difficulty
        construction = Instantiate(listOfContructions[difficulty], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        gameState = GameState.Player1Takes;
	}
	
	void Update () {
       
	}

    private void chooseLevel()
    {
        Debug.Log("Difficulty: " + MGC.Instance.selectedMiniGameDiff);
        switch (MGC.Instance.selectedMiniGameDiff)
        {
            case 0:
                difficulty = 0;
                break;
            case 1:
                difficulty = 1;
                break;
            case 2:
                difficulty = 2;
                break;
            default:
                difficulty = 2;
                break;
        }
    }
}
