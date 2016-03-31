using UnityEngine;
using System.Collections;


namespace GSIv2
{
    public class HandColiderMovement : MonoBehaviour
    {
        public Transform hand;

        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            

            //GetComponent<Rigidbody2D>().velocity = (movement * 800.0f * Time.deltaTime);
            transform.Translate(movement);           
            transform.position = Camera.main.WorldToScreenPoint(hand.position);
            
        }
    }

}
