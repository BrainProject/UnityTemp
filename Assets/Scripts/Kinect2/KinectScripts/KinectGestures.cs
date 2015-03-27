using UnityEngine;
//using Windows.Kinect;

using System.Collections;
using System.Collections.Generic;

namespace Kinect
{
	public class KinectGestures
	{
		public interface GestureListenerInterface
		{
			#if UNITY_STANDALONE
			// Invoked when a new user is detected and tracking starts
			// Here you can start gesture detection with KinectManager.DetectGesture()
			void UserDetected(long userId, int userIndex);
			
			// Invoked when a user is lost
			// Gestures for this user are cleared automatically, but you can free the used resources
			void UserLost(long userId, int userIndex);
			
			// Invoked when a gesture is in progress 
			void GestureInProgress(long userId, int userIndex, Gestures gesture, float progress, 
			                       KinectInterop.JointType joint, Vector3 screenPos);

			// Invoked if a gesture is completed.
			// Returns true, if the gesture detection must be restarted, false otherwise
			bool GestureCompleted(long userId, int userIndex, Gestures gesture,
			                      KinectInterop.JointType joint, Vector3 screenPos);

			// Invoked if a gesture is cancelled.
			// Returns true, if the gesture detection must be retarted, false otherwise
			bool GestureCancelled(long userId, int userIndex, Gestures gesture, 
			                      KinectInterop.JointType joint);
		}
		
		
		public enum Gestures
		{
			None = 0,
			RaiseRightHand,
			RaiseLeftHand,
			Psi,
			Tpose,
			Stop,
			Wave,
			Click,
			SwipeLeft,
			SwipeRight,
			SwipeLeftLHand,
			SwipeRightRHand,
			SwipeUp,
			SwipeDown,
	//		RightHandCursor,
	//		LeftHandCursor,
			ZoomOut,
			ZoomIn,
			Wheel,
			Jump,
			Squat,
			Push,
			Pull,
			HiddenGesture
		}
		
		
		public struct GestureData
		{
			public long userId;
			public Gestures gesture;
			public int state;
			public float timestamp;
			public int joint;
			public Vector3 jointPos;
			public Vector3 screenPos;
			public float tagFloat;
			public Vector3 tagVector;
			public Vector3 tagVector2;
			public float progress;
			public bool complete;
			public bool cancelled;
			public List<Gestures> checkForGestures;
			public float startTrackingAtTime;
		}
		

		
		// Gesture related constants, variables and functions
		private const int leftHandIndex = (int)KinectInterop.JointType.HandLeft;
		private const int rightHandIndex = (int)KinectInterop.JointType.HandRight;
			
		private const int leftElbowIndex = (int)KinectInterop.JointType.ElbowLeft;
		private const int rightElbowIndex = (int)KinectInterop.JointType.ElbowRight;
			
		private const int leftShoulderIndex = (int)KinectInterop.JointType.ShoulderLeft;
		private const int rightShoulderIndex = (int)KinectInterop.JointType.ShoulderRight;
		
		private const int hipCenterIndex = (int)KinectInterop.JointType.SpineBase;
		private const int leftHipIndex = (int)KinectInterop.JointType.HipLeft;
		private const int rightHipIndex = (int)KinectInterop.JointType.HipRight;
		
		private const int leftKneeIndex = (int)KinectInterop.JointType.KneeLeft;
		private const int rightKneeIndex = (int)KinectInterop.JointType.KneeRight;

		private const int neck = (int)KinectInterop.JointType.Neck;
		private const int hipsIndex = (int)KinectInterop.JointType.SpineBase;
		
		
		private static int[] neededJointIndexes = {
			leftHandIndex, rightHandIndex, leftElbowIndex, rightElbowIndex, leftShoulderIndex, rightShoulderIndex,
			hipCenterIndex, neck, leftHipIndex, rightHipIndex
		};
		
		
		// Returns the list of the needed gesture joint indexes
		public static int[] GetNeededJointIndexes()
		{
			return neededJointIndexes;
		}
		
		
		
		private static void SetGestureJoint(ref GestureData gestureData, float timestamp, int joint, Vector3 jointPos)
		{
			gestureData.joint = joint;
			gestureData.jointPos = jointPos;
			gestureData.timestamp = timestamp;
			gestureData.state++;
		}
		
		private static void SetGestureCancelled(ref GestureData gestureData)
		{
			gestureData.state = 0;
			gestureData.progress = 0f;
			gestureData.cancelled = true;
		}
		
		private static void CheckPoseComplete(ref GestureData gestureData, float timestamp, Vector3 jointPos, bool isInPose, float durationToComplete)
		{
			if(isInPose)
			{
				float timeLeft = timestamp - gestureData.timestamp;
				gestureData.progress = durationToComplete > 0f ? Mathf.Clamp01(timeLeft / durationToComplete) : 1.0f;
		
				if(timeLeft >= durationToComplete)
				{
					gestureData.timestamp = timestamp;
					gestureData.jointPos = jointPos;
					gestureData.state++;
					gestureData.complete = true;
				}
			}
			else
			{
				SetGestureCancelled(ref gestureData);
			}
		}
		
