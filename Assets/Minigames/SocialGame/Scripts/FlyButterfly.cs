using UnityEngine;
using System.Collections;

public class FlyButterfly : MonoBehaviour {
	public float maxSpeed;
	public float step;
	private float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(transform.position.y < 3)
		{
			transform.Translate(new Vector3(1,1) * speed * Time.deltaTime);
			if(speed < maxSpeed)
			{
				speed += step;
			}
		}
		else
		{
			GameObject.Destroy(gameObject);
		}
	}
}
