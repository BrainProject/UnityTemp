using UnityEngine;
using System.Collections;

namespace MinigameMaze
{
	/// <summary>
	/// Player script. Computes player movement according to mouse position.
	/// </summary>
	public class PlayerScript : MonoBehaviour
	{
        /// <summary>
        /// Should the holder for player object be used?
        /// </summary>
        public bool useHolder;
        
        /// <summary>
		/// Used for mouse pointer detection.
		/// </summary>
		private Ray ray;
		/// <summary>
		/// Used for mouse pointer detection. Object under mouse cursor.
		/// </summary>
		private RaycastHit hit;

		/// <summary>
		/// True if user currently moves player object.
		/// </summary>
		private bool isPlayerTapped;

		// Use this for initialization
		void Start ()
		{
            if (!useHolder)
            {
                GameObject.Find("Holder").SetActive(false);
            }
		}

        public Vector3 difference;

		// Update is called once per frame
		void Update ()
		{
			if (Input.GetMouseButton (0))
			{
                if (isPlayerTapped)
                {
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = 10f + 1.95f;
                    Vector3 mouseCoordinates = Camera.main.ScreenToWorldPoint(mousePos);
                    transform.position = mouseCoordinates - difference;
                    return;
                }
                
                ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				Vector3 position = new Vector3 (0, 1, -10);
				if (Physics.Raycast (position, ray.direction, out hit))
				{
                    if (hit.collider.name == "PlayerObject" || hit.collider.name == "Holder")
					{
						Vector3 mousePos = Input.mousePosition;
						mousePos.z = 10f + 1.95f;
						Vector3 mouseCoordinates = Camera.main.ScreenToWorldPoint (mousePos);
                        difference = mouseCoordinates - transform.position;
                        transform.position = mouseCoordinates - difference;
						isPlayerTapped = true;
					}
					
				}
			}
			else
			{
				if(isPlayerTapped)
				{
					isPlayerTapped = false;
				}
			}
		}
	}
}
