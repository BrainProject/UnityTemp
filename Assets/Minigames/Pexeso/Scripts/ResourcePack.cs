﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace MinigamePexeso 
{
	public enum GameType { Pexeso, Similarity, Silhouette };

    /// <summary>
    /// Creates a tile for each image-set (resourcePack) and lets player choose which set will be used for game
    /// </summary>
    /// @author Michal Hroš
	public class ResourcePack : MonoBehaviour 
    {
        /// <summary>
        /// The current game type.
        /// </summary>
		public GameType currentGame = GameType.Similarity;

		/// <summary>
		/// GameStart script
		/// </summary>
        public GameStart gameStart;

		/// <summary>
		/// Main game script (GameScript)
		/// </summary>
        public GameScript mainGameScript;

		/// <summary>
		/// The game tile prefab.
		/// </summary>
        public GameObject gameTilePrefab;

        //TODO find better solution...
        public string[] resPacksNames;

        private string resPackPath;
        private string customResPackPath = "\\CustomImages\\";

		/// <summary>
		/// Menu tiles.
		/// </summary>
        private GameObject[] gameTiles;

	    /// <summary>
	    /// Number of rows of menu items.
	    /// </summary>
		private int menuRows;// = 2;

	    /// <summary>
	    /// Number of columns of menu items.
	    /// </summary>
		private int menuColumns;// = 2;

		/// <summary>
		/// Used for mouse click detection
		/// </summary>
        private Ray ray;
        private RaycastHit hit;

		/// <summary>
		/// Calculates proper number of rows and columns in menu.
		/// </summary>
		/// <returns>The menu dimensions.</returns>
		/// <param name="elementsCount">Elements count.</param>
		public int[] GetMenuDimensions(int elementsCount)
		{
			if (elementsCount == 1)
			{
				return new int[]{1, 1};
			} else if (elementsCount == 2)
			{
				return new int[]{1,2};
			} else if (elementsCount <= 4)
			{
				return new int[]{2,2};
			} else if (elementsCount <= 6)
			{
				return new int[]{2,3};
			} else if (elementsCount <= 9)
			{
				return new int[]{3,3};
			} else if (elementsCount <= 12)
			{
				return new int[]{3,4};
			} else
			{
				return new int[]{4,4};
			}
		}

		/// <summary>
		/// Determines if there are any custom resource packs.
		/// </summary>
		/// <returns>Number of custom resource packs.</returns>
		private int GetCustomResroucePacksCount()
		{
            //sort of tests, if directories exists
            Directory.CreateDirectory(Environment.CurrentDirectory + customResPackPath);
            Directory.CreateDirectory(Environment.CurrentDirectory + customResPackPath + "\\" + currentGame);
            
            return Directory.GetDirectories(Environment.CurrentDirectory + customResPackPath + currentGame + "\\").Length;
		}

		/// <summary>
		/// Loads the custom resource packs from external files.
		/// </summary>
		/// <returns>The custom resource packs.</returns>
		private IEnumerator LoadCustomResourcePacks()
		{
            string[] customResourcePacks = Directory.GetDirectories(Environment.CurrentDirectory + customResPackPath + currentGame + "\\");

			//Run from 0 to number of menu items minus default resource packs
			for (int i = 0; i < (menuColumns * menuRows) - resPacksNames.Length; i++)
			{
				//while we have some resource packs left
				if (i < customResourcePacks.Length)
				{
					//set name of game-object (will be used later as chosen resource-pack identifier]
					string[] s = customResourcePacks[i].Split('\\');
					Debug.Log("Custom resource pack name: '" + s[s.Length - 1] + "'");
					gameTiles[i + resPacksNames.Length].name = "[CUSTOM]" + s[s.Length - 1];

					//use first image in pack as tile texture
					Debug.Log ("file://" + customResourcePacks[i] + "\\00.png");
					WWW www = new WWW ("file://" + customResourcePacks[i] + "\\00.png");
					yield return www;
					gameTiles[i + resPacksNames.Length].transform.GetChild(0).renderer.material.mainTexture = www.texture;
				}
				//destroy tiles without resource packs 
				else
				{
					Destroy(gameTiles[i + resPacksNames.Length]);
				}
			}
		}

		/// <summary>
		/// Get resource packs and create menu.
		/// </summary>
		void Start ()
	    {
            Debug.Log("Current chosen game: " + currentGame);

			resPackPath = "Textures/Pictures/" + currentGame + "/";

			//Number of default resource packs
			int menuLength = resPacksNames.Length;

			//Check for custom resource packs
            #if UNITY_EDITOR
            print("code for editor");
            #endif
            
            #if UNITY_STANDALONE_WIN
            menuLength += GetCustomResroucePacksCount();
			Debug.Log("Running on WIN, found " + menuLength + " resrouce packs (" + resPacksNames.Length + " are default)");
			#endif

			//Calculate number of menu items
			int[] menu = GetMenuDimensions (menuLength);
			Debug.Log ("Menu dimensions:" + menu[0] + "x" + menu[1]);
			menuRows = menu [0];
			menuColumns = menu [1];

			//Create menu
	        CreateMenu();
		}

		/// <summary>
		/// Creates the menu and loads resource packs.
		/// </summary>
	    public void CreateMenu()
	    {
            gameTiles = GameTiles.createTiles(menuRows, menuColumns, gameTilePrefab, "PicMenuItem");

	        if (mainGameScript.enabled)
	        {
	            mainGameScript.enabled = false;
	        }

            //string[] resourcePacks = Directory.GetDirectories(Environment.CurrentDirectory + "\\Assets\\Minigames\\Pexeso\\Resources\\Textures\\Pictures\\" + currentGame + "\\");
            //int resPackCount = resourcePacks.Length;
            int resPackCount = resPacksNames.Length;
            print("Number of standard res. packs: " + resPackCount);
            print("Number of game tiles: " + gameTiles.Length);

            for (int i = 0; i < menuColumns * menuRows; i++)
            {
                //while we have some resource packs left
                if (i < resPackCount)
                {
                    print("Resource pack name: '" + resPacksNames[i] + "'");

                    //set name of game-object (will be used later as chosen resource-pack identifier]
                    //string[] s = resourcePacks[i].Split('\\');
                    gameTiles[i].name = resPacksNames[i];

                    //use first image in pack as tile texture
                    gameTiles[i].transform.GetChild(0).renderer.material.mainTexture = Resources.Load(resPackPath + resPacksNames[i] + "/00") as Texture2D;
                }
                //tiles without resource packs 
                /*else
                {
                    Destroy(gameTiles[i]);
                }*/
            }

            //Add custom resoruce packs
            #if UNITY_STANDALONE_WIN
            StartCoroutine(LoadCustomResourcePacks());
            #endif
        }

		/// <summary>
		/// Wait for player to select menu item.
		/// </summary>
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
	        StartCoroutine(SelectButton(chosenButton));
	    }
	    
	    /// <summary>
	    /// COROUTINE. Selected button flies towards camera. Also starts new game.
	    /// </summary>
	    /// <param name="chosenButton">Chosen button.</param>
	    /// <param name="buttons">Buttons.</param>
	    private IEnumerator SelectButton(GameObject chosenButton)
	    {
	        //Vector3 startPosition = chosenButton.transform.position;
	        float t = 0;
	        
	        /*Vector3 endPosition = new Vector3(Camera.main.ViewportToWorldPoint(Vector3.zero).x,
	                                          Camera.main.ViewportToWorldPoint(Vector3.zero).y,
	                                          Camera.main.ViewportToWorldPoint(Vector3.zero).z);*/

			Color backTextureColor = chosenButton.transform.GetChild(0).renderer.material.color;
			backTextureColor.a = 0;
			chosenButton.transform.GetChild(1).renderer.material.color = backTextureColor;

	        while (t - 1f < 0)
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

		/// <summary>
		/// Player selected resource pack. Enable gameScript menu (size selection)
		/// and pass some parameters.
		/// </summary>
		/// <param name="chosenButton">Chosen button.</param>
	    private void CreateMainGameObject(GameObject chosenButton)
	    {
            //destroy used tiles 
            //TODO minor memory waste - tiles objects can be reused...
	        for (int i = 0; i < menuColumns*menuRows; i++)
	        {
                GameObject.Destroy(gameTiles[i]);
	        }

			//if user selected custom resource pack
			if (chosenButton.name.Contains ("[CUSTOM]"))
			{
				mainGameScript.customPack = true;
				chosenButton.name = chosenButton.name.Remove(0, 8);
			}

            Debug.Log("Chosen resource pack: " + chosenButton.name);

	        mainGameScript.resourcePack = chosenButton.name;
			mainGameScript.currentGame = currentGame;


	        gameStart.enabled = true;
            this.gameObject.SetActive(false);
	        gameStart.CreateMenu();
	    }
	}
}