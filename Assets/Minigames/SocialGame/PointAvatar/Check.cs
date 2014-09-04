using UnityEngine;
using System.Collections;

namespace SocialGame{
public class Check : MonoBehaviour {

	public Transform target;
	private bool inPos;
	public bool active;
	public Check[] next;

	void Start()
	{
		show();
	}

	public bool Checked()
	{
		if(next.Length > 0)
		{
			foreach(Check obj in next)
			{
				obj.activate();
			}
			active = false;
			show ();
			return false;
		}
		else
		{
			return true;
		}
	}


	public void activate()
	{
		active = true;
		show();
	}

	public void show()
	{
		MeshRenderer  render = gameObject.GetComponent<MeshRenderer>();
		render.enabled = active;
	}


}
}