		private static void SetScreenPos(long userId, ref GestureData gestureData, ref Vector3[] jointsPos, ref bool[] jointsTracked)
		{
			Vector3 handPos = jointsPos[rightHandIndex];
	//		Vector3 elbowPos = jointsPos[rightElbowIndex];
	//		Vector3 shoulderPos = jointsPos[rightShoulderIndex];
			bool calculateCoords = false;
			
			if(gestureData.joint == rightHandIndex)
			{
				if(jointsTracked[rightHandIndex] /**&& jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex]*/)
				{
					calculateCoords = true;
				}
			}
			else if(gestureData.joint == leftHandIndex)
			{
				if(jointsTracked[leftHandIndex] /**&& jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex]*/)
				{
					handPos = jointsPos[leftHandIndex];
	//				elbowPos = jointsPos[leftElbowIndex];
	//				shoulderPos = jointsPos[leftShoulderIndex];
					
					calculateCoords = true;
				}
			}
			
			if(calculateCoords)
			{
	//			if(gestureData.tagFloat == 0f || gestureData.userId != userId)
	//			{
	//				// get length from shoulder to hand (screen range)
	//				Vector3 shoulderToElbow = elbowPos - shoulderPos;
	//				Vector3 elbowToHand = handPos - elbowPos;
	//				gestureData.tagFloat = (shoulderToElbow.magnitude + elbowToHand.magnitude);
	//			}
				
				if(jointsTracked[hipCenterIndex] && jointsTracked[neck] && 
					jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex])
				{
					Vector3 shoulderToHips = jointsPos[neck] - jointsPos[hipCenterIndex];
					Vector3 rightToLeft = jointsPos[rightShoulderIndex] - jointsPos[leftShoulderIndex];
					
					gestureData.tagVector2.x = rightToLeft.x; // * 1.2f;
					gestureData.tagVector2.y = shoulderToHips.y; // * 1.2f;
					
					if(gestureData.joint == rightHandIndex)
					{
						gestureData.tagVector.x = jointsPos[rightShoulderIndex].x - gestureData.tagVector2.x / 2;
						gestureData.tagVector.y = jointsPos[hipCenterIndex].y;
					}
					else
					{
						gestureData.tagVector.x = jointsPos[leftShoulderIndex].x - gestureData.tagVector2.x / 2;
						gestureData.tagVector.y = jointsPos[hipCenterIndex].y;
					}
				}
		
	//			Vector3 shoulderToHand = handPos - shoulderPos;
	//			gestureData.screenPos.x = Mathf.Clamp01((gestureData.tagFloat / 2 + shoulderToHand.x) / gestureData.tagFloat);
	//			gestureData.screenPos.y = Mathf.Clamp01((gestureData.tagFloat / 2 + shoulderToHand.y) / gestureData.tagFloat);
				
				if(gestureData.tagVector2.x != 0 && gestureData.tagVector2.y != 0)
				{
					Vector3 relHandPos = handPos - gestureData.tagVector;
					gestureData.screenPos.x = Mathf.Clamp01(relHandPos.x / gestureData.tagVector2.x);
					gestureData.screenPos.y = Mathf.Clamp01(relHandPos.y / gestureData.tagVector2.y);
				}
				
				//Debug.Log(string.Format("{0} - S: {1}, H: {2}, SH: {3}, L : {4}", gestureData.gesture, shoulderPos, handPos, shoulderToHand, gestureData.tagFloat));
			}
		}
		
		private static void SetZoomFactor(long userId, ref GestureData gestureData, float initialZoom, ref Vector3[] jointsPos, ref bool[] jointsTracked)
		{
			Vector3 vectorZooming = jointsPos[rightHandIndex] - jointsPos[leftHandIndex];
			
			if(gestureData.tagFloat == 0f || gestureData.userId != userId)
			{
				gestureData.tagFloat = 0.5f; // this is 100%
			}

			float distZooming = vectorZooming.magnitude;
			gestureData.screenPos.z = initialZoom + (distZooming / gestureData.tagFloat);
		}
		
		private static void SetWheelRotation(long userId, ref GestureData gestureData, Vector3 initialPos, Vector3 currentPos)
		{
			float angle = Vector3.Angle(initialPos, currentPos) * Mathf.Sign(currentPos.y - initialPos.y);
			gestureData.screenPos.z = angle;
		}
		
