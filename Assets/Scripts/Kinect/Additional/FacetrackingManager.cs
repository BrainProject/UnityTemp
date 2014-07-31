using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_STANDALONE
public class FacetrackingManager : MonoBehaviour 
{
	// Public bool to determine whether the head rotation and movement should be mirrored or normal
	public bool MirroredHeadMovement = true;
	
	// Public Bool to determine whether to receive and compute the color map
	public bool ComputeColorMap = false;
	
	// Public Bool to determine whether to display color map on the GUI
	public bool DisplayColorMap = false;
	
	// Public Bool to determine whether to display face rectangle on the GUI
	public bool DisplayFaceRect = false;
	
	// Public Bool to determine whether to visualize facetracker lines on the GUI
	public bool VisualizeFacetracker = false;
	
	// GUI Text to show messages.
	public GameObject debugText;

	// Is currently tracking
	private bool isTracking = false;
	
	// Are shape units converged
	private bool isConverged = false;
	
	// Animation units
	private float[] afAU = null;
	private bool bGotAU = false;

	// Shape units
	private float[] afSU = null;
	private bool bGotSU = false;
	
	// Points to visualize (each point consists of 2 elements - for its X and Y)
	private float[] avPointsXY = null;
	private Vector2[] avPoints = null;
	private bool bGotPoints = false;

	///// added by Takefumi
	// 3D-Points of 3D_Face_Model (each point consists of 3 elements - for its X, Y and Z)  http://www.icg.isy.liu.se/candide/  (candide-3)
	private float[] avPointsXYZ = null;
	private Vector3[] av3DPoints = null;
	private bool bGot3DPoints = false;
	///// end of added

	// Head position and rotation
	private Vector3 headPos = Vector3.zero;
	private Quaternion headRot = Quaternion.identity;
	
	// Tracked face rectangle
	private FacetrackingWrapper.FaceRect faceRect;
	
	// Bool to keep track of whether Kinect and FT-library have been initialized
	private bool facetrackingInitialized = false;
	
	// The single instance of FacetrackingManager
	private static FacetrackingManager instance;
	
	// Color image data, if used
	private Texture2D usersClrTex;
	private Rect usersClrRect;
	private Color32[] colorImage;
	private byte[] videoBuffer;
	
	
	// returns the single FacetrackingManager instance
    public static FacetrackingManager Instance
    {
        get
        {
            return instance;
        }
    }
	
	// returns true if SAPI is successfully initialized, false otherwise
	public bool IsFacetrackingInitialized()
	{
		return facetrackingInitialized;
	}
	
	// returns true if the facetracking library is tracking a face at the moment
	public bool IsTracking()
	{
		return isTracking;
	}
	
	// returns the color image texture,if ComputeColorMap is true
    public Texture2D GetUsersClrTex()
    { 
		return usersClrTex;
	}
	
	// returns the tracked head position
	public Vector3 GetHeadPosition()
	{
		return headPos;
	}
	
	// returns the tracked head rotation
	public Quaternion GetHeadRotation()
	{
		return headRot;
	}
	
	// returns animation units count, or 0 if no face has been tracked
	public int GetAnimUnitsCount()
	{
		if(afAU != null)
		{
			return afAU.Length;
		}
		
		return 0;
	}
	
	// returns the animation unit at given index, or 0 if the index is invalid
	public float GetAnimUnit(int index)
	{
		if(afAU != null && index >= 0 && index < afAU.Length)
		{
			return afAU[index];
		}
		
		return 0.0f;
	}
	
	// returns shape units count, or 0 if no face has been tracked
	public int GetShapeUnitsCount()
	{
		if(afSU != null)
		{
			return afSU.Length;
		}
		
		return 0;
	}
	
	// returns the shape unit at given index, or 0 if the index is invalid
	public float GetShapeUnit(int index)
	{
		if(afSU != null && index >= 0 && index < afSU.Length)
		{
			return afSU[index];
		}
		
		return 0.0f;
	}
	
	// returns true if shape is converged, false otherwise
	public bool IsShapeConverged()
	{
		return isConverged;
	}

	///// added by Takefumi
	// returns 3D points count of 3D_FACE_MODEL (av3DPoints), if bGot3DPoints is true.
	public int Get3DShapePointsCount()
	{
		if (bGot3DPoints) {
			return av3DPoints.Length;
		} 
		return 0;
	}
	// returns 3D points of 3D_FACE_MODEL (av3DPoints), if bGot3DPoints is true.
	public Vector3[] Get3DShapePoints()
	{
		if (bGot3DPoints) {
			return av3DPoints;
		}
		return null;
	}
	///// end of added
	
