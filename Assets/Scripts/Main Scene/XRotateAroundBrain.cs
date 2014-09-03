/*
 * Created by: Milan Doležal
 */ 

using UnityEngine;
using System.Collections;

namespace MainScene {
	public class RotateAroundBrain : MonoBehaviour {
		public int speedOfRotation = 50;
		public bool CanRotate{ get; set; }
		private Vector3 previousMousePos;

		// Use this for initialization
		void Start()
		{
			CanRotate = false;
			previousMousePos = new Vector3(0,0,0);
		}

		// Update is called once per frame
		void Update()
		{
			if (CanRotate)
			{
				print ("I'm the MASTER");
				if(previousMousePos.x < Input.mousePosition.x)
					this.transform.Rotate(Vector3.up * Time.deltaTime * speedOfRotation);
				if(previousMousePos.x > Input.mousePosition.x)
					this.transform.Rotate(Vector3.down * Time.deltaTime * speedOfRotation);
				/*if(previousMousePos.y < Input.mousePosition.y)
					this.transform.Rotate(Vector3.left * Time.deltaTime * speedOfRotation);
				if(previousMousePos.y > Input.mousePosition.y)
					this.transform.Rotate(Vector3.right * Time.deltaTime * speedOfRotation);*/
				previousMousePos = Input.mousePosition;
			}
		}
	}
}