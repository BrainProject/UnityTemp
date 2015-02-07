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
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			if(transform.position.y>3)
			{
				LevelManager.finish();
				Destroy(transform.parent.gameObject);
			}
			if(up)
			{
				transform.Translate(Vector3.up * speed * Time.deltaTime * 4);
				speed += step;
			}
		}

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
