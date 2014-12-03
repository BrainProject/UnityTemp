using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CottonBall : MonoBehaviour {
#if !UNITY_WEBPLAYER
		public Transform target;
		public float distance;
		public float deathZone;

		private bool stop = true;
		public Vector3 translate;
		private CheckAnim targetAnim;
		private Animator anim;
		// Use this for initialization
		void Start () {
			anim = gameObject.GetComponent<Animator>();
		}
		
		// Update is called once per frame
		void Update () {
			if(stop && (distance > Vector2.Distance(target.position, transform.position)))
			{
				stop = false;
				targetAnim = target.gameObject.GetComponent<CheckAnim>();
				if(targetAnim)
					 translate = targetAnim.Run("run");
				StartCoroutine("Running");
				if(anim)
				{
					anim.SetBool("run",true);
				}

			}
		}

		IEnumerator Running() {
			while(deathZone < transform.position.x)
			{
				transform.Translate(translate * Time.deltaTime * 100);
				yield return null;
			}
			targetAnim.activated = false;
			LevelManager.finish();
			Destroy(transform.parent.gameObject);
		}
#endif
	}
}
