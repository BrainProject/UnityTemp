
using UnityEngine;
using System.Collections;

namespace MinigamePexeso 
{
	/// <summary>
	/// Flipper script.
	/// Addached to tiles. Ensures flipping the tile image up and down.
	/// </summary>
	public class Flipper : MonoBehaviour 
    {
        /// <summary>
        /// speed of flipping animation
        /// </summary>
        public float moveSpeed = 4.0f;

        /// <summary>
        /// is tile moving?
        /// </summary>
	    public bool isMoving = false;
	    
        /// <summary>
        /// is tile flipped by image side up?
        /// </summary>
        public bool imageSideUp = false;

		/// <summary>
		/// The end position (used for rotating). Will change when tile is moved.
		/// </summary>
	    private Vector3 endRotation;

		/// <summary>
		/// Coroutine time variable.
		/// </summary>
	    private float t;

		/// <summary>
		/// Flip tile.
		/// </summary>
	    public void Flip()
	    {
			if (imageSideUp)
			{
				FlipImageDown ();
			}
			else
			{
				FlipImageUp ();
			}
	    }

		/// <summary>
		/// Flip tile image up.
		/// </summary>
	    public void FlipImageUp()
	    {
	        if (!imageSideUp)
			{
				if (!isMoving)
				{
					imageSideUp = true;
                    endRotation = Vector3.up;
					StartCoroutine(flipAnimation());
				}
			}
	    }

		/// <summary>
		/// Flip tile image down.
		/// </summary>
	    public void FlipImageDown()
	    {
			if (imageSideUp)
			{
				if (!isMoving)
				{
					imageSideUp = false;
                    endRotation = 180f * Vector3.up;
					StartCoroutine(flipAnimation());
				}
			}
	    }
	    
		/// <summary>
		/// Flip coroutine.
		/// Flips this tile.
		/// </summary>
	    private IEnumerator flipAnimation()
		{
            //this will ensure, that user can't click on this tile while it is flipped image-side up
            gameObject.GetComponent<BoxCollider>().enabled = false;

			isMoving = true;
			t = 0;
            Vector3 beginRotation = transform.eulerAngles;
			while (t < 1.0f)
			{
				t += Time.deltaTime * moveSpeed;
                transform.eulerAngles = Vector3.Lerp(beginRotation, endRotation, t);
				yield return null;
			}
            //print("rotation finished");
			isMoving = false;

            //after tile is flipped down, re-enable collider to allow mouse clicks on this object
            if (!imageSideUp)
            {
                gameObject.GetComponent<BoxCollider>().enabled = true;
            }
	    }
	}
}