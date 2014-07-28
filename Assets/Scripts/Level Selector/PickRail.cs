using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickRail: MonoBehaviour {
	private Color OriginalColor { get; set; }
	private Vector3 tmp;
	//private Vector3 trashPosition;
	private bool picked;

	void Start()
	{
		OriginalColor = this.renderer.material.color;
		//trashPosition = GameObject.FindGameObjectWithTag ("Remove").transform.position;
		picked = true;
	}


	public void Update()
	{
		if (picked)
		{
			tmp = Camera.main.WorldToScreenPoint (transform.position);
			tmp.x = Input.mousePosition.x;
			tmp.y = Input.mousePosition.y;
			tmp = Camera.main.ScreenToWorldPoint (tmp);
			tmp.z = 0;
			transform.position = tmp;
			if(Input.GetKeyDown (KeyCode.Delete) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
			   Destroy (gameObject);
			if(Input.GetKeyDown (KeyCode.Space))
				transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.RoundToInt(transform.eulerAngles.z + 180));
		}
	}

	void OnMouseEnter()
	{
		this.renderer.material.color = Color.green;
	}

	void OnMouseExit()
	{
		if(!picked)
			this.renderer.material.color = OriginalColor;
	}

	void OnMouseDown()
	{
		picked = !picked;
		if(!picked)
		{
			//if (Vector3.Distance (this.transform.position, trashPosition) < 2)
				//Destroy (gameObject);
			List<GameObject> connections = new List<GameObject>(GameObject.FindGameObjectsWithTag ("Connection"));
			connections.Remove(transform.GetChild(0).gameObject);
			connections.Remove(transform.GetChild(1).gameObject);
			GameObject nearestConnection;
			if (connections.Count > 0)
			{
				nearestConnection = connections [0];
				foreach (GameObject conn in connections)
					if (Vector3.Distance (nearestConnection.transform.position, this.transform.position) > Vector3.Distance (this.transform.position, conn.transform.position))
						nearestConnection = conn;
				if (Vector3.Distance (nearestConnection.transform.position, this.transform.position) < 10)
				{
					Transform leftConn = transform.Find("L-" + this.name.Substring(0,this.name.Length - 7));
					Transform rightConn = transform.Find("R-" + this.name.Substring(0,this.name.Length - 7));
					//left is closer
					if(Vector3.Distance(nearestConnection.transform.position, leftConn.transform.position)
					   < Vector3.Distance(nearestConnection.transform.position, rightConn.transform.position))
						SnapToConnection(nearestConnection, leftConn);
					//right is closer
					else
						SnapToConnection(nearestConnection, rightConn);
				}
			}
			this.renderer.material.color = OriginalColor;
		}
	}

	void SnapToConnection(GameObject nearestConnection, Transform localConnection)
	{
		//print ("Nearest conn pos: " + nearestConnection.transform.position);
		//print ("Connection pos: " + localConnection.transform.position);
		//print ("Angle: " + (nearestConnection.transform.eulerAngles.z - transform.eulerAngles.z));

		//transform.RotateAround ( localConnection.transform.position, Vector3.forward, (nearestConnection.transform.localEulerAngles.z - localConnection.transform.localEulerAngles.z));
		
		//transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, nearestConnection.transform.eulerAngles.z);

		//transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + (nearestConnection.transform.eulerAngles.z - localConnection.transform.localEulerAngles.z) + 180);
		if(localConnection.name.Contains("R-"))
		{
			transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.RoundToInt((Mathf.RoundToInt(nearestConnection.transform.eulerAngles.z) - (localConnection.transform.localEulerAngles.z)) + 180));
			//print("right");
		}
		else
		{
			transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, Mathf.RoundToInt(nearestConnection.transform.eulerAngles.z));
			//print("left");
		}
		transform.position = nearestConnection.transform.position;
		transform.position = new Vector3(transform.position.x + nearestConnection.transform.position.x - localConnection.position.x,
		                                 transform.position.y + nearestConnection.transform.position.y - localConnection.position.y,
		                                 0);

	}
}
