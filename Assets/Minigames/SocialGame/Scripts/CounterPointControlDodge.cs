using UnityEngine;
using System.Collections;

namespace SocialGame
{
	public class CounterPointControlDodge : MonoBehaviour {
		public SpriteRenderer render;
		public bool activated;

		public void AddThis(bool add)
		{
			if(activated != add)
			{
				activated = add;
				Color col = render.color;
				if(add)
					col.a = 1;
				else
				{

					col.a = 0.3f;
				}
				render.color = col;
			}
		}


	}
}
