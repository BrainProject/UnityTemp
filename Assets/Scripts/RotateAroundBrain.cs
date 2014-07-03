using UnityEngine;
using System.Collections;

public class RotateAroundBrain : MonoBehaviour {
	public int speedOfRotation = 1;
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
				this.transform.Rotate(this.transform.localRotation.x, this.transform.localRotation.y + speedOfRotation, this.transform.localRotation.z);
			if(previousMousePos.x > Input.mousePosition.x)
				this.transform.Rotate(this.transform.localRotation.x, this.transform.localRotation.y - speedOfRotation, this.transform.localRotation.z);
			if(previousMousePos.y < Input.mousePosition.y)
				this.transform.Rotate(this.transform.localRotation.x + speedOfRotation, this.transform.localRotation.y, this.transform.localRotation.z);
			if(previousMousePos.y > Input.mousePosition.y)
				this.transform.Rotate(this.transform.localRotation.x - speedOfRotation, this.transform.localRotation.y, this.transform.localRotation.z);
			previousMousePos = Input.mousePosition;
		}
	}
}
