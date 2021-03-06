﻿using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace MinigameMaze
{
	/// <summary>
	/// Main game script. Assures the game functionality, i.e. checks for hits, changes textures etc.
	/// </summary>
	public class GameScript : MonoBehaviour
	{
        /// <summary>
        /// Level difficulty
        /// </summary>
        public int difficulty;
        
        /// <summary>
		/// The mazes textures array.
		/// </summary>
		public Texture2D[] mazes;

		/// <summary>
		/// Texture that is displayed if game is won.
		/// </summary>
		private Texture2D winScreen;

		/// <summary>
		/// Texture that is displayed if game is lost.
		/// </summary>
		private Texture2D looseScreen;

		/// <summary>
		/// Is slound enabled?
		/// </summary>
		public bool enableSound;

		/// <summary>
		/// Maze texture.
		/// </summary>
		private Texture2D mazeTexture;
		
		/// <summary>
		/// Used for mouse pointer detection.
		/// </summary>
		private Ray ray;
		/// <summary>
		/// Used for mouse pointer detection. Object under mouse cursor.
		/// </summary>
		private RaycastHit hit;

		/// <summary>
		/// The size of the player.
		/// </summary>
		public int playerSize;

		/// <summary>
		/// Is game running?
		/// </summary>
		private bool gameRunning = false;

		/// <summary>
		/// The custom mazes textures path.
		/// </summary>
		private string customMazesPath = "\\CustomImages\\";

		// Use this for initialization
		void Start ()
		{
            difficulty = MGC.Instance.selectedMiniGameDiff;
            
            //load all maze textures
			LoadTextures ();
			//choose random maze texture and assign it to canvas
			int number = UnityEngine.Random.Range (0, mazes.Length);

            //number = 0;

			Debug.Log ("Mazes length: " + mazes.Length);
			mazeTexture = mazes[number];
			transform.GetComponent<Renderer>().material.mainTexture = mazeTexture;

            Debug.Log("Chosen maze:" + mazeTexture);

			gameRunning = true;

			if (enableSound)
			{
				AudioSource musicPlayer = GameObject.Find ("MusicPlayer").GetComponent ("AudioSource") as AudioSource;
				musicPlayer.Play ();
			}
		}

		/// <summary>
		/// Load all maze textures.
		/// </summary>
		private void LoadTextures()
		{
            mazes = Resources.LoadAll<Texture2D>("Textures/Pictures/" + difficulty);
			winScreen = Resources.Load ("Textures/winScreen") as Texture2D;
			looseScreen = Resources.Load ("Textures/looseScreen") as Texture2D;

			StartCustomImagesLoad ();
		}
		
		// Update is called once per frame
		void Update ()
		{
			if (gameRunning)
			{
				if(Input.GetMouseButton(0)) //if left mouse button is pressed
				{
					ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					Vector3 position = new Vector3(0, 1, -10);
					if (Physics.Raycast (position, ray.direction, out hit)) //calculate raycast and check hit
					{
                        if (hit.collider.name == "PlayerObject" || hit.collider.name == "Holder") //if raycast hit player
						{
                                    position = GameObject.Find("PlayerObject").transform.position + (ray.direction.normalized * 0.001f);
                                    position.z = (hit.point + (ray.direction.normalized * 0.001f)).z;

									if(Physics.Raycast(position, ray.direction, out hit)) //new raycast with same direction and new position
									{
                                        //Hit player and now canvas
										//if maze texture is assigned, play normal game turn
										if (transform.GetComponent<Renderer>().material.mainTexture == mazeTexture)
										{
											checkForCollision ();
										}
									}
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks if cursor(player) is on maze road, in wall, or in target zone.
		/// </summary>
		private void checkForCollision()
		{
			Texture2D textureHit = hit.collider.GetComponent<Renderer>().material.mainTexture as Texture2D;

			for(int i = -playerSize; i <= playerSize; i++)
			{
				for(int j = -playerSize; j <= playerSize; j++)
				{
					Vector2 pixelUV = hit.textureCoord;

					int textureX = (int)((pixelUV.x * mazeTexture.width) + i);
					int textureY = (int)((pixelUV.y * mazeTexture.height) + j);

					if(textureX >= 0 & textureY >= 0)
					{
						Color surfaceColor = textureHit.GetPixel((int)((pixelUV.x * mazeTexture.width) + i), (int)((pixelUV.y * mazeTexture.height) + j));

						if(surfaceColor == Color.black)
						{
							LostGame ();
						}
						else if(surfaceColor == Color.green)
						{
							WinGame();
						}
					}
				}
			}
		}

		/// <summary>
		/// Called when player looses the game.
		/// </summary>
		private void LostGame()
		{
			gameRunning = false;
			transform.GetComponent<Renderer>().material.mainTexture = looseScreen;

			//stop game music and play lost sound
			if (enableSound)
			{
				AudioSource musicPlayer = GameObject.Find ("MusicPlayer").GetComponent ("AudioSource") as AudioSource;
				musicPlayer.Stop ();
				AudioSource soundPlayerLoose = GameObject.Find ("SoundPlayerLoose").GetComponent ("AudioSource") as AudioSource;
				soundPlayerLoose.Play();
			}
			
			//StartCoroutine (RestartGame ());
            MGC.Instance.LoseMinigame();
		}

		/// <summary>
		/// Called when player wins the game.
		/// </summary>
		private void WinGame()
		{
			gameRunning = false;
			transform.GetComponent<Renderer>().material.mainTexture = winScreen;

			//stop game music and play wictory sound
			if (enableSound)
			{
				AudioSource musicPlayer = GameObject.Find ("MusicPlayer").GetComponent ("AudioSource") as AudioSource;
				musicPlayer.Stop ();
				this.gameObject.GetComponent<AudioSource>().Play ();
			}

            MGC.Instance.WinMinigame();
			//StartCoroutine (RestartGame ());
		}

		/// <summary>
		/// COROUTINE. Restarts the game.
		/// </summary>
		public IEnumerator RestartGame()
		{
			//wait some time
			yield return new WaitForSeconds(2f);
			Debug.Log("Restarting game");
			Start ();
		}

		/// <summary>
		/// Determines if there are any custom rmazes.
		/// </summary>
		/// <returns>Number of custom mazes.</returns>
		private int GetCustomResroucePacksCount()
		{
			//sort of tests, if directories exists
			Directory.CreateDirectory(Environment.CurrentDirectory + customMazesPath + "//" + difficulty + "//");
            return Directory.GetFiles(Environment.CurrentDirectory + customMazesPath + "//" + difficulty + "//").Length;
		}

		/// <summary>
		/// Get custom mazes.
		/// </summary>
		private void StartCustomImagesLoad ()
		{		
			//Number of default resource packs
			int mazeTexturesCount = mazes.Length;
			
			//Check for custom mazes
			#if UNITY_EDITOR
			print("code for editor");
			#endif
			
			#if UNITY_STANDALONE_WIN
			mazeTexturesCount += GetCustomResroucePacksCount();
			Debug.Log("Running on WIN, found " + mazeTexturesCount + " mazes (" + mazes.Length + " are default)");
			#endif

			//Add custom resoruce packs
			#if UNITY_STANDALONE_WIN
			StartCoroutine(LoadCustomMazes());
			//LoadCustomMazes();
			#endif
		}

		/// <summary>
		/// Loads the custom mazes from external files.
		/// </summary>
		/// <returns>The custom mases.</returns>
		private IEnumerator LoadCustomMazes()
		{
			Texture2D[] defaultMazes = mazes;


            string[] customMazes = Directory.GetFiles(Environment.CurrentDirectory + customMazesPath + difficulty + Path.DirectorySeparatorChar, "*.png");
            Debug.Log("custom test:" + customMazes[0]);

			Texture2D[] images = new Texture2D[customMazes.Length];
			WWW www;

			for (int i = 0; i < customMazes.Length; i++)
			{
				www = new WWW ("file://" + customMazes[i]);
				//yield return www;
				images[i] = www.texture as Texture2D;
			}

			mazes = new Texture2D[defaultMazes.Length + images.Length];
			Array.Copy(defaultMazes, mazes, defaultMazes.Length);
			Array.Copy(images, 0, mazes, defaultMazes.Length, images.Length);
			Debug.Log ("Mazes length: " + mazes.Length);

			yield return 0;
		}
	}
}
