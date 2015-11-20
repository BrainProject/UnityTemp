using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FrogLevelManager : MonoBehaviour {
    public static FrogLevelManager Instance { get; private set; }

    public List<GameObject> carPrefabs = new List<GameObject>();
    public List<GameObject> boatPrefabs = new List<GameObject>();
    public GameObject frogPrefab;
    public GameObject waterSplashPrefab;
    public Transform lakeBorder;
    public Transform frogSpawn;
    public GameObject winScreen;

    public List<FrogGoal> frogGoals;

    void Awake()
    {
        Instance = this;
    }

    public bool IsGoalComplete()
    {
        for(int i=0; i<frogGoals.Count; ++i)
        {
            if(!frogGoals[i].occupied)
            {
                return false;
            }
        }
        return true;
    }

    public void RestartLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public GameObject GetRandomBoatPrefab()
    {
        if (boatPrefabs.Count > 0)
        {
            return boatPrefabs[Random.Range(0, boatPrefabs.Count)];
        }
        return null;
    }

    public GameObject GetRandomCarPrefab()
    {
        if (carPrefabs.Count > 0)
        {
            return carPrefabs[Random.Range(0, carPrefabs.Count)];
        }
        return null;
    }
}
