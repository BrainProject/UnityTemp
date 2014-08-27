//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Puzzle;

//public class GameScript_2 : MonoBehaviour 
//{
//    // number of puzzle cells
//    public int number_pieces = 4;

//    // image to be used
//    public Texture2D puzzle_image;// = Resources.Load ("Pictures/cute-cat") as Texture2D;

//    // size of one piece of puzzle
//    private const uint cell_size = 50;

//    private const uint cell_space = 10;


//    public Dictionary<int,PuzzlePiece> pieces;

//    private float cameraFovy= 60.0f;

//    public HashSet<HashSet<PuzzlePiece>> connected_components;


	
//    // Use this for initialization
//    void Start () 
//    {
//        // loading image texture
//        try
//        {
//            puzzle_image = Resources.Load("Pictures/" + PlayerPrefs.GetString("Image")) as Texture2D;
//        }
//        catch(Exception ex)
//        {
//            puzzle_image = Resources.Load("Pictures/Bonobo") as Texture2D;
//            Debug.Log("Exception occured while trying to load image: " + ex.Message);
//            Debug.Log("Trying to load Bonobo image");
//        }

//        // loading number of pieces
//        try
//        {
//            number_pieces = PlayerPrefs.GetInt("size");
//        }
//        catch(Exception ex)
//        {
//            Debug.Log("Exception occured while trying to get number of pieces: " + ex.Message);
//            Debug.Log("Using number 4.");
//        }

//        pieces = new Dictionary<int,PuzzlePiece >();

//        connected_components = new HashSet<HashSet<PuzzlePiece>>();

//        double dim = Math.Floor (Math.Sqrt (number_pieces));

//        double pos_x = -(dim * cell_size + (dim-1) * cell_space) / 2.0;

//        double pos_y = pos_x; //-(dim * cell_size + (dim-1) * cell_space) / 2.0;

//        int pix_per_cell_x = (int)Math.Floor((double)puzzle_image.width / dim);
//        int pix_per_cell_y = (int)Math.Floor ((double)puzzle_image.height / dim);

//        // IN CASE EVERY CONNECTION HAD A NUMBER
//        //uint num_connections = (uint) (2*(dim-1)*dim);

//        for (int i=1; i<=dim; i++) 
//        {
//            for (int j=1; j<=dim; j++) 
//            {
//                // Create texture
//                Texture2D texture = new Texture2D(pix_per_cell_x,pix_per_cell_y);

//                // this is not correct because [0,0] is bottom left corner and not the top left
//                //Color[] pixels = puzzle_image.GetPixels((j-1)*pix_per_cell_x,(i-1)*pix_per_cell_y,pix_per_cell_x,pix_per_cell_y);


//                Color[] pixels = puzzle_image.GetPixels(
//                    (j-1)*pix_per_cell_x,
//                    puzzle_image.height - i*pix_per_cell_y,
//                    pix_per_cell_x,
//                    pix_per_cell_y);

//                texture.SetPixels( pixels );
//                texture.Apply();

//                // IN CASE EVERY CONNECTION HAD A NUMBER
//                //int dim_x = dim;
//                //int dim_y = dim;
//                //PuzzlePiece piece = new PuzzlePiece(texture,
//                //                                    i==1?-1:(dim_x-1)*(i-1)+dim_y*(i-2)+j,
//                //                                    j== dim_x ? -1 : (dim_x-1)*(i-1)+dim_y*(i-1)+j,
//                //                                    i==dim_y? -1 : (dim_x-1)*i+dim_y*(i-1)+j,
//                //                                    j==1?-1:(dim_x-1)*(i-1)+dim_y*(i-1)+j-1);
				                                    
//                int id = (int)((i-1)*dim + j);

//                PuzzlePiece piece =  new PuzzlePiece(texture,
//                                 i==1?-1:id-(int)dim,
//                                 j== (int)dim ? -1 : id + 1,
//                                 i== (int)dim ? -1 : id + (int)dim,
//                                 j==1 ? -1 : id - 1);

//                piece.setId(id);
				
//                pieces.Add(id,piece);

//                HashSet<PuzzlePiece> pieceSet = new HashSet<PuzzlePiece>();
//                pieceSet.Add(piece);
//                connected_components.Add(pieceSet);

