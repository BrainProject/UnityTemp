using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// main class in london tower
/// chaeck game statce, creat start, check end etc
/// </summary>
public class LondonTowerGameManager : MonoBehaviour {

    /// <summary>
    /// parse string into color (string is from xml level files)
    /// </summary>
    public static Dictionary<string, Color> speheresIDColor = new Dictionary<string, Color>() { { "red", Color.red }, { "blue", Color.blue }, { "green", Color.green }, { "yellow", Color.yellow }, { "brown", new Color(160f / 255f, 82f / 255f, 45f / 255f) } };
    
    /// <summary>
    /// current game state
    /// </summary>
    public static LondonTowerGameState state = LondonTowerGameState.start;

    /// <summary>
    /// data 1,2,3 choosing in main menu
    /// </summary>
    public static int dataSet = 1;
    /// <summary>
    /// data 1,2,3,4 , random "level" from current dataset
    /// </summary>
    public static int levelSet = 1;
    
    public LondonToweSphereScript spherePrefab;

    List<LondonToweGameStartWinData> data;
    public LondonTowerCamera cameraScript;

    public List<LondonToweSphereScript> spheres;
    public List<LondonTowePoleScript> poles;

    LondonToweGameStartWinData endGame;
    LondonToweGameStartWinData startGame;

    public List<TextAsset> xmlLevels = new List<TextAsset>();

    public Texture playAgain, chooseLevel, exitGame,backToMenu;


	// Use this for initialization
	void Start () {
        StartGame();
       
	
	}
	
	// Update is called once per frame
	void Update () {
        if (state == LondonTowerGameState.animationEnd)
        {
            SetLevel(startGame);
            state = LondonTowerGameState.game;
        }
	}

    /// <summary>
    /// check if pole if full - then cant place any sphere on it, until at least one sphere is removed
    /// </summary>
    /// <param name="polePosition"></param>
    /// <returns></returns>
    public bool IsPoleFull(int polePosition)
    {
        foreach (LondonTowePoleScript pole in poles)
        {
            if (((int)pole.transform.position.x) == polePosition)
            {
                return pole.capacity == 0;
            }
        }
        return false;
    }

   /// <summary>
   /// set level from startData, doesn matter if it for expample/screen or for game 
   /// </summary>
   /// <param name="startData"></param>
    public void SetLevel(LondonToweGameStartWinData startData)
    {
        Debug.Log(startData.ToString());
        foreach (LondonToweSphereScript sphere in spheres)
        {
            Destroy(sphere.gameObject);
           
           
        }
        spheres.Clear();
          foreach (LondonTowePoleScript pole in poles)
            {
                if (pole.id == 1)
                {
                   // pole.capacity = startData.Pole1Size;
                    pole.SetCapacity(startData.Pole1Size);
                }
                else if (pole.id == 2)
                {
                    pole.SetCapacity(startData.Pole2Size);
                }
                else if (pole.id == 3)
                {
                    pole.SetCapacity(startData.Pole3Size);
                }
          }
         
                int counter = startData.pole1.Count;
                foreach (string s in startData.pole1)
                {
                    counter--;
                    Debug.Log((0.5f + counter - 1) + "spawn");
                    LondonToweSphereScript createdSpehre = (LondonToweSphereScript)Instantiate(spherePrefab, new Vector3(0,0.5f+ counter, 0), new Quaternion());
                    spheres.Add(createdSpehre);
                    createdSpehre.start = true;
                    createdSpehre.orderOnPole = startData.Pole1Size - counter;
                    createdSpehre.gameManager = this;
                    createdSpehre.setIdColor(s, speheresIDColor[s]);
                }
                counter = startData.pole2.Count;
                foreach (string s in startData.pole2)
                {
                    Debug.Log((0.5f + counter - 1) + "spawn");
                    counter--;
                    LondonToweSphereScript createdSpehre = (LondonToweSphereScript)Instantiate(spherePrefab, new Vector3(4, 0.5f + counter, 0), new Quaternion());
                    spheres.Add(createdSpehre);
                    createdSpehre.start = true;
                    createdSpehre.orderOnPole = startData.Pole2Size - counter;
                    createdSpehre.gameManager = this;
                    createdSpehre.setIdColor(s, speheresIDColor[s]);
                }
                counter = startData.pole3.Count;
                foreach (string s in startData.pole3)
                {
                    Debug.Log((0.5f + counter - 1) + "spawn");
                    counter--;
                    LondonToweSphereScript createdSpehre = (LondonToweSphereScript)Instantiate(spherePrefab, new Vector3(8, 0.5f + counter, 0), new Quaternion());
                    spheres.Add(createdSpehre);
                    createdSpehre.start = true;
                    createdSpehre.orderOnPole = startData.Pole3Size - counter;
                    createdSpehre.gameManager = this;
                    createdSpehre.setIdColor(s, speheresIDColor[s]);
                }

                foreach (LondonToweSphereScript sp in spheres)
                {
                    sp.rigidbody.useGravity = true;
                }
            
    }

    /// <summary>
    /// allow camera script to take a screen
    /// </summary>
    public void TakeScreen()
    {
        cameraScript.takeScreen = true;
    }

