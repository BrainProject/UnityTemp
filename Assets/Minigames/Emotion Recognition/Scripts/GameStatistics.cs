using UnityEngine;
using System.Collections;

namespace EmotionRecognition
{

    public class GameStatistics : MonoBehaviour
    {

        public static int GameTurns
        {
            get;
            set;
        }
        public static int CorrectGameTurns
        {
            get;
            set;
        }
        public static double SuccessfulTurns()
        {
            return (double)CorrectGameTurns / (double)GameTurns;
        }

    }
}
