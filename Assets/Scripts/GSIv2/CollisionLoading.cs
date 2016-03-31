using UnityEngine;
using System.Collections;
using Image = UnityEngine.UI.Image;

//abstract class for colliders
abstract public class CollisionLoading : MonoBehaviour {


    private bool activated;         //collision has been detected
    private bool done;              //image is whole drawn
    protected Image img;

    protected virtual void Start ()
    {
        if (GetComponent<Image>())
        {
            img = GetComponent<Image>();
            img.fillAmount = 0;
            if(!img.sprite)
            {
                this.enabled = false;
            }
        }
        else
        {
            this.enabled = false;
        }
        activated = false;
        done = false;
    }
	
	void Update ()
    {

        if (activated)
        {
            if (img.fillAmount < 1)
            {
                img.fillAmount += (0.6f * Time.deltaTime);
            }
            else
            {
                done = true;
            }
        }
        else
        {
            img.fillAmount = 0;
        }

        if (done)
        {
            Action();
            done = false;
            img.fillAmount = 0;
            this.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        activated = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        activated = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract void Action();
    
}
