using UnityEngine;
using System.Collections;


namespace SocialGame
{
    public class CheckSwitchObj : CheckCancleFigure
    {
#if UNITY_STANDALONE

        public Check parsedCheck;
        public GameObject player1;
        public GameObject player2;
        public string switchName;
        public int owner;
        // Use this for initialization
        public override void Start()
        {
            base.Start();
            if (!player1)
            {
                player1 = GameObjectEx.FindGameObjectWithNameTag(switchName, "Player1");
                owner = 2;
            }

            if (!player2)
            {
                player2 = GameObjectEx.FindGameObjectWithNameTag(switchName, "Player2");
                owner = 1;
            }
        }

        void Update()
        {
            if (parsedCheck && parsedCheck.activated != activated)
            {
                activated = parsedCheck.activated;
                show();
                TimerReset();
            }
        }



        protected override void EndTimer()
        {
            //bool last = false;
            //finishTarget = target;
            TimerReset();
            switch (owner)
            {
                case (1):
                    player2.SetActive(true);
                    player1.SetActive(false);
                    break;
                case (2):
                    player1.SetActive(true);
                    player2.SetActive(false);
                    break;
            }
            //return last;
        }
#endif
    }
}