using UnityEngine;
using System.Collections;

#if UNITY_STANDALONE
public class OnDestroyDestroy : MonoBehaviour {

	public int game;
	public GameObject destroy;

	void start()
	{
		SocialGame.LevelManager.gameSelected = 0;
	}

	void OnDestroy()
	{
		if(destroy)
		{
			SocialGame.LevelManager.gameSelected = game;
			Destroy(destroy);
		}
	}
}
#endif