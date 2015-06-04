using UnityEngine;
using System.Collections;
namespace SocialGame{
	public class ChangeSprites : MonoBehaviour {

		public Sprite[] steps;
		public SpriteRenderer spriteRender;

		/// <summary>
		/// Sets the image.
		/// </summary>
		/// <param name="index">Index.</param>
		public void SetImage(int index)
		{
			if(index < steps.Length)
			{
				spriteRender.sprite = steps [index];
			}
		}

		/// <summary>
		/// Resets the sprites.
		/// </summary>
		public void ResetSprites()
		{
			spriteRender.sprite = steps[steps.Length - 1];
		}
	}
}