	//----------------------------------- end of public functions --------------------------------------//
	
	
	void Awake() 
	{
		//debugText = GameObject.Find("DebugText");
		
		// ensure the needed dlls are in place
		if(FacetrackingWrapper.EnsureKinectWrapperPresence())
		{
			// reload the same level
			Application.LoadLevel(Application.loadedLevel);
		}
	}
	
	void StartFacetracker() 
	{
		try 
		{
			if(debugText != null)
				debugText.guiText.text = "Please, wait...";
			
			// initialize Kinect sensor as needed
			int rc = FacetrackingWrapper.InitKinectSensor();
			if(rc != 0)
			{
				throw new Exception("Initialization of Kinect sensor failed");
			}
			
			// Initialize the kinect speech wrapper
			rc = FacetrackingWrapper.InitFaceTracking();
	        if (rc < 0)
	        {
	            throw new Exception(String.Format("Error initializing Kinect/FT: hr=0x{0:X}", rc));
	        }
			
			if(ComputeColorMap)
			{
				// Initialize color map related stuff
		        usersClrTex = new Texture2D(FacetrackingWrapper.GetImageWidth(), FacetrackingWrapper.GetImageHeight(), TextureFormat.ARGB32, false);
		        usersClrRect = new Rect(Screen.width, Screen.height - usersClrTex.height, -usersClrTex.width, usersClrTex.height);
				
				colorImage = new Color32[FacetrackingWrapper.GetImageWidth() * FacetrackingWrapper.GetImageHeight()];
				videoBuffer = new byte[FacetrackingWrapper.GetImageWidth() * FacetrackingWrapper.GetImageHeight() * 4];
			}
			
			instance = this;
			facetrackingInitialized = true;
			
			DontDestroyOnLoad(gameObject);

			if(debugText != null)
				debugText.guiText.text = "Ready.";
		} 
		catch(DllNotFoundException ex)
		{
			Debug.LogError(ex.ToString());
			if(debugText != null)
				debugText.guiText.text = "Please check the Kinect and FT-Library installations.";
		}
		catch (Exception ex) 
		{
			Debug.LogError(ex.ToString());
			if(debugText != null)
				debugText.guiText.text = ex.Message;
		}
	}

	// Make sure to kill the Kinect on quitting.
	void OnApplicationQuit()
	{
		// Shutdown Speech Recognizer and Kinect
		FacetrackingWrapper.FinishFaceTracking();
		FacetrackingWrapper.ShutdownKinectSensor();
		
		facetrackingInitialized = false;
		instance = null;
	}
	
