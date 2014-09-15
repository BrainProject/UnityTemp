using UnityEngine;
using System.Collections;

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
