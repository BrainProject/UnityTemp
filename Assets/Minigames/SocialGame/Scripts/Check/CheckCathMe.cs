using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckCathMe : Check {
#if UNITY_STANDALONE
		public GameObject obj;
		public float deathZone;
		public GameObject goodObj;
		public GameObject badObj;
		public int points = 1;
		[Range(0, 100)] public float ChanceOfGood;
		public float timeToDie;

		private bool good;
		private bool die;
		private float timeOfKillingSelf = 0;
		private SpriteRenderer visual;


		public override void Start () {
			if (HelpListener.Instance.activatedGUI) {
				Destroy (gameObject);
			}
			GameObject clone = null;
			good =  Random.Range(0,100) <= ChanceOfGood;
			if(good && goodObj)
			{
				clone = (GameObject) GameObject.Instantiate(goodObj,transform.position,Quaternion.identity);
			}
			else
			{
				if(badObj)
				{
					clone = (GameObject) GameObject.Instantiate(badObj,transform.position,Quaternion.identity);

				}
			}
			if(clone)
			{
				clone.transform.parent= transform;
				SpriteRenderer[] temp = clone.GetComponentsInChildren<SpriteRenderer> ();
				if (temp.Length > 0) {
					visual = temp [0];
					if (temp.Length > 1) {
						Debug.LogWarning ("So many spriteRenderers on objects check correct was choosen");
					}
				}
			}
		}
		
		/// <summary>
		/// Update  whith move object.
		/// </summary>
		void Update () {
			if(transform.position.y < deathZone)
			{
				Destroy(gameObject);
			}
			if (die || HelpListener.Instance.activatedGUI) {
				die = true;
				timeOfKillingSelf += Time.deltaTime;
				if (timeOfKillingSelf > timeToDie) {
					Destroy (gameObject);
				} else {
					if (visual) {
						visual.color = new Color (visual.color.r, visual.color.g, visual.color.b, 1 - (timeOfKillingSelf / timeToDie));
					}
				}
			}
		}

		public override void thisActivate()
		{
			GameObject counter = GameObject.FindWithTag("gameTile");
			if(counter)
			{
				int multi;
				if(good)
					multi = 1;
				else
					multi= -1;

				Counter countSrcipt = counter.GetComponent<Counter>();
				//Debug.Log(countSrcipt);
				if(countSrcipt)
				{
					countSrcipt.addPoints(points * multi);
				}
			}
			if(obj)
			{
				GameObject.Instantiate(obj,transform.position,Quaternion.identity);
			}
			Destroy(gameObject);
		}
#endif
	}
}
