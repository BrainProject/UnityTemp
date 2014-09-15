using UnityEngine;
using System.Collections;

public class CreateOnDestroy : MonoBehaviour {

	public GameObject obj;
	private bool isQuitting = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	 
	}

	void OnApplicationQuit() 
	{ 
		isQuitting = true; 
	}

	void OnDestroy()
	{
		if(!isQuitting)
		{
			GameObject.Instantiate(obj,transform.position,Quaternion.identity);
		}
	}
}
