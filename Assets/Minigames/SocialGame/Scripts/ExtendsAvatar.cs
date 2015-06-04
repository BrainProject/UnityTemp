#if UNITY_STANDALONE
using UnityEngine;
using System.Collections;
using Kinect;

namespace SocialGame{
	public class ExtendsAvatar : AvatarController {
		public Transform handLeft;
		public Transform handRight;
		public bool realPosition = true;

		private Vector3 newPos;
		private bool run = true;

		/// <summary>
		/// Crection of posion of avatar.
		/// </summary>
		void Update()
		{
			if(realPosition && KinectManager.Instance && (LastUserID != 0) && run)
			{
				newPos = this.transform.position;
				newPos.x = Kinect.KinectManager.Instance.GetUserPosition(LastUserID).x;
				this.transform.position = newPos;
			}
		}

		void Stop(bool stop)
		{
			Debug.Log (stop);
			run = !stop;
		}
	}
}
#endif