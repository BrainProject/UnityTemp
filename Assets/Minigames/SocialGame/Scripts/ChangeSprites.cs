using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace SocialGame{
	public class ChangeSprites : MonoBehaviour {

		public Sprite[] steps;
		public SpriteRenderer spriteRender;
		public Image imageUI;
		/// <summary>
		/// Sets the image.
		/// </summary>
		/// <param name="index">Index.</param>
		public void SetImage(int index)
		{
			Debug.Log ("set index: " + index);
			if (spriteRender) 
			{
				if (index < steps.Length) 
				{
					spriteRender.sprite = steps [index];
				}

			}
			if (imageUI) 
			{

				if (index < steps.Length) 
				{
					imageUI.sprite = steps [index];
				}
			}
		}

		/// <summary>
		/// Resets the sprites.
		/// </summary>
		public void ResetSprites()
		{
			if (spriteRender)
			{
				spriteRender.sprite = steps [0];

			}
			if (imageUI) 
			{
				imageUI.sprite = steps [0];
			}
		}
	}
}
