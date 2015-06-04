using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class FitCounter : MonoBehaviour {
#if UNITY_STANDALONE
		public  int max;
		private int count;

		public float space;
		public GameObject line;
		public SpriteRenderer[] lines;
		public Vector3 startGUI;


		public int selectedMovements;

		public GameObject[] movements ;
		public int[] numOfPose;
		// Use this for initialization
		void Start () {
			selectedMovements = Random.Range(0,movements.Length);
			max = numOfPose[selectedMovements] * max;
			GameObject.Instantiate(movements[selectedMovements]);
			drawCount();
			redraw();
		}

		/// <summary>
		/// Nexts is complete.
		/// </summary>
		public void  nextComplete()
		{
			count ++;
			if(count >= max)
			{
				redraw();
				LevelManager.win ();
			}
			else
			{
				redraw();
			}
		}

		/// <summary>
		/// Draws the counter.
		/// </summary>
		void drawCount()
		{
			lines = new SpriteRenderer[max];
			for(int i = 0; i < max; i++)
			{
				if(line)
				{
					Vector3 position = startGUI + Vector3.right * space * i;
					GameObject clone = (GameObject) GameObject.Instantiate(line,position, Quaternion.identity);
					clone.transform.parent = transform;
					SpriteRenderer cloneRender = clone.GetComponent<SpriteRenderer>();
					if(cloneRender)
						lines[i] = cloneRender;
					else
					{
						Debug.LogWarning("On"+ gameObject.name +"atribut line must be sprite");
						lines = null;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Redraw this object.
		/// </summary>
		void redraw()
		{
			for(int i = 0; i < lines.Length;i++)
			{
			   if(i < max - count)
				{
					lines[i].enabled = true;
				}
				else
				{
					lines[i].enabled = false;
				}
			}
		}
#endif
	}
}
