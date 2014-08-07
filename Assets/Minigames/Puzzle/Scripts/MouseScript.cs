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

        private void setUp()
        {
            foreach (HashSet<GameObject> pieceSet in Camera.main.GetComponent<GameScript>().connectedComponents)
            {
                if (pieceSet.Contains(gameObject))
                {
                    foreach (GameObject piece in pieceSet)
                    {
                        puzzlePiecesToMove.Add(piece);

                        Vector3 scanPos = piece.transform.position;
                        screenPoint.Add(Camera.main.WorldToScreenPoint(scanPos));
                        offset.Add(scanPos - Camera.main.ScreenToWorldPoint(
                            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint[screenPoint.Count - 1].z)));
                    }
                    break;
                }
            }
        }


        void OnMouseDown()
        {
            screenPoint.Clear();
            offset.Clear();
            puzzlePiecesToMove.Clear();

            setUp();
        }


        void OnMouseDrag()
        {
            for (int i = 0; i < puzzlePiecesToMove.Count; i++)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint[i].z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset[i];
                puzzlePiecesToMove[i].transform.position = (curPosition);
            }
        }

        void OnMouseUp()
        {
            Camera.main.GetComponent<GameScript>().CheckPossibleConnection(gameObject);
        }
    }
}
