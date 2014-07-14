using UnityEngine;
using System.Collections;

public class SmoothCameraMove : MonoBehaviour {
	public Vector3 From { get; set; }
	public Vector3 To { get; set; }
	public bool Move { get; set; }

	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	public Transform target;
	public float smooth = 5.0F;

	void Start() {
		Move = false;
		From = this.transform.position;
		To = this.transform.position;
		startTime = Time.time;
		journeyLength = Vector3.Distance(From, To);
	}

	void Update() {
		if(Move)
		{
			startTime = Time.time;
			journeyLength = Vector3.Distance(From, To);
			Move = false;
		}
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		transform.position = Vector3.Lerp(From, To, fracJourney);
	}
}