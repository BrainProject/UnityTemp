using UnityEngine;
using System.Collections;

public class AntRushLevelManager : MonoBehaviour {
	public static AntRushLevelManager Instance { get; private set; }
	
	public int cocoonCount;
	public int startCount = 10;
    public float durationBetweenSpawn = 5;
	public GameObject cocoonPrefab;
	public GameObject butterflyPrefab;

    private float timestamp;
    private bool isGameFinished;

	void Awake()
	{
		Instance = this;
		cocoonCount = 0;

		for(int i=0; i< startCount; ++i)
		{
			SpawnCacoon();
		}
	}

    void Start()
    {
        timestamp = Time.time;
    }

	void Update()
	{
		if(Time.time - timestamp > durationBetweenSpawn)
		{
			if(!isGameFinished)
            {
				SpawnCacoon();
                timestamp = Time.time;
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
		if(cocoonCount == 0)
        {
            isGameFinished = true;
            MGC.Instance.WinMinigame();
		}
	}

	public void RestartLevel()
	{
		Application.LoadLevel (Application.loadedLevel);
	}
}
