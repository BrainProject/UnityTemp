using UnityEngine;
using System.Collections;

public class SelectBrainPart : MonoBehaviour {
	public string levelName;
	public bool CanRotate{ get; set; }
	private Color originalColor;

	// Use this for initialization
	void Start () {
		originalColor = this.renderer.material.color;
	}
	
	// Update is called once per frame
	void OnMouseEnter () {
		if(CanRotate)
			this.renderer.material.color = Color.green;
	}

	void OnMouseExit()
	{
		this.renderer.material.color = originalColor;
	}

	void OnMouseDown()
	{
		if(CanRotate)
			Application.LoadLevel (levelName);
	}
}
