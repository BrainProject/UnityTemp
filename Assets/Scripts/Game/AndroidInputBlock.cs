using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
    public class AndroidInputBlock : MonoBehaviour {
        public Image thisImage;

#if !UNITY_ANDROID || UNITY_EDITOR
        void Start()
        {
            gameObject.SetActive(false);
        }
#endif

        void OnMouseDown()
        { 
                thisImage.raycastTarget = false;
          }

        void LateUpdate()
        {
            if (Input.GetMouseButtonUp(0))
            {
                thisImage.raycastTarget = true;
                //StopAllCoroutines();
                //StartCoroutine(ReenableBlockWithDelay());
            }
        }

        IEnumerator ReenableBlockWithDelay()
        {
            yield return new WaitForSeconds(0.5f);
        }
    }
}