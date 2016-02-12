using UnityEngine;
using System.Collections;

public class BrainSection : MonoBehaviour {
	public bool showOnAndroid = true;
    
	// Use this for initialization
	void Start () {
		if(!showOnAndroid)
		{
			gameObject.SetActive(false);
		}
    }
}
