using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Snap : MonoBehaviour {
#if !UNITY_WEBPLAYER
		public int player;
		public GameObject after;
		public Vector3 position;
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public void snap()
		{
			GameObject playerObj;
			string tag; 
			if(player <1 || player > 2)
				return;
			if(player == 1)
			{
				playerObj = GameObject.Find("P1");
				tag="Player2";
			}
			else
			{
				playerObj = GameObject.Find("P2");
				tag="Player1";
			}
			if(playerObj)
			{
				FigureCreate figCreate = playerObj.GetComponentInChildren<FigureCreate>();
				GameObject checker = figCreate.createPoints();
				GestChecker checkerScript = checker.GetComponent<GestChecker>();
				//GameObject root = GameObjectEx.findGameObjectWithNameTag("root",tag);
				setTags(checker,tag);
				checker.transform.position = position;
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



		public void setCheckerScript(GestChecker script)
		{
			script.handMode = false;
			script.finish = false;
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