		// estimate the next state and completeness of the gesture
		public static void CheckForGesture(long userId, ref GestureData gestureData, float timestamp, ref Vector3[] jointsPos, ref bool[] jointsTracked)
		{
			if(gestureData.complete)
				return;
			
			switch(gestureData.gesture)
			{
				// check for RaiseRightHand
				case Gestures.RaiseRightHand:
					switch(gestureData.state)
					{
						case 0:  // gesture detection
							if(jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
							}
							break;
								
						case 1:  // gesture complete
							bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
								(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f;

							Vector3 jointPos = jointsPos[gestureData.joint];
							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
							break;
					}
					break;

				// check for RaiseLeftHand
				case Gestures.RaiseLeftHand:
					switch(gestureData.state)
					{
						case 0:  // gesture detection
							if(jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
						            (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f)
							{
								SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
							}
							break;
								
						case 1:  // gesture complete
							bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
								(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f;

							Vector3 jointPos = jointsPos[gestureData.joint];
							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
							break;
					}
					break;

				// check for Psi
				case Gestures.Psi:
					switch(gestureData.state)
					{
						case 0:  // gesture detection
							if(jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f &&
						       jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
						       (jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
							}
							break;
								
						case 1:  // gesture complete
							bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
								(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.1f &&
								jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
								(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.1f;

							Vector3 jointPos = jointsPos[gestureData.joint];
							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
							break;
					}
					break;

				// check for Tpose
				case Gestures.Tpose:
					switch(gestureData.state)
					{
					case 0:  // gesture detection
						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex] &&
					       Mathf.Abs(jointsPos[rightElbowIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.07f
					       Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.7f
					   	   jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex] &&
					  	   Mathf.Abs(jointsPos[leftElbowIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f &&
					       Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f)
						{
							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
						}
						break;
						
					case 1:  // gesture complete
						bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[rightShoulderIndex] &&
								Mathf.Abs(jointsPos[rightElbowIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.7f
							    Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightShoulderIndex].y) < 0.1f &&  // 0.7f
							    jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[leftShoulderIndex] &&
								Mathf.Abs(jointsPos[leftElbowIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f &&
							    Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftShoulderIndex].y) < 0.1f;
						
						Vector3 jointPos = jointsPos[gestureData.joint];
						CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
						break;
					}
					break;
					
				// check for Stop
				case Gestures.Stop:
					switch(gestureData.state)
					{
						case 0:  // gesture detection
							if(jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) < 0f &&
						       jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
						       (jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) < 0f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
							}
							break;
								
						case 1:  // gesture complete
							bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
								(jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) < 0f &&
								jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
								(jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) < 0f;

							Vector3 jointPos = jointsPos[gestureData.joint];
							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
							break;
					}
					break;

				// check for Wave
				case Gestures.Wave:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f &&
						       (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0.05f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.progress = 0.3f;
							}
							else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
						            (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < -0.05f)
							{
								SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
								gestureData.progress = 0.3f;
							}
							break;
					
						case 1:  // gesture - phase 2
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = gestureData.joint == rightHandIndex ?
									jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f && 
									(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < -0.05f :
									jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
									(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
									(jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) > 0.05f;
					
								if(isInPose)
								{
									gestureData.timestamp = timestamp;
									gestureData.state++;
									gestureData.progress = 0.7f;
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
										
						case 2:  // gesture phase 3 = complete
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = gestureData.joint == rightHandIndex ?
									jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f && 
									(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0.05f :
									jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
									(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
									(jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < -0.05f;

								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;

				// check for Click

			case Gestures.Click:
				switch(gestureData.state)
				{
				case 0:  // gesture detection - phase 1
					if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
					   (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f)
					{
						SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
						gestureData.progress = 0.3f;
						
						// set screen position at the start, because this is the most accurate click position
						SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
					}
					else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
					        (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f)
					{
						SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
						gestureData.progress = 0.3f;
						
						// set screen position at the start, because this is the most accurate click position
						SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
					}
					break;
					
				case 1:  // gesture - phase 2
					//						if((timestamp - gestureData.timestamp) < 1.0f)
					//						{
					//							bool isInPose = gestureData.joint == rightHandIndex ?
					//								jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
					//								//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f && 
					//								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f &&
					//								(jointsPos[rightHandIndex].z - gestureData.jointPos.z) < -0.05f :
					//								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
					//								//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
					//								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f &&
					//								(jointsPos[leftHandIndex].z - gestureData.jointPos.z) < -0.05f;
					//				
					//							if(isInPose)
					//							{
					//								gestureData.timestamp = timestamp;
					//								gestureData.jointPos = jointsPos[gestureData.joint];
					//								gestureData.state++;
					//								gestureData.progress = 0.7f;
					//							}
					//							else
					//							{
					//								// check for stay-in-place
					//								Vector3 distVector = jointsPos[gestureData.joint] - gestureData.jointPos;
					//								isInPose = distVector.magnitude < 0.05f;
					//
					//								Vector3 jointPos = jointsPos[gestureData.joint];
					//								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, Constants.ClickStayDuration);
					//							}
					//						}
					//						else
					{
						// check for stay-in-place
						Vector3 distVector = jointsPos[gestureData.joint] - gestureData.jointPos;
						bool isInPose = distVector.magnitude < 0.05f;
						
						Vector3 jointPos = jointsPos[gestureData.joint];
						CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.ClickStayDuration);
						if(gestureData.complete)
						{
							MouseControl.MouseClick();
						}
						//							SetGestureCancelled(gestureData);
					}
					break;
					
					//					case 2:  // gesture phase 3 = complete
					//						if((timestamp - gestureData.timestamp) < 1.0f)
					//						{
					//							bool isInPose = gestureData.joint == rightHandIndex ?
					//								jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
					//								//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f && 
					//								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f &&
					//								(jointsPos[rightHandIndex].z - gestureData.jointPos.z) > 0.05f :
					//								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
					//								//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
					//								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f &&
					//								(jointsPos[leftHandIndex].z - gestureData.jointPos.z) > 0.05f;
					//
					//							if(isInPose)
					//							{
					//								Vector3 jointPos = jointsPos[gestureData.joint];
					//								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
					//							}
					//						}
					//						else
					//						{
					//							// cancel the gesture
					//							SetGestureCancelled(ref gestureData);
					//						}
					//						break;
				}
				break;
				/*
				case Gestures.Click:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.progress = 0.3f;

								// set screen position at the start, because this is the most accurate click position
								SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
							}
							else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f)
							{
								SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
								gestureData.progress = 0.3f;

								// set screen position at the start, because this is the most accurate click position
								SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
							}
							break;
					
						case 1:  // gesture - phase 2
	//						if((timestamp - gestureData.timestamp) < 1.0f)
	//						{
	//							bool isInPose = gestureData.joint == rightHandIndex ?
	//								jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
	//								//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f && 
	//								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f &&
	//								(jointsPos[rightHandIndex].z - gestureData.jointPos.z) < -0.05f :
	//								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
	//								//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
	//								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f &&
	//								(jointsPos[leftHandIndex].z - gestureData.jointPos.z) < -0.05f;
	//				
	//							if(isInPose)
	//							{
	//								gestureData.timestamp = timestamp;
	//								gestureData.jointPos = jointsPos[gestureData.joint];
	//								gestureData.state++;
	//								gestureData.progress = 0.7f;
	//							}
	//							else
	//							{
	//								// check for stay-in-place
	//								Vector3 distVector = jointsPos[gestureData.joint] - gestureData.jointPos;
	//								isInPose = distVector.magnitude < 0.05f;
	//
	//								Vector3 jointPos = jointsPos[gestureData.joint];
	//								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, Constants.ClickStayDuration);
	//							}
	//						}
	//						else
							{
								// check for stay-in-place
								Vector3 distVector = jointsPos[gestureData.joint] - gestureData.jointPos;
								bool isInPose = distVector.magnitude < 0.05f;

								Vector3 jointPos = jointsPos[gestureData.joint];
								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.ClickStayDuration);
	//							SetGestureCancelled(gestureData);
							}
							break;
										
	//					case 2:  // gesture phase 3 = complete
	//						if((timestamp - gestureData.timestamp) < 1.0f)
	//						{
	//							bool isInPose = gestureData.joint == rightHandIndex ?
	//								jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
	//								//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.1f && 
	//								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f &&
	//								(jointsPos[rightHandIndex].z - gestureData.jointPos.z) > 0.05f :
	//								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
	//								//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.1f &&
	//								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f &&
	//								(jointsPos[leftHandIndex].z - gestureData.jointPos.z) > 0.05f;
	//
	//							if(isInPose)
	//							{
	//								Vector3 jointPos = jointsPos[gestureData.joint];
	//								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
	//							}
	//						}
	//						else
	//						{
	//							// cancel the gesture
	//							SetGestureCancelled(ref gestureData);
	//						}
	//						break;
					}
					break;

	*/
				// DEFAULT SWIPE LEFT
				// check for SwipeLeft
	/*
				case Gestures.SwipeLeft:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f &&
						       (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.progress = 0.5f;
							}
	//						else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
	//					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
	//					            (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) > 0f)
	//						{
	//							SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
	//							//gestureData.jointPos = jointsPos[leftHandIndex];
	//							gestureData.progress = 0.5f;
	//						}
							break;
					
						case 1:  // gesture phase 2 = complete
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = gestureData.joint == rightHandIndex ?
									jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f && 
									Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.08f && 
									(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < -0.15f :
									jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
									Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
									Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.08f && 
									(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < -0.15f;

								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;
	*/
				
				//EDITED SwipeLeft
				// check for SwipeLeft
				case Gestures.SwipeLeft:
					switch(gestureData.state)
					{
					case 0:  // gesture detection - phase 1
						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						   (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f)// &&
							//(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0f)
						{
							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
							gestureData.progress = 0.5f;
						}
						//						else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						//					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
						//					            (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) > 0f)
						//						{
						//							SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
						//							//gestureData.jointPos = jointsPos[leftHandIndex];
						//							gestureData.progress = 0.5f;
						//						}
						break;
						
					case 1:  // gesture phase 2 = complete
						if((timestamp - gestureData.timestamp) < 0.5f)
						{	
							bool isInPose = gestureData.joint == rightHandIndex ?
								jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									//Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f && 
									Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.08f && 
									(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < -0.25f :
									jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
									//Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
									Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.08f && 
									(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < -0.25f;
							
							if(isInPose)
							{
								Vector3 jointPos = jointsPos[gestureData.joint];
								CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
								if(gestureData.complete)
								{
									Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_J);
								}
							}
						}
						else
						{
							// cancel the gesture
							SetGestureCancelled(ref gestureData);
						}
						break;
					}
				break;

				// DEFAULT SWIPE RIGHT
				// check for SwipeRight
	/*
				case Gestures.SwipeRight:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
	//						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
	//					       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f &&
	//					       (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < 0f)
	//						{
	//							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
	//							//gestureData.jointPos = jointsPos[rightHandIndex];
	//							gestureData.progress = 0.5f;
	//						}
	//						else 
							if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
						            (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < 0f)
							{
								SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
								gestureData.progress = 0.5f;
							}
							break;
					
						case 1:  // gesture phase 2 = complete
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = gestureData.joint == rightHandIndex ?
									jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f && 
									Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.08f && 
									(jointsPos[rightHandIndex].x - gestureData.jointPos.x) > 0.15f :
									jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
									Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
									Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.08f && 
									(jointsPos[leftHandIndex].x - gestureData.jointPos.x) > 0.15f;

								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;
	*/
					
//					// check for SwipeRight
//					// EDITED SwipeRight
					case Gestures.SwipeRight:
						switch(gestureData.state)
						{
						case 0:  // gesture detection - phase 1
							//						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
							//					       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f &&
							//					       (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < 0f)
							//						{
							//							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
							//							//gestureData.jointPos = jointsPos[rightHandIndex];
							//							gestureData.progress = 0.5f;
							//						}
							//						else 
							if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
							   (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f)// &&
								// (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < 0f)
							{
								SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
								gestureData.progress = 0.5f;
							}
							break;
							
						case 1:  // gesture phase 2 = complete
							if((timestamp - gestureData.timestamp) < 0.5f)
							{
								bool isInPose = gestureData.joint == rightHandIndex ?
									jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
										//Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f && 
										Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.08f && 
										(jointsPos[rightHandIndex].x - gestureData.jointPos.x) > 0.25f :
										jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
										//Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
										Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.08f && 
										(jointsPos[leftHandIndex].x - gestureData.jointPos.x) > 0.25f;
								
								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
									if(gestureData.complete)
									{
										Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_L);
									}
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
						break;
					}
				break;

//				// EDITED SWIPE LEFT v2 (scratched)
//			case Gestures.SwipeRight:
//				switch(gestureData.state)
//				{
//				case 0:  // gesture detection - phase 1
//					//				Debug.Log("0 start");
//					//				Debug.Log("leftHand tracked: " + jointsTracked[leftHandIndex]);
//					//				Debug.Log("Head tracked: " + jointsTracked[spineShoulder] + ", " + spineShoulder);
//					//				Debug.Log("Hips tracked: " + jointsTracked[hipsIndex]);
//					Debug.Log("Detected: " + jointsTracked[leftHandIndex] + ", " + jointsTracked[neck]);
//					if(jointsTracked[leftHandIndex] && jointsTracked[neck])// &&
//						//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < -0.05f)// &&
//						//(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0f)
//					{
//						//					Debug.Log("hand tracked.");
//						SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
//						gestureData.progress = 0.5f;
//						Debug.Log("SWIPE R START");
//					}
//					//						else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
//					//					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
//					//					            (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) > 0f)
//					//						{
//					//							SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
//					//							//gestureData.jointPos = jointsPos[leftHandIndex];
//					//							gestureData.progress = 0.5f;
//					//						}
//					//				Debug.Log("0 end");
//					break;
//					
//				case 1:  // gesture phase 2 = complete
//					//				Debug.Log("1 start");
//					if((timestamp - gestureData.timestamp) < 0.5f)
//					{
//						Debug.Log("SWIPE R IN PROGRESS " + jointsTracked[leftHandIndex] + ", " + jointsTracked[neck]);
//						bool isInPose = gestureData.joint == leftHandIndex ?
//							jointsTracked[leftHandIndex] && jointsTracked[neck] &&
//								//Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f && 
//								Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.1f && 
//								(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.25f :
//								jointsTracked[rightHandIndex] && jointsTracked[neck] &&
//								//Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
//								Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.1f && 
//								(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.25f;
//						
//						if(isInPose)
//						{
//							Debug.Log("SWIPE R DONE.");
//							Vector3 jointPos = jointsPos[gestureData.joint];
//							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
//							if(gestureData.complete)
//							{
//								Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_L);
//							}
//						}
//					}
//					else
//					{
//						// cancel the gesture
//						SetGestureCancelled(ref gestureData);
//					}
//					//				Debug.Log("1 end");
//					break;
//				}
//				break;

				
			case Gestures.SwipeLeftLHand:
				switch(gestureData.state)
				{
				case 0:  // gesture detection - phase 1
	//				Debug.Log("0 start");
	//				Debug.Log("leftHand tracked: " + jointsTracked[leftHandIndex]);
	//				Debug.Log("Head tracked: " + jointsTracked[spineShoulder] + ", " + spineShoulder);
	//				Debug.Log("Hips tracked: " + jointsTracked[hipsIndex]);
					if(jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex])// &&
						//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < -0.05f)// &&
						//(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0f)
					{
	//					Debug.Log("hand tracked.");
						SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
						gestureData.progress = 0.5f;
					}
					//						else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
					//					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
					//					            (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) > 0f)
					//						{
					//							SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
					//							//gestureData.jointPos = jointsPos[leftHandIndex];
					//							gestureData.progress = 0.5f;
					//						}
	//				Debug.Log("0 end");
					break;
					
				case 1:  // gesture phase 2 = complete
	//				Debug.Log("1 start");
					if((timestamp - gestureData.timestamp) < 0.5f)
					{
						bool isInPose = gestureData.joint == leftHandIndex ?
							jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
								//Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f && 
								Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.1f && 
								(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < -0.25f :
								jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
								//Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
								Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.1f && 
								(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < -0.25f;
						
						if(isInPose)
						{
							Vector3 jointPos = jointsPos[gestureData.joint];
							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
							if(gestureData.complete)
							{
								Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_J);
							}
						}
					}
					else
					{
						// cancel the gesture
						SetGestureCancelled(ref gestureData);
					}
	//				Debug.Log("1 end");
					break;
				}
				break;
				
			case Gestures.SwipeRightRHand:
				switch(gestureData.state)
				{
				case 0:  // gesture detection - phase 1
					if(jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex])// &&
						//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < -0.05f)// &&
						//(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0f)
					{
						SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
						gestureData.progress = 0.5f;
					}
					//						else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
					//					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
					//					            (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) > 0f)
					//						{
					//							SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
					//							//gestureData.jointPos = jointsPos[leftHandIndex];
					//							gestureData.progress = 0.5f;
					//						}
					break;
					
				case 1:  // gesture phase 2 = complete
					if((timestamp - gestureData.timestamp) < 0.5f)
					{
						bool isInPose = gestureData.joint == rightHandIndex ?
							jointsTracked[rightHandIndex] && jointsTracked[rightShoulderIndex] &&
								//Mathf.Abs(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < 0.1f && 
								Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.1f && 
								(jointsPos[rightHandIndex].x - gestureData.jointPos.x) > 0.25f :
								jointsTracked[leftHandIndex] && jointsTracked[leftShoulderIndex] &&
								//Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
								Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.1f && 
								(jointsPos[leftHandIndex].x - gestureData.jointPos.x) > 0.25f;
						
						if(isInPose)
						{
							//Debug.Log("right r " + isInPose);
							Vector3 jointPos = jointsPos[gestureData.joint];
							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
							if(gestureData.complete)
							{
								//Debug.Log("SWIPE RIGHT R HAND");
								Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_L);
							}
						}
					}
					else
					{
						// cancel the gesture
						SetGestureCancelled(ref gestureData);
					}
					break;
				}
				break;
				
				// check for SwipeUp
			case Gestures.SwipeUp:
				switch(gestureData.state)
				{
				case 0:  // gesture detection - phase 1
							if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < -0.05f &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.15f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.progress = 0.5f;
							}
							else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < -0.05f &&
						            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.15f)
							{
								SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
								gestureData.progress = 0.5f;
							}
							break;
					
						case 1:  // gesture phase 2 = complete
					Debug.Log("SWIPE UP in progress");
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = gestureData.joint == rightHandIndex ?
									jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] && jointsTracked[leftShoulderIndex] &&
									//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f && 
									//(jointsPos[rightHandIndex].y - gestureData.jointPos.y) > 0.15f && 
									(jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y) > 0.05f && 
									Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f :
									jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] && jointsTracked[rightShoulderIndex] &&
									//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
									//(jointsPos[leftHandIndex].y - gestureData.jointPos.y) > 0.15f && 
									(jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) > 0.05f && 
									Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f;

								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
								if(gestureData.complete)
								{
								Debug.Log("SWIPE UP");
								Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_U);
								}
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;

				// check for SwipeDown
				case Gestures.SwipeDown:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							if(jointsTracked[rightHandIndex] && jointsTracked[leftShoulderIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[leftShoulderIndex].y) >= 0.05f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.progress = 0.5f;
							}
							else if(jointsTracked[leftHandIndex] && jointsTracked[rightShoulderIndex] &&
						            (jointsPos[leftHandIndex].y - jointsPos[rightShoulderIndex].y) >= 0.05f)
							{
								SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
								gestureData.progress = 0.5f;
							}
							break;
					
						case 1:  // gesture phase 2 = complete
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = gestureData.joint == rightHandIndex ?
									jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									//(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < -0.1f && 
									(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < -0.2f && 
									Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f :
									jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
									//(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < -0.1f &&
									(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < -0.2f && 
									Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f;

								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
							if(gestureData.complete)
							{
								Debug.Log("SWIPE DOWN");
								Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_D);
							}
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;

	//			// check for RightHandCursor
	//			case Gestures.RightHandCursor:
	//				switch(gestureData.state)
	//				{
	//					case 0:  // gesture detection - phase 1 (perpetual)
	//						if(jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
	//							//(jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) > -0.1f)
	//				   			(jointsPos[rightHandIndex].y - jointsPos[hipCenterIndex].y) >= 0f)
	//						{
	//							gestureData.joint = rightHandIndex;
	//							gestureData.timestamp = timestamp;
	//							gestureData.jointPos = jointsPos[rightHandIndex];
	//
	//							SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
	//							gestureData.progress = 0.7f;
	//						}
	//						else
	//						{
	//							// cancel the gesture
	//							//SetGestureCancelled(ref gestureData);
	//							gestureData.progress = 0f;
	//						}
	//						break;
	//				
	//				}
	//				break;
	//
	//			// check for LeftHandCursor
	//			case Gestures.LeftHandCursor:
	//				switch(gestureData.state)
	//				{
	//					case 0:  // gesture detection - phase 1 (perpetual)
	//						if(jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
	//							//(jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) > -0.1f)
	//							(jointsPos[leftHandIndex].y - jointsPos[hipCenterIndex].y) >= 0f)
	//						{
	//							gestureData.joint = leftHandIndex;
	//							gestureData.timestamp = timestamp;
	//							gestureData.jointPos = jointsPos[leftHandIndex];
	//
	//							SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
	//							gestureData.progress = 0.7f;
	//						}
	//						else
	//						{
	//							// cancel the gesture
	//							//SetGestureCancelled(ref gestureData);
	//							gestureData.progress = 0f;
	//						}
	//						break;
	//				
	//				}
	//				break;

				// check for ZoomOut
				case Gestures.ZoomOut:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							float distZoomOut = ((Vector3)(jointsPos[rightHandIndex] - jointsPos[leftHandIndex])).magnitude;
					
							if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
							   jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						       (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
							   distZoomOut < 0.2f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.progress = 0.3f;
							}
							break;
					
						case 1:  // gesture phase 2 = zooming
							if((timestamp - gestureData.timestamp) < 1.0f)
							{
								bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						   			jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									((jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f ||
					       			(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f);

								if(isInPose)
								{
									SetZoomFactor(userId, ref gestureData, 1.0f, ref jointsPos, ref jointsTracked);
									gestureData.timestamp = timestamp;
									gestureData.progress = 0.7f;
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;

				// check for ZoomIn
				case Gestures.ZoomIn:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							float distZoomIn = ((Vector3)jointsPos[rightHandIndex] - jointsPos[leftHandIndex]).magnitude;

							if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
							   jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						       (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
							   distZoomIn >= 0.7f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.tagFloat = distZoomIn;
								gestureData.progress = 0.3f;
							}
							break;
					
						case 1:  // gesture phase 2 = zooming
							if((timestamp - gestureData.timestamp) < 1.0f)
							{
								bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						   			jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									((jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f ||
					       			(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f);

								if(isInPose)
								{
									SetZoomFactor(userId, ref gestureData, 0.0f, ref jointsPos, ref jointsTracked);
									gestureData.timestamp = timestamp;
									gestureData.progress = 0.7f;
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;

				// check for Wheel
				case Gestures.Wheel:
					Vector3 vectorWheel = (Vector3)jointsPos[rightHandIndex] - jointsPos[leftHandIndex];
					float distWheel = vectorWheel.magnitude;

					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
							   jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						       (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
							   distWheel > 0.2f && distWheel < 0.7f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.tagVector = vectorWheel;
								gestureData.tagFloat = distWheel;
								gestureData.progress = 0.3f;
							}
							break;
					
						case 1:  // gesture phase 2 = zooming
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						   			jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									((jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f ||
					       			(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
									Mathf.Abs(distWheel - gestureData.tagFloat) < 0.1f);

								if(isInPose)
								{
									SetWheelRotation(userId, ref gestureData, gestureData.tagVector, vectorWheel);
									gestureData.timestamp = timestamp;
									gestureData.tagFloat = distWheel;
									gestureData.progress = 0.7f;
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;
				
				// check for Jump
				case Gestures.Jump:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							if(jointsTracked[hipCenterIndex] && 
								(jointsPos[hipCenterIndex].y > 0.8f) && (jointsPos[hipCenterIndex].y < 1.3f))
							{
								SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
								gestureData.progress = 0.5f;
							}
							break;
					
						case 1:  // gesture phase 2 = complete
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = jointsTracked[hipCenterIndex] &&
									(jointsPos[hipCenterIndex].y - gestureData.jointPos.y) > 0.15f && 
									Mathf.Abs(jointsPos[hipCenterIndex].x - gestureData.jointPos.x) < 0.15f;

								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;

				// check for Squat
				case Gestures.Squat:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							if(jointsTracked[hipCenterIndex] && 
								(jointsPos[hipCenterIndex].y < 0.8f))
							{
								SetGestureJoint(ref gestureData, timestamp, hipCenterIndex, jointsPos[hipCenterIndex]);
								gestureData.progress = 0.5f;
							}
							break;
					
						case 1:  // gesture phase 2 = complete
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = jointsTracked[hipCenterIndex] &&
									(jointsPos[hipCenterIndex].y - gestureData.jointPos.y) < -0.15f && 
									Mathf.Abs(jointsPos[hipCenterIndex].x - gestureData.jointPos.x) < 0.15f;

								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;

				// check for Push
				case Gestures.Push:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f &&
						       Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < 0.15f &&
							   (jointsPos[rightHandIndex].z - jointsPos[rightElbowIndex].z) < -0.05f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.progress = 0.5f;
							}
							else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
						            Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < 0.15f &&
								    (jointsPos[leftHandIndex].z - jointsPos[leftElbowIndex].z) < -0.05f)
							{
								SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
								gestureData.progress = 0.5f;
							}
							break;
					
						case 1:  // gesture phase 2 = complete
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = gestureData.joint == rightHandIndex ?
									jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.15f && 
									Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.15f && 
									(jointsPos[rightHandIndex].z - gestureData.jointPos.z) < -0.15f :
									jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
									Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.15f &&
									Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.15f && 
									(jointsPos[leftHandIndex].z - gestureData.jointPos.z) < -0.15f;

								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);	
							if(gestureData.complete)
							{
								Debug.Log("SWIPE Back");
								Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_B);
							}

								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
					}
					break;

				// check for Pull
				case Gestures.Pull:
					switch(gestureData.state)
					{
						case 0:  // gesture detection - phase 1
							if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
						       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > -0.05f &&
						       Mathf.Abs(jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < 0.15f &&
							   (jointsPos[rightHandIndex].z - jointsPos[rightElbowIndex].z) < -0.15f)
							{
								SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
								gestureData.progress = 0.5f;
							}
							else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
						            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > -0.05f &&
						            Mathf.Abs(jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) < 0.15f &&
								    (jointsPos[leftHandIndex].z - jointsPos[leftElbowIndex].z) < -0.15f)
							{
								SetGestureJoint(ref gestureData, timestamp, leftHandIndex, jointsPos[leftHandIndex]);
								gestureData.progress = 0.5f;
							}
							break;
					
						case 1:  // gesture phase 2 = complete
							if((timestamp - gestureData.timestamp) < 1.5f)
							{
								bool isInPose = gestureData.joint == rightHandIndex ?
									jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
									Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.15f && 
									Mathf.Abs(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < 0.15f && 
									(jointsPos[rightHandIndex].z - gestureData.jointPos.z) > 0.15f :
									jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
									Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.15f &&
									Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.15f && 
									(jointsPos[leftHandIndex].z - gestureData.jointPos.z) > 0.15f;

								if(isInPose)
								{
									Vector3 jointPos = jointsPos[gestureData.joint];
									CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, 0f);
							if(gestureData.complete)
							{
								Debug.Log("SWIPE Front");
								Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_F);
							}
								}
							}
							else
							{
								// cancel the gesture
								SetGestureCancelled(ref gestureData);
							}
							break;
						}
						break;
				case Gestures.HiddenGesture:
					switch(gestureData.state)
					{
					case 0:  // gesture detection
						if(jointsTracked[rightHandIndex] && jointsTracked[hipsIndex] &&
						   (jointsPos[hipsIndex].y - jointsPos[rightHandIndex].y) > 0f &&
						   (jointsPos[rightHandIndex].x - jointsPos[hipsIndex].x) > 0.5f)
						{
							//Debug.Log("START");
							Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.NUMPAD7);
							//Debug.Log(jointsPos[rightHandIndex].x - jointsPos[hipsIndex].x);
							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
						}
						break;
						
					case 1:  // gesture in progress
						/*if(timestamp - gestureData.timestamp <= 3)
						{
							gestureData.progress = (timestamp - gestureData.timestamp) / 3;
							Debug.Log ((timestamp - gestureData.timestamp) / 3);
						}
						if(timestamp - gestureData.timestamp > 3)
							gestureData.state++;
						if(gestureData.cancelled)
						{
							Debug.Log(gestureData.cancelled);
							gestureData.progress = 0;
							SetGestureCancelled(ref gestureData);
						}*/
						
						bool gestureDetected = (jointsTracked[rightHandIndex] && jointsTracked[hipsIndex] &&
						                        (jointsPos[hipsIndex].y - jointsPos[rightHandIndex].y) > 0f &&
						                        (jointsPos[rightHandIndex].x - jointsPos[hipsIndex].x) > 0.5f);
						if(!gestureDetected)
						{
							//Debug.Log("gesture CANCELEEEEED");
							Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.NUMPAD9);
							SetGestureCancelled(ref gestureData);
						}
						
						if(timestamp - gestureData.timestamp <= 3)
						{
							//Debug.Log("DETECTING"); 
							gestureData.progress = (timestamp - gestureData.timestamp) / 3;
							MGC.Instance.minigamesGUI.guiDetection.ShowDetection(gestureData.progress);
							//Debug.Log ((timestamp - gestureData.timestamp) / 3);
						}
						
						if(timestamp - gestureData.timestamp > 3)
							gestureData.state++;
						
						break;
						
					case 2:  // gesture complete
						if((timestamp - gestureData.timestamp) > 3)					
						{
							bool isInPose = jointsTracked[rightHandIndex] && jointsTracked[neck] &&
								(jointsPos[hipsIndex].y - jointsPos[rightHandIndex].y) > 0f &&
									(jointsPos[rightHandIndex].x - jointsPos[hipsIndex].x) > 0.5f;
							
							
							Vector3 jointPos = jointsPos[gestureData.joint];
							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, KinectInterop.Constants.PoseCompleteDuration);
							if(isInPose)
							{
								Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.KEY_I);
								Kinect.Win32.MouseKeySimulator.SendKeyPress(Kinect.Win32.KeyCode.NUMPAD9);
							}
						}
						break;
					}
				break;
				// here come more gesture-cases
			}
			#endif
		}
	}
}