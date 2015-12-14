/**
 * @file GameScript.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

namespace FindIt
{
    /**
     * Game logic for FindIt
     */
    public class GameScript : MonoBehaviour
    {
        // Limits for the game. It does not support more or less pieces. 
        const int MIN_PIECES = 8;
        const int MAX_PIECES = 44;

        // This is a good count
        const int PIECES_RECOMMENDED = 20;

        // ortographic camera size according to the number of pieces. 
        const float CAMERA_SIZE_LESS_28 = 11.5f;
        const float CAMERA_SIZE_MORE_28 = 15.5f;

        // textures of images with which is the game played
        private Texture2D[] images;


        // name of the resource pack
        private string resourcePackName = "";

        private bool customPackChosen = false;

        private Dictionary<int, bool> usedIndices;
        //private List<int> usedIndices = new List<int>();

        private int selectedImage = 0;

        // whether game is won. Property is public because ClickImageScript needs access to read it. 
        public bool gameWon
        {
            get;
            private set;
        }

        // number of pieces of this game
        public int numberPieces = MIN_PIECES;

        // activates when the game is won. Semitransparent.
        public GameObject clona;

        // UI visible elements
        public UnityEngine.UI.Text textProgress;
        public UnityEngine.UI.Image progressBar;

        // image in the middle. 
        public GameObject targetImage;

        /**
         * Loads resource pack in the scene
         */
        private void LoadResourcePack()
        {
            try
            {
                customPackChosen = PlayerPrefs.GetInt("custom") == 1;
                resourcePackName = PlayerPrefs.GetString("resourcePackName");
                Debug.Log("Resource pack name found as " + resourcePackName);
                
                if (customPackChosen)
                {
                    Debug.Log("Loading custom packs ");
                    List<Texture2D> list = new List<Texture2D>();
                    var allFiles = Directory.GetFiles(resourcePackName).Where(
                    p => Path.GetExtension(p).ToLower() == ".png"  || Path.GetExtension(p).ToLower() == ".jpg" ||
                         Path.GetExtension(p).ToLower() == ".jpeg" || Path.GetExtension(p).ToLower() == ".bmp" ||
                         Path.GetExtension(p).ToLower() == ".gif"  || Path.GetExtension(p).ToLower() == ".tif" );

                    Debug.Log("Found " + allFiles.Count() +" files");

                    foreach(string file in allFiles)
                    {
                        Debug.Log("Loading file " + file);
                        WWW www = new WWW("file:///" + file);
                        list.Add(www.texture);
                    }
                    images = list.ToArray<Texture2D>();
                    Debug.Log("Images contain " + images.Count() + " sprites");
                }
                else 
                {
                    images = Resources.LoadAll<Texture2D>(resourcePackName);
                }
            }
            catch (PlayerPrefsException ex)
            {
                Debug.Log("Exception occured while trying to load imaged: " + ex.Message);
                Debug.Log("Trying to load Animals set");

                resourcePackName = "Animals";
                images = Resources.LoadAll<Texture2D>(resourcePackName);
            }
            FindItStatistics.resourcePackName = resourcePackName;

            // loading number of pieces according to the set difficulty
            switch(MGC.Instance.selectedMiniGameDiff)
            {
                case 0:
                    numberPieces = 20;
                    break;
                case 1:
                    numberPieces = 44;
                    break;
                default:
                    numberPieces = PIECES_RECOMMENDED;
                    break;
            }
            FindItStatistics.numberPieces = numberPieces;

            FindItStatistics.expectedGameTurnsTotal = numberPieces;
        }


        /**
         * Sets up tiles textures in the scene
         * @param numPictures number of images
         */
        private void SetUpSprites(int numPictures)
        {
            usedIndices = new Dictionary<int, bool>();

            System.Random random = new System.Random();

            usedIndices.Clear();

            for (int i = 1; i <= numPictures; i++)
            {
                int index = random.Next(images.Length);
                //while (usedIndices.Contains(index))
                while (usedIndices.ContainsKey(index))
                {
                    index++;
                    if (index == images.Length) index = 0;
                }
                usedIndices.Add(index,false);

                GameObject o = GameObject.Find(i.ToString());
                o.SetActive(true);
                o.GetComponent<Renderer>().material.mainTexture = images[index];
            }
            for(int i=numPictures+1;i<=MAX_PIECES;i++)
            {
                GameObject.Find(i.ToString()).SetActive(false);
            }
        }

        /**
         * Modifies camera such that it properly fits objects in the scene
         */
        private void UpdateCameraSize()
        {
            Camera.main.orthographicSize = numberPieces <= 28 ? CAMERA_SIZE_LESS_28 : CAMERA_SIZE_MORE_28;
        }

        /**
         * Chooses new target image
         */
        public void newTargetImage()
        {
            CheckEndGame();
            UpdateGreenBarAndText();
            if(!gameWon)
            { 
                System.Random r = new System.Random();

                int chosen = r.Next(0, numberPieces);

                while (usedIndices.ElementAt(chosen).Value)
                {
                    chosen++;
                    if (chosen == numberPieces) chosen = 0;
                }
                int key = usedIndices.ElementAt(chosen).Key;
                usedIndices.Remove(key);
                usedIndices.Add(key, true);
                selectedImage = key;
                targetImage.GetComponent<Renderer>().material.mainTexture = images[selectedImage];
            }
        }

        /**
         * UnityEngine Start() function
         */
        void Start()
        {
			clona.SetActive(false);
			gameWon = false;
            FindItStatistics.Clear();
            FindItStatistics.StartMeasuringTime();
            LoadResourcePack();
            SetUpSprites(numberPieces);
            UpdateCameraSize();
            newTargetImage();
            
			MGC.Instance.minigamesProperties.SetPlayed (SceneManager.GetActiveScene().name);
        }

        /**
         * Checks for end of the game
         */
        void CheckEndGame()
        {
            if (FindItStatistics.turnsPassed == FindItStatistics.expectedGameTurnsTotal/* && !gameWon*/)
            {
                gameWon = true;
                FindItStatistics.StopMeasuringTime();
                clona.SetActive(true);

				MGC.Instance.WinMinigame();
            }
        }

        /**
         * Updates progress
         */
        void UpdateGreenBarAndText()
        {
            progressBar.GetComponent<RectTransform>().localScale = new Vector3((float)FindItStatistics.turnsPassed / (float)FindItStatistics.expectedGameTurnsTotal * (float)Screen.width / (float)Screen.height, 1f, 0f);
            textProgress.text = FindItStatistics.turnsPassed.ToString() + "/" + FindItStatistics.expectedGameTurnsTotal.ToString();            
        }
    }
}
