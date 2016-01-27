using UnityEngine;
using System.Collections;

namespace SocialGame{
public class Check : MonoBehaviour {

	public Transform[] target;
	private bool inPos;
	public bool activated;
	public float multiDist =1;
	public Check[] next;
	protected Transform finishTarget;

	/// <summary>
	/// Start this instance. Hide or show object.
	/// </summary>
	public virtual void Start()
	{
		show();
	}
	
	/// <summary>
	/// Checked the specified target.
	/// </summary>
	/// <param name="target">Target.</param>
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

	/// <summary>
	/// this is deactivatrd aftert is checked.
	/// </summary>
	public virtual void thisActivate()
	{
			deactivate();
	}
	
	/// <summary>
	/// Activate this instance.
	/// </summary>
	public void activate()
	{
		activated = true;
		show();
	}
	
	/// <summary>
	/// Deactivate this instance.
	/// </summary>
	public void deactivate()
	{
			activated = false;
			show();
	}

		/// <summary>
		/// Show this instance.
		/// </summary>
	public virtual void show()
	{
		show(activated);
	}

		/// <summary>
		/// Show this intance if showObj is true else hide intance.
		/// </summary>
		/// <param name="showObj">If set to <c>true</c> show object.</param>
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
