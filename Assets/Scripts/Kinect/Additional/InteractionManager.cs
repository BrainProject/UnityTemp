#pragma warning disable 414, 219
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using System.IO;

#if UNITY_STANDALONE
namespace Kinect {
	public class InteractionManager : MonoBehaviour 
	{
	//	/// How high off the ground is the sensor (in meters).
	//	public float SensorHeight = 1.0f;

		// GUI-Texture object to be used as screen cursor
		public GameObject handCursor;
		
		// hand cursor textures
		public Texture gripHandTexture;
		public Texture releaseHandTexture;
		public Texture normalHandTexture;

		// smooth factor for cursor movement's lerp
		public float smoothFactor = 3f;
		
		// Bool to specify whether Left/Right-hand-cursor and the Grip-gesture control the mouse cursor and click
		public bool controlMouseCursor;
		
		// GUI-Text object to be used for displaying debug information
		public GameObject debugText;
		
		private uint skeletonTrackingID = 0;
		
		private uint leftHandState = 0;
		private uint rightHandState = 0;
		
		private bool isLeftHandPrimary = false;
		private bool isRightHandPrimary = false;
		
		private bool isLeftHandPress = false;
		private bool isRightHandPress = false;
		
		//	private float leftHandScreenMag = 0f;
	//	private float rightHandScreenMag = 0f;
		
		private Vector3 cursorScreenPos = Vector3.zero;
		private bool dragInProgress = false;
		
		// last event parameters
		private InteractionWrapper.InteractionHandEventType leftHandEvent = InteractionWrapper.InteractionHandEventType.None;
		private InteractionWrapper.InteractionHandEventType lastLeftHandEvent = InteractionWrapper.InteractionHandEventType.Release;
		private Vector3 leftHandScreenPos = Vector3.zero;
		
		private InteractionWrapper.InteractionHandEventType rightHandEvent = InteractionWrapper.InteractionHandEventType.None;
		private InteractionWrapper.InteractionHandEventType lastRightHandEvent = InteractionWrapper.InteractionHandEventType.Release;
		private Vector3 rightHandScreenPos = Vector3.zero;
		
	//	private Matrix4x4 kinectToWorld;
		
		// Pull is considered when the hand's press extent is less than the given value
		private const float pullWhenPressLessThan = 1.0f;
		
		// Push is considered when the hand's press extent is more than the given value
		private const float pushWhenPressMoreThan = 1.0f;
		
		// Pull-gesture internal variables
		private bool bPullRightHandStarted;
		private bool bPullLeftHandStarted;
		private bool bPullRightHandFinished;
		private bool bPullLeftHandFinished;
		private float pullRightHandStartedAt;
		private float pullLeftHandStartedAt;
		
		// Push-gesture internal variables
		private bool bPushRightHandStarted;
		private bool bPushLeftHandStarted;
		private bool bPushRightHandFinished;
		private bool bPushLeftHandFinished;
		private float pushRightHandStartedAt;
		private float pushLeftHandStartedAt;
		
		// Bool to keep track whether Kinect and Interaction library have been initialized
		private bool interactionInited = false;
		
		// The single instance of FacetrackingManager
		private static InteractionManager instance;

		
		
		// returns the single InteractionManager instance
	    public static InteractionManager Instance
	    {
	        get
	        {
	            return instance;
	        }
	    }
		
		// returns true if the InteractionLibrary is initialized, otherwise returns false
		public bool IsInteractionInited()
		{
			return interactionInited;
		}
		
		// returns the user ID (primary skeleton ID), or 0 if no user is currently tracked
		public uint GetUserID()
		{
			return skeletonTrackingID;
		}
		
		// sets new user ID (primary skeleton ID) to be used by the native wrapper
		public void SetUserId(uint userId)
		{
			InteractionWrapper.SetSkeletonTrackingID(userId);
		}
		
		// returns the current left hand event (none, grip or release)
		public InteractionWrapper.InteractionHandEventType GetLeftHandEvent()
		{
			return leftHandEvent;
		}
		
		// returns the last detected left hand event (none, grip or release)
		public InteractionWrapper.InteractionHandEventType GetLastLeftHandEvent()
		{
			return lastLeftHandEvent;
		}
		
		// returns the current screen position of the left hand
		public Vector3 GetLeftHandScreenPos()
		{
			return leftHandScreenPos;
		}
		