	void Update() 
	{
		// start Kinect face tracker as needed
		if(!facetrackingInitialized)
		{
			StartFacetracker();
			
			if(!facetrackingInitialized)
			{
				Application.Quit();
				return;
			}
		}
		
		if(facetrackingInitialized)
		{
			// update the face tracker
			int rc = FacetrackingWrapper.UpdateFaceTracking();
			
			if(rc >= 0)
			{
				// poll the video frame as needed
				if(ComputeColorMap)
				{
					if(FacetrackingWrapper.PollVideo(ref videoBuffer, ref colorImage))
					{
				        usersClrTex.SetPixels32(colorImage);
				        usersClrTex.Apply();
					}
				}
				
				// estimate the tracking state
				isTracking = FacetrackingWrapper.IsFaceTracked();

				// get the facetracking parameters
				if(isTracking)
				{
					//float fTimeNow = Time.realtimeSinceStartup;
					
					// get face rectangle
					bool bGotFaceRect = FacetrackingWrapper.GetFaceRect(ref faceRect);
					
					// get head position and rotation
					Vector4 vHeadPos = Vector4.zero, vHeadRot = Vector4.zero;
					if(FacetrackingWrapper.GetHeadPosition(ref vHeadPos))
					{
						headPos = (Vector3)vHeadPos;
						
						if(!MirroredHeadMovement)
						{
							headPos.z = -headPos.z;
						}
					}
					
					if(FacetrackingWrapper.GetHeadRotation(ref vHeadRot))
					{
						if(MirroredHeadMovement)
						{
							vHeadRot.x = -vHeadRot.x;
							vHeadRot.z = -vHeadRot.z;
						}
						else
						{
							vHeadRot.x = -vHeadRot.x;
							vHeadRot.y = -vHeadRot.y;
						}
						
						headRot = Quaternion.Euler((Vector3)vHeadRot);
					}
					
					// get the animation units
					int iNumAU = FacetrackingWrapper.GetAnimUnitsCount();
					bGotAU = false;
					
					if(iNumAU > 0)
					{
						if(afAU == null)
						{
							afAU = new float[iNumAU];
						}
						
						bGotAU = FacetrackingWrapper.GetAnimUnits(afAU, ref iNumAU);
					}
					
					// get the shape units
					isConverged = FacetrackingWrapper.IsShapeConverged();
					int iNumSU = FacetrackingWrapper.GetShapeUnitsCount();
					bGotSU = false;
					
					if(iNumSU > 0)
					{
						if(afSU == null)
						{
							afSU = new float[iNumSU];
						}
						
						bGotSU = FacetrackingWrapper.GetShapeUnits(afSU, ref iNumSU);
					}
					
					// get the shape points
					int iNumPoints = FacetrackingWrapper.GetShapePointsCount();
					bGotPoints = false;
					
					if(iNumPoints > 0)
					{
						int iNumPointsXY = iNumPoints << 1;

						if(avPointsXY == null)
						{
							avPointsXY = new float[iNumPointsXY];
							avPoints = new Vector2[iNumPoints];
						}
						
						bGotPoints = FacetrackingWrapper.GetShapePoints(avPointsXY, ref iNumPointsXY);

						if(bGotPoints)
						{
							for(int i = 0; i < iNumPoints; i++)
							{
								int iXY = i << 1;
								
								avPoints[i].x = avPointsXY[iXY];
								avPoints[i].y = avPointsXY[iXY + 1];
							}
						}
					}

					///// added by Takefumi 
					// get the 3D shape points
					int i3DNumPoints = FacetrackingWrapper.Get3DShapePointsCount();
					bGot3DPoints = false;
					
					if(i3DNumPoints > 0)
					{
						int iNumPointsXYZ = i3DNumPoints * 3;
						
						if(avPointsXYZ == null)
						{
							avPointsXYZ = new float[iNumPointsXYZ];
							av3DPoints = new Vector3[i3DNumPoints];
						}
						
						bGot3DPoints = FacetrackingWrapper.Get3DShapePoints(avPointsXYZ, ref iNumPointsXYZ);
						
						if(bGot3DPoints)
						{
							for(int i = 0; i < i3DNumPoints; i++)
							{
								int iXYZ = i * 3;

								av3DPoints[i].x = avPointsXYZ[iXYZ];
								av3DPoints[i].y = avPointsXYZ[iXYZ + 1];
								av3DPoints[i].z = avPointsXYZ[iXYZ + 2];
							}
						}
					}
					///// end of added

					if(ComputeColorMap)
					{
						if(DisplayFaceRect && bGotFaceRect)
						{
							DrawFacetrackerRect(usersClrTex, !VisualizeFacetracker);
						}
						
						if(VisualizeFacetracker)
						{
							DrawFacetrackerLines(usersClrTex, avPoints, true);
						}
					}
					
				}
			}
		}
	}
	
	void OnGUI()
	{
		if(facetrackingInitialized)
		{
			if(ComputeColorMap && DisplayColorMap)
			{
				GUI.DrawTexture(usersClrRect, usersClrTex);
			}
			
			if(debugText != null)
			{
				string sAuDebug = string.Empty;
				
//				// debug anim. units
//				sAuDebug = "AU: ";
//				int iNumAU = GetAnimUnitsCount();
//				
//				for(int i = 0; i < iNumAU; i++)
//				{
//					sAuDebug += String.Format("{0}:{1:F2} ", i, afAU[i]);
//				}
					
				if(isTracking)
					debugText.guiText.text = "Tracking... " + sAuDebug;
				else
					debugText.guiText.text = "Not tracking...";
			}
		}
	}
	
