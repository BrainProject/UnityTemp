using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HanoiTowers
{

    public class Column : MonoBehaviour
    {

        public HanoiGameController gameController;
        public GUIText winText;

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
            winText.enabled = false;
        }

        public void addDisk(Disk newDisk)
        {
            disks.Push(newDisk);
            //Logger.addLogEntry("There is/are " + disks.Count + " disk(s) on column: " + this);

            checkWinningCondition();
        }



        void checkWinningCondition()
        {
            if (ending)
            {
                if (disks.Count == gameController.numberOfDisks)
                {
                    //TODO win!
                    print("WIN");
                    Logger.addLogEntry("Game successfully finished | time " + (Time.time - gameController.getGameStartTime()) + " | number of moves " + gameController.getScore());


                    //winText.text = "test";
                    winText.enabled = true;
                }
            }
        }

        bool isValidTarget(int diskSize)
        {
            if (topDisk() == null)
                return true;

            if (topDisk().size >= diskSize)
                return true;
            else
                return false;
        }

        public int getNumberofDisks()
        {
            return disks.Count;
        }




        // highlighting 
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
                //renderer.material.color = gameController.greenColor;
            }
            else
            {
                coloredLight.color = gameController.redColor;
                //renderer.material.color = Color.red;
            }

            coloredLight.enabled = true;
        }

        // highlighting 
        void OnMouseExit()
        {
            coloredLight.enabled = false;
            renderer.material.color = startcolor;
        }

        void OnMouseUp()
        {
            // is there disk "in motion"?
            Disk disk = gameController.getWaitingForTarget();

            if (disk == null)
            {
                Logger.addLogEntry("Kliknutí na sloupec i když není vybrán žádný disk");
                return;
            }

            //can be moving this placed on this column?
            if (isValidTarget(disk.size))
            {
                disk.moveToColumn(this, gameController.disksAnimations);
            }

            else
            {
                //TODO add some "warning effect"
                Debug.Log("Column " + this + " is not a valid target for disk " + disk);
                Logger.addLogEntry("Pokus o zakázaný přesun disku " + disk.size + " na sloupec " + this);
            }
        }

        public void removeTopDisk()
        {
            disks.Pop();
            Debug.Log("There is(are) " + disks.Count + " disk(s) on column: " + this);
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
                return null;

            return disks.Peek();
        }

    }

}