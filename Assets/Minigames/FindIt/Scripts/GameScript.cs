/**
 * @file GameScript.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace FindIt
{
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

        private List<int> usedIndices = new List<int>();

        private int selectedImage = 0;

		public GameObject clona;

        public UnityEngine.UI.Text textProgress;
        public UnityEngine.UI.Image progressBar;

        public GameObject targetImage;

		private bool gameWon = false;

        private void LoadResourcePack()
        {
            try
            {
                resourcePackName = PlayerPrefs.GetString("resourcePackName");
                images = Resources.LoadAll<Sprite>(resourcePackName);
            }
            catch (Exception ex)
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

        private void UpdateCameraSize()
        {
            Camera.main.orthographicSize = numberPieces <= 28 ? CAMERA_SIZE_LESS_28 : CAMERA_SIZE_MORE_28;
        }

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

        
        void Start()
        {
			clona.SetActive(false);
			gameWon = false;
            LoadResourcePack();
            SetUpSprites(numberPieces);
            UpdateCameraSize();
            newTargetImage();
            FindItStatistics.Clear();
			FindItStatistics.StartMeasuringTime();
			MGC.Instance.minigamesProperties.SetPlayed (Application.loadedLevelName);
			//MGC.Instance.SaveMinigamesPropertiesToFile ();
        }

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

        void UpdateGreenBarAndText()
        {
            progressBar.GetComponent<RectTransform>().localScale = new Vector3((float)FindItStatistics.turnsPassed / (float)FindItStatistics.expectedGameTurnsTotal * (float)Screen.width / (float)Screen.height, 1f, 0f);
            textProgress.text = FindItStatistics.turnsPassed.ToString() + "/" + FindItStatistics.expectedGameTurnsTotal.ToString();            
        }

        void Update()
        {
            UpdateGreenBarAndText();
            CheckEndGame();
        }
    }
}
