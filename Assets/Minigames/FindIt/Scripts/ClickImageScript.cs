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
        void OnMouseDown()
        {
            if(this.gameObject.renderer.material.mainTexture.Equals(GameObject.Find("TargetImage").renderer.material.mainTexture))
//            if (this.gameObject.GetComponent<SpriteRenderer>().sprite.Equals(GameObject.Find("TargetImage").GetComponent<SpriteRenderer>().sprite))
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
                Camera.main.GetComponent<GameScript>().newTargetImage();
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
