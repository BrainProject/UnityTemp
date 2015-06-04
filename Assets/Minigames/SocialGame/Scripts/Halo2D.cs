using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Halo2D : MonoBehaviour {
		public SpriteRenderer sprite;
	
		/// <summary>
		/// Acitivate the specified activeta.
		/// </summary>
		/// <param name="activeta">If set to <c>true</c> activeta.</param>
		public void Acitivate(bool activeta)
		{
			sprite.enabled = activeta;
		}
	}
}
