using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class ShakeObject : MonoBehaviour {
		public float width;
		public float height;

		public Vector3 center;

		public Transform[] Objects;
		// Use this for initialization
		void Start () {
			Shake();
		}

		void Update()
		{
			Debug.DrawLine(new Vector3(-width/2 , height/2,0)+  center + transform.position,new Vector3(width/2 , height/2,0)+ center + transform.position,Color.cyan);
			Debug.DrawLine(new Vector3(-width/2 , -height/2,0)+ center + transform.position,new Vector3(width/2 , -height/2,0)+ center + transform.position,Color.cyan);
		}
		
		void Shake()
		{
			for(int i = 0; i < Objects.Length; i++)
			{
				float x = Random.Range(-width/2,width/2);
				float y = Random.Range(-height/2,height/2);
				Vector3 rnd = new Vector3(x,y,0);

				Objects[i].position = transform.position + center + rnd;
			}
		}
	}
}
