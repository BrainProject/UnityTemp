using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class checkShooter : MonoBehaviour {
		public GameObject check;
		public Vector2 range;
		public float power;
		public Vector2 delay;
		public int maxNumOfcheck;

		private GestChecker checker;
		private float nextTime;
		private float time = 0;

		// Use this for initialization
		void Start () {
			/*Vector3  offset = Vector3.right *range.x;
			GameObject shoot = (GameObject) GameObject.Instantiate(check,transform.position + offset,Quaternion.identity);
			offset = Vector3.right *range.y;
			shoot = (GameObject) GameObject.Instantiate(check,transform.position + offset,Quaternion.identity);*/
			checker = gameObject.GetComponent<GestChecker>();
		}
		
		// Update is called once per frame
		void Update () {
			time += Time.deltaTime;

			if(time > nextTime)
			{
				if(transform.childCount < maxNumOfcheck)
					shoot();
				time = 0;
				nextTime = Random.Range(delay.x,delay.y);
			}
		}

		void shoot()
		{
			if(check)
			{
				Vector3  offset = Vector3.right * Random.Range(range.x,range.y);
				GameObject shoot = (GameObject) GameObject.Instantiate(check,transform.position + offset,Quaternion.identity);
				if(checker)
				{
					checker.addCheck(shoot.transform);
				}
				if(shoot.rigidbody)
				{
					shoot.rigidbody.AddForce(Vector3.down * power,ForceMode.Impulse);
				}
			}
		}
	}
}
