using UnityEngine;
using System.Collections;

namespace MinigameMaze
{
	/// <summary>
	/// GameTiles class for creating game tile objects (in menus etc.)
	/// </summary>
	public class GameTiles : MonoBehaviour
	{
		/// <summary>
		/// Creates the tiles.
		/// </summary>
		/// <returns>The tiles.</returns>
		/// <param name="rows">Number of rows</param>
		/// <param name="columns">Number of columns</param>
		/// <param name="tilePrefab">Tile prefab</param>
		/// <param name="tileTag">Tile tag</param>
		public static GameObject[] createTiles(int rows, int columns, GameObject tilePrefab, string tileTag)
		{
			GameObject[] gameTiles = new GameObject[rows * columns];
			for (int i = 0; i < rows; i++)
			{
				for (int o = 0; o < columns; o++)
				{
					/** Scaling of both tiles and spaces between them */
					float scale = 1f;
					/* If there are more than 3 rows, scale the whole board down */
					if(rows > 3)
					{
						scale = 0.77f;
					}
					/** Size of spaces between tiles */
					float spaceSize = 0.3f;
					/** Position for the left column (lowest coordinate) */
					float tileStartColumns = (-0.5f * (columns - 1)) * scale;
					/** Move left column more to the left according to number of columns to allow spacing */
					float tileStartColumnsSpaces = (-0.5f * (columns - 1)) * spaceSize * scale;
					/** Position for the top row (highest y coordinate) */
					float tileStartRows = ((0.5f * (rows - 1)) * scale) + 1;
					/** Move top row more up according to number of rows to allow spacing */
					float tileStartRowsSpaces = (-0.5f * (rows - 1)) * spaceSize * scale;
					
					gameTiles[columns * i + o] = Instantiate(tilePrefab) as GameObject;
					gameTiles[columns * i + o].transform.localScale = (new Vector3(1f, 1f, 0.1f) * scale);
					gameTiles[columns * i + o].transform.position = new Vector3(tileStartColumns + tileStartColumnsSpaces + (o * scale) + (o*spaceSize * scale), tileStartRows - tileStartRowsSpaces - (i * scale) - (i*spaceSize * scale), -0.65f);
					gameTiles[columns * i + o].tag = tileTag;
				}
			}
			
			return gameTiles;
		}
	}
}
