using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Kinect;

namespace GSIv2
{
    public class AvatarSwitcher : MonoBehaviour {
#if UNITY_STANDALONE
        public static GameObject thisLevelKManager;
        public static AvatarSwitcher instance
        {
            get; private set;
        }
        public AvatarController player1;
        //public AvatarController player2;

        public GameObject thisSceneCanvas;

        // Use this for initialization
        void Awake()
        {


            if (!MGC.Instance)
            {
                Debug.Log("Creating MGC " + MGC.Instance);
#if UNITY_EDITOR
                StartCoroutine(GoToCroossroadWithDelay());
#endif
            }

            instance = this;
            setThisLevelManager();
        }

        void setThisLevelManager()
        {
            if (MGC.Instance)
            {
                MGC.Instance.isKinectRestartRequired = true;
                if (KinectManager.Instance)
                {
                    MGC.Instance.kinectManagerInstance = KinectManager.Instance;
                    MGC.Instance.kinectManagerInstance.ClearKinectUsers();
                    MGC.Instance.kinectManagerInstance.avatarControllers.Clear();
                    if (player1)
                        KinectManager.Instance.avatarControllers.Add(player1);

                    //if (player2)
                    //  KinectManager.Instance.avatarControllers.Add(player2);
                    
                    bool bNeedRestart = false;
                    KinectInterop.InitSensorInterfaces(false, ref bNeedRestart);
                    KinectManager.Instance.StartKinect();
                    Debug.Log("Activate This kinect manager");
                }
            }
        }


        public static void activateThisLevelKManager()
        {
            //instance.setThisLevelManager();
            MGC.Instance.ShowCustomCursor(false);
            InteractionManager im = KinectManager.Instance.GetComponent<InteractionManager>();
            im.controlMouseCursor = false;
            im.controlMouseDrag = false;
            im.allowHandClicks = false;
            instance.player1.enabled = true;
            //if(instance.player2)
            //  instance.player2.enabled = true;


            if (instance.thisSceneCanvas)
                instance.thisSceneCanvas.SetActive(true);
        }


        public static void setActiveMGC(bool active)
        {
            if (active)
            {
                MGC.Instance.ShowCustomCursor(true);
                MGC.Instance.kinectManagerInstance.ClearKinectUsers();
                MGC.Instance.kinectManagerInstance.StartKinect();
                MGC.Instance.kinectManagerInstance.avatarControllers.Clear();
            }
            else
            {
                if (MGC.Instance.mouseCursor)
                    MGC.Instance.ShowCustomCursor(false);
            }
        }


        public static void deactivateThisLevelKManager()
        {
            MGC.Instance.ShowCustomCursor(true);
            InteractionManager im = MGC.Instance.kinectManagerInstance.GetComponent<InteractionManager>();
            im.controlMouseCursor = true;
            im.controlMouseDrag = true;
            im.allowHandClicks = true;
            instance.player1.enabled = false;
            //if(instance.player2)
            //  instance.player2.enabled = false;

            if (instance.thisSceneCanvas)
                instance.thisSceneCanvas.SetActive(false);
        }


        IEnumerator GoToCroossroadWithDelay()
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("Crossroad");
        }



        
        public bool activatedGUI = false;
        public GameObject[] ObjToPause;


        

        void OnEnable()
        {
            MGC.TakeControlForGUIEvent += GiveControl;
        }

        void OnDisable()
        {
            MGC.TakeControlForGUIEvent -= GiveControl;
        }

        void GiveControl(bool taken)
        {
            Debug.Log("Control taken: " + taken);
            activatedGUI = taken;
            if (taken)
            {
                deactivateThisLevelKManager();
            }
            else
            {
                activateThisLevelKManager();
            }
            StopAll(taken);
        }

        void Start()
        {
            StartCoroutine(SetThisMinigamePlayed());
        }
        
        public void StopAll(bool stop)
        {
            foreach (GameObject obj in ObjToPause)
            {
                obj.SendMessage("Stop", stop);
            }
        }

        private IEnumerator SetThisMinigamePlayed()
        {
            while (!MGC.Instance)
            {
                Debug.Log(MGC.Instance);
                yield return new WaitForSeconds(1);
            }
            MGC.Instance.minigamesProperties.SetPlayed(SceneManager.GetActiveScene().name);
        }
#endif
    }
}