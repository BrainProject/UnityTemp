using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckSwitch : Check {
		#if UNITY_STANDALONE

		public FinalCount count;
		
		public override void thisActivate()
		{
			activated = false;
			show ();
			CheckClip clip = finishTarget.GetComponent<CheckClip>();
			if(clip)
			{
				clip.Unclip();
			}
			finishTarget.parent = null;
			finishTarget.position = transform.position;
			finishTarget.rotation = transform.rotation;
			Debug.Log(gameObject.name);
			if(count)
				count.Next();
		}
		
		#endif
	}
}
