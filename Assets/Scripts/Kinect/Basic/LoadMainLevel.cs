using UnityEngine;
using System.Collections;

#if UNITY_STANDALONE
namespace Kinect {
	public class LoadMainLevel : MonoBehaviour 
	{
		private bool levelLoaded = false;
		
		
		void Update() 
		{
			KinectManager manager = KinectManager.Instance;
			
			if(!levelLoaded && manager && KinectManager.IsKinectInitialized())
			{
				levelLoaded = true;
				Application.LoadLevel(2);
			}
		}		
	}
}
#endif