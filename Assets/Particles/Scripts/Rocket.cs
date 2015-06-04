
using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour {
	public float time;
	float startTime;
	public float speed;
	public ParticleEmitter fire;
	public ParticleEmitter fireworks;
	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	void Update()
	{
		if((Time.time - startTime) < time)
		{
			transform.Translate(transform.up * Time.deltaTime * speed);
		}
		else
		{
			fire.emit = false;
			fireworks.emit = true;
		}
	}
}
