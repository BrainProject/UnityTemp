﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace MinigamePexeso 
{
	public enum GameType { Pexeso, Similarity, Silhouette };

    /// <summary>
    /// Creates a tile for each image-set (resourcePack) and lets player choose which set will be used for game
    /// Set up difficulty and start main game script
    /// </summary>
    /// @author Michal Hroš
    /// @author Jiří Chmelík
	public class GameSetup : MonoBehaviour 
    {
        /// <summary>
        /// The current game type.
        /// </summary>
		public GameType currentGame = GameType.Similarity;

        ///// <summary>
        ///// GameStart script
        ///// </summary>
        //public GameStart gameStart;

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
        private string customResPackPath;

		/// <summary>
		/// Menu tiles.
		/// </summary>
        private GameObject[] gameTiles;

	    /// <summary>
	    /// Number of rows of menu items.
	    /// </summary>
		private int menuRows;

	    /// <summary>
	    /// Number of columns of menu items.
	    /// </summary>
		private int menuColumns;

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
            print("Getting Custom Resource Packs...");
            //sort of tests, if directories exists
            Directory.CreateDirectory(customResPackPath);
            //Debug.Log("___directory: " + dInfo.FullName + " should exists now");
            DirectoryInfo dInfo = Directory.CreateDirectory(customResPackPath + "/" + currentGame);
            print("___customResrourcePack path: '" + dInfo.FullName + "'");
            MGC.Instance.logger.addEntry("!!! CustomResrourcePack path: " + dInfo.FullName);

            return Directory.GetDirectories(customResPackPath + "/" + currentGame + "/").Length;
		}

		/// <summary>
		/// Loads the custom resource packs from external files.
		/// </summary>
		/// <returns>The custom resource packs.</returns>
		private IEnumerator LoadCustomResourcePacks()
		{
            string[] customResourcePacks = Directory.GetDirectories(customResPackPath + "/" + currentGame + "/");

			//Run from 0 to number of menu items minus default resource packs
			for (int i = 0; i < (menuColumns * menuRows) - resPacksNames.Length; i++)
			{
				//while we have some resource packs left
				if (i < customResourcePacks.Length)
				{
					//set name of game-object (will be used later as chosen resource-pack identifier]
					string[] s = customResourcePacks[i].Split('/');
					Debug.Log("Custom resource pack name: '" + s[s.Length - 1] + "'");
					gameTiles[i + resPacksNames.Length].name = "CUSTOM-" + s[s.Length - 1];

					//use first image in pack as tile texture
					Debug.Log ("file:///" + customResourcePacks[i] + "/00.png");
					WWW www = new WWW ("file:///" + customResourcePacks[i] + "/00.png");
					yield return www;
					gameTiles[i + resPacksNames.Length].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = www.texture;

					if (www.error != null)
					{
						Debug.Log("Custom resource pack '" + s[s.Length - 1] + "' is not valid");
						Destroy(gameTiles[i + resPacksNames.Length]);
					}
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
            customResPackPath = MGC.Instance.getPathtoCustomImageSets();

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
	        CreateResourcePacksIcons();
		}

		/// <summary>
		/// Creates the menu and loads resource packs.
		/// </summary>
	    public void CreateResourcePacksIcons()
	    {
            gameTiles = GameTiles.createTiles(menuRows, menuColumns, gameTilePrefab, "PicMenuItem");

	        if (mainGameScript.enabled)
	        {
	            mainGameScript.enabled = false;
	        }

            int resPackCount = resPacksNames.Length;
            print("Number of standard res. packs: " + resPackCount);
            print("Number of game tiles: " + gameTiles.Length);

		    for (int i = 0; i < menuColumns*menuRows; i++)
		    {
		        //while we have some resource packs left
		        if (i < resPackCount)
		        {
		            print("Resource pack name: '" + resPacksNames[i] + "'");

		            //set name of game-object (will be used later as chosen resource-pack identifier]
		            //string[] s = resourcePacks[i].Split('\\');
		            gameTiles[i].name = resPacksNames[i];

		            //use first image in pack as tile texture
		            gameTiles[i].transform.GetChild(0).GetComponent<Renderer>().material.mainTexture =
		                Resources.Load(resPackPath + resPacksNames[i] + "/00") as Texture2D;
		        }
                
                #if UNITY_ANDROID
                else
                {
                    Destroy(gameTiles[i]);
                }
                #endif
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
	        for (int i = 0; i < buttons.Length; i++)
	        {
	            if(chosenButton != buttons[i])
	            {
                    if(buttons[i] != null)
					{
						buttons[i].GetComponent<Rigidbody>().isKinematic = false;
		                buttons[i].GetComponent<Rigidbody>().useGravity = true;
		                buttons[i].GetComponent<Rigidbody>().AddForce(Vector3.forward * 250);
                        buttons[i].GetComponent<Rigidbody>().AddTorque(Vector3.right * 100);
					}
	            }
	        }
	        
			yield return new WaitForSeconds(1.0F);

			startGame(chosenButton);
	    }
	    

		/// <summary>
		/// Player selected resource pack. Set difficulty and start game itself
		/// and pass some parameters.
		/// </summary>
		/// <param name="chosenButton">Chosen button.</param>
	    private void startGame(GameObject chosenButton)
	    {
            //destroy used tiles 
            //TODO minor memory waste - tiles objects can be reused...
	        for (int i = 0; i < menuColumns*menuRows; i++)
	        {
                GameObject.Destroy(gameTiles[i]);
	        }

			//if user selected custom resource pack
			if (chosenButton.name.Contains ("CUSTOM-"))
			{
				mainGameScript.customPack = true;
				chosenButton.name = chosenButton.name.Remove(0, 7);
			}

            Debug.Log("Chosen resource pack: " + chosenButton.name);

			//Difficulty
			int diff = MGC.Instance.selectedMiniGameDiff;
			int rows;
			int columns;

			switch (diff)
			{
				case 0:
				{
					rows = 2;
					columns = 2;
					break;
				}

				case 1:
				{
					rows = 2;
					columns = 3;
					break;
				}

                // the number of tiles have to be even...
                //case 2:
                //{
                //    rows = 3;
                //    columns = 3;
                //    break;
                //}

				case 2:
				{
					rows = 3;
					columns = 4;
					break;
				}

				case 3:
				{
					rows = 4;
					columns = 4;
					break;
				}

                // not enought pictures in resource packs...
                //case 4:
                //{
                //    rows = 4;
                //    columns = 5;
                //    break;
                //}

                default:
                {
                    rows = 3;
                    columns = 4;
                    break;
                }
			}

            print("Chosen diff: " + diff);
            print("rows: " + rows + "; columns: " + columns);
			
			if (mainGameScript != null)
			{
				//pass parameters to main game script
				mainGameScript.resourcePack = chosenButton.name;
				mainGameScript.currentGame = currentGame;
				mainGameScript.rows = rows;
				mainGameScript.columns = columns;
				mainGameScript.enabled = true;

				
				if(mainGameScript.enableSound)
				{
					AudioSource musicPlayer = GameObject.Find("MusicPlayer").GetComponent("AudioSource") as AudioSource;
					if (musicPlayer == null)
					{
						Debug.Log("ERROR");
					}
					musicPlayer.Play();
				}

                // start it, finally
				mainGameScript.CreateGameBoard();

                // update statistics
                MGC.Instance.minigamesProperties.SetPlayed(MGC.Instance.selectedMiniGameName, MGC.Instance.selectedMiniGameDiff);
                
                // log it...
                MGC.Instance.logger.addEntry("New game starts with: " + rows*columns + " tiles");
			}
			else
			{
				Debug.LogError("Main game script not assigned");
			}
	    }
	}
}