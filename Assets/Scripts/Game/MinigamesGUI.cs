using UnityEngine;
using System.Collections;
using System.Diagnostics;

namespace Game
{
    public class MinigamesGUI : MonoBehaviour
    {
        public GameObject rewardIcon;

        public void show(bool showReward = false)
        {
            gameObject.SetActive(true);
            rewardIcon.SetActive(showReward);            
        }

        public void hide()
        {
            gameObject.SetActive(false);
        }
    }

}