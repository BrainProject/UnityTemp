using UnityEngine;
using System.Collections;

namespace SocialGame{
public class Check : MonoBehaviour {

	public Transform[] target;
	private bool inPos;
	public bool activated;
	public Check[] next;
	protected Transform finishTarget;

	protected virtual void Start()
	{
		show();
	}
	
	
	public virtual bool Checked(Transform target)
	{
		bool last = false;
		finishTarget = target;
		if(next.Length > 0)
		{
			foreach(Check obj in next)
			{
				obj.activate();
			}
		}
		else
		{
			
			last = true;
		}
		thisActivate();
		return last;
	}
	
	[ContextMenu("Check")]
	public virtual void thisActivate()
	{
			activated = false;
			show ();
	}
	
	public void activate()
	{
		activated = true;
		show();
	}
	
	public void deactivate()
	{
			activated = false;
			show();
	}

	public virtual void show()
	{
		MeshRenderer  render = gameObject.GetComponent<MeshRenderer>();
		if(render)
		{
			render.enabled = activated;	
		}
		else
		{
			SpriteRenderer spriteRender = gameObject.GetComponent<SpriteRenderer>();
			if(spriteRender)
			{
				spriteRender.enabled = activated;
			}
		}
	}

		public void show(bool showObj)
		{
			MeshRenderer  render = gameObject.GetComponent<MeshRenderer>();
			if(render)
			{
				render.enabled = showObj;	
			}
			else
			{
				SpriteRenderer spriteRender = gameObject.GetComponent<SpriteRenderer>();
				if(spriteRender)
				{
					spriteRender.enabled = showObj;
				}
			}
		}



}
}
