using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        downPosition = transform.position;
        upPosition = transform.position + moveVector;
	}
	
	// Update is called once per frame
	void Update ()
    {
	}

    public bool isMoving = false;
    public bool isLifted = false;
    public bool toRemove = false;

    private Vector3 moveVector = new Vector3(0, 0, -1);
    private Vector3 downPosition;
    public Vector3 upPosition;

    private float moveSpeed = 2f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float t;

    private bool switchTranslation = false;

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
    
    private IEnumerator move(Vector3 moveVector) {
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
