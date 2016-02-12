using UnityEngine;
using System.Collections;

namespace SocialGame
{
	public class CounterPointControlDodge : MonoBehaviour {
		public SpriteRenderer render;
		public bool activated;

		public Sprite activeSprite;
		public Sprite inactiveSprite;

		public void AddThis(bool add)
		{
			if(activated != add)
			{
				activated = add;
				Color col = render.color;
				if(add)
				{
					if(activeSprite)
						render.sprite = activeSprite;
					col.a = 1;
				}
				else
				{
					if(inactiveSprite)
						render.sprite = inactiveSprite;
					col.a = 0.3f;
				}
				render.color = col;
			}
		}


	}
}
