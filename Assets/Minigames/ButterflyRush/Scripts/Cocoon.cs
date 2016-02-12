using UnityEngine;
using System.Collections;

namespace ButterflyRush
{
    public class Cocoon : MonoBehaviour
    {

        public void HitReaction()
        {
            Instantiate(AntRushLevelManager.Instance.butterflyPrefab, transform.position, Quaternion.identity);
            --AntRushLevelManager.Instance.cocoonCount;
            AntRushLevelManager.Instance.CheckVictory();

            Destroy(this.gameObject);
        }
    }
}