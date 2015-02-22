using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HanoiTowers
{

    /**
     * \brief Handles logic of one column in Hanoi Towers mini-game
     * 
     * Allows add and remove disks to/from column, handles reactions on mouse events, 
     * 
     * TODO adjust layers and collisions of disks and collumns
     * */
    public class Column : MonoBehaviour
    {

        public GameController gameController;

        public bool starting;
        public bool ending;

        public Light coloredLight;


        private Stack<Disk> disks;
        private Color startcolor;

        // Use this for initialization
        void Awake()
        {
            disks = new Stack<Disk>();
        }

        void Start()
        {
            startcolor = renderer.material.color;
        }

        /// <summary>
        /// Add disk to this column
        /// </summary>
        /// <param name="newDisk">reference to disk to be added</param>
        public void addDisk(Disk newDisk)
        {
            disks.Push(newDisk);
            checkWinningCondition();
        }


        /// <summary>
        /// Check if winning condidtions are fulfilled. If so, calls <code>gameController.endGame()</code>
        /// </summary>
        void checkWinningCondition()
        {
            if (ending)
            {
                if (disks.Count == gameController.numberOfDisks)
                {
                    //global stuff, happening for each minigame
                    MGC.Instance.FinishMinigame();

                    //local stuff, specific for this minigame
                    MGC.Instance.logger.addEntry("Game successfully finished | time " + (Time.time - gameController.getGameStartTime()) + " | number of moves " + gameController.getNumberofMoves());
                    gameController.endGame();
                }
            }

        }

        /// <summary>
        /// Checks, if this column is valid target for disk of given size.
        /// </summary>
        /// <param name="diskSize">size of disk to be moved on this columnn</param>
        /// <returns>true if this column is valid target, false otherwise</returns>
        bool isValidTarget(int diskSize)
        {
            if (topDisk() == null)
                return true;

            if (topDisk().size >= diskSize)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>current number of disks on this column</returns>
        public int getNumberofDisks()
        {
            return disks.Count;
        }




        /// <summary>
        /// hightlight column on mouse enter
        /// </summary>
        void OnMouseEnter()
        {
            Disk disk = gameController.getWaitingForTarget();

            //only if some disk is moving...
            if (disk == null)
            {
                return;
            }

            //move light to correct position
            Vector3 newPos = new Vector3(transform.position.x, coloredLight.transform.position.y, coloredLight.transform.position.z);
            coloredLight.transform.position = newPos;

            //set correct color
            if (isValidTarget(disk.size))
            {
                coloredLight.color = gameController.greenColor;
            }
            else
            {
                coloredLight.color = gameController.redColor;
            }

            coloredLight.enabled = true;
        }

        /// <summary>
        /// Turn off highlight on mouse exit
        /// </summary>
        void OnMouseExit()
        {
            coloredLight.enabled = false;
            renderer.material.color = startcolor;
        }

        /// <summary>
        /// solve mouse click on this column. 
        /// </summary>
        ///  
        /// Checks, if there is a disk in motion. If not, returns.
        /// Checks if this column is valid target. If so, move moving disk on this column
        void OnMouseUp()
        {
            // is there disk "in motion"?
            Disk disk = gameController.getWaitingForTarget();

            if (disk == null || disk.getState() != diskState.dragged)
            {
                MGC.Instance.logger.addEntry("Kliknutí na sloupec i když není vybrán žádný disk");
                return;
            }

            //can be moving this placed on this column?
            if (isValidTarget(disk.size))
            {
                //turn off the colored light
                coloredLight.enabled = false;

                //place the disk
                disk.moveToColumn(this, gameController.disksAnimations);
            }

            else
            {
                //TODO ? add some "warning effect" ?

                Debug.Log("Column " + this + " is not a valid target for disk " + disk);
                MGC.Instance.logger.addEntry("Pokus o zakázaný přesun disku " + disk.size + " na sloupec " + this);
            }
        }

        public void removeTopDisk()
        {
            disks.Pop();
            //Debug.Log("There is(are) " + disks.Count + " disk(s) on column: " + this);
        }

        public void removeAllDisks()
        {
            while (disks.Count > 0)
            {
                disks.Pop();
            }
        }


        public Disk topDisk()
        {
            if (disks.Count == 0)
            {
                return null;
            }

            return disks.Peek();
        }

    }

}