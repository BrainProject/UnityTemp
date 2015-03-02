using UnityEngine;
using System.Collections;
#if UNITY_STANDALONE
using Kinect;
#endif

namespace SocialGame{
	public class Snap : MonoBehaviour {
#if UNITY_STANDALONE
		public int player;
		public GameObject after;
		public Vector3 positionLeft;
		public Vector3 positionRight;

		private GameObject targetPlayer;
		private KinectManager kinect;

		void Start()
		{
			kinect = Kinect.KinectManager.Instance;
		}

		public void snap()
		{
			GameObject playerObj = null;
			string tag= "Error";
			KinectManager kinect = Kinect.KinectManager.Instance;
			if(player <0 || player > 1)
				return;
			if(player == 0)
			{

				tag="Player2";
				playerObj = GetAvatarObj(0);
				targetPlayer = GetAvatarObj(1);
			}
			else
			{
				tag="Player1";
				playerObj = GetAvatarObj(1);
				targetPlayer = GetAvatarObj(0);
			}
			if(playerObj)
			{
				FigureCreate figCreate = playerObj.GetComponentInChildren<FigureCreate>();
				GameObject checker = figCreate.createPoints();
				GestCheckerFigure checkerScript = checker.GetComponent<GestCheckerFigure>();
				//GameObject root = GameObjectEx.findGameObjectWithNameTag("root",tag);
				setTags(checker,tag);
				if(targetPlayer && (targetPlayer.transform.position.x > 0))
				{
					checker.transform.position = positionRight;
				}
				else
				{
					checker.transform.position = positionLeft;
				}
				checkerScript.findTartgetByCheckName();
				setCheckerScript(checkerScript);
			}
			Destroy(gameObject);
		}

		static public void setTags(GameObject obj, string tag)
		{
			obj.tag = tag;
			for(int i = 0; i< obj.transform.childCount;i++)
			{
				obj.transform.GetChild(i).tag = tag;
			}
		}



		public void setCheckerScript(GestCheckerFigure script)
		{
			script.handMode = false;
			//script.finish = false;
			script.allChecked = true;
			script.destroy = true;
			script.distance = 0.2f;
			script.next = after;
			script.clipBone = "root";
			if(player == 0 )
			{
				script.player1 = false;
				script.player2 = true;
			}
			else
			{
				script.player1 = true;
				script.player2 = false;
			}
		}

		private GameObject GetAvatarObj(int player)
		{
			if(kinect)
			{
				foreach(Kinect.AvatarController avatar in kinect.avatarControllers)
				{
					if(avatar.playerIndex == player)
					{
						return avatar.gameObject;
					}
				}
			}
			Debug.Log ("chyba");
			return null;
		}

#endif
	}
}