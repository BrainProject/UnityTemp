using UnityEngine;
using System.Collections;


namespace SocialGame{
public class CheckSwitchObj :Check {
		public GameObject player1;
		public GameObject player2;
		public string name;
		public int owner;
		// Use this for initialization
		void Start () {
			if (!player1) 
			{
				player1 = GameObjectEx.FindGameObjectWithNameTag (name, "Player1");
				owner = 2;
			}

			if (!player2) 
			{
				player2 = GameObjectEx.FindGameObjectWithNameTag (name, "Player2");
				owner = 1;
			}
		}
	
		/// <summary>
		/// Checked the specified target.
		/// </summary>
		/// <param name="target">Target.</param>
		public virtual bool Checked(Transform target)
		{
			bool last = false;
			finishTarget = target;
			if(next.Length > 0)
			{
				foreach(Check obj in next)
				{
					obj.activate();
				}
			}
			else
			{

				last = true;
			}
			thisActivate();
			Debug.Log("Check");
			switch (owner) 
			{
			case(1):
				player2.SetActive(true);
				player1.SetActive(false);
				break;
			case(2):
				player1.SetActive(true);
				player2.SetActive(false);
				break;
			}
			return last;
		}
	}
}