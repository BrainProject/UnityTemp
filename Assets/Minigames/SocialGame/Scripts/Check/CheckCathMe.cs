using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckCathMe : Check {
		public GameObject obj;
		public float deathZone;
		public GameObject goodObj;
		public GameObject badObj;
		public int points = 1;
		[Range(0, 100)] public float ChanceOfGood;

		private bool good;


		protected override void Start () {
			good =  Random.Range(0,100) <= ChanceOfGood;
			if(good && goodObj)
			{
				GameObject clone = (GameObject) GameObject.Instantiate(goodObj,transform.position,Quaternion.identity);
				clone.transform.parent= transform;
			}
			else
			{
				if(badObj)
				{
					GameObject clone = (GameObject) GameObject.Instantiate(badObj,transform.position,Quaternion.identity);
					clone.transform.parent= transform;
				}
			}
		}
		
		// Update is called once per frame
		void Update () {
			if(transform.position.y < deathZone)
			{
				Destroy(gameObject);
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
				Debug.Log(countSrcipt);
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
	}
}
