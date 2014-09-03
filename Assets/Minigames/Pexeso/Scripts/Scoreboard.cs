using UnityEngine;
using System.Collections;

namespace MinigamePexeso 
{
	public class Scoreboard : MonoBehaviour 
    {
	    public GUIText correct;
	    public GUIText wrong;
	    public GUIText time;

	    //Update variables
	    private Ray ray;
	    private RaycastHit hit;
	    private GameObject first;

		void Update ()
	    {
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	        if (Physics.Raycast(ray, out hit))
	        {
	            //user clicks left mouse button and hits scoreboard
	            if (Input.GetMouseButtonUp(0) && hit.collider == this.collider)
	            {
					this.gameObject.SetActive(false);
	            }
	        }
		}
	}
}