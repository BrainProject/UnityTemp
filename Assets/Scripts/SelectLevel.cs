using UnityEngine;
using System.Collections;

public class SelectLevel : MonoBehaviour {

	private Color originalColor;
	// Use this for initialization
	void Start () {
		originalColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void OnMouseOver () {
		if(Vector3.Distance(GameObject.Find("Player").transform.position,transform.position) < 5)
			renderer.material.color = Color.green;
	}

	void OnMouseExit()
	{
		renderer.material.color = originalColor;
	}
}
