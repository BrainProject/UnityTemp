#if UNITY_STANDALONE
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Kinect;

namespace SocialGame{
	public class ExtendsAvatar : AvatarController {
		public Transform handLeft;
		public Transform handRight;
		public bool realPosition = true;

		private Vector3 newPos;
		private bool run = true;




	
		// Update the avatar each frame.
		public override void UpdateAvatar(Int64 UserID)
		{	
			playerId = UserID;
			
			if(!transform.gameObject.activeInHierarchy) 
				return;
			
			// Get the KinectManager instance
			if(kinectManager == null && !KinectManager.Instance.isDefaultKM )
			{
				kinectManager = KinectManager.Instance;

			}
			
			// move the avatar to its Kinect position
			MoveAvatar(UserID);

			
			for (var boneIndex = 0; boneIndex < bones.Length; boneIndex++)
			{
				if (!bones[boneIndex]) 
					continue;
				
				if(boneIndex2JointMap.ContainsKey(boneIndex))
				{
					KinectInterop.JointType joint = !mirroredMovement ? boneIndex2JointMap[boneIndex] : boneIndex2MirrorJointMap[boneIndex];
					TransformBone(UserID, joint, boneIndex, !mirroredMovement);
				}
				else if(specIndex2JointMap.ContainsKey(boneIndex))
				{
					// special bones (clavicles)
					List<KinectInterop.JointType> alJoints = !mirroredMovement ? specIndex2JointMap[boneIndex] : specIndex2MirrorJointMap[boneIndex];
					
					if(alJoints.Count >= 2)
					{
						//Debug.Log(alJoints[0].ToString());
						Vector3 baseDir = alJoints[0].ToString().EndsWith("Left") ? Vector3.left : Vector3.right;
						TransformSpecialBone(UserID, alJoints[0], alJoints[1], boneIndex, baseDir, !mirroredMovement);
					}
				}
			}

			if(realPosition && kinectManager && (UserID != 0) && run)
			{
				newPos = this.transform.position;
				Debug.Log(" pos:" + kinectManager.GetUserPosition(playerId).x + "id: " + playerId);
				newPos.x = kinectManager.GetUserPosition(playerId).x;
				this.transform.position = newPos;
			}
		}




		void Stop(bool stop)
		{
			run = !stop;
		}
	}
}
#endif