    /// <summary>
    /// check if player win, after each sphere move
    /// </summary>
    /// <returns></returns>
    public bool CheckWin()
    {
        if (state != LondonTowerGameState.game)
        {
            return false;
        }
        
        List<LondonToweSphereScript> speheresOnPole = new List<LondonToweSphereScript>();
        //pole1
        foreach (LondonToweSphereScript sphere in spheres)
        {
            if (sphere.currentPoleID == 1)
            {
                speheresOnPole.Add(sphere);
            }
        }
        if (endGame.pole1.Count != speheresOnPole.Count) return false;
        speheresOnPole.Sort();
        for (int i = 0; i < speheresOnPole.Count; i++)
        {
            if(!endGame.pole1[i].Equals(speheresOnPole[i].idColor)) return false;
        }
        speheresOnPole.Clear();


        //pole2
        foreach (LondonToweSphereScript sphere in spheres)
        {
            if (sphere.currentPoleID == 2)
            {
                speheresOnPole.Add(sphere);
            }
        }
        if (endGame.pole2.Count != speheresOnPole.Count) return false;
        speheresOnPole.Sort();
        for (int i = 0; i < speheresOnPole.Count; i++)
        {
            if (!endGame.pole2[i].Equals(speheresOnPole[i].idColor)) return false;
        }
        speheresOnPole.Clear();

        //pole3
        foreach (LondonToweSphereScript sphere in spheres)
        {
            if (sphere.currentPoleID == 3)
            {
                speheresOnPole.Add(sphere);
            }
        }
        if (endGame.pole3.Count != speheresOnPole.Count) return false;
        speheresOnPole.Sort();
        for (int i = 0; i < speheresOnPole.Count; i++)
        {
            if (!endGame.pole3[i].Equals(speheresOnPole[i].idColor)) return false;
        }
        speheresOnPole.Clear();
        
        return true;
    }

   /// <summary>
   /// check if sphere is on top/on given order pole so it player can moved with it
   /// </summary>
   /// <param name="poleID"></param>
   /// <param name="orderInPole"></param>
   /// <returns></returns>
    public bool OnTop(int poleID, int orderInPole)
    {
        foreach (LondonTowePoleScript pole in poles)
        {
            if (pole.id == poleID)
            {
                return (pole.capacity + 1) == orderInPole;
            }
        }
        
        return false;
    }


    void OnGUI()
    {

        if (state == LondonTowerGameState.game)
        {
            GUI.DrawTexture(new Rect(Screen.width - Screen.width / 5, Screen.height / 12, Screen.width / 6, Screen.height / 8), backToMenu);
            if (GUI.Button(new Rect(Screen.width - Screen.width / 5,  Screen.height /12, Screen.width / 6, Screen.height / 8), "back to menu"))
            {
                Application.LoadLevel(0);
                state = LondonTowerGameState.start;
                //LoadLevel this
            }
        }
        
        if (state == LondonTowerGameState.winGUI)
        {
            GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height / 7, Screen.width / 2, Screen.height / 7), playAgain);    
            if (GUI.Button(new Rect(Screen.width / 4, Screen.height /7, Screen.width / 2, Screen.height / 7), "play again"))
            {
                Application.LoadLevel(Application.loadedLevel);
                state = LondonTowerGameState.start;
                //LoadLevel this
            }
            GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height * 3 / 7, Screen.width / 2, Screen.height / 7), chooseLevel);
            if (GUI.Button(new Rect(Screen.width / 4, Screen.height*3 / 7, Screen.width / 2, Screen.height / 7), "choose level"))
            {
                //load scenu main menu
                state = LondonTowerGameState.start;
                Application.LoadLevel(0);
                //LoadLevel this
            }
            GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height * 5 / 7, Screen.width / 2, Screen.height / 7), exitGame);
            if (GUI.Button(new Rect(Screen.width / 4, Screen.height*5 /7, Screen.width / 2, Screen.height / 7), "exit game"))
            {
                state = LondonTowerGameState.start;
                //vrátit se zpět do menu
                //Application.LoadLevel(Application.loadedLevel);
                //LoadLevel this
            }
        }
    }

    /// <summary>
    /// call at level start, create all necessary steps to load level, take screen etc
    /// </summary>
    public void StartGame()
    {
        LondonToweXMLGameLoader loader = new LondonToweXMLGameLoader();

       // dataSet = Random.Range(0, 4);
       
        data = loader.ParseXmlTextAsset(xmlLevels[dataSet]);
        //data = loader.ParseXmlTextAsset(xmlLevels[1]);
        levelSet = Random.Range(1, data.Count/2+1);
       
        foreach (LondonToweGameStartWinData level in data)
        {
            if (level.GameID == levelSet)
            {
                if (level.IsStart())
                {
                    startGame = level;
                }
                else
                {
                    endGame = level;
                }
            }
        }
        if (startGame == null || endGame == null)
        {
            Debug.Log("set" + dataSet + "++ level" + levelSet);
        }
        poles = new List<LondonTowePoleScript>(FindObjectsOfType<LondonTowePoleScript>());
        SetLevel(endGame);


        foreach (LondonToweSphereScript sphere in spheres)
        {
            sphere.gameManager = this;
        }

        foreach (LondonTowePoleScript pole in poles)
        {
            pole.gameManager = this;
        }
        TakeScreen();
        state = LondonTowerGameState.animation;

        //  LondondToweXMLGameLoader xx = new LondondToweXMLGameLoader();
        // xx.ParseXmlFile("set1.xml");
        // xx.ParseXmlTextAsset(xmltest);

    }

   
}




/// <summary>
/// game state enum
/// </summary>
public enum LondonTowerGameState {  start = 0, animation = 1,animationEnd =2, game = 3, winGUI =4 };
