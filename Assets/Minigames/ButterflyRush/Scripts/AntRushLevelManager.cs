using UnityEngine;
using System.Collections;

public class AntRushLevelManager : MonoBehaviour {
	public static AntRushLevelManager Instance { get; private set; }
	
	public int cocoonCount;
	public int startCount = 10;
	public GameObject cocoonPrefab;
	public GameObject butterflyPrefab;
	public GameObject victoryGUI;

	void Awake()
	{
		Instance = this;
		cocoonCount = 0;

		for(int i=0; i< startCount; ++i)
		{
			SpawnCacoon();
		}
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(!victoryGUI.activeSelf)
			{
				SpawnCacoon();
			}
		}
	}

	void SpawnCacoon()
	{
		Vector2 spawnPos = Camera.main.ScreenToWorldPoint (new Vector2 (Random.Range (0, Screen.width), Random.Range (0, Screen.height)));
		Instantiate (cocoonPrefab, spawnPos, Quaternion.identity);
		++cocoonCount;
	}

	public void CheckVictory()
	{
		if(cocoonCount <= 0)
		{
			victoryGUI.SetActive(true);
			Debug.Log("VICTORY");
		}
	}

	public void RestartLevel()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}
