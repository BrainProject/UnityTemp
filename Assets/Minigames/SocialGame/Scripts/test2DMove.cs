using UnityEngine;
using System.Collections;

public class test2DMove : MonoBehaviour {
	public Transform target;
	public float speed;
	void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log("baf2");
	}

}
