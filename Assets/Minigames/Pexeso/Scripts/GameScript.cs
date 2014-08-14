using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MinigamePexeso 
{
	public class GameScript : MonoBehaviour 
    {
        /// <summary>
        /// How long to wait before non-matching tiles are flipped back
        /// </summary>
        public float observationTime = 0.5f;

        /// <summary>
        /// not used at the moment
        /// </summary>
	    public Scoreboard scoreboard;
		
	    /// <summary>
        /// determines current resource pack
	    /// </summary>
	    public string resourcePack;

	    public GameObject menu;
	    public GameObject resourcePackMenu;

        public GameObject gameTilePrefab;

        /// <summary>
        /// array of tiles
        /// </summary>
        private GameObject[] gameTiles;

	    //Number of rows.
	    public int rows = 4;
	    
        //number of columns.
	    public int columns = 4;

        /// <summary>
        /// used as score
        /// </summary>
	    private int correctPairs;
	    public GUIText correctPairsDisplay;
	    private int wrongPairs;
	    public GUIText wrongPairsDisplay;

	    /// <summary>
	    /// GUIText for timer.
	    /// </summary>
	    public GUIText timerDisplay;

        
        private Ray ray;
        private RaycastHit hit;
        private GameObject first;

        /// <summary>
        /// If correctPairs variable reaches this value, game successfully ends
        /// </summary>
        private int winningScore;

        private float gameStartTime;
        private float gameEndTime;
        private int   lastDisplayTime = 0;

		/// <summary>
	    /// Initialization. Creates all cubes and planes, sets their position etc.
	    /// </summary>
		void Start ()
		{
	        //CreateGameBoard();
		}

	    public void CreateGameBoard()
	    {
	        //set everything to default values
	        correctPairs = 0;
	        correctPairsDisplay.text = correctPairs.ToString();
	        wrongPairs = 0;
	        wrongPairsDisplay.text = wrongPairs.ToString();
	        //time = 0;
            lastDisplayTime = 0;
	        timerDisplay.text = "0 s";
            winningScore = 0;

            gameStartTime = Time.time;

	        //disable menu object
	        resourcePackMenu.SetActive(false);

	        //initialize game board
            gameTiles = GameTiles.createTiles(rows, columns, gameTilePrefab, "gameTile");

            winningScore = (rows * columns) / 2;
            //print("Winning score set to: " + winningScore);

            // rotate all tiles
            // add Mover script to all tiles
            for (int i = 0; i < columns; i++)
            {
                for (int o = 0; o < rows; o++)
                {
                    gameTiles[rows * i + o].transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                    
                    //TODO what about namespaces here?
                    gameTiles[rows * i + o].AddComponent("Mover");
                }
            }

	        AddPicturesToGameTiles ();
	    }



	    /// <summary>
	    /// Infinite loop.
	    /// </summary>
		void Update()
		{
	        //update GUI text once per second
            //TODO event better solution would be to use coroutine with 
            //     yield return new WaitForSeconds(1f);
            float elapsedTime = Time.time - gameStartTime;
            int displayTime = (int)Math.Floor(elapsedTime);
            if (displayTime > lastDisplayTime)
	        {
	            timerDisplay.text = displayTime + " s";
                lastDisplayTime = displayTime;
	        }

	        //check winning condition
	        if (correctPairs == winningScore)
	        {
                EndGame();
	        }
	        
            //normal game turn
	        else
	        {
	            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	            if (Physics.Raycast(ray, out hit))
	            {
                    GameObject hittedObject = hit.collider.gameObject;

	                //user clicks by left mouse button and hits tile that can be flipped
                    if (Input.GetMouseButtonUp(0) && hit.collider.tag == "gameTile")//
	                {
                        Mover mover = hittedObject.GetComponent("MinigamePexeso.Mover") as Mover;

                        //just flip it 
                        mover.Flip();

	                    // is this the first flipped picture?
	                    if (first == null)
	                    {
                            first = hittedObject;
                            //print("First turned tile: " + first.name);
						}

						// one tile was already flipped, user clicked on another one
						else
						{
							// check if images are matching...
                            if (checkTilesMatch(first, hittedObject))
	                        {
                                //increase score and display it
	                            correctPairs++;
	                            correctPairsDisplay.text = correctPairs.ToString();

                                //run animation of removing flipped pair
                                StartCoroutine(FoundPair(first, hittedObject));
							}

						    //second picture is not matching for the first
	                        else
	                        {
	                            wrongPairs++;
	                            wrongPairsDisplay.text = wrongPairs.ToString();

                                //flipped both picsture back, after some time
                                StartCoroutine(NotFoundPair(first, hittedObject));
							}

							first = null;
	                    }
	                }
	            }
	        }
		}

        private void EndGame()
        {
            print("All pairs found... game ends");

            //to avoid calling this method more than once
            winningScore = -1;

            //stop game music and play wictory sound
            AudioSource musicPlayer = GameObject.Find("MusicPlayer").GetComponent("AudioSource") as AudioSource;
            musicPlayer.Stop();
            this.gameObject.audio.Play();

            //game ends here, show scoreboard...
            gameEndTime = Time.time;
            //GameObject go = Instantiate(Resources.Load("Scoreboard")) as GameObject;
            //Scoreboard scoreboard = go.GetComponent<Scoreboard>() as Scoreboard;
            //scoreboard.correct = correctPairsDisplay;
            //scoreboard.wrong = wrongPairsDisplay;
            scoreboard.time.text = (gameEndTime - gameStartTime).ToString();

            StartCoroutine(RestartGame());
        }

	    /// <summary>
        /// Returns true, if pictures of given tiles are matching.
	    /// </summary>
        /// <returns>true, if pictures of given tiles are matching.</returns>
	    /// <param name="tex1">First tile</param>
	    /// <param name="tex2">Second tile</param>
	    public bool checkTilesMatch(GameObject first, GameObject second)
	    {
            if (first.transform.GetChild(0).renderer.material.mainTexture.name == second.transform.GetChild(0).renderer.material.mainTexture.name)
	        {
	            return true;
	        }
	        return false;
	    }

	    /// <summary>
	    /// COROUTINE. Called when user selected two tiles which different pictures
	    /// </summary>
        /// Wait until they are fully flipped up,
        /// wait some time, so user can observe the tiles
        /// flip them down
	    /// <param name="first">First game tile</param>
	    /// <param name="second">Second game tile</param>
	    public IEnumerator NotFoundPair(GameObject first, GameObject second)
	    {
            Mover mover1 = first.GetComponent("MinigamePexeso.Mover") as Mover;
            Mover mover2 = second.GetComponent("MinigamePexeso.Mover") as Mover;

			while (mover1.isMoving || mover2.isMoving)
	        {
	            yield return new WaitForSeconds(0.1f);
	        }

            //
            yield return new WaitForSeconds(observationTime);

	        mover1.Flip();
	        mover2.Flip();

	        //yield return 0;
	    }

	    /// <summary>
	    /// COROUTINE. Called when user selected two matching pictures
	    /// </summary>
	    /// <returns></returns>
	    /// <param name="first">First object</param>
	    /// <param name="second">Second object</param>
	    public IEnumerator FoundPair(GameObject first, GameObject second)
	    {
            //print("FoundPair");

            Mover mover1 = first.GetComponent("MinigamePexeso.Mover") as Mover;
            Mover mover2 = second.GetComponent("MinigamePexeso.Mover") as Mover;
			
            //wait while tiles finishes their flipping
			while (mover1.isMoving || mover2.isMoving)
			{
				yield return new WaitForSeconds(0.1f);
			}
			
            //set up physics to animate fall of tiles
			first.rigidbody.useGravity = true;
            first.rigidbody.isKinematic = false;
	        second.rigidbody.useGravity = true;
            second.rigidbody.isKinematic = false;

	        first.rigidbody.AddForce(new Vector3(0,1,-5) * 100);
			second.rigidbody.AddForce(new Vector3(0,1,-5) * 100);

            //wait until tiles are out of view, then destroy them
	        yield return new WaitForSeconds(1.3f);
	        Destroy(first);
	        Destroy(second);

	        //yield return 0;
	    }
		
	    /// <summary>
	    /// Randomly assigns pictures to game tiles.
	    /// </summary>
		public void AddPicturesToGameTiles()
		{
	        List<Texture2D> classicPic = new List<Texture2D>();
	        UnityEngine.Object[] images;
	        System.Random random = new System.Random();
	        int num;

            //print("resPack to load: '" + resourcePack + "'");

	        //Load all pictures
			images = Resources.LoadAll("Textures/Pictures/" + resourcePack);

			if (images == null)
			{
				throw new UnityException("Some images failed to load");
			}

			for(int i = 0; i < images.Length; i++)
			{
				if(images[i] != null)
				{
					classicPic.Add (images[i] as Texture2D);
				}
			}

            print("Loaded picture count: " + images.Length);

	        //Choose randomly appropriate number of pictures
	        List<Texture2D> chosen = new List<Texture2D>();
	        for (int i = 0; i < (rows * columns)/2; i++)
	        {
	            num = random.Next(0, classicPic.Count);
	            chosen.Add(classicPic[num]);
	            chosen.Add(classicPic[num]);
	            classicPic.RemoveAt(num);
	        }

	        //Check if appropriate number of pics have been chosen.
	        if (chosen.Count != (rows * columns))
	        {
	            //This happens when some image fails to load
	            throw new UnityException("Some images failed to load");
	        }

	        //Assign textures to planes.
	        for (int i = 0; i < (rows * columns); i++)
	        {
	            num = random.Next(0, chosen.Count);
                gameTiles[i].transform.GetChild(0).renderer.material.mainTexture = chosen[num];
	            chosen.RemoveAt(num);
	        }
		}

        public IEnumerator RestartGame()
        {
            
            //wait some time...
            yield return new WaitForSeconds(2f);
            print("Restarting game...");

            menu.SetActive(true);
            resourcePackMenu.SetActive(true);
            ResourcePack resourceMenuScript = resourcePackMenu.GetComponent("MinigamePexeso.ResourcePack") as ResourcePack;
            this.enabled = false;
            resourceMenuScript.CreateMenu();
        }
	}
}
