using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameState { Player1Takes, Player2Takes, Player1Gives, Player2Gives, Player1Puts, Player2Puts};  

public class LevelManagerBuilding : MonoBehaviour {

    public int Floor;
    public List<GameObject> listOfContructions;
    private int difficulty;
    private GameObject construction;

    public GameObject Player1;
    public GameObject Player2;

    public bool throwObject;
    

    public GameState gameState;

	void Start () {
        chooseLevel();
        Floor = 0;
        // instantiate the building construction based on difficulty
        construction = Instantiate(listOfContructions[difficulty], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        gameState = GameState.Player1Takes;
        ChangeAlpha(Player1, 1);
        ChangeAlpha(Player2, 0.5f);
        throwObject = false;
	}
	
	void Update () {
        //Debug.Log("Game state: " + gameState);
        Floor = SetActualFloor();
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

    private int SetActualFloor()
    {
        int min = 100;
        foreach (GameObject block in construction.GetComponent<ConstructionData>().ListOfBlocks)
        {
            if (!block.GetComponent<TemplateBlockBehaviour>().Filled)
            {
                if (block.GetComponent<TemplateBlockBehaviour>().Floor < min)
                {
                    min = block.GetComponent<TemplateBlockBehaviour>().Floor;
                }
            }
        }
        return min;
    }

    public void ChangeAlpha(GameObject player, float alpha)
    {
        foreach (SkinnedMeshRenderer renderer in player.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);
        }
    }
}
