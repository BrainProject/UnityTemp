using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Kinect;

namespace GSIv2
{
    public class AvatarSwitcher : MonoBehaviour {
#if UNITY_STANDALONE
        public static AvatarSwitcher Instance { get; private set; }
        public AvatarController player1;
        //public AvatarController player2;

        void Awake()
        {
            if (!MGC.Instance)
            {
                Debug.Log("Creating MGC " + MGC.Instance);
#if UNITY_EDITOR
                //StartCoroutine(GoToCroossroadWithDelay());
#endif
            }

            Instance = this;
            BeginGSIMinigame();
        }
        #region public

        /// <summary>
        /// Initiates Kinect for the GSI minigame at the beginning.
        /// </summary>
        void BeginGSIMinigame()
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


        /// <summary>
        /// Activates or deactivates the default GSI interaction with hand cursor.
        /// If parameter is false, GSI interaction (with avatar) is used and hand cursor controls is disabled.
        /// </summary>
        /// <param name="isActive"></param>
        public void ActivateMGCInteraction(bool isActive)
        {
            InteractionManager im = MGC.Instance.kinectManagerInstance.GetComponent<InteractionManager>();
            MGC.Instance.ShowCustomCursor(isActive);
            im.controlMouseCursor = isActive;
            im.controlMouseDrag = isActive;
            im.allowHandClicks = isActive;
            Instance.player1.enabled = !isActive;
        }

        #endregion
        #region private

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
            if (taken)
            {
                ActivateMGCInteraction(false);
            }
            else
            {
                ActivateMGCInteraction(true);
            }
        }

        void Start()
        {
            StartCoroutine(SetThisMinigamePlayed());
        }

        IEnumerator SetThisMinigamePlayed()
        {
            while (!MGC.Instance)
            {
                Debug.Log(MGC.Instance);
                yield return new WaitForSeconds(1);
            }
            MGC.Instance.minigamesProperties.SetPlayed(SceneManager.GetActiveScene().name);
        }
        
        IEnumerator GoToCroossroadWithDelay()
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("Crossroad");
        }

        #endregion
#endif
    }
}