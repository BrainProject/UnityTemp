using UnityEngine;
using System.Collections;
using Kinect;


public class TwoPlayerSwitcher : MonoBehaviour {
	KinectManager KManager;
	public bool TwoPlayer;
	public SkinnedMeshRenderer player2;
	public Animator animPlayer2;
	// Use this for initialization
	void Start () {
		GameObject temp = GameObject.FindWithTag("GameController");
		if(temp != null)
		{
			KManager = temp.GetComponent<Kinect.KinectManager>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(KManager.GetPlayer2ID());
		if(KManager.GetPlayer2ID() != 0)
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
	}

	public void Deactivate2player()
	{
		TwoPlayer = false;
		animPlayer2.SetBool("TwoPlayers",false);
		player2.enabled = false;
	}
}
