using UnityEngine;
using System.Collections;

public class BlockBehaviour2D : MonoBehaviour {

    public LevelManagerBuilding levelManager;
    public Transform ActualHand;

    public float FallSpeed;
    private Vector3 lastPosition;
    private Vector3 actualPosition;

    private bool fallen;

    private int time; // time fo calculating the speed of moving

    void Awake()
    {
        // assigning levelmanager
        LevelManagerBuilding[] managers = FindObjectsOfType(typeof(LevelManagerBuilding)) as LevelManagerBuilding[];
        levelManager = managers[0];
    }

    void Start () {
        actualPosition = transform.position;
        fallen = false;
	}

    void Update()
    {
        if (fallen)
        {
            Destroy(gameObject);
            levelManager.ChangeAlpha(levelManager.Player1, 1f);
            levelManager.ChangeAlpha(levelManager.Player2, 0.5f);
            levelManager.gameState = GameState.Player1Takes;
        }
        else
        {
            transform.position = ActualHand.position;
            float deltaSpeed = FallSpeed * Time.deltaTime;
            Debug.Log("DeltaSpeed " + deltaSpeed);
            lastPosition = actualPosition;
            actualPosition = transform.position;
            if (Vector3.Distance(lastPosition, actualPosition) > deltaSpeed)
            {
                fallen = true;
            }
        }
        
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Colision: " + col.gameObject.name);
        switch (levelManager.gameState)
        {
            case GameState.Player1Gives:
                if ((col.gameObject.name == "2DColliderL" ||
                    col.gameObject.name == "2DColliderR") &&
                    col.gameObject.tag == "Player2")
                {
                    ActualHand = col.gameObject.transform;
                    levelManager.ChangeAlpha(levelManager.Player1, 0.5f);
                    levelManager.gameState = GameState.Player2Puts;
                }
                break;
            case GameState.Player2Puts:
                // TODO: Jak muze dojit ke kolizi, kdyz k ni nedojde???
                if (col.gameObject.tag == gameObject.tag &&
                    col.gameObject.GetComponent<TemplateBlockBehaviour>().Floor == levelManager.Floor &&
                    !col.gameObject.GetComponent<TemplateBlockBehaviour>().Filled)
                {
                    Debug.Log("Collision with " + col.gameObject.name);
                    col.gameObject.GetComponent<TemplateBlockBehaviour>().Filled = true;
                    Destroy(gameObject);
                }
                break;
            case GameState.Player2Gives:
                if ((col.gameObject.name == "2DColliderL" ||
                    col.gameObject.name == "2DColliderR") &&
                    col.gameObject.tag == "Player1")
                {
                    ActualHand = col.gameObject.transform;
                    levelManager.ChangeAlpha(levelManager.Player2, 0.5f);
                    levelManager.gameState = GameState.Player1Puts;
                }
                break;
            case GameState.Player1Puts:
                if (col.gameObject.tag == gameObject.tag &&
                    col.gameObject.GetComponent<TemplateBlockBehaviour>().Floor == levelManager.Floor &&
                    !col.gameObject.GetComponent<TemplateBlockBehaviour>().Filled)
                {
                    col.gameObject.GetComponent<TemplateBlockBehaviour>().Filled = true;
                    Destroy(gameObject);
                }
                break;
        }
    }
}
