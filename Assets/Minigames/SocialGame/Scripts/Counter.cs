using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Counter : MonoBehaviour {
#if UNITY_STANDALONE
		public int max;
		public CounterPointControlDodge[] pointsGraf;
		public int points = 0;
		public Vector2 fieldOnSreen;
		public GameObject PointObj;
		public float step;

		// Use this for initialization
		void Start () {
			CreatePoints();

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

		void CreatePoints()
		{
			pointsGraf = new CounterPointControlDodge[max];
			//float step = (fieldOnSreen.y - fieldOnSreen.x)/max;
			int leftHalf = max/2;
			float center = fieldOnSreen.x + (fieldOnSreen.y - fieldOnSreen.x)/2;
			//float start = center - leftHalf * step;
			for(int i = 0; i < max; i++)
			{
				Vector3 position = transform.position;
				position.x = center + (i - leftHalf) *step;
				GameObject obj = (GameObject)Instantiate(PointObj,position,Quaternion.identity);
				obj.name = "Point_"+i;
				obj.transform.SetParent(transform);
				CounterPointControlDodge scr =obj.GetComponent<CounterPointControlDodge>();
				if(scr)
				{
					pointsGraf[i] = scr;
				}
			}


		}

		void redraw()
		{
			if(points < max)
			{
				for(int i = 0; i < max;i++)
				{
					pointsGraf[i].AddThis(i < points);
				}
				//transform.localScale = new Vector3 (step * points,transform.localScale.y,transform.localScale.z);
			}
			else
			{

				LevelManager.win();
				/*transform.localScale = new Vector3 (step * max,transform.localScale.y,transform.localScale.z);*/
				checkShooter check = (checkShooter) FindObjectOfType(typeof(checkShooter));
				if(check)
				{
					check.Stop(true);
				}
			}
		}
#endif
	}
}
