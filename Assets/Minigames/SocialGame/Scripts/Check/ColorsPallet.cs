using UnityEngine;
using System.Collections;

public class ColorsPallet : MonoBehaviour {

	public Color[] colors;
	static Color[] colorsStatic;

	// Use this for initialization
	void Awake () {
		colorsStatic = colors;
	}
	
	// Update is called once per frame
	public static Color getRandomColor()
	{
		int rnd = Random.Range(0,colorsStatic.Length);
		return colorsStatic[rnd];
	}
}
