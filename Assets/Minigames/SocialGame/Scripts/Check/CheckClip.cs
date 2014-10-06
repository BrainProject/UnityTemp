using UnityEngine;
using System.Collections;

namespace SocialGame{
public class CheckClip : Check {

	public override void thisActivate()
	{
		transform.parent = finishTarget;
		transform.localPosition = Vector3.zero;
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
}
}
