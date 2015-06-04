using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckClip : Check {
		Transform followObj;
		public Halo2D halo;
		public FinalCount count;

		public override void thisActivate()
		{
			followObj = finishTarget;
			if (halo) 
			{
				Destroy (halo.gameObject);
				if(count)
				{
					count.Selected();
				}
			}
			foreach(Check nextP in next)
			{
					nextP.target = new Transform[] {gameObject.transform};
			}
		}

		public override void show ()
		{

		}

		/// <summary>
		/// activate halo.
		/// </summary>
		/// <param name="active">If set to <c>true</c> active.</param>
		public void Halo(bool active)
		{
			if(halo)
			{
				halo.Acitivate(active);
				activated = active;
			}
		}

		void Update()
		{
			if(followObj)
			{
				transform.position = new Vector3 (followObj.position.x, followObj.position.y, transform.position.z);
			}
		}
		/// <summary>
		/// Unclip this object.
		/// </summary>
		public void Unclip()
		{
			followObj = null;
		}
	}
}
