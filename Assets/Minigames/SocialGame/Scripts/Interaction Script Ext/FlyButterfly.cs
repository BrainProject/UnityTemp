using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class FlyButterfly : MonoBehaviour {
		public float maxSpeed;
		public float step;
		private float speed;
	
		/// <summary>
		/// Update this instance to translate with object
		/// </summary>
		void Update () {
		
			if(transform.position.y < 3)
			{
				transform.Translate(new Vector3(1,1) * speed * Time.deltaTime);
				if(speed < maxSpeed)
				{
					speed += step;
				}
			}
			else
			{
				GameObject.Destroy(gameObject);
			}
		}
	}
}
