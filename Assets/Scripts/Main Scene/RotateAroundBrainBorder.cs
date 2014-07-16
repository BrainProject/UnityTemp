using UnityEngine;
using System.Collections;

public class RotateAroundBrainBorder : MonoBehaviour {
	public int speedOfRotation = 50;
	public bool CanRotate{ get; set; }
	private Vector3 rotatingAxis;
	
	// Use this for initialization
	void Start()
	{
		CanRotate = false;
		switch(this.name)
		{
		case "Left": rotatingAxis = Vector3.up; break;
		case "Right": rotatingAxis = Vector3.down; break;
		case "Up": rotatingAxis = Vector3.left; break;
		case "Down": rotatingAxis = Vector3.right; break;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseOver()
	{
		if(CanRotate)
			this.transform.parent.transform.parent.transform.Rotate(rotatingAxis * Time.deltaTime * speedOfRotation);
	}

//	void OnGUI()
//	{
//		if(CanRotate)
//			if(GUI.Button(new Rect(20, 20, 200, 40), "Reset rotation"))
//			   this.transform.parent.transform.parent.transform.eulerAngles = new Vector3(0,0,0);
//	}
}
