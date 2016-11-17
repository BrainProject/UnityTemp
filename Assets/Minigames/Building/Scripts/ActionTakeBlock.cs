using UnityEngine;
using System.Collections;
using GSIv2;


namespace Building
{

    [RequireComponent(typeof(BoxCollider2D))]
    public class ActionTakeBlock : CollisionLoading
    {
        public LevelManagerBuilding levelManager;

        public GameObject BlockPrefab;
        public GameObject block;


        protected override void Start()
        {
            base.Start();
            
        }

        /// <summary>
        /// Action runs when loading is done.
        /// Action depends on the GameState.
        /// </summary>
        public override void Action()
        {
            switch (levelManager.gameState)
            {
                case GameState.Player1Takes:
                    // working with canavas coliders
                    if (LastCanvasColider.name == "HandColider1L" || LastCanvasColider.name == "HandColider1R")
                    {
                        levelManager.gameState = GameState.Player1Gives;
                        block = Instantiate(BlockPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        block.GetComponent<BlockBehaviour>().ActualHand = LastCanvasColider.GetComponent<HandColiderMovement>().hand;
                    } 
                    break;
                case GameState.Player1Gives:                    
                    break;
                case GameState.Player2Takes:
                    if (LastCanvasColider.name == "HandColider2L" || LastCanvasColider.name == "HandColider2R")
                    {
                        levelManager.gameState = GameState.Player2Gives;
                        block = Instantiate(BlockPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                        block.GetComponent<BlockBehaviour>().ActualHand = LastCanvasColider.GetComponent<HandColiderMovement>().hand;
                    }
                    break;

            }
            if (true) //TODO: overovani jestli je kostka zrovna v ruce
            {
                
                
            }     
        }
    }

}