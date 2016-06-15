using UnityEngine;
using System.Collections;

namespace MinigameMaze2
{
    public class PlayerScript : MonoBehaviour
    {

        private Vector3 initialPosition;

        // Use this for initialization
        void Start()
        {
            initialPosition = transform.position;
        }

        void Update()
        {
            if (transform.position.y > 10 || transform.position.y < -10)
            {
                Debug.Log("Seems like ball either fell down from the maze or took trip to the stars");
                ResetPosition();
            }
        }

        public void ResetPosition()
        {
            /*GameObject maze = GameObject.Find("Maze");
            maze.transform.rotation = Quaternion.Euler(0, 0, 0);*/
            
            transform.position = initialPosition;
        }
    }
}
