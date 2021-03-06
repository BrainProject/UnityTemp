/**
 *@file PuzzlePiece.cs
 *@author JÃ¡n Bella
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
                if (gameObject != null)
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
         * @param index unique Identifier of current puzzle piece, expected number in [0, number_pieces]
         * @param puzzle_width number of puzzle pieces horizontally
         * @param puzzle_height number of puzzle pieces vertically
         */
        public PuzzlePiece(Texture2D texture, int index, int puzzle_width, int puzzle_height, Shader shader)
        {
            this.bottom = index / puzzle_width == 0 ? -1 : index - puzzle_width;
            this.right = (index + 1) % puzzle_width == 0 ? -1 : index + 1;
            this.top =  (index + 1 + puzzle_width) > puzzle_width * puzzle_height ? -1 : index + puzzle_width;
            this.left = index % puzzle_width == 0 ? -1 : index - 1;
            //Debug.Log("Puzzle piece with index " + index + ", has neighbours set to L: " + left + ", R: " + right + ", T: " + top + ", B: " + bottom);

            gameObject = new GameObject();
            gameObject.AddComponent<MeshRenderer>();
            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            MeshCollider collider = gameObject.AddComponent<MeshCollider>();

            Vector3[] Vertices = new Vector3[] { new Vector3(-5, 0, 5), new Vector3(5, 0, 5), new Vector3(5, 0, -5), new Vector3(-5, 0, -5) };

            int p1 = ((index + 1) % (puzzle_width));
            if (p1 == 0) p1 = puzzle_width;
            int p2 = ((index + puzzle_width) / (puzzle_width));
            if (p2 > puzzle_height) p2 = puzzle_height;

            //Vector2[] UV = new Vector2[] { 
            //        new Vector2((float)(p1                    ) / (float)(puzzle_width), (float)(index / (puzzle_width)) / (float)(puzzle_height)),
            //        new Vector2((float)(index % (puzzle_width)) / (float)(puzzle_width), (float)(index / (puzzle_width)) / (float)(puzzle_height)),
            //        new Vector2((float)(index % (puzzle_width)) / (float)(puzzle_width), (float)(p2                    ) / (float)(puzzle_height)),
            //        new Vector2((float)(p1                    ) / (float)(puzzle_width), (float)(p2                    ) / (float)(puzzle_height))};

            //     2       3
            //
            //     1       0             

            Vector2[] UV = new Vector2[] { 
                    new Vector2((float)( (p1 * 5  + 1 )        ) / (float)(puzzle_width * 5), (float)( (index / (puzzle_width) * 5 - 1)) / (float)(puzzle_height*5)),
                    new Vector2((float)(index % (puzzle_width)) / (float)(puzzle_width), (float)( (index / (puzzle_width) * 5 - 1)) / (float)(puzzle_height * 5)),
                    new Vector2((float)(index % (puzzle_width)) / (float)(puzzle_width), (float)( p2                  ) / (float)(puzzle_height)),
                    new Vector2((float)( (p1 * 5 + 1 )        ) / (float)(puzzle_width * 5), (float)(p2                    ) / (float)(puzzle_height))};

            Vector2[] alpha = { 
                    new Vector2( 0.99f, 0.01f),
                    new Vector2( 0.01f, 0.01f),
                    new Vector2(0.01f, 0.99f),
                    new Vector2(0.99f, 0.99f)};


            int[] Triangles = new int[] { 0, 1, 2, 0, 2, 3 };

            Mesh mesh = new Mesh();
            mesh.vertices = Vertices;
            mesh.triangles = Triangles;
            mesh.uv2 = alpha;
            mesh.uv2 = alpha;

            mesh.uv = UV;

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.name = "PuzzlePieceMesh";

            filter.mesh = mesh;
            collider.sharedMesh = mesh;

            Material m = new Material(shader);
            m.SetTexture("_MainTex", texture);


            // puzzle texture masks:
            // 1 2 3
            // 4 5 6
            // 7 8 9

            //m.SetTexture("_AlphaTex", Resources.Load<Texture>("TextureMasks/puzzle" +
            m.SetTexture("_AlphaTex", Resources.Load<Texture>("TextureMasks/tileScaled" +
                (  (left == -1)  ? (top == -1 ? 1 : (bottom == -1 ? 7 : 4)) :
                   (right == -1) ? (top == -1 ? 3 : (bottom == -1 ? 9 : 6)) :
                   (top == -1) ? 2 : (bottom == -1 ? 8 : 5))   ));

            this.bottom = index / puzzle_width == 0 ? -1 : index - puzzle_width;
            this.right = (index + 1) % puzzle_width == 0 ? -1 : index + 1;
            this.top =  (index + 1 + puzzle_width) > puzzle_width * puzzle_height ? -1 : index + puzzle_width;
            this.left = index % puzzle_width == 0 ? -1 : index - 1;
            
            //int a = (left == -1) ? (top == -1 ? 1 : (bottom == -1 ? 7 : 4)) :
            //        (right == -1)? (top == -1 ? 3 : (bottom == -1 ? 9 : 6)) :
            //        (top == -1)  ? 2 : (bottom == -1 ? 8 : 5);

            
            // gameObject.renderer.material.mainTexture = texture;
            gameObject.GetComponent<Renderer>().material = m;

            gameObject.AddComponent<MouseScript>();
            Quaternion q = new Quaternion(0, 0, 0, 1);
            q.eulerAngles = new Vector3(90, 180, 0);
            gameObject.transform.rotation = q;
            gameObject.transform.position = Vector3.zero;

            this.id = index; // this must come after gameObject is created


            gameObject.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            
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
