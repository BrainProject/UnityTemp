using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckAnim : Check {
		public string Parametr;
		public float time;
		public Vector3 translate;
		public bool run;
		public bool walk;
		private Animator anim;
		private float WalkTime;
		// Use this for initialization
		protected override void Start () {
			anim = gameObject.GetComponent<Animator>();
		}
		
		// Update is called once per frame
		/*void Update () {
		
		}*/

		IEnumerator Walk() {
			while((Time.time < WalkTime) && !run)
			{
				transform.Translate(translate);
				yield return null;
			}
			playAnim(false);
		}

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

		public Vector3 Run(string parametr2)
		{
			run = true;
			anim.SetTrigger(parametr2);
			translate *=4;
			StartCoroutine("Running");
			return translate;

		}

		IEnumerator Running() {
			while(run)
			{
				transform.Translate(translate);
				yield return null;
			}
		}
	}
}
