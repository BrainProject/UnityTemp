using UnityEngine;
using System.Collections;
#if !UNITY_WEBPLAYER
using Kinect;
#endif

namespace SocialGame{
	public class Snap : MonoBehaviour {
#if !UNITY_WEBPLAYER
		public int player;
		public GameObject after;
		public Vector3 positionLeft;
		public Vector3 positionRight;

		private GameObject targetPlayer;

		public void snap()
		{
			GameObject playerObj;
			string tag;
			KinectManager kinect = Kinect.KinectManager.Instance;
			if(player <1 || player > 2)
				return;
			if(player == 1)
			{

				tag="Player2";
				if(kinect)
				{
					playerObj = GameObjectEx.FindByTagFromList(kinect.Player1Avatars,"Player1");
					targetPlayer = GameObjectEx.FindByTagFromList(kinect.Player2Avatars,tag);
				}
				else
				{
					playerObj = GameObject.Find("P1");
				}
			}
			else
			{
				tag="Player1";
				if(kinect)
				{
					playerObj = GameObjectEx.FindByTagFromList(kinect.Player2Avatars,"Player2");
					targetPlayer = GameObjectEx.FindByTagFromList(kinect.Player1Avatars,tag);
				}
				else
				{
					playerObj = GameObject.Find("P2");
				}
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
			if(player == 1 )
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

#endif
	}
}