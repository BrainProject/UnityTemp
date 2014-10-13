using UnityEngine;
using System.Collections;

#if UNITY_STANDALONE
namespace SocialGame{
	public class FinalCount : MonoBehaviour {

	public int count;

	public void next()
	{
		count--;
		if(count <= 0)
		{
				LevelManager.finish();
		}
	}
}
}
#endif