using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckClip : Check {
		Transform followObj;
		public Halo2D halo;
		public FinalCount count;

		public override void thisActivate()
		{
			followObj = finishTarget;
			if (halo) 
			{
				Destroy (halo.gameObject);
				if(count)
				{
					count.Selected();
				}
			}
			//transform.localPosition = Vector3.zero;
			//showNow();
			foreach(Check nextP in next)
			{
					nextP.target = new Transform[] {gameObject.transform};
			}
		}
		
		/*private void showNow()
		{
				MeshRenderer  render = gameObject.GetComponent<MeshRenderer>();
				if(render)
				{
					render.enabled = true;	
				}
				else
				{
					SpriteRenderer spriteRender = gameObject.GetComponent<SpriteRenderer>();
					if(spriteRender)
					{
						spriteRender.enabled = true;
					}
				}
				
		}*/

		public override void show ()
		{

		}

		public void Halo(bool active)
		{
			if(halo)
			{
				halo.Acitivate(active);
				activated = active;
			}
		}

		void Update()
		{
			if(followObj)
			{
				transform.position = new Vector3 (followObj.position.x, followObj.position.y, transform.position.z);
			}
		}

		public void Unclip()
		{
			followObj = null;
		}
	}
}
