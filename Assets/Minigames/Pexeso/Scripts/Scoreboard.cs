using UnityEngine;
using System.Collections;

namespace MinigamePexeso 
{
	/// <summary>
	/// @Deprecated.
	/// Scoreboard class.
	/// </summary>
	public class Scoreboard : MonoBehaviour 
    {
	    /// <summary>
	    /// Score. Number of correct pairs.
	    /// </summary>
		public GUIText correct;

		/// <summary>
		/// Score. Number of wrong pairs.
		/// </summary>
	    public GUIText wrong;

		/// <summary>
		/// Score. Game time.
		/// </summary>
	    public GUIText time;

	    /// <summary>
	    /// Used for tracking the clicking.
	    /// </summary>
	    private Ray ray;
	    private RaycastHit hit;
	    private GameObject first;

		/// <summary>
		/// Wait for player to click on scoreboard and hide it.
		/// </summary>
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