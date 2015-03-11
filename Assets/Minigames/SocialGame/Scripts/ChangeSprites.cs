using UnityEngine;
using System.Collections;
namespace SocialGame{
	public class ChangeSprites : MonoBehaviour {

		public Sprite[] steps;
		public SpriteRenderer spriteRender;
		public void SetImage(int index)
		{
			if(index < steps.Length)
			{
				spriteRender.sprite = steps [index];
			}
		}

		public void ResetSprites()
		{
			spriteRender.sprite = steps[steps.Length - 1];
		}
	}
}
