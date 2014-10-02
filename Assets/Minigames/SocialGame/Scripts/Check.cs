using UnityEngine;
using System.Collections;

namespace SocialGame{
public class Check : MonoBehaviour {

	public Transform[] target;
	private bool inPos;
	public bool activated;
	public Check[] next;
	protected Transform finishTarget;

	void Start()
	{
		show();
	}

	public bool Checked(Transform target)
	{
		bool last;
		finishTarget = target;
		if(next.Length > 0)
		{
			foreach(Check obj in next)
			{
				obj.activate();
			}
			last = false;
		}
		else
		{
			
			last = true;
		}
		activated = false;
		show ();
		thisActivate();
		return last;
	}

	public virtual void thisActivate()
	{
			Debug.Log("jsem normalni.");
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

	public void show()
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


}
}
