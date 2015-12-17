using UnityEngine;
using System.Collections;

namespace MinigameSelection
{
	public class MinigameSelectionUI : MonoBehaviour 
	{
		public string minigameName;
		public bool isKinectRequired = false;
		public bool isKinectSupported = true;
		public bool isAndroidSupported = true;

		void Start()
		{
#if UNITY_STANDALONE
			if(!isKinectSupported)
			{
				gameObject.SetActive(false);
			}
#elif UNITY_ANDROID
			if(!isAndroidSupported)
			{
				gameObject.SetActive(false);
			}
#endif
		}


		public void SelectMinigame()
		{
            print("Starting: " + minigameName);

			if(isKinectRequired)
			{
				if (!MGC.Instance.kinectManagerObject.activeSelf)
				{
					MenuLevelManager.Instance.FadeInOutKinectIcon();
				}
				else
				{
					MGC.Instance.startMiniGame (minigameName);
				}
			}
			else
			{
				MGC.Instance.startMiniGame (minigameName);
			}
		}
	}
}