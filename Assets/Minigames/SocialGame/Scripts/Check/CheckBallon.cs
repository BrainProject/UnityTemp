using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class CheckBallon : Check {
#if UNITY_STANDALONE
		public GameObject obj;
		public GameObject basket;

		public SpriteRenderer render;

		protected override void Start () {
			Debug.Log("test");
			transform.localScale = Vector3.one * 0.3f;
			render = gameObject.GetComponent<SpriteRenderer>();
			if(render)
			{
				render.color = ColorsPallet.getRandomColor();
			}

		}
		

		public override void thisActivate()
		{
				transform.localScale += Vector3.one * Time.deltaTime;
				if(transform.localScale.x >= 1)
				{
					Next();
				}
		}
		
		void Next()
		{
			if(obj)
			{
				Debug.LogWarning("baf");
				GameObject clone = (GameObject) GameObject.Instantiate(obj,transform.position,Quaternion.identity);
				clone.transform.parent = transform.parent;
				Ballon ballon = clone.GetComponent<Ballon>();
				if(ballon)
				{
					if(render)
						ballon.setColor(render.color);
					ballon.setJoint(basket.rigidbody2D);
				}
			}
			GameObject.Destroy(gameObject);
		}
#endif
	}
}
