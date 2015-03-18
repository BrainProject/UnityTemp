/**
 *@file GameScript.cs
 *@author Ján Bella
 *
 * Main game script for Puzzle minigame
 **/
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Puzzle
{
    /// <summary>
    /// Main game script for Puzzle mini-game
    /// </summary>
    public class GameScript : MonoBehaviour
    {
		/// <summary>
        /// number of puzzle pieces
		/// </summary>
 
        public int numberPieces = 4;

		/// <summary>
        /// texture of image to complete in puzzle 
		/// </summary>
 
        public Texture2D puzzleImage;

		/// <summary>
        /// A set of connected components (connected component is a set of puzzle pieces, 
        /// there are used just their gameObjects
		/// </summary>
        public HashSet<HashSet<GameObject>> connectedComponents;

		/// <summary>
		/// dictionary of all pieces of puzzle. Name used is ID of piece. Dictionary is used
		///to make it faster to get demanded piece
		/// </summary>
        public Dictionary<string, PuzzlePiece> pieces;

		/// <summary>
        /// indicates whether game was won or not
		/// </summary>

        public Image targetImage;

		bool gameWon = false;

        /**
         * Unity Engine method for initialisation
         * 
         * In this method:
         *  - getting proposed number of pieces for puzzle game from Resources
         *  - getting proposed image texture from Resources
         *  - adding guiTexture of target image into the scene
         *  - cutting texture to smaller pieces
         *  - creating puzzlePieces, for each also a connected component
         *  -- their initialisation as well, asigning neighbourhood numbers to edges
         *  -- placing pieces into the scene randomly
         *  - setting up camera
         *  - start to record statistics
         */
        void Start()
        {
			gameWon = false;

            // loading image texture
            try
            {
                puzzleImage = Resources.Load("Pictures/" + PlayerPrefs.GetString("Image")) as Texture2D;
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

            Sprite testSprite = Sprite.Create(puzzleImage, new Rect(0, 0, puzzleImage.width, puzzleImage.height), new Vector2(0.5f, 0.5f));
            targetImage.sprite = testSprite;

            //  loading number of pieces
            try
            {
                numberPieces = PlayerPrefs.GetInt("size");
                PuzzleStatistics.numberPieces = numberPieces;
            }
            catch (Exception ex)
            {
                Debug.Log("Exception occured while trying to get number of pieces: " + ex.Message);
                Debug.Log("Using number 4.");
                numberPieces = 4;
                PuzzleStatistics.numberPieces = numberPieces;
            }

            connectedComponents = new HashSet<HashSet<GameObject>>();
            pieces = new Dictionary<string, PuzzlePiece>();

            int dim = (int)Math.Floor(Math.Sqrt(numberPieces));
            numberPieces = dim * dim;

            bool[] x = new bool[dim];
            bool[] y = new bool[dim];
            for (int i = 0; i < dim; i++)
            {
                x[i] = false; y[i] = false;
            }
            
            for (int i = 0; i < numberPieces; i++)
            {
            
                PuzzlePiece piece = new PuzzlePiece(puzzleImage,i,dim,dim);
                pieces.Add(piece.gameObject.name, piece);

                // add a connected component
                HashSet<GameObject> pieceSet = new HashSet<GameObject>();
                pieceSet.Add(piece.gameObject);
                connectedComponents.Add(pieceSet);
            }

            placePuzzlePieces();

            setOrthographicCamera();

            PuzzleStatistics.Clear();
            PuzzleStatistics.StartMeasuringTime();
			MGC.Instance.minigameStates.SetPlayed (Application.loadedLevelName);
			//MGC.Instance.SaveGame ();
        }

		/**
		 * Sets othographic camera up.
		 */
		private void setOrthographicCamera()
		{
			// get min x and y
			float min_x = float.MaxValue;
			float min_y = float.MaxValue;
			// get max x and y
			float max_x = float.MinValue;
			float max_y = float.MinValue;
			
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
			
			Camera.main.transform.position = new Vector3((min_x + max_x) / 2.25f, (min_y + max_y) / 2.0f, -10);
			Camera.main.orthographicSize = Math.Max(max_x - min_x, max_y - min_y)/1.5f;
		}

		/**
		 * Checks for victory. Does not perform any routines to handle endgame.
		 * @return whether game is won
		 */
        private bool CheckVictory()
        {
            return connectedComponents.Count == 1;
        }

        /**
         * Unity Engine update method.
         * Checks for endgame and performs endgame routines.
         */
        void Update()
        {
            if (CheckVictory() && !gameWon)
            {
				gameWon = true;
                PuzzleStatistics.StopMeasuringTime();
				//MGC.Instance.sceneLoader.LoadScene("PuzzleVictory");
				EndGame();
            }
        }


		/**
		 * Method performing update of connected components. For given gameobject, checks 
		 * whether its neighbours are near and if so, connects them - updates their positions
		 * and puts them in one connected component.
		 * 
		 * @param puzzleObject puzzle piece for which should be connection checking performed
		 * @return true in case of connection at least two pieces, false otherwise
		 */
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

			PuzzlePiece my_piece = pieces[puzzleObject.name];
			//Debug.Log ("ID: " + puzzleObject.name);
			//Debug.Log ("top: " + my_piece.top + ", bottom: " + my_piece.bottom + ", left: " + my_piece.left + ", right: " + my_piece.right);

			PuzzlePiece topPiece = my_piece.top > 0 ? pieces[my_piece.top.ToString()] : null;
			PuzzlePiece bottomPiece = my_piece.bottom > 0 ? pieces[my_piece.bottom.ToString()] : null;
			PuzzlePiece leftPiece = my_piece.left > 0 ? pieces[my_piece.left.ToString()] : null;
			PuzzlePiece rightPiece = my_piece.right > 0 ? pieces[my_piece.right.ToString()] : null;

			HashSet<GameObject> top_component = null;
			HashSet<GameObject> bottom_component = null;
			HashSet<GameObject> left_component = null;
			HashSet<GameObject> right_component = null;
			bool[] set = new bool[4];

			foreach (HashSet<GameObject> component in connectedComponents)
			{
				if (!set[0] && topPiece!=null && component.Contains(topPiece.gameObject))
				{
					set[0] = true;
					top_component = component;
				}
				if (!set[1] && bottomPiece!=null && component.Contains(bottomPiece.gameObject))
				{
					set[1] = true;
					bottom_component = component;
				}
				if (!set[2] && leftPiece!=null && component.Contains(leftPiece.gameObject))
				{
					set[2] = true;
					left_component = component;
				}
				if (!set[3] && rightPiece!=null && component.Contains(rightPiece.gameObject))
				{
					set[3] = true;
					right_component = component;
				}
			}
			
			
			// to connect connected components
			HashSet<HashSet<GameObject>> toConnect = new HashSet<HashSet<GameObject>>();

            if (topPiece != null && my_component!=top_component)
            {
                // CHECK DISTANCE
               if (my_piece.gameObject.renderer.bounds.min.y < topPiece.gameObject.renderer.bounds.min.y &&     // my bound is lower than other
				    Math.Abs(topPiece.gameObject.renderer.bounds.min.y - my_piece.gameObject.renderer.bounds.max.y) < diff &&    // pieces are close vertically
                    Math.Abs(my_piece.gameObject.transform.position.x - topPiece.gameObject.transform.position.x) < diff)  // pieces are close horizontally
                {
                  	Vector3 newPosition = new Vector3(
						topPiece.gameObject.transform.position.x,
						topPiece.gameObject.transform.position.y - topPiece.gameObject.renderer.bounds.size.y,
						my_piece.gameObject.transform.position.z);

					Vector3 moveBy = newPosition - my_piece.gameObject.transform.position;

                    foreach (GameObject o in my_component)
                    {
                    	o.transform.position += moveBy;
                    }
					foreach (HashSet<GameObject> component in toConnect)
					{
						foreach (GameObject o in component)
						{
							o.transform.position += moveBy;
                        }
                    }

					toConnect.Add(top_component);
				}
            }
			if (bottomPiece != null && my_component!=bottom_component)
			{
				// CHECK DISTANCE
				if (my_piece.gameObject.renderer.bounds.min.y > bottomPiece.gameObject.renderer.bounds.min.y &&     // my bound is lower than other
				    Math.Abs(bottomPiece.gameObject.renderer.bounds.max.y - my_piece.gameObject.renderer.bounds.min.y) < diff &&    // pieces are close vertically
				    Math.Abs(my_piece.gameObject.transform.position.x - bottomPiece.gameObject.transform.position.x) < diff)  // pieces are close horizontally
				{
					Vector3 newPosition = new Vector3(
						bottomPiece.gameObject.transform.position.x,
						bottomPiece.gameObject.transform.position.y + bottomPiece.gameObject.renderer.bounds.size.y,
						my_piece.gameObject.transform.position.z);
					
					Vector3 moveBy = newPosition - my_piece.gameObject.transform.position;
					
					foreach (GameObject o in my_component)
					{
						o.transform.position += moveBy;
					}
					foreach (HashSet<GameObject> component in toConnect)
					{
						foreach (GameObject o in component)
						{
							o.transform.position += moveBy;
						}
					}

					toConnect.Add(bottom_component);
				}
			}
			if (leftPiece != null && my_component!=left_component)
			{
				// CHECK DISTANCE
				if (my_piece.gameObject.renderer.bounds.min.x > leftPiece.gameObject.renderer.bounds.min.x &&     // my bound is lower than other
				    Math.Abs(leftPiece.gameObject.renderer.bounds.max.x - my_piece.gameObject.renderer.bounds.min.x) < diff &&    // pieces are close vertically
				    Math.Abs(my_piece.gameObject.transform.position.y - leftPiece.gameObject.transform.position.y) < diff)  // pieces are close horizontally
				{
					Vector3 newPosition = new Vector3(
						leftPiece.gameObject.transform.position.x + leftPiece.gameObject.renderer.bounds.size.x,
						leftPiece.gameObject.transform.position.y,
						my_piece.gameObject.transform.position.z);
					
					Vector3 moveBy = newPosition - my_piece.gameObject.transform.position;
					
					foreach (GameObject o in my_component)
					{
						o.transform.position += moveBy;
					}
					foreach (HashSet<GameObject> component in toConnect)
					{
						foreach (GameObject o in component)
						{
							o.transform.position += moveBy;
						}
					}

					toConnect.Add(left_component);
				}
			}
			if (rightPiece != null && my_component!=right_component)
			{
				// CHECK DISTANCE
				if (my_piece.gameObject.renderer.bounds.min.x < rightPiece.gameObject.renderer.bounds.min.x &&     // my bound is lower than other
				    Math.Abs(rightPiece.gameObject.renderer.bounds.min.x - my_piece.gameObject.renderer.bounds.max.x) < diff &&    // pieces are close vertically
				    Math.Abs(my_piece.gameObject.transform.position.y - rightPiece.gameObject.transform.position.y) < diff)  // pieces are close horizontally
				{
					Vector3 newPosition = new Vector3(
						rightPiece.gameObject.transform.position.x - rightPiece.gameObject.renderer.bounds.size.x,
						rightPiece.gameObject.transform.position.y,
						my_piece.gameObject.transform.position.z);
					
					Vector3 moveBy = newPosition - my_piece.gameObject.transform.position;
					
					foreach (GameObject o in my_component)
					{
						o.transform.position += moveBy;
					}
					foreach (HashSet<GameObject> component in toConnect)
					{
						foreach (GameObject o in component)
						{
							o.transform.position += moveBy;
						}
					}

					toConnect.Add(right_component);
				}
			}
            
            if (toConnect.Count > 0)
            {
                PuzzleStatistics.RegisterClickWithConnection();
				foreach (HashSet<GameObject> component in toConnect)
				{
					my_component.UnionWith(component);
					connectedComponents.Remove(component);
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

        /**
         * Randomly places puzzle pieces (stored in pieces Dictionarary) in virtual grid 
         * in the scene.
         * 
         * Quite time-consuming, but reliable
         */
        private void placePuzzlePieces()
        {
            int dim = (int)Math.Floor(Math.Sqrt(numberPieces));
            
			System.Random random = new System.Random();

			bool[] occupied = new bool[numberPieces];

            foreach (PuzzlePiece piece in pieces.Values)
            {
                int pos = random.Next(numberPieces);
                
                while (occupied[pos])
                {
                    pos++;
                    if (pos >= numberPieces)
                    {
                        pos = 0;
                    }
                }
                occupied[pos] = true;
				float pieceSize = (float)Math.Ceiling(piece.gameObject.renderer.bounds.size.magnitude / 2.0f);
				piece.gameObject.transform.position = new Vector3(2 * (pos / dim) * pieceSize,
				                                                  2 * (pos % dim) * pieceSize,
				                                                  1);
            }
        }


		/**
		 * Action to perform when game is over - animating neuron and showing gui
		 */
		private void EndGame()
		{
			//animate Neuron
			MGC.Instance.neuronHelp.GetComponent<Game.BrainHelp>().ShowSmile(Resources.Load("Neuron/smilyface") as Texture);
			
			MGC.Instance.minigamesGUI.show(false,true,"PuzzleChoosePicture");
		}
    }
}