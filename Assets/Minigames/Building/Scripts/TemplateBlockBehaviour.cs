using UnityEngine;
using System.Collections;

public class TemplateBlockBehaviour : MonoBehaviour {

    public bool Filled;
    private bool inactive;
    public Material FilledMaterial;
    public LevelManagerBuilding levelManager;
    public int Floor;

    void Awake()
    {
        // assigning levelmanager
        LevelManagerBuilding[] managers = FindObjectsOfType(typeof(LevelManagerBuilding)) as LevelManagerBuilding[];
        levelManager = managers[0];
    }

    void Start () {
        Filled = false;
        inactive = false;
	}
	
	void Update () {
        switch (levelManager.gameState)
        {
            case GameState.Player2Puts:
                if (Filled && !(inactive))
                {
                    gameObject.GetComponentInChildren<MeshRenderer>().material = FilledMaterial;
                    inactive = true;
                    levelManager.gameState = GameState.Player2Takes;
                }
                break;
            case GameState.Player1Puts:
                if (Filled && !(inactive))
                {
                    gameObject.GetComponentInChildren<MeshRenderer>().material = FilledMaterial;
                    inactive = true;
                    levelManager.gameState = GameState.Player1Takes;
                }
                break;
            default:
                break;
        }
	    
	}
}
