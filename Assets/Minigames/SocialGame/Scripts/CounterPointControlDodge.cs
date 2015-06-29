using UnityEngine;
using System.Collections;

namespace SocialGame
{
	public class CounterPointControlDodge : MonoBehaviour {
		public SpriteRenderer render;
		public bool activated;

		public Sprite active;
		public Sprite deactive;

		public void AddThis(bool add)
		{
			if(activated != add)
			{
				activated = add;
				Color col = render.color;
				if(add)
				{
					if(active)
						render.sprite = active;
					col.a = 1;
				}
				else
				{
					if(deactive)
						render.sprite = deactive;
					col.a = 0.3f;
				}
				render.color = col;
			}
		}


	}
}
