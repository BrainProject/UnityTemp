using UnityEngine;
using System.Collections;

namespace SocialGame{
public class CheckDestroy : Check {
	public GameObject obj;

	public override void thisActivate()
	{
			if(obj)
			{
				GameObject.Instantiate(obj,transform.position,Quaternion.identity);
			}
			GameObject.Destroy(gameObject);
	}
}
}
