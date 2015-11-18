using UnityEngine;
using System.Collections;

public class BrainSection : MonoBehaviour {
	public bool showOnAndroid = true;

	// Use this for initialization
	void Start () {
#if UNITY_ANDROID
		if(!showOnAndroid)
		{
			gameObject.SetActive(false);
		}
#endif
	}
}
