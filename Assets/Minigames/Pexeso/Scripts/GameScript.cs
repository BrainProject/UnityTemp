using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace MinigamePexeso
{
    public class GameScript : MonoBehaviour
    {
        /// <summary>
        /// True if sounds are enabled.
        /// </summary>
        public bool enableSound;

        // <summary>
        /// How long to wait before non-matching tiles are flipped back
        /// </summary>
        private float observationTime = 0.5f;

        /// <summary>
        /// Scoreboard. Not used at the moment
        /// </summary>
        public GameObject scoreb;

        /// <summary>
        /// Determines current resource pack
        /// </summary>
        public string resourcePack;

        /// <summary>
        /// Current game type.
        /// </summary>
        public GameType currentGame;

        /// <summary>
        /// GameStart script
        /// </summary>
        public GameObject menu;

        /// <summary>
        /// ResourcePack script
        /// </summary>
        public GameObject resourcePackMenu;

        /// <summary>
        /// Game tile.
        /// </summary>
        public GameObject gameTilePrefab;

        /// <summary>
        /// True if custom resource pack is selected.
        /// </summary>
        public Boolean customPack;

        /// <summary>
        /// Array of tiles.
        /// </summary>
        private GameObject[] gameTiles;

        /// <summary>
        /// Number of rows.
        /// </summary>
        public int rows = 4;

        /// <summary>
        /// Number of columns.
        /// </summary>
        public int columns = 4;

        /// <summary>
        /// Number of correct pairs. Used as score.
        /// </summary>
        private int correctPairs;

        /// <summary>
        /// GUI Number of correct pairs. Used as score.
        /// </summary>
        public GUIText correctPairsDisplay;

        /// <summary>
        /// Number of wrong pairs. Used as score.
        /// </summary>
        private int wrongPairs;

        /// <summary>
        /// GUI Number of wrong pairs. Used as score.
        /// </summary>
        public GUIText wrongPairsDisplay;

        /// <summary>
        /// GUIText for timer.
        /// </summary>
        public GUIText timerDisplay;

        /// <summary>
        /// Used for mouse click detection
        /// </summary>
        private Ray ray;

        private RaycastHit hit;

        /// <summary>
        /// The first selected game tile.
        /// </summary>
        private GameObject first;

        /// <summary>
        /// If correctPairs variable reaches this value, game successfully ends.
        /// </summary>
        private int winningScore;

        /// <summary>
        /// The game start time.
        /// </summary>
        private float gameStartTime;

        /// <summary>
        /// The game end time.
        /// </summary>
        private float gameEndTime;

        /// <summary>
        /// The last display time.
        /// </summary>
        private int lastDisplayTime = 0;

        /// <summary>
        /// can user flip a tile? 
        /// No if two tiles are already flipped
        /// </summary>
        private bool canFlip = true;

        /// <summary>
        /// The images loaded as objects.
        /// </summary>
        UnityEngine.Object[] images;

        /// <summary>
        /// Initialization. Creates all cubes and planes, sets their position etc.
        /// </summary>
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
                    if (currentGame == GameType.Pexeso)
                    {
                        gameTiles[rows * i + o].transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                        gameTiles[rows * i + o].AddComponent<Flipper>();
                    }
                    else
                    {
                        gameTiles[rows * i + o].AddComponent<Mover>();
                    }
                }
            }

            if (customPack)
            {
                StartCoroutine(AddCustomPicturesToGameTiles());
            }
            else
            {
                AddPicturesToGameTiles();
            }
        }

        /// <summary>
        /// Game turn of Pexeso. Calculates clicking on tiles, matching of pairs,
        /// starting game tile flip and remove coroutines.
        /// </summary>
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

                    if (!canFlip)
                    {
                        return;
                    }
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

        /// <summary>
        /// Game turn of Silhouette or Similarities. Calculates clicking on tiles, matching of pairs,
        /// starting game tile move and remove coroutines.
        /// </summary>
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

                    if (!canFlip)
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
        /// Infinite loop. Updates game time and launches specific game turn.
        /// </summary>
        void Update()
        {
            //update GUI text once per second
            //TODO better solution would be to use coroutine with 
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
                if (currentGame == GameType.Pexeso)
                {
                    PexesoGameTurn();
                }
                else
                {
                    SilhouetteGameTurn();
                }
            }
        }

        /// <summary>
        /// Game ended. Play winning music, stop timer, etc.
        /// </summary>
        private void EndGame()
        {
            Debug.Log("All pairs found... game ends");

            //to avoid calling this method more than once
            winningScore = -1;

            //animate Neuron
            MGC.Instance.neuronHelp.GetComponent<Game.BrainHelp>().ShowSmile(Resources.Load("Neuron/smilyface") as Texture);

            //stop game music and play wictory sound
            if (enableSound)
            {
                AudioSource musicPlayer = GameObject.Find("MusicPlayer").GetComponent("AudioSource") as AudioSource;
                musicPlayer.Stop();
                this.gameObject.GetComponent<AudioSource>().Play();
            }

            //game ends here, show scoreboard...
            //gameEndTime = Time.time;

            //global stuff, happening for each minigame
            MGC.Instance.WinMinigame();
        }

        /// <summary>
        /// Returns true if pictures of given tiles are matching.
        /// </summary>
        /// <returns>True, if pictures of given tiles are matching.</returns>
        /// <param name="tex1">First tile</param>
        /// <param name="tex2">Second tile</param>
        public bool checkTilesMatch(GameObject first, GameObject second)
        {
            if (currentGame == GameType.Pexeso)
            {
                if (first.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture.name == second.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture.name)
                {
                    return true;
                }
            }
            else
            {
                if (first.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture.name + "a" == second.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture.name || second.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture.name + "a" == first.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture.name)
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
            canFlip = false;

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

            while (flipper1.isMoving || flipper2.isMoving)
            {
                yield return new WaitForSeconds(0.05f);
            }

            canFlip = true;
        }

        /// <summary>
        /// COROUTINE. Called when user selected two matching pictures
        /// </summary>
        /// <returns></returns>
        /// <param name="first">First object</param>
        /// <param name="second">Second object</param>
        public IEnumerator FoundPairPexeso(GameObject first, GameObject second)
        {
            canFlip = false;

            Flipper flipper1 = first.GetComponent("MinigamePexeso.Flipper") as Flipper;
            Flipper flipper2 = second.GetComponent("MinigamePexeso.Flipper") as Flipper;

            //wait while tiles finishes their flipping
            while (flipper1.isMoving || flipper2.isMoving)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //set up physics to animate fall of tiles
            first.GetComponent<Rigidbody>().useGravity = true;
            first.GetComponent<Rigidbody>().isKinematic = false;
            second.GetComponent<Rigidbody>().useGravity = true;
            second.GetComponent<Rigidbody>().isKinematic = false;

            first.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, -5) * 100);
            second.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, -5) * 100);

            //wait some time before user can select another tile
            yield return new WaitForSeconds(0.5f);
            canFlip = true;

            //wait until tiles are out of view, then destroy them
            yield return new WaitForSeconds(1.5f);
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
            canFlip = false;

            Mover mover1 = first.GetComponent("Mover") as Mover;
            Mover mover2 = second.GetComponent("Mover") as Mover;

            mover2.MoveUp();

            while (mover1.isMoving || mover2.isMoving)
            {
                yield return new WaitForSeconds(0.05f);
            }
            mover1.MoveDown();
            mover2.MoveDown();

            while (mover1.isMoving || mover2.isMoving)
            {
                yield return new WaitForSeconds(0.05f);
            }
            canFlip = true;
        }

        /// <summary>
        /// COROUTINE. Called when user selected two pictures which match.
        /// </summary>
        /// <returns></returns>
        /// <param name="first">First object</param>
        /// <param name="second">Second object</param>
        public IEnumerator FoundPairSilhouette(GameObject first, GameObject second)
        {
            canFlip = false;

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

            first.GetComponent<Rigidbody>().useGravity = true;
            first.GetComponent<Rigidbody>().isKinematic = false;
            second.GetComponent<Rigidbody>().useGravity = true;
            second.GetComponent<Rigidbody>().isKinematic = false;

            first.GetComponent<Rigidbody>().AddForce(second.transform.position * 100);
            second.GetComponent<Rigidbody>().AddForce(first.transform.position * 100);

            //wait before user can select another tile
            yield return new WaitForSeconds(0.5f);
            canFlip = true;

            //TODO change - tiles have to be checked for Y axis - if it is below, destroy
            yield return new WaitForSeconds(1.7f);
            Destroy(first);
            Destroy(second);
        }

        /// <summary>
        /// Randomly assigns pictures from custom resource pack to game tiles.
        /// </summary>
        public IEnumerator AddCustomPicturesToGameTiles()
        {
            string customImagesPath = MGC.Instance.getPathtoCustomImageSets() + currentGame.ToString() + "/" + resourcePack + "/";

            if (currentGame == GameType.Pexeso)
            {
                List<Texture2D> classicPic = new List<Texture2D>();

                System.Random random = new System.Random();
                int num;

                //Load all pictures
                string[] files = Directory.GetFiles(customImagesPath, "*.png");
                images = new UnityEngine.Object[files.Length];

                WWW www;
                for (int i = 0; i < files.Length; i++)
                {
                    www = new WWW("file:///" + files[i]);
                    yield return www;
                    images[i] = www.texture;
                    images[i].name = i.ToString("00");
                }

                if (images == null)
                {
                    throw new UnityException("Some images failed to load");
                }

                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] != null)
                    {
                        classicPic.Add(images[i] as Texture2D);
                    }
                }

                //Choose randomly appropriate number of pictures
                List<Texture2D> chosen = new List<Texture2D>();
                for (int i = 0; i < (rows * columns) / 2; i++)
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

                    float height = chosen[num].height;
                    float width = chosen[num].width;
                    float tileSize = gameTiles[i].transform.GetChild(0).transform.localScale.x;
                    if (height > width)
                    {
                        tileSize = (width / height) / 10;
                        gameTiles[i].transform.GetChild(0).transform.localScale = new Vector3(tileSize, gameTiles[i].transform.GetChild(0).transform.localScale.y, gameTiles[i].transform.GetChild(0).transform.localScale.z);
                    }
                    else
                    {
                        tileSize = (height / width) / 10;
                        gameTiles[i].transform.GetChild(0).transform.localScale = new Vector3(gameTiles[i].transform.GetChild(0).transform.localScale.x, gameTiles[i].transform.GetChild(0).transform.localScale.y, tileSize);
                    }

                    gameTiles[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = chosen[num];
                    chosen.RemoveAt(num);
                }
            }

            // game is not classic pexeso, but some other variant (siluethes, similiraties...)
            else
            {
                List<Texture2D> classicPic = new List<Texture2D>();
                List<Texture2D> classicPic_a = new List<Texture2D>();
                Texture2D tex, texA;
                System.Random random = new System.Random();
                int num;

                
                //Load all pictures and their matching silhouettes/similarities.
                int o = 0;
                while (o != -1)
                {
                    WWW www = new WWW("file:///" + customImagesPath + o.ToString("00") + ".png");
                    yield return www;
                    WWW wwwA = new WWW("file:///" + customImagesPath + o.ToString("00") + "a.png");
                    yield return wwwA;

                    tex = www.texture;
                    texA = wwwA.texture;

                    if (www.error != null)
                    {
                        Debug.Log(www.error);
                        tex = null;
                    }

                    if (wwwA.error != null)
                    {
                        Debug.Log(wwwA.error);
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

                //check, if there is enough pictures loaded
                int neededPics = (rows * columns) / 2;
                if(classicPic.Count < neededPics)
                {
                    MGC.Instance.logger.addEntry("There is not enough pictures in costum pack: '" + currentGame.ToString() + "'");
                    throw new UnityException("There is not enough pictures in costum pack: '" + currentGame.ToString() + "'");
                }

                //Choose randomly appropriate number of pictures and their matching silhouettes/similarities.
                List<Texture2D> chosen = new List<Texture2D>();
                for (int i = 0; i < neededPics; i++)
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

                    float height = chosen[num].height;
                    float width = chosen[num].width;
                    float tileSize = gameTiles[i].transform.GetChild(0).transform.localScale.x;
                    if (height > width)
                    {
                        tileSize = (width / height) / 10;
                        gameTiles[i].transform.GetChild(0).transform.localScale = new Vector3(tileSize, gameTiles[i].transform.GetChild(0).transform.localScale.y, gameTiles[i].transform.GetChild(0).transform.localScale.z);
                    }
                    else
                    {
                        tileSize = (height / width) / 10;
                        gameTiles[i].transform.GetChild(0).transform.localScale = new Vector3(gameTiles[i].transform.GetChild(0).transform.localScale.x, gameTiles[i].transform.GetChild(0).transform.localScale.y, tileSize);
                    }

                    gameTiles[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = chosen[num];
                    chosen.RemoveAt(num);
                }
            }
            Debug.Log("Game initialized");
        }

        /// <summary>
        /// Randomly assigns pictures to game tiles.
        /// </summary>
        public void AddPicturesToGameTiles()
        {
            if (currentGame == GameType.Pexeso)
            {
                List<Texture2D> classicPic = new List<Texture2D>();

                System.Random random = new System.Random();
                int num;
                int picturesNeeded = (rows * columns) / 2;

                //Load all pictures
                images = Resources.LoadAll("Textures/Pictures/Pexeso/" + resourcePack);

                print("Resource pack: " + resourcePack + "; images loaded: " + images.Length);

                if (images == null)
                {
                    throw new UnityException("Images was not loaded at all.");
                }

                if (images.Length < picturesNeeded)
                {
                    throw new UnityException("There is to few images in resource pack: 'resourcePack'");
                }

                // create Texture2D from each image
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i] != null)
                    {
                        classicPic.Add(images[i] as Texture2D);
                    }
                    else
                    {
                        throw new UnityException("image[" + i + "] == null");
                    }
                }

                print("Textures created: " + classicPic.Count);
                print("Rows: " + rows + "; columns: " + columns);


                //Choose randomly appropriate number of pictures
                List<Texture2D> chosen = new List<Texture2D>();
                for (int i = 0; i < picturesNeeded; i++)
                {
                    num = random.Next(0, classicPic.Count);
                    chosen.Add(classicPic[num]);
                    chosen.Add(classicPic[num]);
                    classicPic.RemoveAt(num);
                }

                print("Pictures chosen: " + chosen.Count);

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
                    gameTiles[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = chosen[num];

                    float height = chosen[num].height;
                    float width = chosen[num].width;
                    float tileSize = gameTiles[i].transform.GetChild(0).transform.localScale.x;
                    if (height > width)
                    {
                        tileSize = (width / height) / 10;
                        gameTiles[i].transform.GetChild(0).transform.localScale = new Vector3(tileSize, gameTiles[i].transform.GetChild(0).transform.localScale.y, gameTiles[i].transform.GetChild(0).transform.localScale.z);
                    }
                    else
                    {
                        tileSize = (height / width) / 10;
                        gameTiles[i].transform.GetChild(0).transform.localScale = new Vector3(gameTiles[i].transform.GetChild(0).transform.localScale.x, gameTiles[i].transform.GetChild(0).transform.localScale.y, tileSize);
                    }

                    chosen.RemoveAt(num);
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
                while (o != -1)
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
                for (int i = 0; i < (rows * columns) / 2; i++)
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

                    float height = chosen[num].height;
                    float width = chosen[num].width;
                    float tileSize = gameTiles[i].transform.GetChild(0).transform.localScale.x;
                    if (height > width)
                    {
                        tileSize = (width / height) / 10;
                        gameTiles[i].transform.GetChild(0).transform.localScale = new Vector3(tileSize, gameTiles[i].transform.GetChild(0).transform.localScale.y, gameTiles[i].transform.GetChild(0).transform.localScale.z);
                    }
                    else
                    {
                        tileSize = (height / width) / 10;
                        gameTiles[i].transform.GetChild(0).transform.localScale = new Vector3(gameTiles[i].transform.GetChild(0).transform.localScale.x, gameTiles[i].transform.GetChild(0).transform.localScale.y, tileSize);
                    }

                    gameTiles[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = chosen[num];
                    chosen.RemoveAt(num);
                }
            }
            Debug.Log("Game initialized");
        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public IEnumerator RestartGame()
        {
            //TODO do not restart automatically - display "end-game GUI" instead

            //wait some time...
            yield return new WaitForSeconds(2f);
            Debug.Log("Restarting game");

            menu.SetActive(true);
            resourcePackMenu.SetActive(true);
            GameSetup resourceMenuScript = resourcePackMenu.GetComponent("MinigamePexeso.ResourcePack") as GameSetup;
            this.enabled = false;
            resourceMenuScript.CreateResourcePacksIcons();
        }
    }
}
