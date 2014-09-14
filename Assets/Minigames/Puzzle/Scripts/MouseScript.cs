using UnityEngine;
using System.Collections;
using Puzzle;
using System;
using System.Collections.Generic;

namespace Puzzle
{
    public class MouseScript : MonoBehaviour
    {

        private List<Vector3> screenPoint = new List<Vector3>();
        private List<Vector3> offset = new List<Vector3>();
        private List<GameObject> puzzlePiecesToMove = new List<GameObject>();

		// Global of all pieces in the scene
		private float allPiecesMinX = float.MinValue;
		private float allPiecesMaxX = float.MaxValue;
		private float allPiecesMinY = float.MinValue;
		private float allPiecesMaxY = float.MaxValue;

        private void setUp()
        {
            foreach (HashSet<GameObject> pieceSet in ((GameScript)Camera.main.GetComponent("GameScript")).connectedComponents)
            {
                if (pieceSet.Contains(gameObject))
                {
                    foreach (GameObject piece in pieceSet)
                    {
                        puzzlePiecesToMove.Add(piece);
                        piece.transform.position = new Vector3(piece.transform.position.x,
                                                               piece.transform.position.y,
                                                               piece.transform.position.z + 1);

                        Vector3 scanPos = piece.transform.position;
                        screenPoint.Add(Camera.main.WorldToScreenPoint(scanPos));
                        offset.Add(scanPos - Camera.main.ScreenToWorldPoint(
                            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint[screenPoint.Count - 1].z)));
                    }
                    break;
                }
            }
        }

		void setMinMax()
		{
			GameScript gs = Camera.main.GetComponent<GameScript>();

			allPiecesMinX = gs.GetMinPiecePositionX(gameObject);// - renderer.bounds.size.magnitude*0.75f;
			allPiecesMaxX = gs.GetMaxPiecePositionX(gameObject);// + renderer.bounds.size.magnitude*0.75f;
			allPiecesMinY = gs.GetMinPiecePositionY(gameObject);// - renderer.bounds.size.magnitude*0.75f;
			allPiecesMaxY = gs.GetMaxPiecePositionY(gameObject);// + renderer.bounds.size.magnitude*0.75f;
		}

        void OnMouseDown()
        {
			Debug.Log("OnMouseDown called");
            screenPoint.Clear();
            offset.Clear();
            puzzlePiecesToMove.Clear();

            setUp();
			setMinMax();

			/*prevScreenPoint = Input.mousePosition;

			Vector3 moveBy = Camera.main.ScreenToWorldPoint(prevScreenPoint) - gameObject.transform.position + new Vector3(0,0,10);

			foreach(GameObject obj in puzzlePiecesToMove)
			{
				obj.transform.position += moveBy;
			}*/
        }

		//private Vector3 prevScreenPoint;

		System.Object locker = new System.Object();

