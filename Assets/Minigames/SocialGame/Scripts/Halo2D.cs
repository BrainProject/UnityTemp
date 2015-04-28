using UnityEngine;
using System.Collections;

public class Halo2D : MonoBehaviour {
	public SpriteRenderer sprite;

	public void Acitivate(bool activeta)
	{
		sprite.enabled = activeta;
	}
}
