/*
 * Created by: Milan Doležal
 */

using UnityEngine;
using System.Collections;

namespace MinigameSelection {
	public class SmoothCameraMove : MonoBehaviour {
		public float smooth = 5.0F;
		public float defaultSpeed = 1.0F;

		public float Speed { get; set; }
		public Vector3 From { get; set; }
		public Vector3 To { get; set; }
		public float FromYRot { get; set; }
		public float ToYRot { get; set; }
		public bool Move { get; set; }

		private float startTime;
		private float journeyLength;

		void Start() {
			Speed = defaultSpeed;
			Move = false;
			From = this.transform.position;
			To = this.transform.position;
			FromYRot = this.GetComponent<CameraControl>().currentWaypoint.transform.eulerAngles.y;
			ToYRot = this.GetComponent<CameraControl>().currentWaypoint.transform.eulerAngles.y;
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
			float distCovered = (Time.time - startTime) * Speed;
			float fracJourney = distCovered / journeyLength;
			if(journeyLength > 0.0f)
			{
				this.transform.position = Vector3.Lerp(From, To, fracJourney);
				this.transform.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(FromYRot, ToYRot, fracJourney), 0);
				if(Vector3.Distance(this.transform.position, To) < 0.01f)
				{
					//FromYRot = this.GetComponent<CameraControl>().currentWaypoint.transform.eulerAngles.y;
					//ToYRot = this.GetComponent<CameraControl>().currentWaypoint.transform.eulerAngles.y;
					//print (FromYRot + " and " + ToYRot);
				}
			}


			if((Input.GetButtonDown("Vertical") && Input.GetAxis("Vertical") < 0) || Input.GetMouseButtonDown(1))
			{
				//OnSelection = false;
				//StartCoroutine(mainCamera.GetComponent<SmoothCameraMove>().CameraLerp(Time.time));
				Move = true;
				Speed = defaultSpeed;
				From = this.transform.position;
				To = this.GetComponent<CameraControl>().currentWaypoint.transform.position;
				FromYRot = this.transform.eulerAngles.y;
				ToYRot = Camera.main.GetComponent<CameraControl>().currentWaypoint.transform.eulerAngles.y;
				Camera.main.GetComponent<CameraControl>().BackToMain();
				Camera.main.GetComponent<CameraControl>().ReadyToLeave = true;
				//mainCamera.GetComponent<SmoothCameraMove>().To = GameObject.Find("_GameManager").GetComponent<GameManager>().currentCameraDefaultPosition;
			}
		}
	}
}