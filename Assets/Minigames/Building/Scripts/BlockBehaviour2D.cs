using UnityEngine;
using System.Collections;
using Kinect;

public class BlockBehaviour2D : MonoBehaviour
{

    public LevelManagerBuilding levelManager;
    public Transform ActualHand;

    void Awake()
    {
        // assigning levelmanager
        LevelManagerBuilding[] managers = FindObjectsOfType(typeof(LevelManagerBuilding)) as LevelManagerBuilding[];
        levelManager = managers[0];
    }

    void Start()
    {
    }

    void Update()
    {
        if (levelManager.throwObject)
        {

                levelManager.ChangeAlpha(levelManager.Player1, 1f);
                levelManager.ChangeAlpha(levelManager.Player2, 0.5f);
                levelManager.gameState = GameState.Player1Takes;
                levelManager.throwObject = false;
                Destroy(gameObject);


        }

        transform.position = ActualHand.position;
    }




    void OnCollisionEnter2D(Collision2D col)
    {
        switch (levelManager.gameState)
        {
            case GameState.Player1Gives:
                if (col.gameObject.name == "2DColliderL" ||
                    col.gameObject.name == "2DColliderR")
                {
                    if (col.gameObject.tag == "Player2")
                    {
                        ActualHand = col.gameObject.transform;
                        levelManager.ChangeAlpha(levelManager.Player1, 0.5f);
                        levelManager.gameState = GameState.Player2Puts;
                    }
                    else
                    {
                        ActualHand = col.gameObject.transform;
                    }
                    
                }
                break;
            case GameState.Player2Puts:

                // giving from left/right hand of one player
                if (col.gameObject.name == "2DColliderL" ||
                    col.gameObject.name == "2DColliderR")
                {
                    if (col.gameObject.tag == "Player2")
                    {
                        ActualHand = col.gameObject.transform;
                    }
                }

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
                if (col.gameObject.name == "2DColliderL" ||
                    col.gameObject.name == "2DColliderR")
                {
                    if (col.gameObject.tag == "Player1")
                    {
                        ActualHand = col.gameObject.transform;
                        levelManager.ChangeAlpha(levelManager.Player2, 0.5f);
                        levelManager.gameState = GameState.Player1Puts;
                    }
                    else
                    {
                        ActualHand = col.gameObject.transform;
                    }
                    
                }
                break;
            case GameState.Player1Puts:
                // giving from left/right hand of one player
                if (col.gameObject.name == "2DColliderL" ||
                    col.gameObject.name == "2DColliderR")
                {
                    if (col.gameObject.tag == "Player1")
                    {
                        ActualHand = col.gameObject.transform;
                    }
                }

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
