using UnityEngine;
using System.Collections;

public class SwipeDetector : MonoBehaviour
{
    private float minSwipeDistY;
    private float minSwipeDistX;
    private Vector2 startPos;

    void Start()
    {
        minSwipeDistY = Screen.height / 4;
        minSwipeDistX = Screen.width / 8;
    }

    void Update()
    {
        //#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {

            Touch touch = Input.touches[0];

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Ended:

                    float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;

                    if (swipeDistVertical > minSwipeDistY)
                    {
                        float swipeValue = Mathf.Sign(touch.position.y - startPos.y);

                        if (swipeValue > 0)//up swipe
                        {
                            // TODO: Put "up swipe" event here.
                        }
                        else if (swipeValue < 0)//down swipe
                        {
                            // TODO: Put "down swipe" event here.
                        }
                    }

                    float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;

                    if (swipeDistHorizontal > minSwipeDistX)
                    {
                        float swipeValue = Mathf.Sign(touch.position.x - startPos.x);

                        if (swipeValue > 0)//right swipe
                        {
                            // TODO: Put "right swipe" event here.
                        }

                        else if (swipeValue < 0)//left swipe
                        {
                            // TODO: Put "left swipe" event here.
                        }
                    }
                    break;
            }
        }
    }
}