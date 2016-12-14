using UnityEngine;
using System.Collections;

namespace Building
{
    public class BlockBehaviour : MonoBehaviour
    {
        public LevelManagerBuilding levelManager;
        public Transform ActualHand;

        //public GameObject secondPlayer;

        void Awake()
        {
            // assigning levelmanager
            LevelManagerBuilding[] managers = FindObjectsOfType(typeof(LevelManagerBuilding)) as LevelManagerBuilding[];
            levelManager = managers[0];
        }

        void Update()
        {
            switch (levelManager.gameState)
            {
                case GameState.Player1Gives:
                    transform.position = new Vector3(ActualHand.position.x, ActualHand.position.y, 0);
                    break;
                case GameState.Player2Puts:
                    transform.position = ActualHand.position;
                    break;
                case GameState.Player2Gives:
                    transform.position = ActualHand.position;
                    break;
                case GameState.Player1Puts:
                    transform.position = ActualHand.position;
                    break;
            }
        }

        void OnCollisionEnter2D(Collision2D col)
        {
            Debug.Log("Colision: " + col.gameObject.name);
            switch (levelManager.gameState)
            {
                case GameState.Player1Gives:
                    if ((col.gameObject.name == "mixamorig:LeftHand" ||
                        col.gameObject.name == "mixamorig:RightHand") &&
                        col.gameObject.tag == "Player2")
                    {
                        ActualHand = col.gameObject.transform;
                        levelManager.gameState = GameState.Player2Puts;
                    }
                    break;
                case GameState.Player2Puts:
                    Debug.Log("We are in the state: " + levelManager.gameState);
                    if (col.gameObject.name == gameObject.name )
                    {
                        col.gameObject.GetComponent<TemplateBlockBehaviour>().Filled = true;
                        levelManager.gameState = GameState.Player2Takes;
                        Destroy(gameObject);
                    }
                    break;
                case GameState.Player2Gives:
                    if ((col.gameObject.name == "mixamorig:LeftHand" ||
                        col.gameObject.name == "mixamorig:RightHand") &&
                        col.gameObject.tag == "Player1")
                    {
                        ActualHand = col.gameObject.transform;
                        levelManager.gameState = GameState.Player1Puts;
                    }
                    break;
                case GameState.Player1Puts:
                    // block in a hand is the same name as a template block
                    Debug.Log("GameObject1: " + col.gameObject.name);
                    Debug.Log("GameObject2: " + gameObject.name);
                    if (col.gameObject.name == gameObject.name)
                    {
                        col.gameObject.GetComponent<TemplateBlockBehaviour>().Filled = true;
                        levelManager.gameState = GameState.Player2Takes;
                        Destroy(gameObject);
                    }
                    break;
            }
        }

        public bool IsInHand()
        {
            return true;
        }
    }
}

