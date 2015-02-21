using UnityEngine;
using System.Collections;

/**
 * \brief namespace for Hanoi Towers mini-game 
 * Hanoi Towers mini-game
 * 
 * The goal of the game is to move all disks from one column to another. Only top disk can be moved
 * 
 * Functionality is divided into GameController class,
 * Column class and Disk class.
 * There is also simple end-game GUI
 *
 */
namespace HanoiTowers
{
    /// <summary>
    /// Enumerate names of columns. Just for convenience
    /// </summary>
    public enum ColumnsNames
    {
        Left,
        Middle,
        Right
    }

    public class GameController : MonoBehaviour
    {

        //TODO proper initialization - ??
        //TODO better graphics ??

        public ColumnsNames startingColumnName;
        public ColumnsNames endingColumnName;

        [Range(2, 8)]
        public int numberOfDisks;


        public bool disksAnimations;
        public float animationTime;

        public Color greenColor;
        public Color redColor;


        public GameObject[] columns;
        public GameObject[] disks;
        public GameObject ceilingObject;


        private Column startingColumn;
        private Column endingColumn;
        private int numberofMoves = 0;
        private float diskHeight = 0.41f;
        private Disk waitingForTarget;

        private float gameStartTime;

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
			MGC.Instance.minigameStates.SetPlayed (Application.loadedLevelName);
        }

        public void ResetGame()
        {
            //load difficulty from ...
            numberOfDisks = MGC.Instance.selectedMiniGameDiff;

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
            }

            //enable chosen number of disks and move them to correct column
            Disk disk;
            for (int i = numberOfDisks - 1; i >= 0; i--)
            {
                disk = disks[i].GetComponent<Disk>();

                disk.moveToColumn(startingColumn, false);
                disks[i].SetActive(true);
            }

            numberofMoves = 0;
            gameStartTime = Time.time;
        }

        //// parameter has to be float to be usable with Unity UI
        //public void setDifficulty(float newNumberofDisks)
        //{
        //    print("Hanoi Towers: setting difficulty to: " + newNumberofDisks);
        //    numberOfDisks = (int)newNumberofDisks;

        //    //TODO temporary hack - solve by implementing mini-game statistics saving
        //    MGC.Instance.hanoiTowersNumberOfDisks = numberOfDisks;
        //}

        public void increaseNumberofMoves()
        {
            numberofMoves++;
            //Debug.Log("Number of moves: " + numberofMoves);
        }

        public float getCeilingPosition()
        {
            return ceilingObject.transform.position.y + ceilingObject.transform.localScale.y;
        }

        public int getNumberofMoves()
        {
            return numberofMoves;
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
            
            GameObject Neuronek = MGC.Instance.neuronHelp;
            if (Neuronek)
            {
                Neuronek.GetComponent<Game.BrainHelp>().ShowSmile(Resources.Load("Neuron/smilyface") as Texture);
            }

            //global GUI
            MGC.Instance.minigamesGUI.show(true);
        }
    }
}