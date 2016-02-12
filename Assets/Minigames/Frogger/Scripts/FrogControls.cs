using UnityEngine;
using System.Collections;

namespace Frogger
{
    public class FrogControls : MonoBehaviour
    {
        public SpriteRenderer frogSpriteRenderer;
        public BoxCollider frogCollider;
        public Sprite frogSprite;
        public Sprite bloodSprite;
        public bool isOnBoat = false;
        public bool isSafe = false;

        private FrogLevelManager thisLevelManager;
        private bool canControl = true;
        private float tapTimestamp;

        void Start()
        {
            thisLevelManager = FrogLevelManager.Instance;
        }

        // Update is called once per frame
        void Update()
        {
            // If on boat, check, wheter not out of bounds
            if (isOnBoat)
            {
                if (transform.position.x > 11.5f || transform.position.x < -11.5f)
                {
                    RespawnFrog();
                }
            }
#if UNITY_ANDROID
            if (canControl)
            {
                
                if (Input.GetMouseButtonDown(0))
                {
                    tapTimestamp = Time.time;
                    MGC.Instance.initialTouchPosition = Input.mousePosition;
                }

                if(Input.GetMouseButtonUp(0))
                {
                    //Debug.Log("Tap duration: " + (Time.time - tapTimestamp));
                    if(Time.time - tapTimestamp < 0.2f)
                    {
                        CheckBoat(transform.forward);
                        if (!isSafe)
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3);
                            MoveToParentsPosition();
                        }
                        CheckDrowned();
                    }
                }

                if (Input.GetMouseButtonUp(0) && (MGC.Instance.initialTouchPosition.x != 0))
                {
                    if (((MGC.Instance.initialTouchPosition.x - Input.mousePosition.x) < -MGC.Instance.swipeDistance.x) && transform.position.x < 10)
                    {
                        CheckBoat(transform.right);
                        transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
                        MoveToParentsPosition();
                    }
                    else if (((MGC.Instance.initialTouchPosition.x - Input.mousePosition.x) > MGC.Instance.swipeDistance.x) && transform.position.x > -10)
                    {
                        CheckBoat(-transform.right);
                        transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
                        MoveToParentsPosition();
                    }
                    CheckDrowned();
                    MGC.Instance.initialTouchPosition.x = 0;
                }
                if (Input.GetMouseButtonUp(0) && (MGC.Instance.initialTouchPosition.y != 0))
                {
                    if (((MGC.Instance.initialTouchPosition.y - Input.mousePosition.y) < -MGC.Instance.swipeDistance.y))
                    {
                        CheckBoat(transform.forward);
                        if (!isSafe)
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3);
                            MoveToParentsPosition();
                        }
                    }
                    else if (((MGC.Instance.initialTouchPosition.y - Input.mousePosition.y) > MGC.Instance.swipeDistance.y) && transform.position.z > -7)
                    {
                        CheckBoat(-transform.forward);
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3);
                        MoveToParentsPosition();
                    }
                    CheckDrowned();
                    MGC.Instance.initialTouchPosition.y = 0;
                }
            }
#else
            // Input handling
            if (canControl)
            {
                if (Input.GetButtonDown("Vertical"))
                {
                    if (Input.GetAxis("Vertical") > 0)
                    {
                        CheckBoat(transform.forward);
                        if (!isSafe)
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 3);
                            MoveToParentsPosition();
                        }
                    }
                    if (Input.GetAxis("Vertical") < 0 && transform.position.z > -7)
                    {
                        CheckBoat(-transform.forward);
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3);
                        MoveToParentsPosition();
                    }
                    CheckDrowned();
                }
                if (Input.GetButtonDown("Horizontal"))
                {
                    if (Input.GetAxis("Horizontal") > 0 && transform.position.x < 10)
                    {
                        CheckBoat(transform.right);
                        transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
                        MoveToParentsPosition();
                    }
                    if (Input.GetAxis("Horizontal") < 0 && transform.position.x > -10)
                    {
                        CheckBoat(-transform.right);
                        transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
                        MoveToParentsPosition();
                    }
                    CheckDrowned();
                }
            }
#endif
        }

        void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case "Enemy":
                    {
                        Debug.Log("Frog is ran over by a truck.");
                        RespawnFrog();
                        break;
                    }
                case "Respawn":
                    {
                        Debug.Log("Wrong way!");
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3);
                        break;
                    }
            }
        }

        void RespawnFrog(bool isDrowned = false)
        {
            StopAllCoroutines();
            StartCoroutine(RespawnFrogCoroutine());

            if (isDrowned)
            {
                frogSpriteRenderer.sprite = null;
                Instantiate(thisLevelManager.waterSplashPrefab, transform.position, Quaternion.Euler(new Vector3(90, 0, 0)));
            }
            else
            {
                frogSpriteRenderer.sprite = bloodSprite;
            }
        }

        IEnumerator RespawnFrogCoroutine()
        {
            frogCollider.enabled = false;
            canControl = false;
            isOnBoat = false;
            transform.SetParent(null);
            yield return new WaitForSeconds(1);
            frogSpriteRenderer.sprite = frogSprite;
            transform.position = thisLevelManager.frogSpawn.position;
            canControl = true;
            frogCollider.enabled = true;
        }

        void CheckBoat(Vector3 direction)
        {
            //if(direction>0)
            //{
            //    Debug.Log("Forward");
            //}
            //if(direction<0)
            //{
            //    Debug.Log("Backward");
            //}


            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 3))
            {
                if (hit.transform.tag == "Boat")
                {
                    //Debug.Log("Boarding on " + hit.transform.name);
                    isOnBoat = true;
                    transform.SetParent(hit.transform);
                }
                else
                {
                    //Debug.Log("No boat!");
                    isOnBoat = false;
                    transform.SetParent(null);
                    CheckGoal(hit.transform);
                }
            }
            else
            {
                //Debug.Log("Nothing in sight!");
                isOnBoat = false;
                transform.SetParent(null);
            }
        }

        void CheckGoal(Transform hitTarget)
        {
            Debug.Log(hitTarget.name);
            if (hitTarget.tag == "Finish")
            {
                Debug.Log("In finish.");
                if (!hitTarget.GetComponent<FrogGoal>().occupied)
                {
                    Debug.Log("This is my spot!");
                    transform.SetParent(hitTarget.GetComponent<FrogGoal>().mesh.transform);
                    hitTarget.GetComponent<FrogGoal>().occupied = true;
                    isSafe = true;
                    this.enabled = false;
                    MoveToParentsPosition();
                    if (thisLevelManager.IsGoalComplete())
                    {
                        MGC.Instance.WinMinigame();
                    }
                    else
                    {
                        Instantiate(thisLevelManager.frogPrefab, thisLevelManager.frogSpawn.position, Quaternion.identity);
                    }
                }
                else
                {
                    Debug.Log("Someone's here!");
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 3);
                }
            }
        }

        void CheckDrowned()
        {
            if (transform.position.z > thisLevelManager.lakeBorder.position.z)
            {
                if (!isOnBoat && !isSafe)
                {
                    RespawnFrog(true);
                    Debug.Log("Drowned");
                }
            }
            else
            {
                if (transform.parent)
                {
                    Debug.Log("Not on boat! But still not drowned!");
                    isOnBoat = false;
                    transform.SetParent(null);
                }
            }
        }

        void MoveToParentsPosition()
        {
            if (transform.parent)
            {
                transform.localPosition = Vector3.zero;
                transform.Translate(Vector3.up / 10);
            }
        }
    }
}