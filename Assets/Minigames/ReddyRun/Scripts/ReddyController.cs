using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Reddy
{

    public enum ReddyStates
    {
        WAITING,
        RUNNING,
        CELEBRATING,
        DYING
    }


    public class ReddyController : MonoBehaviour
    {

        public float runningSpeed;
        public Text winText;
        public Animator anim;
        internal int pathCount;
        private string pathTurn;
        private float cursorPositionX;
        public GameObject cursorObject;
        internal ReddyStates currentState;
        private bool isCelebrating;
        private bool isDead;

        public GameObject pathOwner;

        private bool isJumpInProgress;
        private bool isSlideInProgress;


        // Use this for initialization
        void Start()
        {
            if (MGC.Instance)
            {
                MGC.Instance.ResetKinect();
                MGC.Instance.kinectManagerInstance.ClearKinectUsers();
                MGC.Instance.kinectManagerInstance.StartKinect();
                MGC.Instance.isKinectRestartRequired = true;
                MGC.Instance.getMinigameStates().SetPlayed("ReddyRun", 0);
            }
            anim = GetComponent<Animator>();
            pathCount = 1;
            isJumpInProgress = false;
            isCelebrating = false;
            isDead = false;
            

            currentState = ReddyStates.WAITING;
        }


        void Update()
        {
            pathTurn = "";


            switch (currentState)
            {
                case ReddyStates.WAITING:
                    UpdateWaiting();
                    break;
                case ReddyStates.RUNNING:
                    UpdateRunning();
                    break;

                case ReddyStates.CELEBRATING:
                    UpdateCelebrating();
                    break;
                case ReddyStates.DYING:
                    transform.SetParent(null);
                    UpdateDying();
                    break;
            }







        }

        void UpdateWaiting()
        {
            if (ReddyKinect.Instance.IsSquat())
            {
                anim.SetBool("isRunning", true);
                currentState = ReddyStates.RUNNING;
            }

            cursorObject.transform.position = Input.mousePosition;
            cursorPositionX = cursorObject.transform.position.x;

            if (pathCount == 0)
            {
                if (cursorPositionX > (Screen.width / 3))
                {
                    pathTurn = "right";
                    MoveRight();
                }
            }
            if (pathCount == 1)
            {
                if (cursorPositionX > ((Screen.width / 3) * 2))
                {
                    pathTurn = "right";
                    MoveRight();
                }
                else if (cursorPositionX < (Screen.width / 3))
                {
                    pathTurn = "left";
                    MoveLeft();
                }
            }
            if (pathCount == 2)
            {
                if (cursorPositionX < ((Screen.width / 3) * 2))
                {
                    pathTurn = "left";
                    MoveLeft();
                }
            }

        }
        void UpdateRunning()
        {
            // for debug
            if (Input.GetKeyDown(KeyCode.R))
            {
                MGC.Instance.sceneLoader.LoadScene("ReddyRun");
            }

            transform.Translate(Vector3.forward * 0.2f * Time.deltaTime * runningSpeed);

           
            if (ReddyKinect.Instance.IsSquat()) // SLIDE
            {
                anim.SetTrigger("slideTrigger");
                StartCoroutine(SlideCoroutine());
            }
            if (ReddyKinect.Instance.IsJump()) // JUMP
            {
                if (!isJumpInProgress)
                {
                    anim.SetTrigger("jumpTrigger");
                    StartCoroutine(JumpCoroutine());
                }
            }
           

            cursorObject.transform.position = Input.mousePosition;
            cursorPositionX = cursorObject.transform.position.x;

            if (pathCount == 0)
            {
                if (cursorPositionX > (Screen.width / 3))
                {
                    pathTurn = "right";
                    MoveRight();
                }
            }
            if (pathCount == 1)
            {
                if (cursorPositionX > ((Screen.width / 3) * 2))
                {
                    pathTurn = "right";
                    MoveRight();
                }
                else if (cursorPositionX < (Screen.width / 3))
                {
                    pathTurn = "left";
                    MoveLeft();
                }
            }
            if (pathCount == 2)
            {
                if (cursorPositionX < ((Screen.width / 3) * 2))
                {
                    pathTurn = "left";
                    MoveLeft();
                }
            }


            
        }

        void MoveLeft()
        {
            if (pathCount - 1 >= 0) // to prevent character from falling from path
            {
                Camera camera = ReddyLevelManager.Instance.cameraReference;
                Vector3 camPosition = camera.transform.position;
                transform.Translate(Vector3.right * -4);
                pathCount -= 1;
                camera.transform.position = camPosition;

            }
        }

        void MoveRight()
        {
            if (pathCount + 1 <= 2) // to prevent character from falling from path
            {
                Camera camera = ReddyLevelManager.Instance.cameraReference;
                Vector3 camPosition = camera.transform.position;
                transform.Translate(Vector3.right * 4);
                pathCount += 1;
                camera.transform.position = camPosition;

            }
        }

        void UpdateCelebrating()
        {

            if (!isCelebrating)
            {
                MGC.Instance.WinMinigame();
                isCelebrating = true;
            } 
//            MGC.Instance.getMinigameStates().GetMinigame("Reedy"/*MGC.Instance.getSelectedMinigameProperties().readableName*/). FinishMinigame();
        }
        void UpdateDying()
        {
            if (isDead)
            {
                MGC.Instance.LoseMinigame();
                isDead = false;
            }

            
            if (Input.GetKeyDown(KeyCode.R))
            {
                //SceneManager.LoadScene(0);
                
                MGC.Instance.sceneLoader.LoadScene("ReddyRun");
                

            }
            //MGC.Instance.FinishMinigame();
        }



        void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.CompareTag("Finished"))
            {
                currentState = ReddyStates.CELEBRATING;
                anim.SetTrigger("finishTrigger");
                transform.Rotate(new Vector3(0, 180, 0));
                Camera camera = ReddyLevelManager.Instance.cameraReference;
                camera.transform.Rotate(new Vector3(40, 180, 0));
                camera.transform.Translate(Vector3.back * 16);
                //camera.transform.Translate(Vector3.down * 10);
                //GameObject.Find("MainCamera").transform.Rotate(new Vector3(0, -180, 0));
                //GameObject.Find("MainCamera").transform.Translate(Vector3.back * 13);
            }
            else if (other.gameObject.CompareTag("Obstacle"))
            {
                
                //GameObject.Find("mixamorig:Spine").GetComponent<CapsuleCollider>().enabled = false;
                currentState = ReddyStates.DYING;
                if (anim.GetCurrentAnimatorStateInfo(0).IsName("sprinting_forward_roll_inPlace") || anim.GetCurrentAnimatorStateInfo(0).IsName("jump_inPlace"))
                {
                    Vector3 newPosition = transform.position;

                    newPosition.y = 0;
                    transform.position = newPosition;
                    transform.Translate(Vector3.forward * 1);
                }
                else if (anim.GetCurrentAnimatorStateInfo(0).IsName("running_inPlace 1"))
                {

                    transform.Translate(Vector3.forward * -2);

                }
                anim.SetBool("isFalling", true);
                isDead = true;
            }
        }

        IEnumerator JumpCoroutine()
        {
            float height = anim.GetFloat("Jumpheight");
            isJumpInProgress = true;

            yield return new WaitForSeconds(0.1f);

            do
            {
                height = anim.GetFloat("Jumpheight");

                Vector3 newPosition = transform.position;
                newPosition.y = anim.GetFloat("Jumpheight");
                transform.position = newPosition;

                yield return null;
            } while (height > 0);

            isJumpInProgress = false;
        }

        IEnumerator SlideCoroutine()
        {

            float colliderY = 1.16f;
            isSlideInProgress = true;

            CapsuleCollider collider = GetComponent<CapsuleCollider>();
            collider.center = new Vector3(0, colliderY, 0);

            yield return new WaitForSeconds(1.1f);


            collider.center = new Vector3(0, 2.16f, 0);

                
            

            isJumpInProgress = false;
        }
    }

}
