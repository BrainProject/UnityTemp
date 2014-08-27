using UnityEngine;
using System.Collections;

namespace MinigamePexeso 
{
	public class Flipper : MonoBehaviour 
    {
        /// <summary>
        /// speed of flipping animation
        /// </summary>
        public float moveSpeed = 2f;

        /// <summary>
        /// is tile moving?
        /// </summary>
	    public bool isMoving = false;
	    
        /// <summary>
        /// is tile flipped by image side up?
        /// </summary>
        public bool imageSideUp = false;

	    
	    private Vector3 endRotation;
	    private float t;

	    public void Flip()
	    {
            //print("Flip");
			if (imageSideUp)
			{
				FlipImageDown ();
			}
			else
			{
				FlipImageUp ();
			}
	    }

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
	    
	    private IEnumerator flipAnimation()
		{
            //this will ensure, that user can't click on this tile while it is flipped image-side up
            gameObject.GetComponent<BoxCollider>().enabled = false;

			isMoving = true;
			t = 0;

			while (t < 1f)
			{
				t += Time.deltaTime * moveSpeed;
				transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, endRotation, t);
				yield return null;
			}

			isMoving = false;

            //after tile is flipped down, re-enable collider to allow mouse clicks on this object
            if (!imageSideUp)
            {
                gameObject.GetComponent<BoxCollider>().enabled = true;
            }
	    }
	}
}