		// returns true if the left hand is primary for the user
		public bool IsLeftHandPrimary()
		{
			return isLeftHandPrimary;
		}
		
		// returns true if the left hand has finished Pull-gesture.
		public bool IsLeftHandPull(bool bResetFlag)
		{
			if(bPullLeftHandFinished)
			{
				if(bResetFlag)
				{
					bPullLeftHandFinished = false;
				}
				
				return true;
			}
			
			return false;
		}
		
		// returns true if the left hand has finished Push-gesture.
		public bool IsLeftHandPush(bool bResetFlag)
		{
			if(bPushLeftHandFinished)
			{
				if(bResetFlag)
				{
					bPushLeftHandFinished = false;
				}
				
				return true;
			}
			
			return false;
		}

		// returns true if the left hand is pressing
		public bool IsLeftHandPress()
		{
			return isLeftHandPress;
		}
		
	//	// resets the last valid left hand event
	//	public void ResetLeftHandEvent()
	//	{
	//		lastLeftHandEvent = InteractionWrapper.InteractionHandEventType.None;
	//	}
		
		// returns the current valid right hand event (none, grip or release)
		public InteractionWrapper.InteractionHandEventType GetRightHandEvent()
		{
			return rightHandEvent;
		}
		
		// returns the last detected right hand event (none, grip or release)
		public InteractionWrapper.InteractionHandEventType GetLastRightHandEvent()
		{
			return lastRightHandEvent;
		}
		
		// returns the current screen position of the right hand
		public Vector3 GetRightHandScreenPos()
		{
			return rightHandScreenPos;
		}
		
		// returns true if the right hand is primary for the user
		public bool IsRightHandPrimary()
		{
			return isRightHandPrimary;
		}
		
		// returns true if the right hand has finished Pull-gesture.
		public bool IsRightHandPull(bool bResetFlag)
		{
			if(bPullRightHandFinished)
			{
				if(bResetFlag)
				{
					bPullRightHandFinished = false;
				}
				
				return true;
			}
			
			return false;
		}
		
		// returns true if the right hand has finished Push-gesture.
		public bool IsRightHandPush(bool bResetFlag)
		{
			if(bPushRightHandFinished)
			{
				if(bResetFlag)
				{
					bPushRightHandFinished = false;
				}
				
				return true;
			}
			
			return false;
		}
		
		// returns true if the right hand is pressing
		public bool IsRightHandPress()
		{
			return isRightHandPress;
		}
		
		//	// resets the last valid right hand event
	//	public void ResetRightHandEvent()
	//	{
	//		lastRightHandEvent = InteractionWrapper.InteractionHandEventType.None;
	//	}
		
		//----------------------------------- end of public functions --------------------------------------//
		
		void Awake() 
		{
	//		// get reference to gui texts for left/right hand
	//		handGuiText = GameObject.Find("HandGuiText");
	//		handCursor = GameObject.Find("HandCursor");
			
			// ensure the needed dlls are in place
			if(InteractionWrapper.EnsureKinectWrapperPresence())
			{
				// reload the same level
				Application.LoadLevel(Application.loadedLevel);
			}
		}
		

		void StartInteraction() 
		{
			int hr = 0;
			
			try 
			{
				// initialize Kinect sensor as needed
				hr = InteractionWrapper.InitKinectSensor();
				if(hr != 0)
				{
					throw new Exception("Initialization of Kinect sensor failed");
				}
				
				// initialize Kinect interaction
				hr = InteractionWrapper.InitKinectInteraction();
				if(hr != 0)
				{
					throw new Exception("Initialization of KinectInteraction failed");
				}
				
				// kinect interaction was successfully initialized
				instance = this;
				interactionInited = true;
			} 
			catch(DllNotFoundException ex)
			{
				Debug.LogError(ex.ToString());
				if(debugText != null)
					debugText.guiText.text = "Please check the Kinect SDK installation.";
			}
            //catch (Exception ex) 
            //{
            //    //string message = ex.Message + " - " + InteractionWrapper.GetNuiErrorString(hr);
            //    //Debug.LogError(ex.ToString());
				
            //    //if(debugText != null)
            //    //{
            //    //	debugText.guiText.text = message;
            //    //}
            //    return;
            //}
			
	//		// transform matrix - kinect to world
	//		Quaternion quatTiltAngle = new Quaternion();
	//		int sensorAngle = InteractionWrapper.GetKinectElevationAngle();
	//		quatTiltAngle.eulerAngles = new Vector3(-sensorAngle, 0.0f, 0.0f);
	//			
	//		float heightAboveHips = SensorHeight - 1.0f;
	//		kinectToWorld.SetTRS(new Vector3(0.0f, heightAboveHips, 0.0f), quatTiltAngle, Vector3.one);
			
	//		// load cursor textures once
	//		if(!gripHandTexture)
	//		{
	//			gripHandTexture = (Texture)Resources.Load("GripCursor");
	//		}
	//		if(!releaseHandTexture)
	//		{
	//			releaseHandTexture = (Texture)Resources.Load("ReleaseCursor");
	//		}
	//		if(!normalHandTexture)
	//		{
	//			normalHandTexture = (Texture)Resources.Load("HandCursor");
	//		}
			
			// don't destroy the object on loading levels
			DontDestroyOnLoad(gameObject);
		}
		
