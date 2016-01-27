using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckAnim : Check {
		public string Parametr;
		public float time;
		public Vector3 translate;
		public bool run;
		public bool walk;
		public Animator anim;
		private float WalkTime;

		/// <summary>
		/// Start this instance.
		/// </summary>
		public override void Start () {
			if (!anim) {
				anim = gameObject.GetComponent<Animator> ();
			}
		}
		
		/// <summary>
		/// Walk this objec.
		/// </summary>
		IEnumerator Walk() {
			while((Time.time < WalkTime) && !run)
			{
				transform.Translate(translate * Time.deltaTime * 100);
				yield return null;
			}
			playAnim(false);
		}

		/// <summary>
		/// Plaies the animation.
		/// </summary>
		/// <param name="start">If set to <c>true</c> start.</param>
		public void playAnim(bool start)
		{
			if(anim)
				anim.SetBool(Parametr,start);
			walk = start;
		}


		public override void thisActivate()
		{
			if(!walk && !run)
			{
				playAnim(true);
				WalkTime = Time.time + time;
				StartCoroutine("Walk");
			}
			if(walk)
			{
				WalkTime = Time.time + time;
			}
		}

		/// <summary>
		/// Run the object.
		/// </summary>
		/// <param name="parametr2">name of animation for run</param>
		public Vector3 Run(string parametr2)
		{
			run = true;
			anim.SetTrigger(parametr2);
			translate *=4;
			StartCoroutine("Running");
			return translate;

		}
		/// <summary>
		/// Running this object.
		/// </summary>
		IEnumerator Running() {
			while(run)
			{
				transform.Translate(translate * Time.deltaTime * 100);
				yield return null;
			}
		}
	}
}
 