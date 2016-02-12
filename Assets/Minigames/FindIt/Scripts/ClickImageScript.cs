/**
 * @file ClickImageScript.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;


namespace FindIt
{
    /**
     * Callback for clicking on an image
     */
    public class ClickImageScript : MonoBehaviour
    {
        public GameObject gameScriptHolder;

        public GameObject targetImage;

        private GameScript gameScript;

        void Start()
        {
            if (gameScriptHolder == null)
            {
                Debug.LogError("GameScriptHolder in some ClickImageScript instance is not set! ");
            }
            else
            {
                gameScript = gameScriptHolder.GetComponent<GameScript>();
                if(gameScript == null)
                {
                    Debug.LogError("GameScript is not assigned to some GameScriptHolder! ");
                }
            }
            if(targetImage == null)
            {
                Debug.LogError("TargetImage in some ClickImageScript instance is not set!");
            }
        }

        void OnMouseDown()
        {
            if (!gameScript.gameWon)
            {
                if (this.gameObject.GetComponent<Renderer>().material.mainTexture.Equals(targetImage.GetComponent<Renderer>().material.mainTexture))
                {
                    if (this.gameObject.tag == "Left")
                    {
                        FindItStatistics.RecordLeftGoodClick();
                    }
                    else if (this.gameObject.tag == "Right")
                    {
                        FindItStatistics.RecordRightGoodClick();
                    }
                    // else unexpected error
                    gameScript.newTargetImage();
                }
                else
                {
                    if (this.gameObject.tag == "Left")
                    {
                        FindItStatistics.RecordLeftWrongClick();
                    }
                    else if (this.gameObject.tag == "Right")
                    {
                        FindItStatistics.RecordRightWrongClick();
                    }
                }
            }
        }
    }
}
