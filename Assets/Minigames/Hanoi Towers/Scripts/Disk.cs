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

    public class Disk : MonoBehaviour
    {

        public GameController gameController;

        public int size;


        private Color startcolor;
        private Column actualColumn;

        private bool waitingForTarget;
        private bool jumpInProgress = false;
        private bool draggingEnabled = false;
        private Vector3 animationTarget;
        private float animationFraction;
        private float animationStartTime;

        private Vector3 initialDiskScreenPoint;
        private Vector3 offset;
        private Boundary boundary;

        void Awake()
        {
            startcolor = renderer.material.color;
        }

        void Start()
        {
            //initialization
            waitingForTarget = false;
            boundary = new Boundary();


        }

        private void animateDiskUp()
        {
            jumpInProgress = true;

            //animationTarget = new Vector3(actualColumn.gameObject.transform.position.x, 4.75f, actualColumn.gameObject.transform.position.z);
            //animationFraction = 0.0f;
            //animationStartTime = Time.time;

            //adjust impulse to number of disk on the column - the more disks, the less necessary impulse
            rigidbody.AddForce(transform.up * (10.75f - (0.375f * actualColumn.getNumberofDisks())), ForceMode.Impulse);


        }


        private bool isMovable()
        {
            if (actualColumn == null)
            {
                Debug.LogError("column not set!");
            }

            if (this == actualColumn.topDisk())
            {
                return true;
            }

            return false;
        }

        public void moveToColumn(Column target, bool animate)
        {

            draggingEnabled = false;

            waitingForTarget = false;
            gameController.setWaitingForTarget(null);

            if (actualColumn != null)
            {
                if (actualColumn != target)
                {
                    //Debug.Log("Moving from column: " + actualColumn);
                    //Debug.Log("Moving to column: " + target);
                    MGC.Instance.logger.addLogEntry("akce - přesun | disk: " + this.size + " | ze sloupce: " + actualColumn + " | na sloupec: " + target);

                    gameController.increaseScore();
                }
                else
                {
                    MGC.Instance.logger.addLogEntry("Disk položen na stejný sloupec, ze kterého byl zvednut");
                }

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

            renderer.material.color = startcolor;
            rigidbody.isKinematic = false;

        }

        void OnMouseUp()
        {
            if (isMovable() && gameController.getWaitingForTarget() == null)
            {
                MGC.Instance.logger.addLogEntry("Disk " + this.size + " lifted");
                waitingForTarget = true;
                gameController.setWaitingForTarget(this);

                if (gameController.disksAnimations)
                {
                    animateDiskUp();
                }
            }
            else
            {
                Debug.Log("This disk (" + this + ") is not movable now.");
                MGC.Instance.logger.addLogEntry("Disk " + this.size + " can't be lifted now");
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
                renderer.material.color = gameController.greenColor;
            }
            else
            {
                renderer.material.color = gameController.redColor;
            }

        }

        // highlighting 
        void OnMouseExit()
        {
            if (!waitingForTarget)
            {
                renderer.material.color = startcolor;
            }

        }

        public void setColumn(Column newColumn)
        {
            actualColumn = newColumn;
        }


        //void OnMouseDown()
        //{
        //print("Mouse down");
        //Vector3 diskPosition = transform.position;

        //initialScreenPoint = Camera.main.WorldToScreenPoint(diskPosition);

        //compute offset, that will be used during 'dragging' the disk
        //we are interested only in X axis offset...
        //offset = diskPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        //offset = diskPosition - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, initialScreenPoint.y, initialScreenPoint.z));
        //}


        //void FixedUpdate()
        //{
        //    if (draggingEnabled)
        //    {


        //Vector3 currentMouseScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        //Vector3 currentDiskSreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        //float horizontalDistance = currentMouseScreenPoint.x - currentDiskSreenPoint.x;
        //float verticalDistance = currentMouseScreenPoint.y - currentDiskSreenPoint.y; 

        ////print("Mouse distance: " + horizontalDistance);

        ////move disk horizontally
        //float force = 0.01f * horizontalDistance;
        //rigidbody.AddForce(transform.right * force, ForceMode.VelocityChange);

        ////if (verticalDistance > 0)
        ////{
        ////    rigidbody.AddForce(transform.up * 0.1f * verticalDistance);
        ////}

        ////Vector3 curPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + offset;
        ////transform.position = curPosition;
        //    }

        //}

        //update position of disk
        private void updatePosition(float fallHeight)
        {
            gameController.ceilingObject.SetActive(false);

            float y;

            if (fallHeight == 0.0)
            {
                y = (float)actualColumn.getNumberofDisks() * gameController.getDiskHeight() - 0.5f * gameController.getDiskHeight();
            }
            else
            {
                y = fallHeight;
            }

            Vector3 newPos = new Vector3(actualColumn.gameObject.transform.position.x, y, transform.position.z);
            transform.position = newPos;
            rigidbody.position = newPos;
        }




        void Update()
        {
            if (jumpInProgress)
            {
                float cp = gameController.getCeilingPosition();
                //print("Ceiling position y: " + cp);
                if (transform.position.y > cp)
                {
                    gameController.ceilingObject.SetActive(true);
                    jumpInProgress = false;
                    draggingEnabled = true;

                    //disable movement in Z axis
                    //disable rotations of disk

                    //rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

                    rigidbody.isKinematic = true;

                    initialDiskScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
                    //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, initialDiskScreenPoint.z));
                }
            }

            if (draggingEnabled)
            {

                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, initialDiskScreenPoint.z);

                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);// +offset;

                //TODO clamp position
                curPosition.x = Mathf.Clamp(curPosition.x, boundary.xMin, boundary.xMax);
                curPosition.y = Mathf.Clamp(curPosition.y, boundary.yMin, boundary.yMax);
                curPosition.z = 0.0f;
                transform.position = curPosition;

            }


            //    float deltaTime = Time.time - animationStartTime;
            //    animationFraction = deltaTime / gameController.animationTime;

            //    //Debug.Log("Delta time: " + deltaTime);
            //    //Debug.Log("fraction: " + animationFraction);


            //    transform.position = Vector3.Lerp(transform.position, animationTarget, animationFraction);

            //    if(animationFraction >= 1.0f)
            //    {
            //        jumpInProgress = false;
            //    }
            //}
        }
    }

}