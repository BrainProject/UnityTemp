using UnityEngine;
using System.Collections;

namespace Kinect
{
	public class SetSceneAvatars : MonoBehaviour 
	{
#if UNITY_STANDALONE
		void Start () 
		{
			KinectManager manager = KinectManager.Instance;
			
			if(manager)
			{
				manager.ClearKinectUsers();
				
				AvatarController[] avatars = FindObjectsOfType(typeof(AvatarController)) as AvatarController[];
				
				foreach(AvatarController avatar in avatars)
				{
					manager.avatarControllers.Add(avatar);
				}
				
			}
		}
#endif
	}	
}