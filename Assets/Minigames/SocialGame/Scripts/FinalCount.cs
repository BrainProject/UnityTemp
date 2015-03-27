using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class FinalCount : MonoBehaviour {
		#if UNITY_STANDALONE

		public int count;
		public bool switching = false;
		public GestChecker gestPlayer1;
		public GestChecker gestPlayer2;
		public int playerOnTurn =0;
		
		public Color defColor;
		public Color P1Color;
		public Color P2Color;

		void Start(){
			if(switching)
			{
				switchPlayer();
			}
		}

		public void Next()
		{
			count--;
			if(count <= 0)
			{
					LevelManager.win();
			}
			if(switching)
			{
					switchPlayer();
			}
		}

		void switchPlayer()
		{
				if(!gestPlayer1 || !gestPlayer2)
				{
					switching = false;
					if(gestPlayer1)
						gestPlayer1.ActivateChecking( true);
					if(gestPlayer2)
						gestPlayer2.ActivateChecking( true);
					//Camera.main.backgroundColor = defColor;
					return;
				}
				switch(playerOnTurn)
				{
					case 1 :
					//Camera.main.backgroundColor = P2Color;
					gestPlayer1.ActivateChecking( false);
					gestPlayer2.ActivateChecking( true);
					playerOnTurn = 2;
					break;
					case 2 :
					//Camera.main.backgroundColor = P1Color;
					gestPlayer1.ActivateChecking( true);
					gestPlayer2.ActivateChecking( false);
					playerOnTurn = 1;
					break;
					default:
						switching = false;
					break;
				}
		}
		#endif
}
}