using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseUI : MonoBehaviour {
	public Sprite normal;
	public Sprite selected;
	public Image image;

	// Use this for initialization
	void Start () {
		image.sprite = normal;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Input.mousePosition;
	}

	public void MouseDown()
	{
		image.sprite = selected;
	}

	public void MouseUp()
	{
		image.sprite = normal;
	}
}
