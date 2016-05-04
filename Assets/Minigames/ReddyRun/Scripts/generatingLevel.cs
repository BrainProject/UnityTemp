using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Reddy
{
    public class GeneratingLevel : MonoBehaviour
    {
        public int pathLength;
        // public int oxygensNumber;
        // public int dodgeObstaclesNumber;
        // public int jumpObstaclesNumber;

        private int stackLeftTurn;
        private int stackRightTurn;
        public GameObject finishPrefab;
        
        public GameObject[] prefabs = new GameObject[3];

        private Transform connectionPoint; // position, rotation...


        private bool turning;


        private List<GameObject> arrayPath = new List<GameObject>();


        // Use this for initialization
        void Start()
        {


            pathLength = MGC.Instance.selectedMiniGameDiff + 10;
            //pathLength = 10;
            stackLeftTurn = 0;
            stackRightTurn = 0;


            for (int i = 0; i < pathLength; i++)
            {
                int randomValue;
                randomValue = Random.Range(0, 3);


                if (i == 0)
                {
                    GameObject addingTile = (GameObject)Instantiate(prefabs[0], Vector3.zero, Quaternion.Euler(new Vector3(-90,90,0)));
                    
                    //Quaternion rotation = Quaternion.identity;
                    //rotation.eulerAngles = new Vector3(-90, -60, 0);
                    //addingTile.transform.rotation = rotation;

                    arrayPath.Add(addingTile);
                    

                }
                
                else
                {
                    switch (randomValue) // switch controls non-circle path
                    {
                        case 0:
                            break;
                        case 1: // RIGHT
                            if (stackRightTurn >= 3)
                            {
                                randomValue = 0;
                            }
                            else
                            {
                                stackRightTurn++;
                                if (stackLeftTurn > 0)
                                {
                                    stackLeftTurn--;
                                }
                            }

                            break;
                        case 2: // LEFT
                            if (stackLeftTurn >= 3)
                            {
                                randomValue = 0;
                            }
                            else
                            {
                                stackLeftTurn++;
                                if (stackRightTurn > 0)
                                {
                                    stackRightTurn--;
                                }
                            }
                            break;
                    }

                    GameObject lastTileObject = arrayPath[i - 1];

                    Tile lastTile = lastTileObject.GetComponent<Tile>();

                    if (lastTile.nextTileTransform)
                    {
                        arrayPath.Add(Instantiate(prefabs[randomValue], lastTile.nextTileTransform.position, lastTile.nextTileTransform.rotation) as GameObject);
                        lastTile.nextTile = arrayPath[i].GetComponent<Tile>();
                    }

                }

            } 
            
            GameObject lastTileObject2 = arrayPath[arrayPath.Count - 1];
            Tile lastTile2 = lastTileObject2.GetComponent<Tile>();

            
            Instantiate(finishPrefab, lastTile2.nextTileTransform.position, lastTile2.nextTileTransform.rotation);
            

            


        }

        // Update is called once per frame
        void Update()
        {

        }

        


    }

}

