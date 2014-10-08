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
		// Use this for initialization
		void Start () {
			anim = gameObject.GetComponent<Animator>();
		}
		
		// Update is called once per frame
		/*void Update () {
		
		}*/

		IEnumerator Walk() {
			float curretTime = 0;
			while((curretTime < time) && !run)
			{
				curretTime += Time.deltaTime;
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
				StartCoroutine("Walk");
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
