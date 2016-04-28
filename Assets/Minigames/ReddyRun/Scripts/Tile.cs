using UnityEngine;
using System.Collections;


namespace Reddy
{
    public enum TileType
    {
        STRAIGHT,
        LEFT,
        RIGHT,
        FINISH
    }

    public class Tile : MonoBehaviour
    {

        public Tile nextTile;
        public TileType typeOfTile;
        public Transform nextTileTransform;

        public GameObject leavingTrigger;



        // Use this for initialization
        void Start()
        {
            
            if (nextTile)
            {
                if (nextTile.typeOfTile == TileType.LEFT || nextTile.typeOfTile == TileType.RIGHT)
                {
                    if (leavingTrigger)
                    {
                        leavingTrigger.SetActive(false);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

