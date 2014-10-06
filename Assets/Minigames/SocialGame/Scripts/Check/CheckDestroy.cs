using UnityEngine;
using System.Collections;

namespace SocialGame{
public class CheckDestroy : Check {
	public GameObject obj;

	/*// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

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
