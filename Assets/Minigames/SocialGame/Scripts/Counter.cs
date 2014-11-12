using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Counter : MonoBehaviour {
		public int max;
		private int points = 0;
		private float step;

		// Use this for initialization
		void Start () {
			step = transform.localScale.x / max;
			setPoints(0);
			redraw();
		}

		public void addPoints(int point)
		{
			if((point + points)<0)
				points = 0;
			else
				points += point;
			redraw();
		}

		public void setPoints(int point)
		{
			if(point < 0)
				points = 0;
			else
				points = point;
			redraw();
		}

		void redraw()
		{
			if(points < max)
			{
				transform.localScale = new Vector3 (step * points,transform.localScale.y,transform.localScale.z);
			}
			else
			{

				LevelManager.finish();
				transform.localScale = new Vector3 (step * max,transform.localScale.y,transform.localScale.z);
				checkShooter check = (checkShooter) FindObjectOfType(typeof(checkShooter));
				if(check)
				{
					check.stop();
				}
			}
		}
	}
}
