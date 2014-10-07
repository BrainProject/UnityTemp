using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public float speedThrow;
	Transform hand;
	public string handName;
	public bool onHand;
	bool up;
	Vector3 lastPosition;
	float maxHeight;

	// Use this for initialization
	void Start () {
		lastPosition = transform.position;
//		var rotation = transform.rotation;
		addParent();
		transform.localPosition = Vector3.zero;
		transform.rotation = Quaternion.LookRotation(Vector3.up);
		lastPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(onHand)
		{
			if(transform.position.y -lastPosition.y > speedThrow )
			{
				removeParent();
				up = true;
				maxHeight = 2.5f;
			}
		}
		else
		{
			if(up)
			{
				transform.Translate(Vector3.forward * Time.deltaTime);
				up = transform.position.y < maxHeight;
			}
			else
			{
				transform.Translate(Vector3.back * Time.deltaTime);
				if(Vector2.Distance(transform.position,hand.position)< 0.2)
				{
					addParent();
				}
				if( transform.position.y < 0)
				{
					up = true;
					maxHeight =Mathf.Max(0,maxHeight - 0.2f);
				}
			}
		}
		lastPosition = transform.position;
	}

	void addParent()
	{
		hand = GameObjectEx.findGameObjectWithNameTag(handName,gameObject.tag).transform;
		if(hand == null)
		{
			Debug.Log("Parent not found");
		}
		transform.parent = hand;
		onHand = true;
	}

	void removeParent()
	{
		transform.parent = null;
		onHand = false;
	}
}
