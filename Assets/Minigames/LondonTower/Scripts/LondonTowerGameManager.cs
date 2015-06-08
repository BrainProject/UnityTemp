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
    public static Dictionary<string, Color> spheresIDColor = new Dictionary<string, Color>() { { "red", Color.red }, { "blue", Color.blue }, { "green", Color.green }, { "yellow", Color.yellow }, { "brown", new Color(160f / 255f, 82f / 255f, 45f / 255f) } };
    
    /// <summary>
    /// current game state
    /// </summary>
    public static LondonTowerGameState state = LondonTowerGameState.start;

    /// <summary>
    /// data 1,2,3 choosing in main menu
    /// </summary>
   // public static int dataSet = 1;
    /// <summary>
    /// data 1,2,3,4 , random "level" from current dataset
    /// </summary>
    public static int levelSet = 1;

        
    public LondonToweSphereScript spherePrefab;

    public GameObject minimap;

    List<LondonToweGameStartWinData> data = new List<LondonToweGameStartWinData>();
   
    public List<LondonToweSphereScript> spheres;
    public List<LondonTowePoleScript> poles;

    LondonToweGameStartWinData endGame;
    LondonToweGameStartWinData startGame;

    public List<TextAsset> xmlLevels = new List<TextAsset>();


	// Use this for initialization
	void Start () {
        StartGame();	
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
        foreach (LondonToweSphereScript sphere in spheres)
        {
            Destroy(sphere.gameObject);
           
           
        }
        spheres.Clear();
          foreach (LondonTowePoleScript pole in poles)
            {
                if (pole.id == 1)
                {
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
                    LondonToweSphereScript createdSphere = (LondonToweSphereScript)Instantiate(spherePrefab, new Vector3(0,0.5f+ counter, 0), new Quaternion());
                    spheres.Add(createdSphere);
                    createdSphere.start = true;
                    createdSphere.orderOnPole = startData.Pole1Size - counter;
                    createdSphere.gameManager = this;
                    createdSphere.setIdColor(s, spheresIDColor[s]);
                }
                counter = startData.pole2.Count;
                foreach (string s in startData.pole2)
                {
                    counter--;
                    LondonToweSphereScript createdSphere = (LondonToweSphereScript)Instantiate(spherePrefab, new Vector3(4, 0.5f + counter, 0), new Quaternion());
                    spheres.Add(createdSphere);
                    createdSphere.start = true;
                    createdSphere.orderOnPole = startData.Pole2Size - counter;
                    createdSphere.gameManager = this;
                    createdSphere.setIdColor(s, spheresIDColor[s]);
                }
                counter = startData.pole3.Count;
                foreach (string s in startData.pole3)
                {
                   counter--;
                    LondonToweSphereScript createdSphere = (LondonToweSphereScript)Instantiate(spherePrefab, new Vector3(8, 0.5f + counter, 0), new Quaternion());
                    spheres.Add(createdSphere);
                    createdSphere.start = true;
                    createdSphere.orderOnPole = startData.Pole3Size - counter;
                    createdSphere.gameManager = this;
                    createdSphere.setIdColor(s, spheresIDColor[s]);
                }

                foreach (LondonToweSphereScript sp in spheres)
                {
                    sp.rigidbody.useGravity = true;
                }
                minimap.SetActive(true);
                string path = "GoalsPictures/" + startGame.GameID ;
                Texture minimapSprite = (Resources.Load<Texture>(path) as Texture);
                if (minimapSprite == null)
                {
                    Debug.Log("cannot load");
                }
                minimap.renderer.material.mainTexture = minimapSprite;
                
       
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
        
        List<LondonToweSphereScript> spheresOnPole = new List<LondonToweSphereScript>();
        //pole1
        foreach (LondonToweSphereScript sphere in spheres)
        {
            if (sphere.currentPoleID == 1)
            {
                spheresOnPole.Add(sphere);
            }
        }
        if (endGame.pole1.Count != spheresOnPole.Count) return false;
        spheresOnPole.Sort();
        for (int i = 0; i < spheresOnPole.Count; i++)
        {
            if(!endGame.pole1[i].Equals(spheresOnPole[i].idColor)) return false;
        }
        spheresOnPole.Clear();


        //pole2
        foreach (LondonToweSphereScript sphere in spheres)
        {
            if (sphere.currentPoleID == 2)
            {
                spheresOnPole.Add(sphere);
            }
        }
        if (endGame.pole2.Count != spheresOnPole.Count) return false;
        spheresOnPole.Sort();
        for (int i = 0; i < spheresOnPole.Count; i++)
        {
            if (!endGame.pole2[i].Equals(spheresOnPole[i].idColor)) return false;
        }
        spheresOnPole.Clear();

        //pole3
        foreach (LondonToweSphereScript sphere in spheres)
        {
            if (sphere.currentPoleID == 3)
            {
                spheresOnPole.Add(sphere);
            }
        }
        if (endGame.pole3.Count != spheresOnPole.Count) return false;
        spheresOnPole.Sort();
        for (int i = 0; i < spheresOnPole.Count; i++)
        {
            if (!endGame.pole3[i].Equals(spheresOnPole[i].idColor)) return false;
        }
        spheresOnPole.Clear();
        
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

    /// <summary>
    /// after sphere is place it check if the sphere is definetly on top of pole
    /// and if not t place the sphere on top
    /// </summary>
    /// <param name="sphere"></param>
    public void SetOnTop(LondonToweSphereScript sphere)
    {
        Vector3 topPosition = sphere.transform.position;
        foreach (LondonToweSphereScript spereCheck in spheres)
        {
            
            if (spereCheck.currentPoleID !=0 &&  spereCheck.currentPoleID == sphere.currentPoleID && spereCheck.transform.position.y > topPosition.y && spereCheck.orderOnPole> sphere.orderOnPole)
            {
                topPosition = spereCheck.transform.position;
               //Debug.Log("toher sphere is higher");
            }
        }
        if (topPosition.y != sphere.transform.position.y)
        {
            sphere.transform.position = (topPosition + new Vector3(0, 0.5f, 0));
        }
    }


    

    /// <summary>
    /// call at level start, create all necessary steps to load level, take screen etc
    /// </summary>
    public void StartGame()
    {
        MGC.Instance.getMinigameStates().SetPlayed(MGC.Instance.selectedMiniGameName, MGC.Instance.selectedMiniGameDiff);
        
        LondonToweXMLGameLoader loader = new LondonToweXMLGameLoader();

         
        foreach (TextAsset gameData in xmlLevels)
        {
            data.AddRange(loader.ParseXmlTextAsset(gameData));
        }
       
        // numbers 0, 1,2 ; 0 easier - only 3 spheres on poles
       
        int numberOfSpehre =  MGC.Instance.selectedMiniGameDiff +3;
      
        
        List<LondonToweGameStartWinData> filteredDificulty = new List<LondonToweGameStartWinData>();
        foreach (LondonToweGameStartWinData game in data)
        {
            if((game.pole1.Count + game.pole2.Count + game.pole3.Count)== numberOfSpehre) {
                filteredDificulty.Add(game);
            }
        }

        
       

       levelSet = filteredDificulty[Random.Range(0, filteredDificulty.Count-1)].GameID;

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
       
        poles = new List<LondonTowePoleScript>(FindObjectsOfType<LondonTowePoleScript>());
        SetLevel(startGame);


        foreach (LondonToweSphereScript sphere in spheres)
        {
            sphere.gameManager = this;
        }

        foreach (LondonTowePoleScript pole in poles)
        {
            pole.gameManager = this;
        }
        state = LondonTowerGameState.game;
    
    }



    public void GameEnd()
    {
        LondonTowerGameManager.state = LondonTowerGameState.winGUI;
        minimap.SetActive(false);
        MGC.Instance.WinMinigame();
    }

   
}




/// <summary>
/// game state enum
/// </summary>
public enum LondonTowerGameState {  start = 0, game = 4, winGUI =5 };
