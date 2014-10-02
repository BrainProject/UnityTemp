//using System;
//using UnityEngine;
//using System.Collections.Generic;


//namespace Puzzle
//{
//    public class PuzzlePiece_2
//    {
//        private int[] neighbourIndices = new int[4];
//        private PuzzlePiece[] neighbourPieces = new PuzzlePiece[4];
//        private GameObject piece = null;
//        private Texture2D texture = null;

//        private int id = 0;


//        public PuzzlePiece (Texture2D texture, int top, int right, int bottom, int left)
//        {
//            neighbourIndices[0] = top;
//            neighbourIndices[1] = right;
//            neighbourIndices[2] = bottom;
//            neighbourIndices[3] = left;
//            this.texture = texture;

//            neighbourPieces[0] = null;
//            neighbourPieces[1] = null;
//            neighbourPieces[2] = null;
//            neighbourPieces[3] = null;

//        }

////		public PuzzlePiece (Texture2D texture, uint index_x, uint index_y, uint size_x, uint size_y, uint texture_size)
////		{
////
////
////
////
////
////			neighbourIndices[0] = top;
////			neighbourIndices[1] = right;
////			neighbourIndices[2] = bottom;
////			neighbourIndices[3] = left;
////			this.texture = texture;
////			
////			
////			
////		}



//        public int getTop()
//        {
//            return this.neighbourIndices[0];
//        }

//        public int getRight()
//        {
//            return this.neighbourIndices[1];
//        }

//        public int getBottom()
//        {
//            return this.neighbourIndices[2];
//        }

//        public int getLeft()
//        {
//            return this.neighbourIndices[3];
//        }

//        public void setTopPiece(PuzzlePiece value)
//        {
//            this.neighbourPieces[0] = value;

//            Debug.Log("setTop called");
//        }
		
//        public void setRightPiece(PuzzlePiece value)
//        {
//            this.neighbourPieces[1] = value;
//            Debug.Log("setRight called");
//        }
		
//        public void setBottomPiece(PuzzlePiece value)
//        {
//            this.neighbourPieces[2] = value;
//            Debug.Log("setBottom called");
//        }
		
//        public void setLeftPiece(PuzzlePiece value)
//        {
//            this.neighbourPieces[3] = value;
//            Debug.Log("setLeft called");
//        }

//        public PuzzlePiece getTopPiece()
//        {
//            return this.neighbourPieces[0];
//        }
		
//        public PuzzlePiece getRightPiece()
//        {
//            return this.neighbourPieces[1];
//        }
		
//        public PuzzlePiece getBottomPiece()
//        {
//            return this.neighbourPieces[2];
//        }
		
//        public PuzzlePiece getLeftPiece()
//        {
//            return this.neighbourPieces[3];
//        }



//        public void createGameObject(Vector3 position)
//        {
//            piece = GameObject.CreatePrimitive(PrimitiveType.Plane);

//            piece.transform.position = position;

//            Quaternion q = new Quaternion(0, 0, 0, 1);
//            //assign values to quaternion
//            q.SetLookRotation(new Vector3(0,-1,0), new Vector3(0,1,0));
//            piece.transform.rotation = q;

//            piece.renderer.material.mainTexture = texture;
//            piece.AddComponent("MouseScript");
//            piece.renderer.material.shader = Shader.Find("Particles/Alpha Blended");

//        }

//        public Vector3 getPieceSize()
//        {
//            Debug.Log("Min of the piece is: " + piece.renderer.bounds.min);
//            Debug.Log("Max of the piece is: " + piece.renderer.bounds.max);
//            return piece == null ? new Vector3() : piece.renderer.bounds.size; //piece.renderer.bounds.max - piece.renderer.bounds.min;
//        }

//        public Bounds getPieceBounds()
//        {
//            return piece.renderer.bounds;
//        }

//        public void setGameObjectPosition(Vector3 position)
//        {
//            if(piece!=null)
//            {
//                piece.transform.position = position;
//                Debug.Log("New Position set for gameObject.");

//            }
//        }

//        public Vector3 getGameObjectPosition()
//        {
//            return piece.transform.position;
//        }

//        public Boolean isWon()
//        {
//            Boolean ret = true;
//            for(int i=0;i<4;i++)
//            {
//                ret = ret && (neighbourPieces[i]!=null || neighbourIndices[i]<0); 
//            }
//            return ret;
//        }

//        public void setId(int id)
//        {
//            this.id = id;
//        }

//        public int getId()
//        {
//            return this.id;
//        }

//        public GameObject getPiece()
//        {
//            return piece;
//        }

//    }
//}

