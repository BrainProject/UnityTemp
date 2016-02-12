using UnityEngine;
using System.Collections;

namespace HanoiTowers
{

    [System.Serializable]
    public class Boundary
    {
        public float xMin = -6.5f;
        public float xMax = 6.5f;
        public float yMin = 5.0f;
        public float yMax = 5.5f;
    }

    public enum diskState
    {
        lying,
        animatedUp,
        dragged,
        animatedDown
    }

    /**
     * \brief Handles logic of one disk in Hanoi Towers mini-game
     * 
     * Allows add and remove disks to/from column, handles reactions on mouse events, 
     * */
    public class Disk : MonoBehaviour
    {

        public GameController gameController;

        /// size of this disk - larger disk can not be put on top of smaller one
        public int size;

        private diskState state;

        private Color startcolor;
        private Column actualColumn;

        private bool waitingForTarget;
        //private bool draggingEnabled = false;
        private Vector3 animationTarget;
        private float animationFraction;
        private float animationStartTime;

        private Vector3 initialDiskScreenPoint;
        private Vector3 offset;
        private Boundary boundary;

        void Awake()
        {
            startcolor = GetComponent<Renderer>().material.color;
        }

        void Start()
        {
            //initialization
            state = diskState.lying;

            waitingForTarget = false;
            boundary = new Boundary();
        }

        private void animateDiskUp()
        {
            state = diskState.animatedUp;

            //adjust impulse to number of disk on the column - the more disks, the less necessary impulse
            GetComponent<Rigidbody>().AddForce(transform.up * (10.75f - (0.375f * actualColumn.getNumberofDisks())), ForceMode.Impulse);


        }

        /// <summary>
        /// returns true if disk can be lifted right now (it is top disk on column)
        /// </summary>
        /// <returns>true if disk can be lifted right now (it is top disk on column), else otherwise</returns>
        private bool isMovable()
        {
            if (actualColumn == null)
            {
                Debug.LogError("column not set!");
            }

            if (this == actualColumn.topDisk() && ( state == diskState.lying))
            {
                return true;
            }

            return false;
        }


        public void moveToColumn(Column target, bool animate)
        {
            //todo
            //draggingEnabled = false;

            waitingForTarget = false;
            gameController.setWaitingForTarget(null);

            if (actualColumn != null)
            {
                if (actualColumn != target)
                {
                    //Debug.Log("Moving from column: " + actualColumn);
                    //Debug.Log("Moving to column: " + target);
                    MGC.Instance.logger.addEntry("akce - přesun | disk: " + this.size + " | ze sloupce: " + actualColumn + " | na sloupec: " + target);
                }
                else
                {
                    MGC.Instance.logger.addEntry("Disk položen na stejný sloupec, ze kterého byl zvednut");
                }

                gameController.increaseNumberofMoves();
                actualColumn.removeTopDisk();
            }

            actualColumn = target;
            actualColumn.addDisk(this);

            if (animate)
            {
                updatePosition(4.8f);
            }
            else
            {
                updatePosition(0.0f);
            }

            GetComponent<Renderer>().material.color = startcolor;
            GetComponent<Rigidbody>().isKinematic = false;

        }

        void OnMouseUp()
        {
            //lift the disk
            if (isMovable() && gameController.getWaitingForTarget() == null)
            {
                MGC.Instance.logger.addEntry("Disk " + this.size + " lifted");
                waitingForTarget = true;
                gameController.setWaitingForTarget(this);

                //should be disk animated?
                if (gameController.disksAnimations)
                {
                    animateDiskUp();
                }
            }
            else
            {
                Debug.Log("This disk (" + this + ") is not movable now.");
                MGC.Instance.logger.addEntry("Disk " + this.size + " can't be lifted now");
            }

        }

        // highlighting 
        void OnMouseEnter()
        {
            //print("Mouse enter");

            if (gameController.getWaitingForTarget() != null)
            {
                return;
            }

            if (isMovable())
            {
                GetComponent<Renderer>().material.color = gameController.greenColor;
            }
            else
            {
                GetComponent<Renderer>().material.color = gameController.redColor;
            }

        }

        // returns current state of disk
        public diskState getState()
        {
            return state;
        }

        // highlighting 
        void OnMouseExit()
        {
            if (!waitingForTarget)
            {
                GetComponent<Renderer>().material.color = startcolor;
            }

        }

        public void setColumn(Column newColumn)
        {
            actualColumn = newColumn;
        }

        //update position of disk
        private void updatePosition(float fallHeight)
        {
            gameController.ceilingObject.SetActive(false);

            float y;

            if (fallHeight == 0.0)
            {
                y = (float)actualColumn.getNumberofDisks() * gameController.getDiskHeight() - 0.5f * gameController.getDiskHeight();
                state = diskState.lying;
            }
            else
            {
                y = fallHeight;
                state = diskState.animatedDown;
            }

            Vector3 newPos = new Vector3(actualColumn.gameObject.transform.position.x, y, transform.position.z);

            //TODO is it necessary to have both lines here?
            transform.position = newPos;
            //rigidbody.position = newPos;
        }

        
        void Update()
        {
            if (state == diskState.animatedUp)
            {
                float cp = gameController.getCeilingPosition();
                //print("Ceiling position y: " + cp);
                if (transform.position.y > cp)
                {
                    gameController.ceilingObject.SetActive(true);

                    state = diskState.dragged;
                    

                    GetComponent<Rigidbody>().isKinematic = true;


                    initialDiskScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
                    //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, initialDiskScreenPoint.z));
                }
            }

            //"ground" falling disk
            if (state == diskState.animatedDown)
            {
                if (transform.position.y < (float)actualColumn.getNumberofDisks() * gameController.getDiskHeight() - 0.5f * gameController.getDiskHeight() + 0.1)
                {
                    state = diskState.lying;
                }
            }

            if (state == diskState.dragged)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, initialDiskScreenPoint.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);// +offset;

                //TODO clamp position
                curPosition.x = Mathf.Clamp(curPosition.x, boundary.xMin, boundary.xMax);
                curPosition.y = Mathf.Clamp(curPosition.y, boundary.yMin, boundary.yMax);
                curPosition.z = 0.0f;
                transform.position = curPosition;

            }

        }
    }

}