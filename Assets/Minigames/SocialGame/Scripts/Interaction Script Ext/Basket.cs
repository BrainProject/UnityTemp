using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Basket : MonoBehaviour {
#if UNITY_STANDALONE
		public int max;
		public float speed;
		public float step;
		private int num;
		private  bool up;
		


		void Update () {
			if(transform.position.y>3)
			{
				LevelManager.win();
				Destroy(transform.parent.gameObject);
			}
			if(up)
			{
				transform.Translate(Vector3.up * speed * Time.deltaTime * 4);
				speed += step;
			}
		}

		/// <summary>
		/// Adds the ballon.
		/// </summary>
		public void addBallon()
		{
			num++;
			if( max <= num)
			{
				up = true;
			}
		}
#endif
	}
}
