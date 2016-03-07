using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Kinect;

namespace GSIv2
{
    public class AvatarSwitcher : MonoBehaviour {
#if UNITY_STANDALONE
        public static AvatarSwitcher Instance { get; private set; }
        public List<AvatarController> players = new List<AvatarController>();

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
            StartCoroutine(BeginGSIMinigame());
        }

        #region public

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
            for (int i = 0; i < players.Count; ++i)
            {
                players[i].enabled = !isActive;
            }
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
                ActivateMGCInteraction(true);
            }
            else
            {
                ActivateMGCInteraction(false);
            }
        }

        void Start()
        {
            StartCoroutine(SetThisMinigamePlayed());
        }

        /// <summary>
        /// Initiates Kinect for the GSI minigame at the beginning.
        /// </summary>
        IEnumerator BeginGSIMinigame()
        {
            while (!MGC.Instance)
            {
                yield return new WaitForSeconds(1);
            }
            while (!KinectManager.Instance)
            {
                yield return new WaitForSeconds(1);
            }

            MGC.Instance.isKinectRestartRequired = true;
            MGC.Instance.kinectManagerInstance = KinectManager.Instance;
            MGC.Instance.kinectManagerInstance.ClearKinectUsers();
            MGC.Instance.kinectManagerInstance.avatarControllers.Clear();

            for (int i = 0; i < players.Count; ++i)
            {
                KinectManager.Instance.avatarControllers.Add(players[i]);
            }

            bool bNeedRestart = false;
            KinectInterop.InitSensorInterfaces(false, ref bNeedRestart);
            KinectManager.Instance.StartKinect();
            Debug.Log("Activate This kinect manager");
            ActivateMGCInteraction(false);
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