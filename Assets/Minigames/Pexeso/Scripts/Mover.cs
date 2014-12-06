using UnityEngine;
using System.Collections;

namespace MinigamePexeso
{
	/// <summary>
	/// Mover script.
	/// Addached to tiles. Ensures moving the tile up to the camera and back down.
	/// </summary>
	public class Mover : MonoBehaviour
	{
		/// <summary>
		/// Define down and up position.
		/// </summary>
		void Start ()
		{
			downPosition = transform.position;
			upPosition = transform.position + moveVector;
		}
		
		// Update is called once per frame
		void Update ()
		{
		}

		/// <summary>
		/// Defines if tile is moving.
		/// </summary>
		public bool isMoving = false;

		/// <summary>
		/// Defines if tile is lifted.
		/// </summary>
		public bool isLifted = false;

		/// <summary>
		/// Defines if tile should be removed.
		/// </summary>
		public bool toRemove = false;

		/// <summary>
		/// The move vector.
		/// </summary>
		private Vector3 moveVector = new Vector3(0, 0, -1);

		/// <summary>
		/// Down position. Fixed for the whole time.
		/// </summary>
		private Vector3 downPosition;

		/// <summary>
		/// Up position. Fixed for the whole time.
		/// </summary>
		public Vector3 upPosition;

		/// <summary>
		/// The move speed. Fixed for the whole time.
		/// </summary>
		private float moveSpeed = 2f;

		/// <summary>
		/// The start position (used for moving). Will change when tile is moved.
		/// </summary>
		private Vector3 startPosition;

		/// <summary>
		/// The end position (used for moving). Will change when tile is moved.
		/// </summary>
		private Vector3 endPosition;

		/// <summary>
		/// Coroutine time variable.
		/// </summary>
		private float t;

		/// <summary>
		/// Determines if movement should change. Used when player clicks on tile
		/// when it is still moving.
		/// </summary>
		private bool switchTranslation = false;

		/// <summary>
		/// Move tile.
		/// </summary>
		public void Move()
		{
			if (isLifted)
			{
				MoveDown();
			}
			else
			{
				MoveUp();
			}
		}

		/// <summary>
		/// Move tile up.
		/// </summary>
		public void MoveUp()
		{
			if(!isLifted)
			{
				if(!isMoving)
				{
					isLifted = true;
					StartCoroutine(move(moveVector));
				}
				else
				{
					isLifted = true;
					switchTranslation = true;
				}
			}
			else if(transform.position == downPosition)
			{
				isLifted = false;
				MoveUp();
			}
		}

		/// <summary>
		/// Move tile down.
		/// </summary>
		public void MoveDown()
		{
			if (isLifted)
			{
				if (!isMoving)
				{
					isLifted = false;
					StartCoroutine(move(-moveVector));
				} else
				{
					isLifted = false;
					switchTranslation = true;
				}
			}
			else if(transform.position == upPosition)
			{
				isLifted = true;
				MoveDown();
			}
		}

		/// <summary>
		/// Switches end position bewteen down position and up position.
		/// Used when tile changes position.
		/// </summary>
		private void SwitchTargets()
		{
			startPosition = transform.position;
			if (isLifted)
			{
				endPosition = upPosition;
			}
			else
			{
				endPosition = downPosition;
			}
		}

		/// <summary>
		/// Move coroutine.
		/// Moves the tile along given vector.
		/// </summary>
		/// <param name="moveVector">Move vector.</param>
		private IEnumerator move(Vector3 moveVector)
		{
			isMoving = true;
			startPosition = transform.position;
			t = 0;
			
			endPosition = new Vector3(startPosition.x + moveVector.x,
			                          startPosition.y + moveVector.y,
			                          startPosition.z + moveVector.z);
			
			while (t < 1f)
			{
				if(switchTranslation)
				{
					t = 0;
					switchTranslation = false;
					SwitchTargets();
				}
				else
				{
					t += Time.deltaTime * moveSpeed;
					transform.position = Vector3.Lerp(startPosition, endPosition, t);
				}
				yield return null;
			}
			isMoving = false;
			yield return 0;
		}
	}
}