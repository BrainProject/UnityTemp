using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Ballon : MonoBehaviour {

		public DistanceJoint2D joint;
		public SpriteRenderer render;

		void Awake()
		{
			joint = gameObject.GetComponent<DistanceJoint2D>();
			render = gameObject.GetComponent<SpriteRenderer>();
		}

		public void setJoint(Rigidbody2D body)
		{
			Debug.LogWarning(joint);
			if(joint)
			{
				joint.connectedBody = body;
			}
			Basket basket = body.gameObject.GetComponent<Basket>();
			if(basket)
			{
				basket.addBallon();
			}

		}

		public void setColor(Color color)
		{
			if(render)
				render.color = color;
		}
	}
}
