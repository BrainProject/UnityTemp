using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Ballon : MonoBehaviour {
#if UNITY_STANDALONE

		public DistanceJoint2D joint;
		public SpriteRenderer render;
		public LineRenderer lineRender;
		private Transform target;
		public float speedRope;
		public float speed;

		private int phase = 0;

		private float startTime = -1;
		private float journeyLength;
		private Rigidbody2D targetRig;
		private Rigidbody2D rig;
		void Awake()
		{
			joint = gameObject.GetComponent<DistanceJoint2D>();
			render = gameObject.GetComponent<SpriteRenderer>();
			lineRender = gameObject.GetComponent<LineRenderer>();
			rig = gameObject.GetComponent<Rigidbody2D>();
			if(joint)
			{
				joint.enabled = false;
			}


		}

		/// <summary>
		/// Sets the joint.
		/// </summary>
		/// <param name="body">Body.</param>
		public void setJoint(Rigidbody2D body)
		{
			targetRig = body;
			target = targetRig.transform;
		}

		public void Join()
		{
			if(joint)
			{
				joint.enabled = true;
				joint.connectedBody = targetRig;
			}
			if (rig) 
			{
				rig.isKinematic =false;
			}
			Basket basket = targetRig.gameObject.GetComponent<Basket>();
			if(basket)
			{
				basket.addBallon();
			}
		}

		/// <summary>
		/// Sets the color.
		/// </summary>
		/// <param name="color">Color.</param>
		public void setColor(Color color)
		{
			if(render)
				render.color = color;
		}

		void Update()
		{
			float distCovered;
			float fracJourney;
			switch(phase)
			{
				case 0 :
					if(startTime == -1)
					{
						startTime = Time.time;
						journeyLength = Vector3.Distance(transform.position,target.position);
					}
					if(Vector3.Distance(transform.position, target.position) > 1.5)
					{
						float step = speed * Time.deltaTime;
						transform.position = Vector3.MoveTowards(transform.position, target.position, step);
					}
					else
					{
						startTime = Time.time;
						phase = 1;
					}
					break;
				case 1:
					distCovered = (Time.time - startTime) * speedRope;
					fracJourney = distCovered / journeyLength;
					if(Vector3.Distance(transform.position, target.position) > 1)
					{
						float step = speed * Time.deltaTime;
						transform.position = Vector3.MoveTowards(transform.position, target.position, step);
					}
					if(fracJourney > 1)
					{
						phase = 2;
						Join();
					}
					Vector3 positionNext = Vector3.Lerp(transform.position, target.position, fracJourney);
					lineRender.SetPosition(0,transform.position);
					lineRender.SetPosition(1,positionNext);
				break;
				case 2:
					if(lineRender && target)
					{
						lineRender.SetPosition(0,transform.position);
						lineRender.SetPosition(1,target.position);
					}
				break;
			}
			


		}
#endif
	}
}
