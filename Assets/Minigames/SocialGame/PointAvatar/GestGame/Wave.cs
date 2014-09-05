using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour {
	Vector3 startPosition;
	Vector3 previousPosition;
	bool left;
	GameObject target;
	Transform head;
	public string targetName;
	// Use this for initialization
	void Start () {
		startPosition = Vector3.zero;
		//gameObject.transform.getch

	}
	
	// Update is called once per frame
	void Update () {
	if(transform.position.y >= head.position.y)
		{
			if(startPosition== Vector3.zero)
			{
				startPosition = transform.position;
				previousPosition = transform.position;
			}
			else
			{

			}
		}
	}
}
