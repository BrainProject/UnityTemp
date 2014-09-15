using UnityEngine;
using System.Collections;

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
