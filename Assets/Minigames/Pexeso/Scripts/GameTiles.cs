using UnityEngine;
using System.Collections;

public class GameTiles : MonoBehaviour
{

    public static GameObject[] createTiles(int rows, int columns, GameObject tilePrefab, string tileTag)
    {
        GameObject[] gameTiles = new GameObject[rows * columns];
        for (int i = 0; i < columns; i++)
        {
            for (int o = 0; o < rows; o++)
            {
                gameTiles[rows * i + o] = Instantiate(tilePrefab) as GameObject;
                gameTiles[rows * i + o].transform.position = new Vector3((i * 1.2f) - (0.1125f * (columns * columns)), (o * 1.2f) - (0.05625f * (rows * rows)), -1);//flipAnimation them
                gameTiles[rows * i + o].tag = tileTag;
            }
        }

        return gameTiles;
    }
}
