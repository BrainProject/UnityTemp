
using UnityEngine;
using System.Collections;

public class ConfetControl : MonoBehaviour {
	public ParticleEmitter[] emit;
	public float EmitTime;
	public RocketLauncher launcher;
	// Use this for initialization

	void Start () {
	
	}
	
	// Update is called once per frame

	[ContextMenu("gdfg")]
	public void Fire()
	{

		if(launcher)
			launcher.Launch();
		StartCoroutine(snow());
	}

	IEnumerator snow()
	{
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

	void OnGUI()
	{
		if(GUI.Button(new Rect(0,0,90,40),"lop"))
		{
			Fire();
		}
	}
}
