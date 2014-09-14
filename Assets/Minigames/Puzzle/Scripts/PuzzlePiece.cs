using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puzzle
{
    public class PuzzlePiece
    {
        public int left;
        public int right;
        public int top;
        public int bottom;

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

        public GameObject gameObject;


        public PuzzlePiece(Texture2D texture, int top, int right, int bottom, int left, Vector3 position)
        {
            this.top = top;
            this.right = right;
            this.bottom = bottom;
            this.left = left;

            gameObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            //gameObject.renderer.material.shader = Shader.Find("VertexLit");
            Debug.Log(gameObject.renderer.material.shader.name);
            gameObject.renderer.material.mainTexture = texture;
            gameObject.AddComponent("MouseScript");
            //gameObject.renderer.material.shader = Shader.Find("Particles/Alpha Blended");
            Quaternion q = new Quaternion(0, 0, 0, 1);
            //q.SetLookRotation(new Vector3(1/3, 2/3, 0), new Vector3(0, 1, 0));
			q.eulerAngles = new Vector3(90,180,0);
            gameObject.transform.rotation = q;
            gameObject.transform.position = position;
			gameObject.AddComponent("BoxCollider2D");

        }


        public Vector3 GetPieceSize()
        {
            return gameObject.renderer.bounds.size; //piece.renderer.bounds.max - piece.renderer.bounds.min;
        }

        public void SetGameObjectPosition(Vector3 position)
        {
            gameObject.transform.position = position;
        }
    }
}
