using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Snap : MonoBehaviour {

		public int player;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void snap()
		{
			GameObject playerObj;
			if(player <1 || player > 2)
				return;
			if(player == 1)
			{
				playerObj = GameObject.Find("P1");
			}
			else
			{
				playerObj = GameObject.Find("P2");
			}
			if(playerObj)
			{
				FigureCreate figCreate = playerObj.GetComponentInChildren<FigureCreate>();
				GameObject checker = figCreate.createPoints();
				GestChecker checkerScript = checker.GetComponent<GestChecker>();

			}
			Destroy(gameObject);
		}
	}
}