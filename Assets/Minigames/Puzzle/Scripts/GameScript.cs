using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Puzzle
{
    public class GameScript : MonoBehaviour
    {
        public int numberPieces = 4;
        public Texture2D puzzleImage;// = Resources.Load ("Pictures/cute-cat") as Texture2D;



        // size of one piece of puzzle
        private const uint CELL_SIZE = 50;

        private const uint CELL_SPACE = 10;

        public HashSet<HashSet<GameObject>> connectedComponents;
        public Dictionary<string, PuzzlePiece> pieces;


        // Use this for initialization
        void Start()
        {
            Debug.Log("Trying to load image texture.");
            // loading image texture
            try
            {
                puzzleImage = Resources.Load("Pictures/" + PlayerPrefs.GetString("Image")) as Texture2D;
                Debug.Log("Image texture Loaded.");
            }
            catch (Exception ex)
            {
                Debug.Log("Exception occured while trying to load image: " + ex.Message);
                Debug.Log("Trying to load Bonobo image");

                puzzleImage = Resources.Load("Pictures/Bonobo") as Texture2D;
                Debug.Log("Bonobo image texture Loaded.");
            }


            Debug.Log("Trying to load number of pieces.");
            // loading number of pieces
            try
            {
                numberPieces = PlayerPrefs.GetInt("size");
                Debug.Log("Number of pieces loaded.");
            }
            catch (Exception ex)
            {
                Debug.Log("Exception occured while trying to get number of pieces: " + ex.Message);
                Debug.Log("Using number 4.");
                numberPieces = 4;
            }

            Debug.Log("Allocating collections.");
            connectedComponents = new HashSet<HashSet<GameObject>>();
            pieces = new Dictionary<string, PuzzlePiece>();


            int dim = (int)Math.Floor(Math.Sqrt(numberPieces));
            numberPieces = dim * dim;


            //double pos_x = -(dim * cell_size + (dim - 1) * cell_space) / 2.0;
            //double pos_y = pos_x; //-(dim * cell_size + (dim-1) * cell_space) / 2.0;

            int pixels_per_cell_x = (int)Math.Floor((double)puzzleImage.width / dim);
            int pixels_per_cell_y = (int)Math.Floor((double)puzzleImage.height / dim);

            // IN CASE EVERY CONNECTION HAD A NUMBER
            //uint num_connections = (uint)(2 * (dim - 1) * dim); // in the end not needed.

            Debug.Log("Starting loops.");
            for (int i = 1; i <= dim; i++)
            {
                for (int j = 1; j <= dim; j++)
                {
                    // Create texture
                    Texture2D texture = new Texture2D(pixels_per_cell_x, pixels_per_cell_y);

                    // this is not correct because [0,0] is bottom left corner and not the top left
                    //Color[] pixels = puzzle_image.GetPixels((j-1)*pix_per_cell_x,(i-1)*pix_per_cell_y,pix_per_cell_x,pix_per_cell_y);

                    Debug.Log("Copying texture pixels.");
                    Color[] pixels = puzzleImage.GetPixels(
                        (j - 1) * pixels_per_cell_x,
                        puzzleImage.height - i * pixels_per_cell_y,
                        pixels_per_cell_x,
                        pixels_per_cell_y);

                    texture.SetPixels(pixels);
                    texture.Apply();
                    Debug.Log("Smaller texture created.");

                    // IN CASE EVERY CONNECTION HAD A NUMBER
                    int dim_x = dim;
                    int dim_y = dim;

                    Debug.Log("Creating Puzzle Piece.");
                    PuzzlePiece piece = new PuzzlePiece(texture,
                                                        i == 1 ? -1 : (dim_x - 1) * (i - 1) + dim_y * (i - 2) + j,
                                                        j == dim_x ? -1 : (dim_x - 1) * (i - 1) + dim_y * (i - 1) + j,
                                                        i == dim_y ? -1 : (dim_x - 1) * i + dim_y * (i - 1) + j,
                                                        j == 1 ? -1 : (dim_x - 1) * (i - 1) + dim_y * (i - 1) + j - 1,
                                                        Vector3.zero);

                    Debug.Log("Puzzle Piece created.");


                   // PuzzlePiece piece = new PuzzlePiece(texture,
                   //                  i == 1 ? -1 : id - (int)dim,
                   //                  j == (int)dim ? -1 : id + 1,
                   //                  i == (int)dim ? -1 : id + (int)dim,
                   //                  j == 1 ? -1 : id - 1);

                    // add to pieces

                    Debug.Log("Setting ID.");

                    piece.id = (int)((i - 1) * dim + j);

                    Debug.Log("ID set.");


                    Debug.Log("Putting in Collections.");

                    pieces.Add(piece.gameObject.name, piece);

                    // add a connected component
                    HashSet<GameObject> pieceSet = new HashSet<GameObject>();
                    pieceSet.Add(piece.gameObject);
                    connectedComponents.Add(pieceSet);




                    double angle = 2 * Math.PI / (Math.Pow(dim, 2));
                    double angle_triangle = (Math.PI / 2.0 - angle / 2.0f);
                    double piece_size = Math.Ceiling(piece.GetPieceSize().magnitude / 2.0);
                    double radius = Math.Ceiling((piece_size - Math.Sin(angle_triangle) * piece_size) / Math.Sin(angle_triangle));
                    Debug.Log("Angle: " + angle * Mathf.Rad2Deg);
                    Debug.Log("Size of the piece: " + piece_size);
                    Debug.Log("Radius: " + radius);

                    Debug.Log("Setting piece position.");

                    piece.SetGameObjectPosition(new Vector3((float)((radius + piece_size) * Math.Cos(angle * piece.id)),
                                                            (float)((radius + piece_size) * Math.Sin(angle * piece.id)),
                                                            0.0f));

                    Debug.Log("position set.");

                }
            }
            Debug.Log("Updating camera position.");

            updateOrthographicCameraPosition();

            Debug.Log("Camera position updated.");


        }

        private void updateOrthographicCameraPosition()
        {
            // get min x and y
            float min_x = Mathf.Infinity;
            float min_y = Mathf.Infinity;
            // get max x and y
            float max_x = Mathf.NegativeInfinity;
            float max_y = Mathf.NegativeInfinity;

            foreach (HashSet<GameObject> pieceComponent in connectedComponents)
            {
                foreach (GameObject piece in pieceComponent)
                {
                    Vector3 piece_min = piece.renderer.bounds.min;
                    Vector3 piece_max = piece.renderer.bounds.max;

                    if (piece_min.x < min_x) min_x = piece_min.x;
                    if (piece_min.y < min_y) min_y = piece_min.y;
                    if (piece_max.x > max_x) max_x = piece_max.x;
                    if (piece_max.y > max_y) max_y = piece_max.y;
                }
            }

            Camera.main.transform.position = new Vector3((min_x + max_x) / 2, (min_y + max_y) / 2, 1);
            Camera.main.orthographicSize = Math.Max(max_x - min_x, max_y - min_y) / 2 + 1;
        }

        private bool CheckVictory()
        {
            return connectedComponents.Count == 1;
        }


        // Update is called once per frame
        void Update()
        {
            updateOrthographicCameraPosition();
            if (CheckVictory())
            {
                ///System.Threading.Thread.Sleep(3000);
                Application.LoadLevel("Victory");
            }
        }




        public void CheckPossibleConnection(GameObject puzzleObject)
        {
            // distance, in which two pieces will be connected.
            const int diff = 5;

            HashSet<GameObject> my_component = null;
            foreach (HashSet<GameObject> component in connectedComponents)
            {
                if (component.Contains(puzzleObject))
                {
                    my_component = component;
                    break;
                }
            }

            HashSet<HashSet<GameObject>> toConnect = new HashSet<HashSet<GameObject>>();
            foreach (GameObject my_component_object in my_component)
            {
                foreach (HashSet<GameObject> other_component in connectedComponents)
                {
                    // components_connected = false;
                    if (other_component == my_component) continue;
                    foreach (GameObject other_component_object in other_component)
                    {
                        PuzzlePiece my_piece = pieces[my_component_object.name];
                        PuzzlePiece other_piece = pieces[other_component_object.name];

                        Vector3 my_piece_position = my_component_object.transform.position;
                        Vector3 other_piece_position = other_component_object.transform.position;

                        Bounds my_piece_bounds = my_component_object.renderer.bounds;
                        Bounds other_piece_bounds = other_component_object.renderer.bounds;

                        // CHECK OTHER ON TOP OF MINE
                        if (my_piece.top != -1 && my_piece.top == other_piece.bottom)
                        {
                            // CHECK DISTANCE
                            if (my_piece_bounds.min.y < other_piece_bounds.min.y &&     // my bound is lower than other
                                Math.Abs(other_piece_bounds.min.y - my_piece_bounds.max.y) < diff &&    // pieces are close vertically
                                Math.Abs(my_piece_position.x - other_piece_position.x) < diff)  // pieces are close horizontally
                            {
                                Vector3 newPosition = new Vector3(
                                    other_piece_position.x,
                                    other_piece_position.y - other_piece_bounds.size.y,
                                    my_piece_position.z);

                                Vector3 moveBy = newPosition - my_component_object.transform.position;

                                foreach(GameObject o in my_component)
                                {
                                    o.transform.position += moveBy;
                                }
                                foreach(HashSet<GameObject> set in toConnect)
                                {
                                    foreach (GameObject o in set)
                                    {
                                        o.transform.position += moveBy;
                                    }
                                }


                                // seems like it does not work
                                //my_component_object.transform.parent = other_component_object.transform;
                                //other_component_object.transform.parent = my_component_object.transform;

                                //components_connected = true;
                                toConnect.Add(other_component);
                                continue;
                            }
                        }

                        // CHECK OTHER ON BOTTOM OF MINE
                        if (my_piece.bottom != -1 && my_piece.bottom == other_piece.top)
                        {
                            // CHECK DISTANCE
                            if (my_piece_bounds.min.y > other_piece_bounds.min.y &&
                                Math.Abs(other_piece_bounds.max.y - my_piece_bounds.min.y) < diff &&
                                Math.Abs(my_piece_position.x - other_piece_position.x) < diff)
                            {

                                Vector3 newPosition = new Vector3(
                                    other_piece_position.x,
                                    other_piece_position.y + other_piece_bounds.size.y,
                                    my_piece_position.z);

                                Vector3 moveBy = newPosition - my_component_object.transform.position;

                                foreach (GameObject o in my_component)
                                {
                                    o.transform.position += moveBy;
                                }
                                foreach (HashSet<GameObject> set in toConnect)
                                {
                                    foreach (GameObject o in set)
                                    {
                                        o.transform.position += moveBy;
                                    }
                                }


                                // seems like it does not work
                                //my_component_object.transform.parent = other_component_object.transform;
                                //other_component_object.transform.parent = my_component_object.transform;

                                //components_connected = true;
                                toConnect.Add(other_component);
                                continue;
                            }

                        }

                        // CHECK OTHER ON LEFT OF MINE
                        if (my_piece.left != -1 && my_piece.left == other_piece.right)
                        {
                            // CHECK DISTANCE
                            if (my_piece_bounds.min.x < other_piece_bounds.min.x &&
                                Math.Abs(other_piece_bounds.min.x - my_piece_bounds.max.x) < diff &&
                                Math.Abs(my_piece_position.y - other_piece_position.y) < diff)
                            {
                                Vector3 newPosition = new Vector3(
                                    other_piece_position.x - other_piece_bounds.size.x,
                                    other_piece_position.y,
                                    my_piece_position.z);

                                Vector3 moveBy = newPosition - my_component_object.transform.position;

                                foreach (GameObject o in my_component)
                                {
                                    o.transform.position += moveBy;
                                }
                                foreach (HashSet<GameObject> set in toConnect)
                                {
                                    foreach (GameObject o in set)
                                    {
                                        o.transform.position += moveBy;
                                    }
                                }


                                // seems like it does not work
                                //my_component_object.transform.parent = other_component_object.transform;
                                //other_component_object.transform.parent = my_component_object.transform;

                                //components_connected = true;
                                toConnect.Add(other_component);
                                continue;
                            }

                        }

                        // CHECK OTHER ON RIGHT OF MINE
                        if (my_piece.right != -1 && my_piece.right == other_piece.left)
                        {
                            // CHECK DISTANCE
                            if (my_piece_bounds.min.x > other_piece_bounds.min.x &&
                                Math.Abs(other_piece_bounds.max.x - my_piece_bounds.min.x) < diff &&
                                Math.Abs(my_piece_position.y - other_piece_position.y) < diff)
                            {
                                Vector3 newPosition = new Vector3(
                                    other_piece_position.x + other_piece_bounds.size.x,
                                    other_piece_position.y,
                                    my_piece_position.z);

                                Vector3 moveBy = newPosition - my_component_object.transform.position;

                                foreach (GameObject o in my_component)
                                {
                                    o.transform.position += moveBy;
                                }
                                foreach (HashSet<GameObject> set in toConnect)
                                {
                                    foreach (GameObject o in set)
                                    {
                                        o.transform.position += moveBy;
                                    }
                                }


                                // seems like it does not work
                                //my_component_object.transform.parent = other_component_object.transform;
                                //other_component_object.transform.parent = my_component_object.transform;

                                //components_connected = true;
                                toConnect.Add(other_component);
                                continue;
                            }
                        }
                    }
                    //if (components_connected)
                    //{
                    //    my_component.UnionWith(other_component);
                    //    connected_components.Remove(other_component);
                    //    components_connected = false;
                    //}
                }
            }
            if (toConnect.Count > 0)
            {
                foreach (HashSet<GameObject> set in toConnect)
                {
                    my_component.UnionWith(set);
                    connectedComponents.Remove(set);
                }
                toConnect.Clear();
            }
        }
    }
}

// TO DO: statistics
// TO DO:  timer
