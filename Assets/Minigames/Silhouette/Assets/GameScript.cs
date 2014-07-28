using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class GameScript : MonoBehaviour {

    public Scoreboard scoreboard;

    //determines current game type
    public ResourcePack.GameType gameType;
    //determines current resource pack
    public string resourcePack;

    public GameObject menu;
    public GameObject resourcePackMenu;

    //Array of cubes.
    private GameObject[] cubes;
    //Array of planes.
    private GameObject[] planes;

    //Number of rows.
    public int rows = 4;
    //number of columns.
    public int columns = 4;

    private int correctPairs;
    public GUIText correctPairsDisplay;
    private int wrongPairs;
    public GUIText wrongPairsDisplay;

    /// <summary>
    /// Elapsed game time in seconds.
    /// </summary>
    private int time;

    /// <summary>
    /// Determines if it is necessarry to update timer text.
    /// </summary>
    private bool updateTimer = false;

    /// <summary>
    /// GUIText for timer.
    /// </summary>
    public GUIText timerDisplay;

    /// <summary>
    /// Timer.
    /// </summary>
    Timer timer = new Timer();

    /// <summary>
    /// Creates and starts timer.
    /// </summary>
    void CreateTimer()
    {
        timer = new Timer();
        timer.Interval = 1000;
        timer.Elapsed +=TimerTick;
        
        timer.Start();
    }

    /// <summary>
    /// Timer tick event method.
    /// </summary>
    /// <param name="o">O.</param>
    /// <param name="e">E.</param>
    void TimerTick(object o, System.EventArgs e)
    {
        time++;
        updateTimer = true;
    }

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
        updateTimer = false;
        correctPairs = 0;
        correctPairsDisplay.text = correctPairs.ToString();
        wrongPairs = 0;
        wrongPairsDisplay.text = wrongPairs.ToString();
        time = 0;
        timerDisplay.text = time + " s";
        canWin = 0;
        CreateTimer();

        //disable menu object
        //menu.SetActive(false);
        resourcePackMenu.SetActive(false);

        //initialize game board
        cubes = new GameObject[rows * columns];
        planes = new GameObject[rows * columns];
        for (int i = 0; i < columns; i++)
        {
            for (int o = 0; o < rows; o++)
            {
                cubes[rows*i + o] = GameObject.CreatePrimitive(PrimitiveType.Cube);//create cube
                cubes[rows*i + o].transform.localScale = new Vector3(1, 1, 0.1f);//flatten it
                cubes[rows*i + o].transform.position = new Vector3((i * 1.2f) - (0.1125f*(float)Math.Pow(columns, 2)),(o * 1.2f) - (0.05625f*(float)Math.Pow(rows, 2)),0);//move them
                cubes[rows*i + o].renderer.material.mainTexture = Resources.Load("Textures/back") as Texture2D;//load texture
                
                planes[rows*i + o] = GameObject.CreatePrimitive(PrimitiveType.Plane);//create plane
                planes[rows*i + o].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);//shrink them
                Quaternion q = new Quaternion(0, 0, 0, 1);//create rotation quaternion
                q.SetLookRotation(new Vector3(0,-1000,-1), new Vector3(0,1,0));//assign values to quaternion
                planes[rows*i + o].transform.rotation = q;//assign quaternion to object
                planes[rows*i + o].transform.position = new Vector3((i * 1.2f) - (0.1125f*(float)Math.Pow(columns, 2)),(o * 1.2f) - (0.05625f*(float)Math.Pow(rows, 2)),-0.051f);//move them
                
                cubes[rows*i + o].transform.parent = planes[rows*i + o].transform;//make plane parent of cube
                
                planes[rows*i + o].AddComponent("Mover");//attach script
                planes[rows*i + o].renderer.material.shader = Shader.Find("Particles/Alpha Blended");//set shader
                cubes[rows*i + o].renderer.material.shader = Shader.Find("Particles/Alpha Blended");//set shader

                Destroy(planes[rows*i + o].collider);
                planes[rows*i + o].AddComponent<BoxCollider>(); 
                Rigidbody gameObjectsRigidBody = planes[rows*i + o].AddComponent<Rigidbody>(); // Add the rigidbody.
                gameObjectsRigidBody.mass = 5;//set weight
                gameObjectsRigidBody.useGravity = false;//disable gravity
            }
        }
        Pick ();
    }

    //Update variables
    private Ray ray;
    private RaycastHit hit;
    private GameObject first;

    private short canWin = 0;

    /// <summary>
    /// Infinite loop.
    /// </summary>
	void Update()
	{
        //update GUI text if necessarry
        if (updateTimer)
        {
            updateTimer = false;
            timerDisplay.text = time + " s";
        }

        //game ended
        if (canWin == 2)
        {
            timer.Stop();
            menu.SetActive(true);
            resourcePackMenu.SetActive(true);
            ResourcePack resourceMenuScript = resourcePackMenu.GetComponent("ResourcePack") as ResourcePack;
            resourceMenuScript.CreateMenu();
            //game ends here
            /*GameObject go = Instantiate(Resources.Load("Scoreboard")) as GameObject;
            Scoreboard scoreboard = go.GetComponent<Scoreboard>() as Scoreboard;
            scoreboard.correct = correctPairsDisplay;
            scoreboard.wrong = wrongPairsDisplay;
            scoreboard.time = timerDisplay;*/

        }
        //player found pair, check if game should end
        else if (canWin == 1)
        {
            bool picsRemain = false;
            for (int i = 0; i < planes.Length; i++)
            {
                if (planes [i] != null)
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
                //user clicks left mouse button and hits picture
                if (Input.GetMouseButtonUp(0) && hit.collider.name == "Plane")
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
                    else if (pictureMatch(first.renderer.material.mainTexture.name, hit.collider.gameObject.renderer.material.mainTexture.name))
                        {
                            correctPairs++;
                            correctPairsDisplay.text = correctPairs.ToString();
                            StartCoroutine(FoundPair(first, hit.collider.gameObject));
                        }
                    //seond picture is not matching for the first
                    else
                        {
                            wrongPairs++;
                            wrongPairsDisplay.text = wrongPairs.ToString();
                            StartCoroutine(NotFoundPair(first, hit.collider.gameObject));
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
        if (tex1 + "a" == tex2 || tex2 + "a" == tex1)
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
        Mover mover1 = first.GetComponent("Mover") as Mover;
        Mover mover2 = second.GetComponent("Mover") as Mover;

        mover2.MoveUp();

        while (mover1.isMoving || mover2.isMoving)
        {
            yield return new WaitForSeconds(0.1f);
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
        second.rigidbody.useGravity = true;

        first.rigidbody.AddForce(second.transform.position * 100);
        second.rigidbody.AddForce(first.transform.position * 100);

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
        List<Texture2D> classicPic_a = new List<Texture2D>();
        Texture2D tex, texA;
        System.Random random = new System.Random();
        int num;

        //Load all pictures and their matching silhouettes/similarities.
        int o = 0;
        while(o != -1)
        {
            tex = Resources.Load("Textures/Pictures/" + gameType.ToString() + "/" + resourcePack + "/" + o.ToString("00")) as Texture2D;
            texA = Resources.Load("Textures/Pictures/" + gameType.ToString() + "/" + resourcePack + "/" + o.ToString("00") + "a") as Texture2D;
            
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
            planes[i].renderer.material.mainTexture = chosen[num];
            chosen.RemoveAt(num);
        }
	}
}