	// draws the lines of tracked face
	private void DrawFacetrackerLines(Texture2D aTexture, Vector2[] avPoints, bool bApplyTexture)
	{
		if(avPoints == null || avPoints.Length < 87)
			return;
		
		Color color = Color.yellow;
		
	    for (int ipt = 0; ipt < 8; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt+1)%8];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 8; ipt < 16; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 8 + 1) % 8 + 8];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 16; ipt < 26; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 16 + 1) % 10 + 16];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 26; ipt < 36; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 26 + 1) % 10 + 26];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 36; ipt < 47; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[ipt + 1];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 48; ipt < 60; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 48 + 1) % 12 + 48];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 60; ipt < 68; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[(ipt - 60 + 1) % 8 + 60];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
	
	    for (int ipt = 68; ipt < 86; ++ipt)
	    {
	        Vector2 ptStart = avPoints[ipt];
	        Vector2 ptEnd = avPoints[ipt + 1];
			
	        DrawLine(aTexture, ptStart, ptEnd, color);
	    }
		
		if(bApplyTexture)
		{
			aTexture.Apply();
		}
	}
	
	// draws the bounding rectangle of tracked face
	private void DrawFacetrackerRect(Texture2D aTexture, bool bApplyTexture)
	{
		if(avPoints == null || avPoints.Length < 87)
			return;
		
		Color color = Color.magenta;
		Vector2 pt1, pt2;
		
		// bottom
		pt1.x = faceRect.x; pt1.y = faceRect.y;
		pt2.x = faceRect.x + faceRect.width - 1; pt2.y = pt1.y;
		DrawLine(aTexture, pt1, pt2, color);
		
		// right
		pt1.x = pt2.x; pt1.y = pt2.y;
		pt2.x = pt1.x; pt2.y = faceRect.y + faceRect.height - 1;
		DrawLine(aTexture, pt1, pt2, color);
		
		// top
		pt1.x = pt2.x; pt1.y = pt2.y;
		pt2.x = faceRect.x; pt2.y = pt1.y;
		DrawLine(aTexture, pt1, pt2, color);
		
		// left
		pt1.x = pt2.x; pt1.y = pt2.y;
		pt2.x = pt1.x; pt2.y = faceRect.y;
		DrawLine(aTexture, pt1, pt2, color);
		
		if(bApplyTexture)
		{
			aTexture.Apply();
		}
	}
	
	// draws a line in a texture
	private void DrawLine(Texture2D a_Texture, Vector2 ptStart, Vector2 ptEnd, Color a_Color)
	{
		int width = FacetrackingWrapper.Constants.ImageWidth;
		int height = FacetrackingWrapper.Constants.ImageHeight;
		
		DrawLine(a_Texture, width - (int)ptStart.x, height - (int)ptStart.y, 
					width - (int)ptEnd.x, height - (int)ptEnd.y, a_Color, width, height);
	}
	
	// draws a line in a texture
	private void DrawLine(Texture2D a_Texture, int x1, int y1, int x2, int y2, Color a_Color, int width, int height)
	{
		int dy = y2 - y1;
		int dx = x2 - x1;
	 
		int stepy = 1;
		if (dy < 0) 
		{
			dy = -dy; 
			stepy = -1;
		}
		
		int stepx = 1;
		if (dx < 0) 
		{
			dx = -dx; 
			stepx = -1;
		}
		
		dy <<= 1;
		dx <<= 1;
	 
		if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
			a_Texture.SetPixel(x1, y1, a_Color);
//			for(int x = -1; x <= 1; x++)
//				for(int y = -1; y <= 1; y++)
//					a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
		
		if (dx > dy) 
		{
			int fraction = dy - (dx >> 1);
			
			while (x1 != x2) 
			{
				if (fraction >= 0) 
				{
					y1 += stepy;
					fraction -= dx;
				}
				
				x1 += stepx;
				fraction += dy;
				
				if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
					a_Texture.SetPixel(x1, y1, a_Color);
//					for(int x = -1; x <= 1; x++)
//						for(int y = -1; y <= 1; y++)
//							a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
			}
		}
		else 
		{
			int fraction = dx - (dy >> 1);
			
			while (y1 != y2) 
			{
				if (fraction >= 0) 
				{
					x1 += stepx;
					fraction -= dy;
				}
				
				y1 += stepy;
				fraction += dx;
				
				if(x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
					a_Texture.SetPixel(x1, y1, a_Color);
//					for(int x = -1; x <= 1; x++)
//						for(int y = -1; y <= 1; y++)
//							a_Texture.SetPixel(x1 + x, y1 + y, a_Color);
			}
		}
		
	}
	
	
}
#endif