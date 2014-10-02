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
				if(gameTiles.Length > 12)
				{
					gameTiles[columns * i + o].transform.position = new Vector3((o * 1.2f) - (0.1125f * (columns * columns)),  (0.7f * (rows)) - (i * 1.2f), -0.65f);//flipAnimation them
				}
				else
				{
					gameTiles[columns * i + o].transform.position = new Vector3((o * 1.2f) - (0.1125f * (columns * columns)),  (0.7f * (rows)) - (i * 1.2f), -1);//flipAnimation them
				}
				gameTiles[columns * i + o].tag = tileTag;
            }
        }

        return gameTiles;
    }
}
