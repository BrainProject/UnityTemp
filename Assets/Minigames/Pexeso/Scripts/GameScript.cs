using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MinigamePexeso 
{
	public class GameScript : MonoBehaviour 
    {
        /// <summary>
        /// How long to wait before non-matching tiles are flipped back
        /// </summary>
        private float observationTime = 0.5f;

        /// <summary>
        /// not used at the moment
        /// </summary>
	    public GameObject scoreb;
		
	    /// <summary>
        /// determines current resource pack
	    /// </summary>
	    public string resourcePack;

		public GameType currentGame;

	    public GameObject menu;
	    public GameObject resourcePackMenu;

        public GameObject gameTilePrefab;

		public Boolean customPack;

        /// <summary>
        /// path to custom image-sets
        /// </summary>
        private string customResPackPath = "\\CustomImages\\";

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
            Debug.Log("Initializing game");
	        correctPairs = 0;
	        correctPairsDisplay.text = correctPairs.ToString();
	        wrongPairs = 0;
	        wrongPairsDisplay.text = wrongPairs.ToString();
            lastDisplayTime = 0;
	        timerDisplay.text = "0 s";
            winningScore = 0;

            gameStartTime = Time.time;

	        //disable menu object
	        resourcePackMenu.SetActive(false);

	        //initialize game board
            gameTiles = GameTiles.createTiles(rows, columns, gameTilePrefab, "gameTile");

            winningScore = (rows * columns) / 2;
            Debug.Log("Winning score set to: " + winningScore);

            // rotate all tiles
            // add Mover or Flipper script to all tiles
            for (int i = 0; i < columns; i++)
            {
                for (int o = 0; o < rows; o++)
                {
					if(currentGame == GameType.Pexeso)
					{
                        gameTiles[rows * i + o].transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                        gameTiles[rows * i + o].AddComponent("Flipper");
					}
					else
					{
						gameTiles[rows * i + o].AddComponent("Mover");
					}
                }
            }

			if (customPack)
			{
				StartCoroutine(AddCustomPicturesToGameTiles());
			}
			else
			{
				AddPicturesToGameTiles ();
			}
	    }

		private void PexesoGameTurn()
		{
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				GameObject hittedObject = hit.collider.gameObject;
				
				//user clicks by left mouse button and hits tile that can be flipped
				if (Input.GetMouseButtonUp(0) && hit.collider.tag == "gameTile")//
				{
					Flipper flipper = hittedObject.GetComponent("MinigamePexeso.Flipper") as Flipper;
					
					//just flip it 
					flipper.Flip();
					
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
							StartCoroutine(FoundPairPexeso(first, hittedObject));
						}
						
						//second picture is not matching for the first
						else
						{
							wrongPairs++;
							wrongPairsDisplay.text = wrongPairs.ToString();
							
							//flipped both picsture back, after some time
							StartCoroutine(NotFoundPairPexeso(first, hittedObject));
						}
						
						first = null;
					}
				}
			}
		}

		private void SilhouetteGameTurn()
		{
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //user clicks left mouse button and hits picture
                if (Input.GetMouseButtonUp(0) && hit.collider.tag == "gameTile")
                {
                    Mover mover = hit.collider.gameObject.GetComponent("Mover") as Mover;
                    //picture is already being removed
                    if (mover.toRemove)
                    {
                        return;
                    }
                    //this is first selected picture
                    if (first == null)
                    {
                        mover.MoveUp();
                        first = hit.collider.gameObject;
                    }
                    //this is second selected picture
                    else
                    {
                        //second selected picture is the same as first
                        if (first == hit.collider.gameObject)
                        {
                            mover = first.GetComponent("Mover") as Mover;
                            mover.MoveDown();
                        }
                        //second picture is matching for the first
                        else if (checkTilesMatch(first, hit.collider.gameObject))
                        {
                            correctPairs++;
                            correctPairsDisplay.text = correctPairs.ToString();
                            StartCoroutine(FoundPairSilhouette(first, hit.collider.gameObject));
                        }
                        //seond picture is not matching for the first
                        else
                        {
                            wrongPairs++;
                            wrongPairsDisplay.text = wrongPairs.ToString();
                            StartCoroutine(NotFoundPairSilhouette(first, hit.collider.gameObject));
                        }
                        first = null;
                    }
                }
            }
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
	            if(currentGame == GameType.Pexeso)
				{
					PexesoGameTurn();
				}
				else
				{
					SilhouetteGameTurn();
				}
	        }
		}

        private void EndGame()
        {
            Debug.Log("All pairs found... game ends");

            //to avoid calling this method more than once
            winningScore = -1;

            //stop game music and play wictory sound
            AudioSource musicPlayer = GameObject.Find("MusicPlayer").GetComponent("AudioSource") as AudioSource;
            musicPlayer.Stop();
            this.gameObject.audio.Play();

            //game ends here, show scoreboard...
            gameEndTime = Time.time;

			/*scoreb.SetActive (true);
            Scoreboard scoreboard = scoreb.GetComponent<Scoreboard>() as Scoreboard;
			scoreboard.correct.text = correctPairsDisplay.text;
			scoreboard.wrong.text = wrongPairsDisplay.text;
            scoreboard.time.text = (gameEndTime - gameStartTime).ToString();*/

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
            if (currentGame == GameType.Pexeso)
			{
				if (first.transform.GetChild (0).renderer.material.mainTexture.name == second.transform.GetChild (0).renderer.material.mainTexture.name)
				{
					return true;
				}
			}
			else
			{
				if (first.transform.GetChild (0).renderer.material.mainTexture.name + "a" == second.transform.GetChild (0).renderer.material.mainTexture.name || second.transform.GetChild (0).renderer.material.mainTexture.name + "a" == first.transform.GetChild (0).renderer.material.mainTexture.name)
				{
					return true;
				}
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
	    public IEnumerator NotFoundPairPexeso(GameObject first, GameObject second)
	    {
			Flipper flipper1 = first.GetComponent("MinigamePexeso.Flipper") as Flipper;
			Flipper flipper2 = second.GetComponent("MinigamePexeso.Flipper") as Flipper;

			while (flipper1.isMoving || flipper2.isMoving)
	        {
	            yield return new WaitForSeconds(0.1f);
	        }

            //
            yield return new WaitForSeconds(observationTime);

	        flipper1.Flip();
	        flipper2.Flip();
	    }

	    /// <summary>
	    /// COROUTINE. Called when user selected two matching pictures
	    /// </summary>
	    /// <returns></returns>
	    /// <param name="first">First object</param>
	    /// <param name="second">Second object</param>
	    public IEnumerator FoundPairPexeso(GameObject first, GameObject second)
	    {
			Flipper flipper1 = first.GetComponent("MinigamePexeso.Flipper") as Flipper;
			Flipper flipper2 = second.GetComponent("MinigamePexeso.Flipper") as Flipper;
			
            //wait while tiles finishes their flipping
			while (flipper1.isMoving || flipper2.isMoving)
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
	    }

        /// <summary>
        /// COROUTINE. Called when user selected two pictures which do not match.
        /// </summary>
        /// <returns></returns>
        /// <param name="first">First object</param>
        /// <param name="second">Second object</param>
        public IEnumerator NotFoundPairSilhouette(GameObject first, GameObject second)
        {
            Mover mover1 = first.GetComponent("Mover") as Mover;
            Mover mover2 = second.GetComponent("Mover") as Mover;

            mover2.MoveUp();

            while (mover1.isMoving || mover2.isMoving)
            {
                yield return new WaitForSeconds(0.1f);
            }
            mover1.MoveDown();
            mover2.MoveDown();
        }

        /// <summary>
        /// COROUTINE. Called when user selected two pictures which match.
        /// </summary>
        /// <returns></returns>
        /// <param name="first">First object</param>
        /// <param name="second">Second object</param>
        public IEnumerator FoundPairSilhouette(GameObject first, GameObject second)
        {
            Mover mover1 = first.GetComponent("Mover") as Mover;
            Mover mover2 = second.GetComponent("Mover") as Mover;
            mover1.toRemove = true;
            mover2.toRemove = true;

            mover1.MoveUp();
            mover2.MoveUp();

            while (mover1.isMoving || mover2.isMoving)
            {
                yield return new WaitForSeconds(0.1f);
            }

            first.rigidbody.useGravity = true;
            first.rigidbody.isKinematic = false;
            second.rigidbody.useGravity = true;
            second.rigidbody.isKinematic = false;

            first.rigidbody.AddForce(second.transform.position * 100);
            second.rigidbody.AddForce(first.transform.position * 100);

            yield return new WaitForSeconds(1.3f);

            Destroy(first);
            Destroy(second);
        }

		public IEnumerator AddCustomPicturesToGameTiles()
		{
			if (currentGame == GameType.Pexeso)
			{
				List<Texture2D> classicPic = new List<Texture2D> ();
				
				System.Random random = new System.Random ();
				int num;
				
				//Load all pictures
                string[] files = Directory.GetFiles(Environment.CurrentDirectory + customResPackPath + currentGame.ToString() + "\\" + resourcePack + "\\", "*.png");
				images = new UnityEngine.Object[files.Length];
				
				WWW www;
				for(int i = 0; i < files.Length; i++)
				{
					www = new WWW ("file://" + files[i]);
					yield return www;
					images[i] = www.texture;
					images[i].name = i.ToString("00");
				}
				
				if (images == null)
				{
					throw new UnityException ("Some images failed to load");
				}
				
				for (int i = 0; i < images.Length; i++)
				{
					if (images [i] != null)
					{
						classicPic.Add (images [i] as Texture2D);
					}
				}
				
				//Choose randomly appropriate number of pictures
				List<Texture2D> chosen = new List<Texture2D> ();
				for (int i = 0; i < (rows * columns)/2; i++)
				{
					num = random.Next (0, classicPic.Count);
					chosen.Add (classicPic [num]);
					chosen.Add (classicPic [num]);
					classicPic.RemoveAt (num);
				}
				
				//Check if appropriate number of pics have been chosen.
				if (chosen.Count != (rows * columns))
				{
					//This happens when some image fails to load
					throw new UnityException ("Some images failed to load");
				}
				
				//Assign textures to planes.
				for (int i = 0; i < (rows * columns); i++)
				{
					num = random.Next (0, chosen.Count);
					gameTiles[i].transform.GetChild(0).renderer.material.mainTexture = chosen [num];
					chosen.RemoveAt (num);
				}
			}
			else
			{
				List<Texture2D> classicPic = new List<Texture2D>();
				List<Texture2D> classicPic_a = new List<Texture2D>();
				Texture2D tex, texA;
				System.Random random = new System.Random();
				int num;

				string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\Assets\\Minigames\\Pexeso\\Resources\\Textures\\Pictures\\" + currentGame.ToString() + "\\" +  resourcePack + "\\");
				//Load all pictures and their matching silhouettes/similarities.
				int o = 0;
				while(o != -1)
				{
					WWW www = new WWW ("file://" + Environment.CurrentDirectory + "\\Assets\\Minigames\\Pexeso\\Resources\\Textures\\Pictures\\" + currentGame.ToString() + "\\" + resourcePack + "\\" + o.ToString("00") + ".png");
					yield return www;
					WWW wwwA = new WWW ("file://" + Environment.CurrentDirectory + "\\Assets\\Minigames\\Pexeso\\Resources\\Textures\\Pictures\\" + currentGame.ToString() + "\\" + resourcePack + "\\" + o.ToString("00") + "a.png");
					yield return wwwA;

					tex = www.texture;
					texA = wwwA.texture;

					if(www.error != null)
					{
						Debug.Log (www.error);
						tex = null;
					}
					if(wwwA.error != null)
					{
						Debug.Log (wwwA.error);
						texA = null;
					}

					if (tex != null && texA != null)
					{
						tex.name = o.ToString("00");
						texA.name = o.ToString("00") + "a";

						classicPic.Add(tex);
						classicPic_a.Add(texA);
						o++;
					}
					else
					{
						o = -1;
					}
				}
				
				//Choose randomly appropriate number of pictures and their matching silhouettes/similarities.
				List<Texture2D> chosen = new List<Texture2D>();
				for (int i = 0; i < (rows * columns)/2; i++)
				{
					num = random.Next(0, classicPic.Count);
					chosen.Add(classicPic[num]);
					chosen.Add(classicPic_a[num]);
					classicPic.RemoveAt(num);
					classicPic_a.RemoveAt(num);
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
			Debug.Log("Game initialized");
		}

		UnityEngine.Object[] images;
		
	    /// <summary>
	    /// Randomly assigns pictures to game tiles.
	    /// </summary>
		public void AddPicturesToGameTiles()
		{
	        if (currentGame == GameType.Pexeso)
			{
					List<Texture2D> classicPic = new List<Texture2D> ();
					
					System.Random random = new System.Random ();
					int num;
	
					//Load all pictures
					images = Resources.LoadAll ("Textures/Pictures/Pexeso/" + resourcePack);

					//images = Resources.LoadAll ("Textures/Pictures/Pexeso/" + resourcePack);
	
					if (images == null)
					{
							throw new UnityException ("Some images failed to load");
					}
	
					for (int i = 0; i < images.Length; i++)
					{
							if (images [i] != null)
							{
									classicPic.Add (images [i] as Texture2D);
							}
					}
	
					//Choose randomly appropriate number of pictures
					List<Texture2D> chosen = new List<Texture2D> ();
					for (int i = 0; i < (rows * columns)/2; i++)
					{
							num = random.Next (0, classicPic.Count);
							chosen.Add (classicPic [num]);
							chosen.Add (classicPic [num]);
							classicPic.RemoveAt (num);
					}
	
					//Check if appropriate number of pics have been chosen.
					if (chosen.Count != (rows * columns))
					{
							//This happens when some image fails to load
							throw new UnityException ("Some images failed to load");
					}
	
					//Assign textures to planes.
					for (int i = 0; i < (rows * columns); i++)
					{
							num = random.Next (0, chosen.Count);
							gameTiles[i].transform.GetChild(0).renderer.material.mainTexture = chosen [num];
							chosen.RemoveAt (num);
					}
			}
			else
			{
				List<Texture2D> classicPic = new List<Texture2D>();
				List<Texture2D> classicPic_a = new List<Texture2D>();
				Texture2D tex, texA;
				System.Random random = new System.Random();
				int num;
				
				//Load all pictures and their matching silhouettes/similarities.
				int o = 0;
				while(o != -1)
				{
					tex = Resources.Load("Textures/Pictures/" + currentGame.ToString() + "/" + resourcePack + "/" + o.ToString("00")) as Texture2D;
					texA = Resources.Load("Textures/Pictures/" + currentGame.ToString() + "/" + resourcePack + "/" + o.ToString("00") + "a") as Texture2D;
					
					if (tex != null && texA != null)
					{
						classicPic.Add(tex);
						classicPic_a.Add(texA);
						o++;
					}
					else
					{
						o = -1;
					}
				}
				
				//Choose randomly appropriate number of pictures and their matching silhouettes/similarities.
				List<Texture2D> chosen = new List<Texture2D>();
				for (int i = 0; i < (rows * columns)/2; i++)
				{
					num = random.Next(0, classicPic.Count);
					chosen.Add(classicPic[num]);
					chosen.Add(classicPic_a[num]);
					classicPic.RemoveAt(num);
					classicPic_a.RemoveAt(num);
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
            Debug.Log("Game initialized");
		}

        public IEnumerator RestartGame()
        {
            //TODO do not restart automatically - display "end-game GUI" instead

            //wait some time...
            yield return new WaitForSeconds(2f);
            Debug.Log("Restarting game");

            menu.SetActive(true);
            resourcePackMenu.SetActive(true);
            ResourcePack resourceMenuScript = resourcePackMenu.GetComponent("MinigamePexeso.ResourcePack") as ResourcePack;
            this.enabled = false;
            resourceMenuScript.CreateMenu();
        }
	}
}
