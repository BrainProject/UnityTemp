using UnityEngine;
using System.Collections;

public class ScoreLoadAfterSnakeDeath : MonoBehaviour {

	public int level;
	public bool death;
	public GameObject gm;

	void OnDestroy(){
		gm = GameObject.Find ("_GameManager_");
		if(gm!=null)
		{
		death = GameObject.Find ("_GameManager_").GetComponent<GameManager> ().death;
		level = GameObject.Find ("_GameManager_").GetComponent<GameManager> ().currentLevel;
		print("checking death: " + death);
		if (death) {
			Screen.showCursor = true;
			GameObject.Find ("loserMessage").guiText.enabled = true;
			}

			//GameObject.Find ("_GameManager_").GetComponent<GameManager> ().game = false;
//			KinectManager manager = KinectManager.Instance;
			/*if (manager!=null)//pohyb
			{
						manager.Player1Gestures [2] = KinectGestures.Gestures.RightHandCursor;
						manager.Player1Gestures [3] = KinectGestures.Gestures.LeftHandCursor;
						manager.Player1Gestures [4] = KinectGestures.Gestures.Click;
						manager.Player1Gestures [5] = KinectGestures.Gestures.None;
						manager.ControlMouseCursor = true;
			}*/
						print ("score screen");
					//	Application.LoadLevel ("Score");
						print ("score screen LOADED");
				}


	}
}
