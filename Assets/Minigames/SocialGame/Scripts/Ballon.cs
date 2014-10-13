using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Ballon : MonoBehaviour {

		public DistanceJoint2D joint;
		public SpriteRenderer render;
		public LineRenderer lineRender;
		private Transform target;

		void Awake()
		{
			joint = gameObject.GetComponent<DistanceJoint2D>();
			render = gameObject.GetComponent<SpriteRenderer>();
			lineRender = gameObject.GetComponent<LineRenderer>();

		}

		public void setJoint(Rigidbody2D body)
		{
			Debug.LogWarning(joint);
			if(joint)
			{
				joint.connectedBody = body;
				target = body.transform;
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

		void Update()
		{
			if(lineRender && target)
			{
				lineRender.SetPosition(0,transform.position);
				lineRender.SetPosition(1,target.position);
			}
		}
	}
}
