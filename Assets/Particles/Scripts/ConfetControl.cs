
using UnityEngine;
using System.Collections;

public class ConfetControl : MonoBehaviour {
	public ParticleEmitter[] emit;
	public float StartDelay;
	public float EmitTime;
	public RocketLauncher launcher;
	public bool AutoLaunch = false;
	// Use this for initialization

	void Start () {
		if(AutoLaunch)
		{
			Fire();
		}
	}
	
	// Update is called once per frame

	[ContextMenu("fire")]
	public void Fire()
	{

		if(launcher)
			launcher.Launch();
		StartCoroutine(snow());
	}

	IEnumerator snow()
	{
		yield return new WaitForSeconds(StartDelay);
		foreach(ParticleEmitter emiter in emit)
		{
			emiter.emit = true;
		}
		yield return new WaitForSeconds(EmitTime);
		foreach(ParticleEmitter emiter in emit)
		{
			emiter.emit = false;
		}
	}
}
