using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace MinigamePexeso 
{
	public class ResourcePack : MonoBehaviour 
    {

        public GameStart gameStart;
        public GameScript mainGameScript;
        public GameObject gameTilePrefab;

        //TODO find better solution...
        private string[] resPacksNames;
        private string resPackPath;

        private GameObject[] gameTiles;

	    /// <summary>
	    /// Number of rows of menu items.
	    /// </summary>
	    private int menuRows = 2;
	    /// <summary>
	    /// Number of columns of menu items.
	    /// </summary>
	    private int menuColumns = 2;

        private Ray ray;
        private RaycastHit hit;


		void Start ()
	    {
            //TODO find better solution for resource packs path and names
            resPackPath = "Textures/Pictures/";
            
            resPacksNames = new string[2];
            resPacksNames[0] = "Animals";
            resPacksNames[1] = "Landscapes";

            //no need of confusing tiles
            menuRows = 1;

	        CreateMenu();
		}

	    public void CreateMenu()
	    {
	        if (mainGameScript.enabled)
	        {
	            mainGameScript.enabled = false;
	        }

            gameTiles = GameTiles.createTiles(menuRows, menuColumns, gameTilePrefab, "PicMenuItem");

	        //string[] resourcePacks = Directory.GetDirectories(Environment.CurrentDirectory + "\\Assets\\Minigames\\Pexeso\\Resources\\Textures\\Pictures\\");
            int resPackCount = resPacksNames.Length;
            //print("Number of resource packs: " + resPackCount);

            //iterate through all tiles
            for (int i = 0; i < menuColumns * menuRows; i++)
            {
                //while we have some resource packs left
                if (i < resPackCount)
                {
                    print("Resource pack name: '" + resPacksNames[i] + "'");
                    
                    //set name of game-object (will be used later as chosen resource-pack identifier]
                    gameTiles[i].name = resPacksNames[i];

                    //use first image in pack as tile texture
                    gameTiles[i].transform.GetChild(0).renderer.material.mainTexture = Resources.Load(resPackPath + resPacksNames[i] + "/00") as Texture2D;
                }

                //tiles without resource packs 
                else
                {
                    gameTiles[i].name = "Empty";

                    //use "back" texture...
                    //TODO hard-code path here...
                    gameTiles[i].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("Textures/back") as Texture2D;
                }
            }
        }

		void Update ()
	    {
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        if (Physics.Raycast(ray, out hit))
	        {
	            if (Input.GetMouseButtonUp(0) && hit.collider.tag == "PicMenuItem" && hit.collider.name != "Empty")
	            {
	                StartCoroutine(DropOther(hit.collider.gameObject, gameTiles));
	            }
	        }
		}

	    /// <summary>
	    /// COROUTINE. Not selected buttons fall down.
	    /// </summary>
	    /// <param name="chosenButton">Chosen button.</param>
	    /// <param name="buttons">Buttons.</param>
	    private IEnumerator DropOther(GameObject chosenButton, GameObject[] buttons)
	    {
	        float t = 0;
	        
	        for (int i = 0; i < buttons.Length; i++)
	        {
	            if(chosenButton != buttons[i])
	            {
                    buttons[i].rigidbody.isKinematic = false;
	                buttons[i].rigidbody.useGravity = true;
	                buttons[i].rigidbody.AddForce(chosenButton.transform.position * (-100));
	            }
	        }
	        
	        while (t < 1)
	        {
	            t += Time.deltaTime;
	            yield return null;
	        }
	        StartCoroutine(SelectButton(chosenButton));
	        //yield return 0;
	    }
	    
	    /// <summary>
	    /// COROUTINE. Selected button flies towards camera. Also starts new game.
	    /// </summary>
	    /// <param name="chosenButton">Chosen button.</param>
	    /// <param name="buttons">Buttons.</param>
	    private IEnumerator SelectButton(GameObject chosenButton)
	    {
	        Vector3 startPosition = chosenButton.transform.position;
	        float t = 0;
	        
	        Vector3 endPosition = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x,
	                                          Camera.main.ViewportToWorldPoint(Vector3.zero).y,
	                                          Camera.main.ViewportToWorldPoint(Vector3.zero).z);
	        
	        while (t - 0.9f < 0)
	        {
	            t += Time.deltaTime * 2;
	            chosenButton.transform.position = Vector3.Lerp(startPosition, endPosition, t);
	            yield return null;
	        }

	        CreateMainGameObject(chosenButton);
	        chosenButton.transform.position = endPosition;
	        
	        //yield return 0;
	    }

	    private void CreateMainGameObject(GameObject chosenButton)
	    {
            //destroy used tiles 
            //TODO minor memory waste - tiles objects can be reused...
	        for (int i = 0; i < menuColumns*menuRows; i++)
	        {
                GameObject.Destroy(gameTiles[i]);
	        }
            print("Chosen resource pack: " + chosenButton.name);
	        mainGameScript.resourcePack = chosenButton.name;

	        gameStart.enabled = true;
            this.gameObject.SetActive(false);
	        gameStart.CreateMenu();
	    }
	}
}