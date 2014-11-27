using UnityEngine;
using System;
using System.Collections;

namespace MinigamePexeso 
{
	public class GameStart : MonoBehaviour 
    {
        public GameScript mainGameScript;
	    
        public GameObject gameTilePrefab;

        private GameObject[] gameTiles;

	    /// <summary>
	    /// Number of rows of menu items.
	    /// </summary>
	    private int menuRows = 2;

        /// <summary>
	    /// Number of columns of menu items.
	    /// </summary>
	    private int menuColumns = 2;

        /// <summary>
        /// Used for mouse click detection
        /// </summary>
        private Ray ray;
        private RaycastHit hit;



        /// <summary>
        /// creates "menu" for selecting size of board
        /// </summary>
	    public void CreateMenu()
	    {
            gameTiles = GameTiles.createTiles(menuRows, menuColumns, gameTilePrefab, "MenuItem");

	        gameTiles [0].name = "2x2";
            gameTiles[0].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("Textures/Menu/2x2") as Texture2D;
			gameTiles [1].name = "2x3";
            gameTiles[1].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("Textures/Menu/2x3") as Texture2D;
			gameTiles [2].name = "3x4";
            gameTiles[2].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("Textures/Menu/3x4") as Texture2D;
			gameTiles [3].name = "4x4";
            gameTiles[3].transform.GetChild(0).renderer.material.mainTexture = Resources.Load("Textures/Menu/4x4") as Texture2D;
	    }
		

		// Update is called once per frame
		void Update ()
	    {
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        if (Physics.Raycast(ray, out hit))
	        {
	            if (Input.GetMouseButtonUp(0) && hit.collider.tag == "MenuItem")
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
					if(buttons[i] != null)
					{
						buttons[i].rigidbody.isKinematic = false;
		                buttons[i].rigidbody.useGravity = true;
		                buttons[i].rigidbody.AddForce(chosenButton.transform.position * (-100));
					}
	            }
	        }

	        while (t < 1)
	        {
	            t += Time.deltaTime;
	            yield return null;
	        }
			CreateMainGameObject(chosenButton);
	    }

	    /// <summary>
	    /// COROUTINE. Selected button flies towards camera. Also starts new game.
	    /// </summary>
	    /// <param name="chosenButton">Chosen button.</param>
	    /// <param name="buttons">Buttons.</param>
/*	    private IEnumerator SelectButton(GameObject chosenButton)
	    {
	        //Vector3 startPosition = chosenButton.transform.position;
	        float t = 0;
	        
	        /*Vector3 endPosition = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x,
	                                  Camera.main.ViewportToWorldPoint(Vector3.zero).y,
	                                  Camera.main.ViewportToWorldPoint(Vector3.zero).z);*/

//			Color backTextureColor = chosenButton.transform.GetChild(0).renderer.material.color;
//			backTextureColor.a = 0;
//			chosenButton.transform.GetChild(1).renderer.material.color = backTextureColor;
			
/*			while (t - 1f < 0)
			{
				t += Time.deltaTime * 2;
				
				Color frontTextureColor = chosenButton.transform.GetChild(0).renderer.material.color;
				Color backColor = chosenButton.renderer.material.color;
				
				frontTextureColor.a = 1f - t;
				backColor.a = 1f - t;
				
				chosenButton.transform.GetChild(0).renderer.material.color = frontTextureColor;
				chosenButton.renderer.material.color = backColor;
				
				//chosenButton.transform.position = Vector3.Lerp(startPosition, endPosition, t);
				yield return null;
			}

	        CreateMainGameObject(chosenButton);
	        //chosenButton.transform.position = endPosition;
	    }
*/
	    /// <summary>
	    /// Creates new game by setting game dimensions and starting main game script.
	    /// </summary>
	    /// <param name="chosenButton">Chosen button.</param>
	    private void CreateMainGameObject(GameObject chosenButton)
	    {
            Debug.Log("Selected board size: " + chosenButton.name);
			MGC.Instance.minigameStates.SetPlayed (Application.loadedLevelName);
			//MGC.Instance.SaveGame ();
            string[] dimensions = chosenButton.name.Split('x');
	        int rows = Int32.Parse(dimensions [0]);
	        int columns = Int32.Parse(dimensions [1]);

	        for (int i = 0; i < menuColumns*menuRows; i++)
	        {
                GameObject.Destroy(gameTiles[i]);
	        }

            if (mainGameScript != null)
            {
                mainGameScript.rows = rows;
                mainGameScript.columns = columns;
                mainGameScript.enabled = true;
                mainGameScript.CreateGameBoard();

                AudioSource musicPlayer = GameObject.Find("MusicPlayer").GetComponent("AudioSource") as AudioSource;
                if (musicPlayer == null)
                {
                    Debug.Log("ERROR");
                }
                musicPlayer.Play();
            }
            else
            {
                Debug.LogError("Main game script not assigned");
            }
	    }
	}
}