using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MinigamePexeso 
{
	public class GameScript : MonoBehaviour 
    {

	    public Scoreboard scoreboard;
		
	    //determines current resource pack
	    public string resourcePack;

	    public GameObject menu;
	    public GameObject resourcePackMenu;

        public GameObject gameTilePrefab;

        private GameObject[] gameTiles;

	    //Number of rows.
	    public int rows = 4;
	    //number of columns.
	    public int columns = 4;

	    private int correctPairs;
	    public GUIText correctPairsDisplay;
	    private int wrongPairs;
	    public GUIText wrongPairsDisplay;

	    /// <summary>
	    /// GUIText for timer.
	    /// </summary>
	    public GUIText timerDisplay;

        //Update variables
        private Ray ray;
        private RaycastHit hit;
        private GameObject first;

        private short canWin = 0;

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
	        canWin = 0;

            gameStartTime = Time.time;

	        //disable menu object
	        resourcePackMenu.SetActive(false);

	        //initialize game board
            gameTiles = GameTiles.createTiles(rows, columns, gameTilePrefab, "gameTile");

            // rotate all tiles
            // add Mover script to all tiles
            for (int i = 0; i < columns; i++)
            {
                for (int o = 0; o < rows; o++)
                {
                    gameTiles[rows * i + o].transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                    
                    //TODO what about namespaces?
                    gameTiles[rows * i + o].AddComponent("Mover");
                }
            }
	        Pick ();
	    }



	    /// <summary>
	    /// Infinite loop.
	    /// </summary>
		void Update()
		{
	        //update GUI text once per second
            float elapsedTime = Time.time - gameStartTime;
            int displayTime = (int)Math.Floor(elapsedTime);
            if (displayTime > lastDisplayTime)
	        {
	            timerDisplay.text = displayTime + " s";
                lastDisplayTime = displayTime;
	        }

	        //game ended
	        if (canWin == 2)
	        {
	            //timer.Stop();
                gameEndTime = Time.time;

	            menu.SetActive(true);
	            resourcePackMenu.SetActive(true);
                ResourcePack resourceMenuScript = resourcePackMenu.GetComponent("MinigamePexeso.ResourcePack") as ResourcePack;
	            resourceMenuScript.CreateMenu();

	            //game ends here, show scoreboard...
                //GameObject go = Instantiate(Resources.Load("Scoreboard")) as GameObject;
                //Scoreboard scoreboard = go.GetComponent<Scoreboard>() as Scoreboard;
                //scoreboard.correct = correctPairsDisplay;
                //scoreboard.wrong = wrongPairsDisplay;
                scoreboard.time.text = (gameEndTime - gameStartTime).ToString();
	        }

	        //player found pair, check if game should end
	        else if (canWin == 1)
	        {
	            bool picsRemain = false;
	            for (int i = 0; i < gameTiles.Length; i++)
	            {
                    if (gameTiles[i] != null)
	                {
	                    picsRemain = true;
	                    break;
	                }
	            }
	            if (!picsRemain)
	            {
	                AudioSource musicPlayer = GameObject.Find("MusicPlayer").GetComponent("AudioSource") as AudioSource;
	                musicPlayer.Stop();
	                this.gameObject.audio.Play();
	                canWin = 2;
	            }
	            else
	            {
	                canWin = 0;
	            }
	        }
	        
            //normal game turn
	        else
	        {
	            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	            if (Physics.Raycast(ray, out hit))
	            {
                    GameObject hittedObject = hit.collider.gameObject;
	                //user clicks left mouse button and hits picture
                    if (Input.GetMouseButtonUp(0) && hit.collider.tag == "gameTile")//
	                {
                        Mover mover = hittedObject.GetComponent("MinigamePexeso.Mover") as Mover;
	                    
                        //picture is already being removed
	                    if (mover.toRemove)
	                    {
	                        return;
	                    }
	                    //this is first selected picture
	                    if (first == null)
	                    {
	                        mover.MoveUp();
                            first = hittedObject;
                            //print("First turned tile: " + first.name);
						}

						//second selected picture is the same as first
                        else if (first == hittedObject.transform.gameObject)
						{
							return;
						}

						//this is second selected picture, different from the first
						else
						{
							//second picture is matching for the first
                            if (pictureMatch(first.transform.GetChild(0).renderer.material.mainTexture.name, hittedObject.transform.GetChild(0).renderer.material.mainTexture.name))
	                        {
	                            correctPairs++;
	                            correctPairsDisplay.text = correctPairs.ToString();
                                StartCoroutine(FoundPair(first, hittedObject));
							}

						    //second picture is not matching for the first
	                        else
	                        {
	                            wrongPairs++;
	                            wrongPairsDisplay.text = wrongPairs.ToString();
                                StartCoroutine(NotFoundPair(first, hittedObject));
							}

							first = null;
	                    }
	                }
	            }
	        }
		}

	    /// <summary>
	    /// Returns true, if two pictures should match.
	    /// </summary>
	    /// <returns></returns>
	    /// <param name="tex1">Picture 1 name</param>
	    /// <param name="tex2">Picture 2 name</param>
	    public bool pictureMatch(string tex1, string tex2)
	    {
	        if (tex1  == tex2)
	        {
	            return true;
	        }
	        return false;
	    }

	    /// <summary>
	    /// COROUTINE. Called when user selected two pictures which do not match.
	    /// </summary>
	    /// <returns></returns>
	    /// <param name="first">First object</param>
	    /// <param name="second">Second object</param>
	    public IEnumerator NotFoundPair(GameObject first, GameObject second)
	    {
            Mover mover1 = first.GetComponent("MinigamePexeso.Mover") as Mover;
            Mover mover2 = second.GetComponent("MinigamePexeso.Mover") as Mover;

			while (mover2.isMoving)
			{
				yield return new WaitForSeconds(0.2f);
			}
			
			mover2.MoveUp();
			
			while (mover1.isMoving || mover2.isMoving)
	        {
	            yield return new WaitForSeconds(0.2f);
	        }
	        mover1.MoveDown();
	        mover2.MoveDown();

	        yield return 0;
	    }

	    /// <summary>
	    /// COROUTINE. Called when user selected two pictures which match.
	    /// </summary>
	    /// <returns></returns>
	    /// <param name="first">First object</param>
	    /// <param name="second">Second object</param>
	    public IEnumerator FoundPair(GameObject first, GameObject second)
	    {
            Mover mover1 = first.GetComponent("MinigamePexeso.Mover") as Mover;
            Mover mover2 = second.GetComponent("MinigamePexeso.Mover") as Mover;
	        mover1.toRemove = true;
	        mover2.toRemove = true;

	        while (mover1.isMoving || mover2.isMoving)
	        {
	            yield return new WaitForSeconds(0.1f);
	        }

			mover1.MoveUp();
			mover2.MoveUp();

			while (mover1.isMoving || mover2.isMoving)
			{
				yield return new WaitForSeconds(0.1f);
			}
			
			first.rigidbody.useGravity = true;
	        second.rigidbody.useGravity = true;

	        first.rigidbody.AddForce(new Vector3(0,1,-5) * 100);
			second.rigidbody.AddForce(new Vector3(0,1,-5) * 100);

	        yield return new WaitForSeconds(1.3f);

	        Destroy(first);
	        Destroy(second);

	        canWin = 1;

	        yield return 0;
	    }
		
	    /// <summary>
	    /// Randomly assigns pictures to planes.
	    /// </summary>
		public void Pick()
		{
	        List<Texture2D> classicPic = new List<Texture2D>();
	        UnityEngine.Object[] images;
	        System.Random random = new System.Random();
	        int num;

            print("resPack to load: '" + resourcePack + "'");

	        //Load all pictures and their matching silhouettes/similarities.
			images = Resources.LoadAll("Textures/Pictures/" + resourcePack);

			if (images == null)
			{
				throw new UnityException("Some images failed to load");
			}

            //print("image count: " + images.Length);

			for(int i = 0; i < images.Length; i++)
			{
				if(images[i] != null)
				{
					classicPic.Add (images[i] as Texture2D);
				}
			}

            print("picture count: " + images.Length);

	        //Choose randomly appropriate number of pictures and their matching silhouettes/similarities.
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
	}
}
