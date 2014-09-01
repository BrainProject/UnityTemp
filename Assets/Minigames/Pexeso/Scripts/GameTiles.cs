using UnityEngine;
using System.Collections;

public class GameTiles : MonoBehaviour
{

    public static GameObject[] createTiles(int rows, int columns, GameObject tilePrefab, string tileTag)
    {
        GameObject[] gameTiles = new GameObject[rows * columns];
        for (int i = 0; i < rows; i++)
        {
            for (int o = 0; o < columns; o++)
            {
				gameTiles[columns * i + o] = Instantiate(tilePrefab) as GameObject;
				gameTiles[columns * i + o].transform.position = new Vector3((o * 1.2f) - (0.1125f * (columns * columns)),  (0.225f * (rows * rows)) - (i * 1.2f), -1);//flipAnimation them
				gameTiles[columns * i + o].tag = tileTag;
            }
        }

        return gameTiles;
    }
}
