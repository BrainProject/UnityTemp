using UnityEngine;
using System.Collections;

namespace HanoiTowers
{

    public enum ColumnsNames
    {
        Left,
        Middle,
        Right
    }

    public class GameController : MonoBehaviour
    {

        //TODO proper initialization - ??
        //TODO better graphics

        public ColumnsNames startingColumnName;
        public ColumnsNames endingColumnName;

        [Range(1, 8)]
        public int numberOfDisks;


        public bool disksAnimations;
        public float animationTime;

        public Color greenColor;
        public Color redColor;


        public GameObject[] columns;
        public GameObject[] disks;
        public GameObject ceilingObject;

        public GameObject endGameGUI;

        private Column startingColumn;
        private Column endingColumn;
        private int score = 0;
        private float diskHeight = 0.41f;
        private Disk waitingForTarget;

        private float gameStartTime;

        private bool showEndGameGUI = false;
        void Start()
        {
            //set up columns
            //print("Columns count: " + columns.GetLength(0));
            //print("Starting column index = " + (int)startingColumnName);
            //print("Ending column index = " + (int)endingColumnName);

            startingColumn = columns[(int)startingColumnName].GetComponent<Column>();
            endingColumn = columns[(int)endingColumnName].GetComponent<Column>();

            if (startingColumn == null || endingColumn == null)
            {
                Debug.LogError("Wrong pointers to columns...");
            }

            ResetGame();

            //QualitySettings.antiAliasing = 4;
        }

        public void ResetGame()
        {
            //TODO temporary hack
            numberOfDisks = MGC.Instance.hanoiTowersNumberOfDisks;

            MGC.Instance.logger.addEntry("New game starts with: " + numberOfDisks + " disks");

            //reset columns
            for (int i = 0; i < columns.GetLength(0); i++)
            {
                columns[i].GetComponent<Column>().removeAllDisks();
            }

            //disable any possibly enabled disk
            for (int i = 0; i < disks.GetLength(0); i++)
            {
                disks[i].SetActive(false);

                disks[i].GetComponent<Disk>().setColumn(null);
                //disks[i].gameObject.rigidbody.isKinematic = true;

                //disks[i].transform.position = new Vector3(100, 0, 0);
            }

            //enable disks and move them to correct column
            Disk disk;
            for (int i = numberOfDisks - 1; i >= 0; i--)
            {
                disk = disks[i].GetComponent<Disk>();

                disk.moveToColumn(startingColumn, false);

                //disks[i].rigidbody.isKinematic = false;
                disks[i].SetActive(true);
            }

            score = 0;
            gameStartTime = Time.time;
        }

        public void increaseScore()
        {
            score++;
            //Debug.Log("Number of moves: " + score);
        }

        public float getCeilingPosition()
        {
            return ceilingObject.transform.position.y + ceilingObject.transform.localScale.y;
        }

        public int getScore()
        {
            return score;
        }

        public float getGameStartTime()
        {
            return gameStartTime;
        }


        public float getDiskHeight()
        {
            return diskHeight;
        }

        public Column getStartingColumn()
        {
            return startingColumn;
        }

        public Disk getWaitingForTarget()
        {
            return waitingForTarget;
        }

        public void setWaitingForTarget(Disk disk)
        {
            waitingForTarget = disk;
        }


        public void endGame()
        {
            //animate Neuron
            MGC.Instance.neuronHelp.GetComponent<Game.BrainHelp>().ShowSmile(Resources.Load("Neuron/smilyface") as Texture);

            //global GUI
            MGC.Instance.minigamesGUI.show(true);

            //mini-game specific gui
            endGameGUI.SetActive(true);
            showEndGameGUI = true;


        }


        
        void OnGUI()
        {
            if (showEndGameGUI)
            {
                float w = Screen.width;
                float h = Screen.height;

                numberOfDisks = (int)GUI.HorizontalSlider(new Rect(0.25f * w, 0.2f * h, 0.5f * w, 50), (int)numberOfDisks, 2.0F, 8.0F);
                
                //TODO temporary hack - solve by implementing mini-game statistics saving
                MGC.Instance.hanoiTowersNumberOfDisks = numberOfDisks;
            }
        }
    }

}