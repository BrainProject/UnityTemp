﻿using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class FinalCount : MonoBehaviour {
		#if UNITY_STANDALONE

		public int count;
		public bool switching = false;
		public GestChecker gestPlayer1;
		public GestChecker gestPlayer2;
		public int playerOnTurn =0;

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

		/// <summary>
		/// say next part is selected.
		/// </summary>
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

		/// <summary>
		/// Next part is added on place
		/// </summary>
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

		/// <summary>
		/// Switchs the player.
		/// </summary>
		void switchPlayer()
		{
				if(!gestPlayer1 || !gestPlayer2)
				{
					switching = false;
					if(gestPlayer1)
						gestPlayer1.ActivateChecking( true);
					if(gestPlayer2)
						gestPlayer2.ActivateChecking( true);
					return;
				}
				switch(playerOnTurn)
				{
					case 1 :
					gestPlayer2.ActivateChecking( true);
					playerOnTurn = 2;
					break;
					case 2 :
					gestPlayer1.ActivateChecking( true);
					playerOnTurn = 1;
					break;
					default:
						switching = false;
					break;
				}
		}
		#else
		public void Selected()
		{

		}
		#endif
}
}