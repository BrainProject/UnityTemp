using UnityEngine;
using System.Collections;

namespace SocialGame{
public class CheckDestroy : Check {

	/*// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}*/

	public override void thisActivate()
	{
		GameObject.Destroy(gameObject);
	}
}
}
