using UnityEngine;
using System.Collections;

namespace LondonTower
{
    /// <summary>
    /// Spehre gameobject script
    /// </summary>
    public class LondonToweSphereScript : MonoBehaviour, System.IComparable<LondonToweSphereScript>
    {

        public static bool anySphereMove = false;
        // public float spehrePlaceWaitTime = 0.5f;

        //private float waitAfterClicked = 0;
        //private bool animatedUp = false;
        //private bool clicked = false;
        //private bool lastClicked = false;
        private bool moveX = true;
        private bool isControlled = false;
        private float lastPolePolesition;

        public LondonTowerGameManager gameManager;
        private float heightY = 5.2f;
        public bool start = true;
        public string idColor;
        public int currentPoleID;
        public int orderOnPole;

        public Rigidbody thisRigidbody;
        //  private float maxY;
        //ratio of moving spehre by mouse - to scene
        //  private float moveConstantX =(Screen.height /9.3f);
        //  private float moveConstantY = (Screen.height / 9.3f);
        // private Vector3 lastPosition;
        //  private Vector3 polePosition = new Vector3(0,0,0);


        void Start()
        {
            thisRigidbody.useGravity = true;
        }

        // Update is called once per frame
        void Update()
        {
            /*if (waitAfterClicked > 0)
            {
                waitAfterClicked -= Time.deltaTime;
            }
            if (transform.GetComponent<Rigidbody>().velocity.y > 0)
            {
                transform.GetComponent<Rigidbody>().velocity = new Vector3();
            }
            if (transform.position.y <= 0)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 6, this.transform.position.z);
            }
            if (clicked)
            {
                if (animatedUp)
                {
                    if (transform.position.y >= 5.2f)
                    {
                        animatedUp = false;
                    }
                    else
                    {
                        transform.Translate(Vector3.up * Time.deltaTime * 8);
                    }

                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x, heightY, this.transform.position.z);
                    Vector3 mosePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9));
                    this.transform.position = new Vector3(Mathf.Min(Mathf.Max(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 9)).x, mosePosition.x), Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 9)).x), this.transform.position.y, this.transform.position.z);

                    if (lastClicked)
                    {
                        lastClicked = !lastClicked;

                    }
                    else
                    {
                        if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && waitAfterClicked <= 0)
                        {
                            clicked = false;
                            this.GetComponent<Rigidbody>().useGravity = true;
                        }
                    }
                }

            }
            else
            {

                int xPosition = (((int)this.transform.position.x) / 4 * 4);
                float rozdil = this.transform.position.x - xPosition;
                if (rozdil > 2)
                {
                    xPosition = xPosition + 4;
                }
                if (gameManager == null)
                {
                    Debug.Log("no game manager, add gammanager in editor");
                }

                if (moveX && gameManager.IsPoleFull(xPosition))
                {
                    // Debug(xPosition);
                    Debug.Log(lastPolePolesition);
                    //    Debug.Log(currentPoleID + "poleID");
                    this.transform.position = (new Vector3((currentPoleID - 1) * 4, this.transform.position.y + 1, this.transform.position.z));
                    //this.transform.position = (new Vector3(lastPolePolesition, this.transform.position.y, this.transform.position.z));

                }
                else
                {
                    this.transform.position = (new Vector3(xPosition, this.transform.position.y, this.transform.position.z));
                }
            }
            */

            if(isControlled)
            {
                this.transform.position = new Vector3(this.transform.position.x, heightY, this.transform.position.z);
                Vector3 mosePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 9));
                this.transform.position = new Vector3(Mathf.Min(Mathf.Max(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 9)).x, mosePosition.x), Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 9)).x), this.transform.position.y, this.transform.position.z);


                if(Input.GetMouseButtonDown(0))
                {
                    LondonTowePoleScript dropPole = GetClosestPole();
                    if(dropPole.IsFull())
                    {
                        BackToLastPole(0);
                    }
                    else
                    {
                        Vector3 tmp = transform.position;
                        tmp.x = dropPole.transform.position.x;
                        transform.position = tmp;
                        //this.GetComponent<Rigidbody>().useGravity = true;
                        //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                        //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
                        //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
                        currentPoleID = dropPole.id;
                    }
                    SetContraints(true);
                    anySphereMove = false;
                    isControlled = false;
                }
            }
        }



        void OnMouseDown()
        {
            //if (LondonTowerGameManager.state == LondonTowerGameState.game)
            //{
            if (gameManager.OnTop(currentPoleID, orderOnPole) && !anySphereMove && /*!clicked &&*/ !gameManager.movingSphere)
            {
                //clicked = true;
                //lastClicked = true;
                //animatedUp = true;
                //someSphereMove = true;
                gameManager.movingSphere = true;
                anySphereMove = true;
                this.GetComponent<Rigidbody>().useGravity = false;
                StartCoroutine(PickUpAnimation());
            }
            //}
        }


        public void EnableXMove(bool enableX)
        {
            moveX = enableX;
        }

        /// <summary>
        /// set position of last pole,where sphere was
        /// </summary>
        /// <param name="polePosition"></param>
        public void SetLastPolePosition(float polePosition)
        {
            this.lastPolePolesition = polePosition;
        }


        /// <summary>
        /// return spehre to last pole
        /// </summary>
        /// <param name="poleHeight"> how big pole is, so spehre can be move directly on top of pole</param>
        public void BackToLastPole(float poleHeight = 0)
        {
            //this.transform.position = new Vector3(lastPolePolesition, poleHeight, this.transform.position.z);
            Vector3 tmp = this.transform.position;
            tmp.x = lastPolePolesition;
            this.transform.position = tmp;
            //GetComponent<Rigidbody>().useGravity = true;
        }

        /// <summary>
        /// set game data about sphere, pole id, order on pole and if game 
        /// </summary>
        /// <param name="poleID"></param>
        /// <param name="orderOnPole"></param>
        /// <param name="game"></param>
        public void SetPoleData(int poleID, int orderOnPole, bool game)
        {
            currentPoleID = poleID;
            if (start)
            {
                start = false;
            }
            else
            {
                this.orderOnPole = orderOnPole;
            }

        }

        /// <summary>
        /// set color/material
        /// </summary>
        /// <param name="id"></param>
        /// <param name="color"></param>
        public void setIdColor(string id, Color color)
        {
            this.idColor = id;
            GetComponent<Renderer>().material.color = color;
        }



        public int CompareTo(LondonToweSphereScript other)
        {
            if (other == null) return 1;
            return orderOnPole.CompareTo(other.orderOnPole);
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<LondonTowePoleScript>() != null)
            {
                //Debug.Log(" sphere is hitted by pole");
                //someSphereMove = false;
                //if (clicked)
                //{
                //clicked = false;
                //}
                gameManager.movingSphere = false;
                anySphereMove = false;
                //this.GetComponent<Rigidbody>().useGravity = false;
                //this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            }
        }

        public LondonTowePoleScript GetClosestPole()
        {
            if (gameManager.poles.Count > 0)
            {
                LondonTowePoleScript closestPole = gameManager.poles[0];
                for (int i = 0; i < gameManager.poles.Count; ++i)
                {
                    if (Mathf.Abs(gameManager.poles[i].transform.position.x - transform.position.x)
                       < Mathf.Abs(closestPole.transform.position.x - transform.position.x))
                    {
                        closestPole = gameManager.poles[i];
                    }
                }

                return closestPole;
            }

            return null;
        }

        IEnumerator PickUpAnimation()
        {
            while (transform.position.y < 5.2f)
            {
                transform.Translate(Vector3.up * Time.deltaTime * 8);
                yield return null;
            }

            isControlled = true;
            yield return null;
        }

        void OnCollisionEnter(Collision col)
        {
            if(col.gameObject.GetComponent<LondonToweSphereScript>() || col.gameObject.name == "Floor")
            {
                SetContraints(false);
                anySphereMove = false;
            }
        }

        void SetContraints(bool isPhysical)
        {
            if(isPhysical)
            {
                thisRigidbody.constraints = RigidbodyConstraints.None;
                thisRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                thisRigidbody.freezeRotation = true;
                thisRigidbody.useGravity = true;
            }
            else
            {
                thisRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                thisRigidbody.freezeRotation = true;
                thisRigidbody.useGravity = false;
            }
        }
    }
}