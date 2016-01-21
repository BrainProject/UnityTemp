using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ButterflyRush
{
    public class AntRushLevelManager : MonoBehaviour
    {
        public static AntRushLevelManager Instance { get; private set; }

        public int cocoonCount;
        public int startCount = 10;
        public float durationBetweenSpawnAndroid = 5;
        public float durationBetweenSpawnStandalone = 10;
        public GameObject cocoonPrefab;
        public GameObject butterflyPrefab;

        private float durationBetweenSpawn;
        private float timestamp;
        private bool isGameFinished;

        void Awake()
        {
            Instance = this;
            cocoonCount = 0;
            startCount += 3 * MGC.Instance.selectedMiniGameDiff;

            for (int i = 0; i < startCount; ++i)
            {
                SpawnCacoon();
            }
        }

        void Start()
        {
#if UNITY_ANDROID
            durationBetweenSpawn = durationBetweenSpawnAndroid;       
#else
            durationBetweenSpawn = durationBetweenSpawnStandalone;
#endif
            timestamp = Time.time;
            durationBetweenSpawn = Mathf.Clamp(durationBetweenSpawn - (float)MGC.Instance.selectedMiniGameDiff / 4, 0.01f, 60);
        }

        void Update()
        {
            if (Time.time - timestamp > durationBetweenSpawn)
            {
                if (!isGameFinished)
                {
                    SpawnCacoon();
                    timestamp = Time.time;
                }
            }
        }

        void SpawnCacoon()
        {
            Vector2 spawnPos = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(Screen.width / 5, Screen.width - Screen.width / 5),
                                                                            Random.Range(Screen.height / 4, Screen.height - Screen.height / 4)));
            Instantiate(cocoonPrefab, spawnPos, Quaternion.identity);
            ++cocoonCount;
        }

        public void CheckVictory()
        {
            if (cocoonCount == 0)
            {
                isGameFinished = true;
                MGC.Instance.WinMinigame();
            }
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}