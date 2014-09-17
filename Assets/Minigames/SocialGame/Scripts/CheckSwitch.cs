using UnityEngine;
using System.Collections;

#if UNITY_STANDALONE
namespace SocialGame{
public class CheckSwitch : Check {

		public FinalCount count;

		public override void thisActivate()
		{
			finishTarget.parent = null;
			finishTarget.position = transform.position;
			finishTarget.rotation = transform.rotation;
			Debug.Log(gameObject.name);
			if(count)
				count.next();
		}



		/*protected void removeFromTarget()
		{

		}*/
	}
}
#endif