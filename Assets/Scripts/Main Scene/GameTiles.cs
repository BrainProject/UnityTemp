using UnityEngine;
using System.Collections;

namespace MainScene {

	/// <summary>
	/// GameTiles class for creating game tile objects (in menus etc.)
	/// </summary>
	public class GameTiles : MonoBehaviour {

		/// <summary>
		/// Creates the tiles.
		/// </summary>
		/// <returns>The tiles.</returns>
		/// <param name="count">Number of menu items</param>
		/// <param name="tilePrefab">Tile prefab</param>
		/// <param name="tileTag">Tile tag</param>
		public static GameObject[] createTiles(int count, GameObject tilePrefab, string tileTag)
		{
			int[] menuDimensions = GetMenuDimensions (count);
			int rows = menuDimensions [0];
			int columns = menuDimensions [1];

			GameObject[] gameTiles = new GameObject[count];
			for (int i = 0; i < rows; i++)
			{
				for (int o = 0; o < columns; o++)
				{
					//Debug.Log ("Current run is " + ((i * columns) + o));
					if(((i * columns) + o) >= count)
					{
						return gameTiles;
					}

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

		/// <summary>
		/// Calculates proper number of rows and columns in menu.
		/// </summary>
		/// <returns>The menu dimensions.</returns>
		/// <param name="elementsCount">Elements count.</param>
		public static int[] GetMenuDimensions(int elementsCount)
		{
			if (elementsCount == 1)
			{
				return new int[]{1, 1};
			} else if (elementsCount == 2)
			{
				return new int[]{1,2};
			} else if (elementsCount <= 4)
			{
				return new int[]{2,2};
			} else if (elementsCount <= 6)
			{
				return new int[]{2,3};
			} else if (elementsCount <= 9)
			{
				return new int[]{3,3};
			} else if (elementsCount <= 12)
			{
				return new int[]{3,4};
			} else
			{
				return new int[]{4,4};
			}
		}
	}
}