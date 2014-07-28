using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public bool isMoving = false;
    public bool isLifted = false;
    public bool toRemove = false;

    private float moveSpeed = 2f;
	private Vector3 startPosition;
    private Vector3 endPosition;
    private float t;

    public void Move()
    {
		if (isLifted)
		{
			MoveDown ();
		}
		else
		{
			MoveUp ();
		}
    }

    public void MoveUp()
    {
        if (!isLifted)
		{
			if (!isMoving)
			{
				isLifted = true;
				StartCoroutine(move());
			}
		}
    }

    public void MoveDown()
    {
		if (isLifted)
		{
			if (!isMoving)
			{
				isLifted = false;
				StartCoroutine(move());
			}
		}
    }
    
    private IEnumerator move()
	{
		isMoving = true;
		startPosition = transform.eulerAngles;
		t = 0;
		endPosition = transform.eulerAngles + 180f * Vector3.up; // what the new angles should be

		while (t < 1f)
		{
			t += Time.deltaTime * moveSpeed;
			transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, endPosition, t);
			yield return null;
		}
		isMoving = false;
		yield return 0;
    }
}
