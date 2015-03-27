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
				switch(playerOnTurn)
				{
				case 1 :
					playerOnTurn = 2;
					break;
				case 2 :
					playerOnTurn = 1;
					break;
				default:
					playerOnTurn = Random.Range(1,3);
					Debug.Log("on turn: " + playerOnTurn);
					break;
				}
				Selected();
				switchPlayer();
			}
		}

		public void Selected()
		{
			switch(playerOnTurn)
			{
			case 1 :
				gestPlayer1.ActivateChecking( false);
				break;
			case 2 :
				gestPlayer2.ActivateChecking( false);
				break;
			default:
				break;
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
					//gestPlayer1.ActivateChecking( false);
					gestPlayer2.ActivateChecking( true);
					playerOnTurn = 2;
					break;
					case 2 :
					//Camera.main.backgroundColor = P1Color;
					gestPlayer1.ActivateChecking( true);
					//gestPlayer2.ActivateChecking( false);
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