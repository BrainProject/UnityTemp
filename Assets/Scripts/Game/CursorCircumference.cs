using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Can be used as visual feedback for "click-by-waiting" technique
/// </summary>
/// @author Jiří Chmelík
public class CursorCircumference : MonoBehaviour
{
    Image image;
    int direction = 0;
    
    /// <summary>
    /// speed of animation
    /// </summary>
    float speed = 0.75f;

    /// <summary>
    /// get reference for image component
    /// </summary>
    void Start()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// called by Event system whenever pointer enters object (collider)
    /// </summary>
    public void pointerEnter()
    {
        direction = 1;
    }

    /// <summary>
    /// called by Event system whenever pointer exits object (collider)
    /// </summary>
    public void pointerExit()
    {
        direction = -1;
    }
    
    /// <summary>
    /// updates fill amount of image based on direction
    /// </summary>
    void Update()
    {
        if(direction != 0)
        {
            image.fillAmount += direction * speed * Time.deltaTime;

            if ((image.fillAmount == 0) || (image.fillAmount == 1))
            {
                direction = 0;
            }
        }
    }
}
