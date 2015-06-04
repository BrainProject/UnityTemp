using UnityEngine;
using System.Collections;

public class ColorsPallet : MonoBehaviour {

	public Color[] colors;
	static Color[] colorsStatic;

	// Use this for initialization
	void Awake () {
		colorsStatic = colors;
	}

	/// <summary>
	/// Gets the random color.
	/// </summary>
	/// <returns>The random color.</returns>
	public static Color getRandomColor()
	{
		int rnd = Random.Range(0,colorsStatic.Length);
		return colorsStatic[rnd];
	}
}
