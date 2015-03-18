//using UnityEngine;
//using System.Collections;
//using Puzzle;
//using System;
//using System.Collections.Generic;

//public class MouseScript_2 : MonoBehaviour {
	
//    private  List<Vector3> screenPoint = new List<Vector3>();
//    private  List<Vector3> offset = new List<Vector3>();
//    private  List<PuzzlePiece> puzzlePiecesToMove = new List<PuzzlePiece>();
	
//    private void setUpForPiece(PuzzlePiece piece)
//    {
//        if(piece == null) return;
//        if(puzzlePiecesToMove.Contains(piece)) return;

//        puzzlePiecesToMove.Add(piece);

//        Vector3 scanPos = piece.getGameObjectPosition();
//        screenPoint.Add(Camera.main.WorldToScreenPoint(scanPos));
//        offset.Add(scanPos - Camera.main.ScreenToWorldPoint(
//            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint[screenPoint.Count-1].z)));

//        setUpForPiece(piece.getTopPiece());
//        setUpForPiece(piece.getRightPiece());
//        setUpForPiece(piece.getBottomPiece());
//        setUpForPiece(piece.getLeftPiece());
//    }


//    void OnMouseDown()
//    {
//        screenPoint.Clear();
//        offset.Clear();
//        puzzlePiecesToMove.Clear();

//        PuzzlePiece thisPiece = getPuzzlePieceFromGameObject(gameObject);
		
//        setUpForPiece(thisPiece);
//    }
	
	
//    void OnMouseDrag()
//    {
//        for(int i=0;i<puzzlePiecesToMove.Count;i++)
//        {
//            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint[i].z);
//            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset[i];
//            puzzlePiecesToMove[i].setGameObjectPosition(curPosition);

//            //Debug.Log ("Piece " + i + " position: " + puzzlePiecesToMove[i].getGameObjectPosition());
//        }
//    }

//    private static HashSet<PuzzlePiece> done = new HashSet<PuzzlePiece>();

//    void OnMouseUp()
//    {
//        done.Clear();
//        checkPossibleConnection(getPuzzlePieceFromGameObject(gameObject));
//    }

//    private PuzzlePiece getPuzzlePieceFromGameObject(GameObject gameObject)
//    {
//        var pieces = ((GameScript)Camera.main.GetComponent("GameScript")).pieces;

//        foreach(PuzzlePiece piece_val in pieces.Values)
//        {
//            if(piece_val.getPiece() == gameObject)
//            {
//                return piece_val;
//            }
//        }
//        return null;
//    }



//    public void checkPossibleConnection(PuzzlePiece piece)
//    {
//        if(piece == null) return;
//        if(done.Contains(piece)) return;
//        else done.Add(piece);

//        const int diff = 5;

//        var pieces = ((GameScript)Camera.main.GetComponent("GameScript")).pieces;

//        int piece_key = -1;

//        foreach(int key in pieces.Keys)
//        {
//            if(pieces[key] == piece)
//            {
//                piece_key = key;
//                break;
//            }
//        }

//        Vector3 piece_position = piece.getGameObjectPosition();
//        Bounds piece_bounds = piece.getPieceBounds();
			
//        Debug.Log ("Piece position: " + piece_position + " with ID: " + piece.getId());

//        Debug.Log ("Piece situation: Top: " + piece.getTop() + ", Right: " + piece.getRight() 
//                       +  ", Bottom: " + piece.getBottom() + ", Left: " + piece.getLeft());

//        foreach(int piece2_key in pieces.Keys)
//        {
//            PuzzlePiece piece2 = pieces[piece2_key];
//            if(piece == piece2) continue;

//            Vector3 piece2_position = piece2.getGameObjectPosition();
				
//            Bounds piece2_bounds = piece2.getPieceBounds();
				
//            Debug.Log ("Piece2 position: " + piece2_position + " with ID: " + piece2.getId());
//            Debug.Log ("Piece2 situation: Top: " + piece2.getTop() + ", Right: " + piece2.getRight() 
//                           +  ", Bottom: " + piece2.getBottom() + ", Left: " + piece2.getLeft());
				

//            // check up
//            if(piece2.getBottom() == piece_key && piece.getTop() == piece2_key 				   
//            //	&& piece.getTopPiece() == null && piece2.getBottomPiece() == null
//                && piece2.getBottom()!=-1 && piece.getTop()!=-1)
//            {
//                if(piece_bounds.min.y < piece2_bounds.min.y &&
//                   Math.Abs(piece2_bounds.min.y - piece_bounds.max.y)<diff &&
//                   Math.Abs (piece_position.x - piece2_position.x) < diff)
//                {
//                    piece.setTopPiece(piece2);
//                    piece2.setBottomPiece(piece);