//                piece.createGameObject(new Vector3(0,0,0));


//                double angle =  2*Math.PI/(Math.Pow(dim,2));
//                double angle_triangle = (Math.PI / 2.0 - angle / 2.0f);
//                double piece_size = Math.Ceiling(piece.getPieceSize().magnitude / 2.0);
//                double radius = Math.Ceiling((piece_size - Math.Sin (angle_triangle) * piece_size) / Math.Sin (angle_triangle));
//                Debug.Log("Angle: " + angle*Mathf.Rad2Deg);
//                Debug.Log("Size of the piece: " + piece_size);
//                Debug.Log("Radius: " + radius);


                
//                piece.setGameObjectPosition(new Vector3((float)((radius+piece_size) * Math.Cos (angle * id)),
//                                                        (float)((radius+piece_size)*Math.Sin(angle * id)),
//                                                        0.0f));
//            }
//        }
//        updateOrthographicCameraPosition();

//    }

//    #region Perspective camera - not used
//    /*private void updatePerspectiveCameraPosition()
//    {
//        // get min x and y
//        float min_x = Mathf.Infinity;
//        float min_y = Mathf.Infinity;
//        float min_z = Mathf.Infinity;
//        // get max x and y
//        float max_x = Mathf.NegativeInfinity;
//        float max_y = Mathf.NegativeInfinity;
//        float max_z = Mathf.NegativeInfinity;


//        foreach(PuzzlePiece piece in pieces.Values)
//        {
//            Vector3 piece_min = piece.getPieceBounds().min;
//            Vector3 piece_max = piece.getPieceBounds().max;

//            if (piece_min.x < min_x)  min_x = piece_min.x;
//            if (piece_min.y < min_y)  min_y = piece_min.y;
//            if (piece_max.x > max_x)  max_x = piece_max.x;
//            if (piece_max.y > max_y)  max_y = piece_max.y;

//            if (piece_min.z < min_z)  min_z = piece_min.z;
//            if (piece_max.z > max_z)  max_z = piece_max.z;
//        }

//        double length = Math.Sqrt(Math.Pow(max_x-min_x,2) + Math.Pow(max_y - min_y,2) + Math.Pow(max_z - min_z,2) );


//        Camera.main.fieldOfView = cameraFovy;
		
//        double distance = (length / 2.0) / Math.Atan(cameraFovy);
		
//        Camera.main.transform.position = new Vector3(0,0,(float)distance*1.1f);
//        Camera.main.farClipPlane = (float)distance * 2.0f;
//    }*/
//    #endregion

//    private void updateOrthographicCameraPosition()
//    {
//        // get min x and y
//        float min_x = Mathf.Infinity;
//        float min_y = Mathf.Infinity;
//        // get max x and y
//        float max_x = Mathf.NegativeInfinity;
//        float max_y = Mathf.NegativeInfinity;

		
//        foreach(PuzzlePiece piece in pieces.Values)
//        {
//            Vector3 piece_min = piece.getPieceBounds().min;
//            Vector3 piece_max = piece.getPieceBounds().max;
			
//            if (piece_min.x < min_x)  min_x = piece_min.x;
//            if (piece_min.y < min_y)  min_y = piece_min.y;
//            if (piece_max.x > max_x)  max_x = piece_max.x;
//            if (piece_max.y > max_y)  max_y = piece_max.y;
//        }

//        Camera.mainCamera.transform.position = new Vector3((min_x + max_x )/2,(min_y+max_y)/2,1);

//        Camera.main.orthographicSize = Math.Max(max_x-min_x, max_y - min_y)/2 + 1;	
//    }

//    private bool checkVictory()
//    {
//        bool ret = true;
//        foreach(PuzzlePiece piece in pieces.Values)
//        {
//            ret = ret && piece.isWon();
//        }

//        return ret;
//        //return connected_components.Count == 1;
//    }



//    // Update is called once per frame
//    void Update () 
//    {
//        updateOrthographicCameraPosition();

//        if(checkVictory())
//        {
//            Application.LoadLevel("Victory");
//        }
//    }	
//}



//// TO DO: statistics
//// TO DO: main screen - menu and jump to this.
//// TO DO: skybox and timer
//// TO DO: image to build.