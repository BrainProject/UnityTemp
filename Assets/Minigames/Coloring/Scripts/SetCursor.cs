using UnityEngine;
using System.Collections;

public class SetCursor : MonoBehaviour {

	public Texture2D cursorTexture;
	CursorMode cursorMode = CursorMode.Auto;
	Vector2 hotSpot = Vector2.zero;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
