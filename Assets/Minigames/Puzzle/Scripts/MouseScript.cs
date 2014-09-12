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
		private float minX = float.MinValue;
		private float maxX = float.MaxValue;
		private float minY = float.MinValue;
		private float maxY = float.MaxValue;

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

			minX = gs.getMinPiecePositionX() - renderer.bounds.size.magnitude*0.75f;
			maxX = gs.getMaxPiecePositionX()  + renderer.bounds.size.magnitude*0.75f;
			minY = gs.getMinPiecePositionY() - renderer.bounds.size.magnitude*0.75f;
			maxY = gs.getMaxPiecePositionY()  + renderer.bounds.size.magnitude*0.75f;
		}

        void OnMouseDown()
        {
            screenPoint.Clear();
            offset.Clear();
            puzzlePiecesToMove.Clear();

            setUp();
			setMinMax();

			prevScreenPoint = Input.mousePosition;
        }

		private Vector3 prevScreenPoint;

        void OnMouseDrag()
        {
			float pieceSetWidth = getPieceSetWidth();
			float pieceSetHeight = getPieceSetHeight();

			float mix = getPieceSetMinX();
			float max = getPieceSetMaxX();
			float miy = getPieceSetMinY();
			float may = getPieceSetMaxY();

            for (int i = 0; i < puzzlePiecesToMove.Count; i++)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint[i].z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset[i];

				/*Vector3 piecePosition = puzzlePiecesToMove[i].transform.position;

				if(curScreenPoint.x < prevScreenPoint.x) // going left
				{
					if(!(mix<minX - pieceSetWidth))
						piecePosition.x = (curPosition.x);
				}
				else if(curScreenPoint.x > prevScreenPoint.x) // going left
				{
					if(!(max>maxX + pieceSetWidth))
						piecePosition.x = (curPosition.x);
				}

				if(curScreenPoint.y > prevScreenPoint.y) // going down
				{
					if(!(miy<minY - pieceSetHeight))
						piecePosition.y = (curPosition.y);
				}
				else if(curScreenPoint.y < prevScreenPoint.y) // going up
				{
					if(!(miy>maxY + pieceSetHeight))
						piecePosition.y = (curPosition.y);
				}
				puzzlePiecesToMove[i].transform.position = piecePosition;
				prevScreenPoint = curScreenPoint;*/
				if(!(mix<minX - pieceSetWidth || //max>maxX + pieceSetWidth ||
				   miy<minY - pieceSetHeight || may>maxY + pieceSetHeight ))
                	puzzlePiecesToMove[i].transform.position = (curPosition);
            }
        }

        void OnMouseUp()
        {
            ((GameScript)Camera.main.GetComponent("GameScript")).CheckPossibleConnection(gameObject);
            foreach (GameObject piece in puzzlePiecesToMove)
            {
                piece.transform.position = new Vector3(piece.transform.position.x,
                                       piece.transform.position.y,
                                       piece.transform.position.z - 1);

            }

        }

		private float getPieceSetHeight()
		{
			return getPieceSetMaxY()-getPieceSetMinY();
		}

		private float getPieceSetWidth()
		{
			return getPieceSetMaxX() - getPieceSetMinX();
		}

		private float getPieceSetMinX()
		{
			float min = float.MaxValue;

			foreach(GameObject o in puzzlePiecesToMove)
			{
				if(o.transform.position.x < min)
					min = o.transform.position.x;
			}
			return min;
		}

		private float getPieceSetMinY()
		{
			float min = float.MaxValue;
			
			foreach(GameObject o in puzzlePiecesToMove)
			{
				if(o.transform.position.y < min)
					min = o.transform.position.y;
			}
			return min;
		}

		private float getPieceSetMaxX()
		{
			float max = float.MinValue;
			
			foreach(GameObject o in puzzlePiecesToMove)
			{
				if(o.transform.position.x > max)
					max = o.transform.position.x;
			}
			return max;
		}

		private float getPieceSetMaxY()
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