//                    Vector3 newPosition = new Vector3(
//                        piece2_position.x,
//                        piece2_position.y - piece2_bounds.size.y,
//                        piece_position.z);
							
//                    Debug.Log ("Setting Up: ");
//                    Debug.Log ("Piece position: " + piece_position);
//                    Debug.Log ("Piece 2 position: " + piece2_position);
//                    Debug.Log ("Piece 2 new position: " + newPosition);
						
//                    piece.setGameObjectPosition(newPosition);
//                    continue;
//                }
//            }
				
//            // check down
//            if(piece2.getTop() == piece_key && piece.getBottom() == piece2_key
//            //   && piece.getBottomPiece() == null && piece2.getTopPiece() == null
//               && piece2.getTop()!=-1 && piece.getBottom()!=-1)
//            {
//                if(piece_bounds.min.y > piece2_bounds.min.y &&
//                   Math.Abs(piece2_bounds.max.y - piece_bounds.min.y)<diff &&
//                   Math.Abs (piece_position.x - piece2_position.x) < diff)
//                {
//                    piece.setBottomPiece(piece2);
//                    piece2.setTopPiece(piece);
						
//                    Vector3 newPosition = new Vector3(
//                    piece2_position.x,
//                    piece2_position.y + piece2_bounds.size.y,
//                    piece_position.z);

//                    Debug.Log ("Setting Down: ");
//                    Debug.Log ("Piece position: " + piece_position);
//                    Debug.Log ("Piece 2 position: " + piece2_position);
//                    Debug.Log ("Piece 2 new position: " + newPosition);
						
//                    piece.setGameObjectPosition(newPosition);

//                    continue;
//                }
//            }
				
//            // check left
//            if(piece2.getRight() == piece_key && piece.getLeft() == piece2_key
//            //   && piece.getLeftPiece() == null && piece2.getRightPiece() == null
//               && piece2.getRight()!=-1 && piece.getLeft()!=-1)
//            {
//                if(piece_bounds.min.x < piece2_bounds.min.x &&
//                   Math.Abs(piece2_bounds.min.x - piece_bounds.max.x)<diff &&
//                   Math.Abs (piece_position.y - piece2_position.y) < diff)
//                {
//                    piece.setLeftPiece(piece2);
//                    piece2.setRightPiece(piece);
						
//                    Vector3 newPosition = new Vector3(
//                        piece2_position.x - piece2_bounds.size.x,
//                        piece2_position.y,
//                        piece_position.z);
						
////					Debug.Log ("Setting Left: ");
////					Debug.Log ("Piece position: " + piece_position);
////					Debug.Log ("Piece 2 position: " + piece2_position);
////					Debug.Log ("Piece 2 new position: " + newPosition);
						
//                    piece.setGameObjectPosition(newPosition);
						
//                    continue;
//                }
//            }
				
//            // check right
//            if(piece2.getLeft() == piece_key && piece.getRight() == piece2_key
//            //   && piece.getRightPiece() == null && piece2.getLeftPiece() == null
//               && piece2.getLeft()!=-1 && piece.getRight()!=-1)
//            {
//                if(piece_bounds.min.x > piece2_bounds.min.x &&
//                   Math.Abs(piece2_bounds.max.x - piece_bounds.min.x)<diff &&
//                   Math.Abs (piece_position.y - piece2_position.y) < diff)
//                {
//                    piece.setRightPiece(piece2);
//                    piece2.setLeftPiece(piece);

//                    Vector3 newPosition = new Vector3(
//                        piece2_position.x + piece2_bounds.size.x,
//                        piece2_position.y,
//                        piece_position.z);

////					Debug.Log ("Setting Right: ");
////					Debug.Log ("Piece position: " + piece_position);
////					Debug.Log ("Piece 2 position: " + piece2_position);
////					Debug.Log ("Piece 2 new position: " + newPosition);

//                    piece.setGameObjectPosition(newPosition);
						
//                    continue;
//                }
//            }
//        }

//        checkPossibleConnection(piece.getRightPiece());
//        checkPossibleConnection(piece.getLeftPiece());
//        checkPossibleConnection(piece.getTopPiece());
//        checkPossibleConnection(piece.getBottomPiece());
//    }



//}
