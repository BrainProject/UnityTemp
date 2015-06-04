using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class checkShooter : MonoBehaviour {
		public GameObject check;
		public Vector2 range;
		public float power;
		public Vector2 delay;
		public int maxNumOfCheck;

		private GestChecker checker;
		private float nextTime;
		private float time = 0;
		private bool run = true;

		/// <summary>
		/// Start this instance.
		///  </summary>
		void Start () {
			/*Vector3  offset = Vector3.right *range.x;
			GameObject shoot = (GameObject) GameObject.Instantiate(check,transform.position + offset,Quaternion.identity);
			offset = Vector3.right *range.y;
			shoot = (GameObject) GameObject.Instantiate(check,transform.position + offset,Quaternion.identity);*/
			checker = gameObject.GetComponent<GestChecker>();
		}
		
		/// <summary>
		/// reload next check
		/// </summary>
		void Update () {
			time += Time.deltaTime;

			if(time > nextTime && run)
			{
				if(transform.childCount < maxNumOfCheck)
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

		public void Stop(bool stop)
		{
			run = !stop;
		}
	}
}
