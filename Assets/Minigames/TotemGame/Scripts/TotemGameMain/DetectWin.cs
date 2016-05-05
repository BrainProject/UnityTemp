using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TotemGame
{
    public class DetectWin : MonoBehaviour
    {
        private float collisionStartTimestamp;
        private bool isWon;
        public int durationToWin = 2;
        public Image circularSilder;
        public RectTransform circularSliderTransform;

        void Start()
        {
            isWon = false;
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject == TotemLevelManager.Instance.player)
            {
                if (!isWon)
                {
                    collisionStartTimestamp = Time.time;
                    StartCoroutine(WinningProgress());
                }
            }
        }

        void OnCollisionExit(Collision col)
        {
            if (col.gameObject == TotemLevelManager.Instance.player)
            {
                StopAllCoroutines();
                circularSilder.fillAmount = 0;
            }
        }

        void GameOver()
        {
            TotemLevelManager.Instance.player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

            GameObject[] allObjects = FindObjectsOfType<GameObject>();
            foreach (GameObject go in allObjects)
            {
                if (go.GetComponent<DestroyObject>())
                {
                    go.GetComponent<DestroyObject>().enabled = false;
                }
            }
        }

        IEnumerator WinningProgress()
        {
            float step = 0;

            while(circularSilder.fillAmount < 1)
            {
                step = (Time.time - collisionStartTimestamp) / durationToWin;
                circularSilder.fillAmount = Mathf.Lerp(0, 1, step);
                circularSliderTransform.position = Camera.main.WorldToScreenPoint(TotemLevelManager.Instance.player.transform.position);

                yield return null;
            }
            isWon = true;
            GameOver();
            MGC.Instance.WinMinigame();
            circularSilder.fillAmount = 0;
        }
    }
}
