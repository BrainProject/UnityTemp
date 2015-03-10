using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckClip : Check {
		Transform followObj;

		public override void thisActivate()
		{
			followObj = finishTarget;
			//transform.localPosition = Vector3.zero;
			showNow();
			foreach(Check nextP in next)
			{
					nextP.target = new Transform[] {gameObject.transform};
			}
		}
		
		private void showNow()
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