		void OnApplicationQuit()
		{
			// uninitialize Kinect interaction
			if(interactionInited)
			{
				InteractionWrapper.FinishKinectInteraction();
				InteractionWrapper.ShutdownKinectSensor();
				
				interactionInited = false;
				instance = null;
			}
		}
		
		void Update () 
		{
			// start Kinect interaction as needed
			if(!interactionInited)
			{
				StartInteraction();
				
//				if(!interactionInited)
//				{
//					Application.Quit();
//					return;
//				}
			}
			
			// update Kinect interaction
			if(InteractionWrapper.UpdateKinectInteraction() == 0)
			{
				//int lastSkeletonTrackingID = skeletonTrackingID;
				skeletonTrackingID = InteractionWrapper.GetSkeletonTrackingID();
				
				if(skeletonTrackingID != 0)
				{
					InteractionWrapper.InteractionHandEventType handEvent = InteractionWrapper.InteractionHandEventType.None;
					Vector4 handPos = Vector4.zero;
					Vector4 shoulderPos = Vector4.zero;
					Vector3 screenPos = Vector3.zero;
					
					// process left hand
					leftHandState = InteractionWrapper.GetLeftHandState();
					handEvent = (InteractionWrapper.InteractionHandEventType)InteractionWrapper.GetLeftHandEvent();
					isLeftHandPress = InteractionWrapper.GetLeftHandPressed();
					
					InteractionWrapper.GetLeftCursorPos(ref handPos);
					leftHandScreenPos.x = Mathf.Clamp01(handPos.x);
					leftHandScreenPos.y = 1.0f - Mathf.Clamp01(handPos.y);
					//leftHandScreenPos.z = Mathf.Clamp01(handPos.z);
					leftHandScreenPos.z = handPos.z;
					
					bool bLeftHandPrimaryNow = (leftHandState & (uint)InteractionWrapper.NuiHandpointerState.PrimaryForUser) != 0;
					bool bRightHandPrimaryNow = (rightHandState & (uint)InteractionWrapper.NuiHandpointerState.PrimaryForUser) != 0;
					
					if((bLeftHandPrimaryNow != isLeftHandPrimary) || (bRightHandPrimaryNow != isRightHandPrimary))
					{
						if(controlMouseCursor && dragInProgress)
						{
							MouseControl.MouseRelease();
							dragInProgress = false;
						}
						
						lastLeftHandEvent = InteractionWrapper.InteractionHandEventType.Release;
						lastRightHandEvent = InteractionWrapper.InteractionHandEventType.Release;
					}
					
					if(controlMouseCursor && (handEvent != lastLeftHandEvent))
					{
						if(!dragInProgress && (handEvent == InteractionWrapper.InteractionHandEventType.Grip))
						{
							dragInProgress = true;
							MouseControl.MouseDrag();
						}
						else if(dragInProgress && (handEvent == InteractionWrapper.InteractionHandEventType.Release))
						{
							MouseControl.MouseRelease();
							dragInProgress = false;
						}
					}
					
					leftHandEvent = handEvent;
					if(handEvent != InteractionWrapper.InteractionHandEventType.None)
					{
						lastLeftHandEvent = handEvent;
					}
					
					// if the hand is primary, set the cursor position
					if(bLeftHandPrimaryNow)
					{
						isLeftHandPrimary = true;
						cursorScreenPos = Vector3.Lerp(cursorScreenPos, leftHandScreenPos, smoothFactor * Time.deltaTime);
						
						if(controlMouseCursor)
						{
							MouseControl.MouseMove(cursorScreenPos);
						}
					}
					else
					{
						isLeftHandPrimary = false;
					}
					
					// check for left hand Pull-gesture
					if(lastLeftHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						if(!bPullLeftHandStarted)
						{
							if(leftHandScreenPos.z > pullWhenPressLessThan)
							{
								bPullLeftHandStarted = true;
								bPullLeftHandFinished = false;
								pullLeftHandStartedAt = Time.realtimeSinceStartup;
							}
						}
					}

					// check for left hand Push-gesture
					if(lastLeftHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						if(!bPushLeftHandStarted)
						{
							if(leftHandScreenPos.z < pushWhenPressMoreThan)
							{
								bPushLeftHandStarted = true;
								bPushLeftHandFinished = false;
								pushLeftHandStartedAt = Time.realtimeSinceStartup;
							}
						}
					}
					
					if(lastLeftHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						if(bPullLeftHandStarted)
						{
							if((Time.realtimeSinceStartup - pullLeftHandStartedAt) <= 1.5f)
							{
								if(leftHandScreenPos.z <= pullWhenPressLessThan)
								{
									bPullLeftHandFinished = true;
									bPullLeftHandStarted = false;
								}
							}
							else
							{
								bPullLeftHandStarted = false;
							}
						}
					}
					else
					{
						// no more hand grip
						bPullLeftHandFinished = false;
						bPullLeftHandStarted = false;
					}

					if(lastLeftHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						if(bPushLeftHandStarted)
						{
							if((Time.realtimeSinceStartup - pushLeftHandStartedAt) <= 1.5f)
							{
								if(leftHandScreenPos.z > pushWhenPressMoreThan)
								{
									bPushLeftHandFinished = true;
									bPushLeftHandStarted = false;
								}
							}
							else
							{
								bPushLeftHandStarted = false;
							}
						}
					}
					else
					{
						// no more hand release
						bPushLeftHandFinished = false;
						bPushLeftHandStarted = false;
					}
					
					// process right hand
					rightHandState = InteractionWrapper.GetRightHandState();
					handEvent = (InteractionWrapper.InteractionHandEventType)InteractionWrapper.GetRightHandEvent();
					isRightHandPress = InteractionWrapper.GetRightHandPressed();
					
					InteractionWrapper.GetRightCursorPos(ref handPos);
					rightHandScreenPos.x = Mathf.Clamp01(handPos.x);
					rightHandScreenPos.y = 1.0f - Mathf.Clamp01(handPos.y);
					//rightHandScreenPos.z = Mathf.Clamp01(handPos.z);
					rightHandScreenPos.z = handPos.z;
					
					if(controlMouseCursor && (handEvent != lastRightHandEvent))
					{
						if(!dragInProgress && (handEvent == InteractionWrapper.InteractionHandEventType.Grip))
						{
							dragInProgress = true;
							MouseControl.MouseDrag();
						}
						else if(dragInProgress && (handEvent == InteractionWrapper.InteractionHandEventType.Release))
						{
							MouseControl.MouseRelease();
							dragInProgress = false;
						}
					}
					
					rightHandEvent = handEvent;
					if(handEvent != InteractionWrapper.InteractionHandEventType.None)
					{
						lastRightHandEvent = handEvent;
					}	
					
					// if the hand is primary, set the cursor position
					if(bRightHandPrimaryNow)
					{
						isRightHandPrimary = true;
						cursorScreenPos = Vector3.Lerp(cursorScreenPos, rightHandScreenPos, smoothFactor * Time.deltaTime);
						
						if(controlMouseCursor)
						{
							MouseControl.MouseMove(cursorScreenPos);
						}
					}
					else
					{
						isRightHandPrimary = false;
					}

					// check for right hand Pull-gesture
					if(lastRightHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						if(!bPullRightHandStarted)
						{
							if(rightHandScreenPos.z > pullWhenPressLessThan)
							{
								bPullRightHandStarted = true;
								bPullRightHandFinished = false;
								pullRightHandStartedAt = Time.realtimeSinceStartup;
							}
						}
					}

					// check for right hand Push-gesture
					if(lastRightHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						if(!bPushRightHandStarted)
						{
							if(rightHandScreenPos.z < pushWhenPressMoreThan)
							{
								bPushRightHandStarted = true;
								bPushRightHandFinished = false;
								pushRightHandStartedAt = Time.realtimeSinceStartup;
							}
						}
					}
					
					if(lastRightHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						if(bPullRightHandStarted)
						{
							if((Time.realtimeSinceStartup - pullRightHandStartedAt) <= 1.5f)
							{
								if(rightHandScreenPos.z <= pullWhenPressLessThan)
								{
									bPullRightHandFinished = true;
									bPullRightHandStarted = false;
								}
							}
							else
							{
								bPullRightHandStarted = false;
							}
						}
					}
					else
					{
						// no more hand grip
						bPullRightHandFinished = false;
						bPullRightHandStarted = false;
					}

					if(lastRightHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						if(bPushRightHandStarted)
						{
							if((Time.realtimeSinceStartup - pushRightHandStartedAt) <= 1.5f)
							{
								if(rightHandScreenPos.z > pushWhenPressMoreThan)
								{
									bPushRightHandFinished = true;
									bPushRightHandStarted = false;
								}
							}
							else
							{
								bPushRightHandStarted = false;
							}
						}
					}
					else
					{
						// no more hand release
						bPushRightHandFinished = false;
						bPushRightHandStarted = false;
					}
					
				}
				else
				{
					leftHandState = 0;
					rightHandState = 0;
					
					isLeftHandPrimary = false;
					isRightHandPrimary = false;

					isLeftHandPress = false;
					isRightHandPress = false;
					
					leftHandEvent = InteractionWrapper.InteractionHandEventType.None;
					rightHandEvent = InteractionWrapper.InteractionHandEventType.None;
					
					lastLeftHandEvent = InteractionWrapper.InteractionHandEventType.Release;
					lastRightHandEvent = InteractionWrapper.InteractionHandEventType.Release;
					
					if(controlMouseCursor && dragInProgress)
					{
						MouseControl.MouseRelease();
						dragInProgress = false;
					}
				}
			}
			
		}
		
