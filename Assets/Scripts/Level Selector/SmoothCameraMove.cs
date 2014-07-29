/*
 * Created by: Milan Doležal
 */

using UnityEngine;
using System.Collections;

public class SmoothCameraMove : MonoBehaviour {
	public float smooth = 5.0F;
	public float speed = 1.0F;

	public Vector3 From { get; set; }
	public Vector3 To { get; set; }
	public bool Move { get; set; }

	private float startTime;
	private float journeyLength;
	//public Transform target;

	void Start() {
		Move = false;
		From = this.transform.position;
		To = this.transform.position;
		startTime = Time.time;
		journeyLength = Vector3.Distance(From, To);
	}

	void Update()
	{
		if(Move)
		{
			startTime = Time.time;
			journeyLength = Vector3.Distance(From, To);
			Move = false;
		}
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		if(journeyLength > 0.0f)
			this.transform.position = Vector3.Lerp(From, To, fracJourney);
	}

//	public IEnumerator CameraLerp(float startTime)
//	{	
//		print ("start");
//		if(Move)
//		{
//			print ("moving");
//			print ("here?");
//			journeyLength = Vector3.Distance(From, To);
//			print (journeyLength);
//			Move = false;
//		}
//		print ("distance");
//		float distCovered = (Time.time - startTime) * speed;
//		float fracJourney = distCovered / journeyLength;
//		print (distCovered);
//		print (fracJourney);
//		while(journeyLength > 0.0f)
//		{
//			this.transform.position = Vector3.Lerp(From, To, fracJourney);
//			yield return null;
//		}
//	}

//	public IEnumerator CameraLerp(Vector3 from, Vector3 to)
//	{
//		Move = true;
//		if(Move)
//		{
//			startTime = Time.time;
//			journeyLength = Vector3.Distance(from, to);
//			Move = false;
//		}
//		while(journeyLength > 0.0f)
//		{
//			float distCovered = (Time.time - startTime) * speed;
//			float fracJourney = distCovered / journeyLength;
//			this.transform.position = Vector3.Lerp(from, to, fracJourney);
//			yield return null;
//		}
//	}
}