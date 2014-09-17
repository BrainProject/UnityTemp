using UnityEngine;
using System.Collections;
using Kinect;


namespace SocialGame{
	public class TwoPlayerSwitcher : MonoBehaviour {
		KinectManager KManager;
		public bool TwoPlayer;
		public SkinnedMeshRenderer player2;
		public Animator animPlayer2;
		public Check button;
		// Use this for initialization
		void Start () {
				SocialGame.LevelManager.gameSelected = 0;
			GameObject temp = GameObject.FindWithTag("GameController");
			if(temp != null)
			{
				KManager = temp.GetComponent<Kinect.KinectManager>();
			}
		}
		
		// Update is called once per frame
		void Update () {
			//Debug.Log(KManager.GetPlayer2ID());
			if(KManager && KManager.GetPlayer2ID() != 0)
			{
				if(!TwoPlayer)
				{
					Activate2player();
				}
			}
			else
			{
				if(TwoPlayer)
				{
					Deactivate2player();
				}
			}
			/*if(TwoPlayer)
			{
				Activate2player();
			}
			else
			{
				Deactivate2player();
			}*/
		}

		 public void Activate2player()
		{
			TwoPlayer = true;
			animPlayer2.SetBool("TwoPlayers",true);
			player2.enabled = true;
			if(LevelManager.gameSelected == 0 && button)
			{
				button.activate();
			}
		}

		public void Deactivate2player()
		{
			TwoPlayer = false;
			animPlayer2.SetBool("TwoPlayers",false);
			player2.enabled = false;
			if(LevelManager.gameSelected == 0 && button)
			{
				button.deactivate();
			}
			if(LevelManager.gameSelected == 2)
			{
				LevelManager.finish();
			}
		}
	}
}
