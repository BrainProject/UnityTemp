using UnityEngine;
using System.Collections;

namespace Kinect {
	public class LoadMainLevel : MonoBehaviour 
	{
		#if UNITY_STANDALONE
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
		#endif	
	}
}