		void OnGUI()
		{
			if(!interactionInited)
				return;
			
			// display debug information
			if(debugText)
			{
				string sGuiText = "Cursor: " + cursorScreenPos.ToString();
				
				if(IsRightHandPrimary())
				{
					if(lastRightHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						sGuiText += "  RightGrip";
					}
					else if(lastRightHandEvent == InteractionWrapper.InteractionHandEventType.Release)
					{
						sGuiText += "  RightRelease";
						//bPullRightHandFinished = false;
					}
					
					if(bPullRightHandFinished)
					{
						sGuiText += "  RightPull";
					}
					
					if(bPushRightHandFinished)
					{
						sGuiText += "  RightPush";
					}

					if(isRightHandPress)
					{
						sGuiText += "  RightPress";
					}
				}
				
				if(IsLeftHandPrimary())
				{
					if(lastLeftHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
					{
						sGuiText += "  LeftGrip";
					}
					else if(lastLeftHandEvent == InteractionWrapper.InteractionHandEventType.Release)
					{
						sGuiText += "  LeftRelease";
						//bPullLeftHandFinished = false;
					}
					
					if(bPullLeftHandFinished)
					{
						sGuiText += "  LeftPull";
					}
					
					if(bPushLeftHandFinished)
					{
						sGuiText += "  LeftPush";
					}

					if(isLeftHandPress)
					{
						sGuiText += "  LeftPress";
					}


				}
				
				debugText.guiText.text = sGuiText;
			}
			
			// display the cursor status and position
			if(handCursor != null)
			{
				Texture texture = null;
				
				if(IsLeftHandPrimary())
				{
					if(lastLeftHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
						texture = gripHandTexture;
					else if(lastLeftHandEvent == InteractionWrapper.InteractionHandEventType.Release)
						texture = releaseHandTexture;
				}
				else if(IsRightHandPrimary())
				{
					if(lastRightHandEvent == InteractionWrapper.InteractionHandEventType.Grip)
						texture = gripHandTexture;
					else if(lastRightHandEvent == InteractionWrapper.InteractionHandEventType.Release)
						texture = releaseHandTexture;
				}
				
				if(texture == null)
				{
					texture = normalHandTexture;
				}
				
				if(handCursor && handCursor.guiTexture && texture)
				{
					handCursor.guiTexture.texture = texture;
					handCursor.transform.position = cursorScreenPos;
				}
			}
		}
	}
}
#endif