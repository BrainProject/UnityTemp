using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class FinalCount : MonoBehaviour {
		#if UNITY_STANDALONE

	public int count;

	public void next()
	{
		count--;
		if(count <= 0)
		{
				LevelManager.win();
		}
		}
		#endif
}
}