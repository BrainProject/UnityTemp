using UnityEngine;
using System.Collections;
using System.IO;


namespace EmotionRecognition
{

    public class ClickButtonScript : MonoBehaviour
    {
        public GameObject scriptHolder;

        private GameScript gameScript;

        void Start()
        {
            if (scriptHolder == null)
            {
                Debug.LogError("GameScriptHolder instance is not set! ");
            }
            else
            {
                gameScript = scriptHolder.GetComponent<GameScript>();
                if (gameScript == null)
                {
                    Debug.LogError("GameScript is not assigned to GameScriptHolder! ");
                }
            }
            
        }

        public void Clicked()
        {
            Debug.Log(this.name + "," + gameScript.GetCorrectButton());

            if (string.Equals(this.name, gameScript.GetCorrectButton()) && gameScript.first)
            {
                GameStatistics.CorrectGameTurns += 1;
                gameScript.GameTurn();
            }
            else if (string.Equals(this.name, gameScript.GetCorrectButton()))
            {
                gameScript.GameTurn();
            }
            else
            {
                gameScript.first = false;
            }
        }
    }
}
