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

namespace FindIt
{
    /**
     * Game logic for FindIt
     */
    public class GameScript : MonoBehaviour
    {
        const int MIN_PIECES = 8;
        const int MAX_PIECES = 44;

        const int PIECES_RECOMMENDED = 20;

        const float CAMERA_SIZE_LESS_28 = 11.5f;
        const float CAMERA_SIZE_MORE_28 = 15.5f;


        private Sprite[] images;
        private string resourcePackName = "";
        public int numberPieces = MIN_PIECES;
        public int numberTurns = 50;
        private bool customPackChosen = false;

        private List<int> usedIndices = new List<int>();

        private int selectedImage = 0;

		public GameObject clona;

        public UnityEngine.UI.Text textProgress;
        public UnityEngine.UI.Image progressBar;

        public GameObject targetImage;

		private bool gameWon = false;

        /**
         * Loads resource pack to the scene
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
                    List<Sprite> list = new List<Sprite>();
                    var allFiles = Directory.GetFiles(resourcePackName).Where(
                    p => Path.GetExtension(p).ToLower() == ".png"  || Path.GetExtension(p).ToLower() == ".jpg" ||
                         Path.GetExtension(p).ToLower() == ".jpeg" || Path.GetExtension(p).ToLower() == ".bmp" ||
                         Path.GetExtension(p).ToLower() == ".gif"  || Path.GetExtension(p).ToLower() == ".tif" );

                    Debug.Log("Found " + allFiles.Count() +" files");

                    foreach(string file in allFiles)
                    {
                        Debug.Log("Loading file " + file);
                        WWW www = new WWW("file://" + file);
                        Sprite s = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
                        list.Add(s);
                    }
                    images = list.ToArray<Sprite>();
                    Debug.Log("Images contain " + images.Count() + " sprites");
                }
                else 
                {
                    images = Resources.LoadAll<Sprite>(resourcePackName);
                }
            }
            catch (PlayerPrefsException ex)
            {
                Debug.Log("Exception occured while trying to load imaged: " + ex.Message);
                Debug.Log("Trying to load Animals set");

                resourcePackName = "Animals";
                images = Resources.LoadAll<Sprite>(resourcePackName);
            }
            FindItStatistics.resourcePackName = resourcePackName;

            FindItStatistics.expectedGameTurnsTotal = numberTurns;

            // loading number of pieces
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
        }


        /**
         * Sets up tiles textures in the scene
         * @param numPictures number of images
         */
        private void SetUpSprites(int numPictures)
        {
            System.Random random = new System.Random();

            usedIndices.Clear();

            for (int i = 1; i <= numPictures; i++)
            {
                int index = random.Next(images.Length);
                while (usedIndices.Contains(index))
                {
                    index++;
                    if (index == images.Length) index = 0;
                }
                usedIndices.Add(index);

                GameObject o = GameObject.Find(i.ToString());
                o.SetActive(true);
                o.GetComponent<SpriteRenderer>().sprite = images[index];
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
            System.Random r = new System.Random();

            int chosen = r.Next(0, numberPieces);
            if (usedIndices[chosen] == selectedImage)
            {
                chosen++;
                if (chosen == numberPieces) chosen = 0;
            }
            selectedImage = usedIndices[chosen];
            targetImage.GetComponent<SpriteRenderer>().sprite = images[selectedImage];
        }

        /**
         * UnityEngine Start() function
         */
        void Start()
        {
			clona.SetActive(false);
			gameWon = false;
            Debug.Log("Load sprites");
            LoadResourcePack();
            SetUpSprites(numberPieces);
            UpdateCameraSize();
            newTargetImage();
            FindItStatistics.Clear();
			FindItStatistics.StartMeasuringTime();
			MGC.Instance.minigamesProperties.SetPlayed (Application.loadedLevelName);
			//MGC.Instance.SaveMinigamesPropertiesToFile ();
        }

        /**
         * Checks for end of the game
         */
        void CheckEndGame()
        {
            if(FindItStatistics.turnsPassed == FindItStatistics.expectedGameTurnsTotal && !gameWon)
            {
				gameWon = true;
				FindItStatistics.StopMeasuringTime();
				clona.SetActive(true);

				MGC.Instance.neuronHelp.GetComponent<Game.BrainHelp>().ShowSmile(Resources.Load("Neuron/smilyface") as Texture);
				MGC.Instance.minigamesGUI.show(false,true,"FindIt");

                MGC.Instance.minigamesProperties.SetPlayed(MGC.Instance.selectedMiniGameName, MGC.Instance.selectedMiniGameDiff);
                MGC.Instance.FinishMinigame();
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

        /**
         * UnityEngine Update() function
         */
        void Update()
        {
            UpdateGreenBarAndText();
            CheckEndGame();
        }
    }
}