		void OnMouseDrag()
		{
			lock(locker)
			{
			Debug.Log("OnMouseDrag called");
			float pieceSetWidth = GetPieceSetWidth();
			float pieceSetHeight = GetPieceSetHeight();

			//Debug.Log ("Piece set width: " + pieceSetWidth);
			//Debug.Log ("Piece set height: " + pieceSetHeight);

			float pieceSetMinX = GetPieceSetMinX();
			float pieceSetMaxX = GetPieceSetMaxX();
			float pieceSetMinY = GetPieceSetMinY();
			float pieceSetMaxY = GetPieceSetMaxY();

			//Debug.Log("pieceSetMinX: " + pieceSetMinX + "; pieceSetMaxX: " + pieceSetMaxX +
			//         "; pieceSetMinY: " + pieceSetMinY + "; pieceSetMaxY: " + pieceSetMaxY);

            for (int i = 0; i < puzzlePiecesToMove.Count; i++)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint[i].z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset[i];
			   
				Vector3 prevScreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
				
				Vector3 piecePosition = puzzlePiecesToMove[i].transform.position;

				//Debug.Log("Piece position: " + piecePosition.ToString());

				//Debug.Log ("Current Screen Point: " + curScreenPoint + "; Previous screen point: " + prevScreenPoint);
				//Debug.Log ("PieceSetMaxX: " + pieceSetMaxX + "; allPiecesMaxX: " + allPiecesMaxX);
				//Debug.Log("Piece width: " + pieceSetWidth);
				//Debug.Log("Piece position: " + piecePosition + "; current position: " + curPosition);

				if((curScreenPoint.x > prevScreenPoint.x)) // going right
				{
					if((pieceSetMaxX < (allPiecesMaxX + pieceSetWidth)))
					{
						piecePosition.x = curPosition.x;
					}
				}
				else 
				{
					if((curScreenPoint.x < prevScreenPoint.x)) // going left
					{
						if((pieceSetMinX > (allPiecesMinX - pieceSetWidth)))
						{
							piecePosition.x = curPosition.x;
						}
					}
				}

				if((curScreenPoint.y > prevScreenPoint.y)) // going up
				{
					if((pieceSetMaxY < (allPiecesMaxY + pieceSetHeight)))
					{
						piecePosition.y = (curPosition.y);
					}
				}
				else 
				{
					if((curScreenPoint.y < prevScreenPoint.y)) // going up
					{
						if((pieceSetMinY > (allPiecesMinY - pieceSetHeight)))
						{
							piecePosition.y = curPosition.y;
						}
					}
				}
				puzzlePiecesToMove[i].transform.position = piecePosition;
				//if(!(mix<minX - pieceSetWidth || max>maxX + pieceSetWidth ||
				//   miy<minY - pieceSetHeight || may>maxY + pieceSetHeight ))
                //	puzzlePiecesToMove[i].transform.position = (curPosition);
            }
				//prevScreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

			}
        }

        void OnMouseUp()
        {
			Debug.Log("OnMouseUp called");
			((GameScript)Camera.main.GetComponent("GameScript")).CheckPossibleConnection(gameObject);


            foreach (GameObject piece in puzzlePiecesToMove)
            {
                piece.transform.position = new Vector3(piece.transform.position.x,
                                       piece.transform.position.y,
                                       piece.transform.position.z - 1);

            }

        }

		private float GetPieceSetHeight()
		{
			return GetPieceSetMaxY() + (gameObject.renderer.bounds.size.y / 2) 
				- (GetPieceSetMinY()  - (gameObject.renderer.bounds.size.y / 2));
			//return GetPieceSetMaxY() - GetPieceSetMinY();
		}

		private float GetPieceSetWidth()
		{
			return GetPieceSetMaxX() + (gameObject.renderer.bounds.size.x / 2) 
				- (GetPieceSetMinX() - (gameObject.renderer.bounds.size.x / 2));
			//return GetPieceSetMaxX() - GetPieceSetMinX();
		}

		private float GetPieceSetMinX()
		{
			float min = float.MaxValue;

			foreach(GameObject o in puzzlePiecesToMove)
			{
				if(o.transform.position.x < min)
					min = o.transform.position.x;
			}
			return min;
		}

		private float GetPieceSetMinY()
		{
			float min = float.MaxValue;
			
			foreach(GameObject o in puzzlePiecesToMove)
			{
				if(o.transform.position.y < min)
					min = o.transform.position.y;
			}
			return min;
		}

		private float GetPieceSetMaxX()
		{
			float max = float.MinValue;
			
			foreach(GameObject o in puzzlePiecesToMove)
			{
				if(o.transform.position.x > max)
					max = o.transform.position.x;
			}
			return max;
		}

		private float GetPieceSetMaxY()
		{
			float max = float.MinValue;
			
			foreach(GameObject o in puzzlePiecesToMove)
			{
				if(o.transform.position.y > max)
					max = o.transform.position.y;
			}
			return max;
		}
    }
}
