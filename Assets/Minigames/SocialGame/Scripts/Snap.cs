using UnityEngine;
using System.Collections;
#if UNITY_STANDALONE
using Kinect;
#endif

namespace SocialGame{
	public class Snap : Check {
#if UNITY_STANDALONE
		public int player;
		public GameObject after;
		public Vector3 positionLeft;
		public Vector3 positionRight;
		public CheckCancleFigure cancle;
		public Animator anim;

		private GameObject targetPlayer;


		private KinectManager kinect
		{
			get{
				return Kinect.KinectManager.Instance;
			}
		}

		private Vector3 oldLocalPos;
		private Transform oldParent;

		protected override void Start()
		{
			base.Start();
		}

		public override bool Checked (Transform target)
		{
			return false;
		}

		public override void show ()
		{
			if (activated) 
			{
				if(transform.parent.parent)
				{
					oldLocalPos = transform.parent.localPosition;
					oldParent = transform.parent.parent;
					transform.parent.parent = null;
				}
			}
			else
			{
				if(oldParent)
				{
					transform.parent.parent = oldParent;
					transform.parent.localPosition = oldLocalPos;
					oldParent = null;
				}
			}
			anim.SetBool ("active", activated);
			base.show ();
		}

		/// <summary>
		/// Snap figure.
		/// </summary>
        [ContextMenu("Snap")]
		public void snap()
		{
			GameObject playerObj = null;
			string tag= "Error";
			//KinectManager kinect = Kinect.KinectManager.Instance;
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
				setTags(checker,tag);
				if(targetPlayer && (targetPlayer.transform.position.x > 0))
				{
					checker.transform.position = positionRight;
				}
				else
				{
					checker.transform.position = positionLeft;
				}
				if(checkerScript)
				{
					checkerScript.findTartgetByCheckName();
					setCheckerScript(checkerScript);
				}
				else
				{
					Debug.LogError("checker script missing");
				}
				if(cancle)
				{
					cancle.figure = checkerScript;
					cancle.activate();

				}
			}

			deactivate ();
		}

		static public void setTags(GameObject obj, string tag)
		{
			obj.tag = tag;
			for(int i = 0; i< obj.transform.childCount;i++)
			{
				obj.transform.GetChild(i).tag = tag;
			}
		}

		void Stop(bool stop)
		{
			if (stop)
			{
			  //bugbug
			}
				//Destroy (gameObject);
		}

		/// <summary>
		/// Sets the checker script.
		/// </summary>
		/// <param name="script">Script.</param>
		public void setCheckerScript(GestCheckerFigure script)
		{
			script.allChecked = true;
			script.destroy = true;
			script.distance = 0.2f;
			script.next = after;
			script.clipBone = "root";
			script.cancle = cancle;
			script.nextCheck = cancle.next;
			script.next = null;
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

		/// <summary>
		/// Gets the avatar object.
		/// </summary>
		/// <returns>The avatar object.</returns>
		/// <param name="player">Player.</param>
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
			return null;
		}

#endif
	}
}