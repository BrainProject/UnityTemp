/**
 *@file MouseScript.cs
 *@author Ján Bella
 *
 * Mouse Interaction for Puzzle game
 **/

using UnityEngine;
using Puzzle;
using System.Collections.Generic;

namespace Puzzle
{
    public class MouseScript : MonoBehaviour
    {
		// difference between mouse down point and piece in component
        private List<Vector3> offset = new List<Vector3>();
		// pieces in selected component to move
        private List<GameObject> puzzlePiecesToMove = new List<GameObject>();

		// used for critical section in MouseDrag
		private System.Object locker = new System.Object();

		// Bounds of playground. No PuzzlePiece can cross them.
		private float cameraMaxX;
		private float cameraMinX;
		private float cameraMaxY;
		private float cameraMinY;

		/**
		 * Finds connected component of clicked piece, sets up attributes
		 */
        private void setUp()
        {
            foreach (HashSet<GameObject> pieceSet in Camera.main.GetComponent<GameScript>().connectedComponents)
            {
                if (pieceSet.Contains(gameObject))
                {
                    foreach (GameObject piece in pieceSet)
                    {
                        puzzlePiecesToMove.Add(piece);
						piece.transform.position = 
							new Vector3(piece.transform.position.x,
                                        piece.transform.position.y,
                                        0);

						offset.Add(piece.transform.position 
						           	- Camera.main.ScreenToWorldPoint(
                            		new Vector3(Input.mousePosition.x, 
						            			Input.mousePosition.y,
						            			0)));
                    }
                    break;
                }
            }

			Camera cam = Camera.main;
			
			cameraMaxX = cam.transform.position.x + 10 + cam.orthographicSize / 1.0f;
			cameraMinX = cam.transform.position.x + 10 - cam.orthographicSize / 1.0f;
			cameraMaxY = cam.transform.position.y + cam.orthographicSize /1.2f;
			cameraMinY = cam.transform.position.y + 10 - cam.orthographicSize /1.0f;
        }

		/**
		 * Handles MouseDown event
		 */
        void OnMouseDown()
        {
            offset.Clear();
            puzzlePiecesToMove.Clear();
            setUp();
        }

		/**
		 * Handles MouseDrag event. Moving of pieces, together with bounds checking
		 */
		void OnMouseDrag()
		{
			lock(locker)
			{
				float pieceSetMinX = 0;
				float pieceSetMaxX = 0;
				float pieceSetMinY = 0;
				float pieceSetMaxY = 0;
				GetPieceSetMinMaxXY(ref pieceSetMinX,ref pieceSetMinY,ref pieceSetMaxX,ref pieceSetMaxY);

				for (int i = 0; i < puzzlePiecesToMove.Count; i++)
				{
					Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
					Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset[i];
					Vector3 prevScreenPoint = Camera.main.WorldToScreenPoint(puzzlePiecesToMove[i].transform.position - offset[i]);
					Vector3 piecePosition = puzzlePiecesToMove[i].transform.position;

					if((curScreenPoint.x > prevScreenPoint.x)) // going right
					{
						if((pieceSetMaxX < (cameraMaxX)))
						{
							piecePosition.x = curPosition.x;
						}
					}
					else
					{
						if((curScreenPoint.x < prevScreenPoint.x)) // going left
						{
							if((pieceSetMinX > (cameraMinX)))
							{
								piecePosition.x = curPosition.x;
							}
						}
					}
					if((curScreenPoint.y > prevScreenPoint.y)) // going up
					{
						if((pieceSetMaxY < (cameraMaxY)))
						{
							piecePosition.y = (curPosition.y);
						}
					}
					else
					{
						if((curScreenPoint.y < prevScreenPoint.y)) // going up
						{
							if((pieceSetMinY > (cameraMinY)))
							{
								piecePosition.y = curPosition.y;
							}
						}
					}
					puzzlePiecesToMove[i].transform.position = piecePosition;
				}
			}
		}

		/**
		 * Handles MouseUp event. Releasing of piece and corresponding tasks.
		 */
        void OnMouseUp()
        {
			// connecting pieces if near each other
			Camera.main.GetComponent<GameScript>().CheckPossibleConnection(gameObject);

			int depth = 0;

			// count depth
			foreach (HashSet<GameObject> pieceSet in Camera.main.GetComponent<GameScript>().connectedComponents)
			{
				if (pieceSet.Contains(gameObject))
				{
					depth = pieceSet.Count;
					break;
				}
			}

			// update depth of pieces so larger sets are behind small
            foreach (GameObject piece in puzzlePiecesToMove)
            {
                piece.transform.position = 
					new Vector3(piece.transform.position.x,
                                piece.transform.position.y,
				                depth);				                                  
            }
        }

		/**
		 * Method to get bounds of connectedComponent, which is saved in attribute puzzlePiecesToMove
		 * @param minX minimal X coordinate
		 * @param minY minimal Y coordinate
		 * @param maxX maximal X coordinate
		 * @param maxY maximal Y coordinate
		 */
		private void GetPieceSetMinMaxXY(ref float minX, ref float minY, ref float maxX, ref float maxY)
		{
			minX = float.MaxValue;
			minY = float.MaxValue;
			maxX = float.MinValue;
			maxY = float.MinValue;

			for(int i=0;i<puzzlePiecesToMove.Count;i++)
			{
				if(puzzlePiecesToMove[i].transform.position.x > maxX)
					maxX = puzzlePiecesToMove[i].transform.position.x;
				if(puzzlePiecesToMove[i].transform.position.x < minX)
					minX = puzzlePiecesToMove[i].transform.position.x;

				if(puzzlePiecesToMove[i].transform.position.y > maxY)
					maxY = puzzlePiecesToMove[i].transform.position.y;
				if(puzzlePiecesToMove[i].transform.position.y < minY)
					minY = puzzlePiecesToMove[i].transform.position.y;
			}
		}
    }
}
