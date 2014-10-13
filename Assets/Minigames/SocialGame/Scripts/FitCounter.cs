using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class FitCounter : MonoBehaviour {
		public  int max;
		private int count;

		public int selectedMovements;

		public GameObject[] movements ;
		// Use this for initialization
		void Start () {
			selectedMovements = Random.Range(0,movements.Length);
			GameObject.Instantiate(movements[selectedMovements]);
		}


		public void  nextComplete()
		{
			count ++;
			if(count > max)
			{
				LevelManager.finish();
			}
			else
			{
				GameObject.Instantiate(movements[selectedMovements]);
			}
		}

		//bugbug
		void OnGUI()
		{
			GUI.TextArea(new Rect(0,0,60,30),count.ToString());
		}
	}
}
