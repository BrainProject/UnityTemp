using UnityEngine;
using System.Collections;
using Kinect;


namespace SocialGame{
	public class TwoPlayerSwitcher : MonoBehaviour {
		#if UNITY_STANDALONE
		KinectManager KManager;
		public bool TwoPlayer;
		public SkinnedMeshRenderer player2;
		public Animator animPlayer2;
		public Check button;

		public Material ghost;
		private Material normal;
		// Use this for initialization
		void Start () {
			SocialGame.LevelManager.gameSelected = 0;
			normal = player2.material;
			GameObject temp = GameObject.FindWithTag("GameController");
			if(temp != null)
			{
				KManager = temp.GetComponent<Kinect.KinectManager>();
			}
		}
		
		/// <summary>
		/// check second player and show him
		/// </summary>
		void Update () {
			if(KManager && KManager.GetUserIdByIndex(1) != 0)
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
		}

		/// <summary>
		/// Activate2player this instance.
		/// </summary>
		 public void Activate2player()
		{
			TwoPlayer = true;
			if(ghost)
			{
				player2.material = normal;
			}
			else
			{
				player2.enabled = true;
			}
			if(animPlayer2)
				animPlayer2.SetBool("TwoPlayers",true);
			if(LevelManager.gameSelected == 0 && button)
			{
				button.activate();
			}
		}

		/// <summary>
		/// Deactivate2player this instance.
		/// </summary>
		public void Deactivate2player()
		{
			TwoPlayer = false;
			if(ghost)
			{
				player2.material = ghost;
			}
			else
			{
				player2.enabled = false;
			}
		}
		#endif
	}
}