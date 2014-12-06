/**
 *@file PuzzlePiece.cs
 *@author Ján Bella
 *
 *Contains definition of PuzzlePiece class
 */
using UnityEngine;

namespace Puzzle
{
	/**
	 * PuzzlePiece represents one piece of Puzzle
	 */
    public class PuzzlePiece
    {
		// identifies matching puzzle piece on the left
        public int left;
		// identifies matching puzzle piece on the right
        public int right;
		// identifies matching puzzle piece on the top
        public int top;
		// identifies matching puzzle piece on the bottom
        public int bottom;

		// id of piece, used also as gameObject name (important in GameObject collections)
        private int _id = -1;
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                if(gameObject!=null)
                {
                    gameObject.name = _id.ToString();
                }
            }
        }

		// instance of object in the scene
        public GameObject gameObject;

		/**
		 * Constructor.
		 * @param texture Texture to be set on this PuzzlePiece
		 * @param top Identifier of upper matching PuzzlePiece
		 * @param bottom Identifier of lower matching PuzzlePiece
		 * @param right Identifier of right matching PuzzlePiece
		 * @param left Identifier of left matching PuzzlePiece
		 * @param position Position of PuzzlePiece in the scene
		 */
        public PuzzlePiece(Texture2D texture, int top, int right, int bottom, int left, Vector3 position)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;

            gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            gameObject.renderer.material.mainTexture = texture;
            gameObject.AddComponent("MouseScript");
            Quaternion q = new Quaternion(0, 0, 0, 1);
			q.eulerAngles = new Vector3(90,180,0);
            gameObject.transform.rotation = q;
            gameObject.transform.position = position;
			//gameObject.AddComponent("BoxCollider2D");
        }

		/**
		 * Updates PuzzlePiece position
		 * @param position Position to be set.
		 */
        public void SetGameObjectPosition(Vector3 position)
        {
            gameObject.transform.position = position;
        }
    }
}
