using UnityEngine;
using System;
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

		double piece_size;

        // Use this for initialization
        void Start()
        {
            Debug.Log("Trying to load image texture.");
            // loading image texture
            try
            {
                puzzleImage = Resources.Load("Pictures/" + PlayerPrefs.GetString("Image")) as Texture2D;
                Debug.Log("Image texture Loaded.");
                PuzzleStatistics.pictureName = PlayerPrefs.GetString("Image");

            }
            catch (Exception ex)
            {
                Debug.Log("Exception occured while trying to load image: " + ex.Message);
                Debug.Log("Trying to load Bonobo image");

                puzzleImage = Resources.Load("Pictures/Bonobo") as Texture2D;
                Debug.Log("Bonobo image texture Loaded.");
                PuzzleStatistics.pictureName = "Bonobo";
            }

			GameObject targetImageGUITexture = new GameObject("TargetImage");

			targetImageGUITexture.AddComponent("GUITexture");

			targetImageGUITexture.guiTexture.texture = puzzleImage;

			targetImageGUITexture.transform.localScale = 
				new Vector3(0.3f,
				            0.3f * Screen.width/Screen.height,
				            1.0f);

			targetImageGUITexture.transform.position = 
				new Vector3(0.15F,
				            1.0f - targetImageGUITexture.transform.localScale.y/2.0f,
				            1.0f);

			
			
			
            Debug.Log("Trying to load number of pieces.");
            // loading number of pieces
            try
            {
                numberPieces = PlayerPrefs.GetInt("size");
                Debug.Log("Number of pieces loaded.");
                PuzzleStatistics.numberPieces = numberPieces;
            }
            catch (Exception ex)
            {
                Debug.Log("Exception occured while trying to get number of pieces: " + ex.Message);
                Debug.Log("Using number 4.");
                numberPieces = 4;
                PuzzleStatistics.numberPieces = numberPieces;
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
            bool[] x = new bool[dim];
            bool[] y = new bool[dim];
            for (int i = 0; i < dim; i++)
            {
                x[i] = false; y[i] = false;
            }
            System.Random random = new System.Random();
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
					                                    j == 1 ? -1 : (dim_x - 1) * (i - 1) + dim_y * (i - 1) + j - 1,
                                                        i == dim_y ? -1 : (dim_x - 1) * i + dim_y * (i - 1) + j,
					                                    j == dim_x ? -1 : (dim_x - 1) * (i - 1) + dim_y * (i - 1) + j,
					                                    Vector3.zero);

                    Debug.Log("Puzzle Piece created.");

                    // add to pieces

                    Debug.Log("Setting ID.");

                    piece.id = (int)((i - 1) * dim + j);

                    Debug.Log("ID set.");


                    Debug.Log("Putting in Collections.");

                    pieces.Add(piece.gameObject.name, piece);

                    Debug.Log("GameObject name: " + piece.gameObject.name);


                    // add a connected component
                    HashSet<GameObject> pieceSet = new HashSet<GameObject>();
                    pieceSet.Add(piece.gameObject);
                    connectedComponents.Add(pieceSet);

					piece_size = Math.Ceiling(piece.GetPieceSize().magnitude / 2.0);

                    piece.SetGameObjectPosition(new Vector3(2 * i * (float)piece_size,
                                                            2 * j * (float)piece_size,
                                                            0.0f));

                }
                // placement in grid
                //placePuzzlePieces();

            }
            Debug.Log("Updating camera position.");

            updateOrthographicCameraPosition();

            Debug.Log("Camera position updated.");

            PuzzleStatistics.Clear();
            PuzzleStatistics.StartMeasuringTime();
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

            Camera.main.transform.position = new Vector3((min_x + max_x) / 2 - 15, (min_y + max_y) / 2, -10);
            Camera.main.orthographicSize = Math.Max(max_x - min_x, max_y - min_y) / 2 + 7;
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
                PuzzleStatistics.StopMeasuringTime();
                Application.LoadLevel("PuzzleVictory");
            }
        }


        public bool CheckPossibleConnection(GameObject puzzleObject)
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
                PuzzleStatistics.RegisterClickWithConnection();
                foreach (HashSet<GameObject> set in toConnect)
                {
                    my_component.UnionWith(set);
                    connectedComponents.Remove(set);
                }
                toConnect.Clear();
				return true;
            }
            else 
			{
				PuzzleStatistics.RegisterClickWithoutConnection();
				return false;
			}
        }

        // did not work, do not know why... 
        private void placePuzzlePieces()
        {

            int dim = (int)Math.Floor(Math.Sqrt(numberPieces));
            Debug.Log("Dim: " + dim);
            Debug.Log("numberPieces: " + numberPieces);
            GameObject[] objects = new GameObject[numberPieces];
            Debug.Log("objects size: " + objects.Length);
            System.Random random = new System.Random();

            Debug.Log("pieces values count: " + pieces.Values.Count);

            foreach (PuzzlePiece piece in pieces.Values)
            {
                int pos = random.Next(numberPieces);
                Debug.Log("generated pos: " + pos);
                while (objects[pos] != null)
                {
                    pos++;
                    if (pos >= numberPieces)
                    {
                        pos = 0;
                    }
                    Debug.Log("pos updated: " + pos);
                }
                Debug.Log("pos: ");
                objects[pos] = piece.gameObject;
                Debug.Log("number pieces: " + pos);

            }

            for (int i = 0; i < numberPieces; i++)
            {
                //Debug.Log("for loop: " + i);

                int x = i / dim;
                int y = i % dim;

                //Debug.Log("(x,y) = (" + x + ", " + y + ")");


                // suppose that pieces are squared all of the same size
                //float size = objects[i].renderer.bounds.size.x;

                objects[i].transform.position = new Vector3(2 * x * CELL_SIZE,// - (Screen.width / 2),
                                                            2 * y * CELL_SIZE,// - (Screen.height / 2),
                                                            0);

                Debug.Log("trans. set");
            }
        }


		public float GetMinPiecePositionX(GameObject excludeComponent = null)
		{
			float min = float.MaxValue;
			foreach(HashSet<GameObject> component in connectedComponents)
			{
				if(component.Contains(excludeComponent))
				{
					continue;
				}
				else
				{
					foreach(GameObject piece in component)
					{
						if(piece.transform.position.x < min)
						{
							min = piece.gameObject.transform.position.x;
						}
					}
				}
			}
			return min;
		}

		public float GetMaxPiecePositionX(GameObject excludeComponent = null)
		{
			float max = float.MinValue;
			foreach(HashSet<GameObject> component in connectedComponents)
			{
				if(component.Contains(excludeComponent))
				{
					continue;
				}
				else
				{
					foreach(GameObject piece in component)
					{
						if(piece.transform.position.x > max)
						{
							max = piece.gameObject.transform.position.x;
						}
					}
				}
			}
			return max;
		}

		public float GetMinPiecePositionY(GameObject excludeComponent = null)
		{
			float min = float.MaxValue;
			foreach(HashSet<GameObject> component in connectedComponents)
			{
				if(component.Contains(excludeComponent))
				{
					continue;
				}
				else
				{
					foreach(GameObject piece in component)
					{
						if(piece.transform.position.y < min)
						{
							min = piece.gameObject.transform.position.y;
						}
					}
				}
			}
			return min;
		}

		public float GetMaxPiecePositionY(GameObject excludeComponent = null)
		{
			float max = float.MinValue;
			foreach(HashSet<GameObject> component in connectedComponents)
			{
				if(component.Contains(excludeComponent))
				{
					continue;
				}
				else
				{
					foreach(GameObject piece in component)
					{
						if(piece.gameObject.transform.position.y > max)
						{
							max = piece.transform.position.y;
						}
					}
				}
			}
			return max;
		}
    }
}