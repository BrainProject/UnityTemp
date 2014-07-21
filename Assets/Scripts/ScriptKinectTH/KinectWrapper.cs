using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.IO;
using System.Text; 

// Wrapper class that holds the various structs, vand dll imports
// needed to set up a model with the Kinect.
public class KinectWrapper
{
	// Kinect-given Variables to keep track of the skeleton's joints.
	public enum SkeletonJoint
	{ 
		//NONE = 0,
		HEAD = NuiSkeletonPositionIndex.Head,
        NECK = NuiSkeletonPositionIndex.ShoulderCenter, 
        SPINE = NuiSkeletonPositionIndex.Spine,  // TORSO_CENTER
		HIPS = NuiSkeletonPositionIndex.HipCenter,  // WAIST

		LEFT_COLLAR = -1,
		LEFT_SHOULDER = NuiSkeletonPositionIndex.ShoulderLeft,
        LEFT_ELBOW = NuiSkeletonPositionIndex.ElbowLeft,
        LEFT_WRIST = NuiSkeletonPositionIndex.WristLeft,
        LEFT_HAND = NuiSkeletonPositionIndex.HandLeft,
        LEFT_FINGERTIP = -1,

        RIGHT_COLLAR = -1,
		RIGHT_SHOULDER = NuiSkeletonPositionIndex.ShoulderRight,
		RIGHT_ELBOW = NuiSkeletonPositionIndex.ElbowRight,
		RIGHT_WRIST = NuiSkeletonPositionIndex.WristRight,
		RIGHT_HAND = NuiSkeletonPositionIndex.HandRight,
        RIGHT_FINGERTIP = -1,

        LEFT_HIP = NuiSkeletonPositionIndex.HipLeft,
        LEFT_KNEE = NuiSkeletonPositionIndex.KneeLeft,
        LEFT_ANKLE = NuiSkeletonPositionIndex.AnkleLeft,
        LEFT_FOOT = NuiSkeletonPositionIndex.FootLeft,

        RIGHT_HIP = NuiSkeletonPositionIndex.HipRight,
		RIGHT_KNEE = NuiSkeletonPositionIndex.KneeRight,
        RIGHT_ANKLE = NuiSkeletonPositionIndex.AnkleRight,
		RIGHT_FOOT = NuiSkeletonPositionIndex.FootRight,
		
		END 
	};
	
	public static class Constants
	{
		public const int NuiSkeletonCount = 6;
    	public const int NuiSkeletonMaxTracked = 2;
    	public const int NuiSkeletonInvalidTrackingID = 0;
		
		public const float NuiDepthHorizontalFOV = 58.5f;
		public const float NuiDepthVerticalFOV = 45.6f;
		
		public const int ImageWidth = 640;
		public const int ImageHeight = 480;
		public const NuiImageResolution ImageResolution = NuiImageResolution.resolution640x480;
		
		public const float PoseCompleteDuration = 1.0f;
		public const float ClickStayDuration = 2.5f;
	}
	
	public enum Gestures
	{
		None = 0,
		RaiseRightHand,
		RaiseLeftHand,
		Psi,
		Stop,
		Wave,
		Click,
		SweepLeft,
		SweepRight,
		SweepUp,
		SweepDown,
		RightHandCursor,
		LeftHandCursor,
		ZoomOut,
		ZoomIn,
		Wheel
	}
	
	public struct GestureData
	{
		public uint userId;
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
	}
	
	/// <summary>
	///Structs and constants for interfacing C# with the Kinect.dll 
	/// </summary>

    [Flags]
    public enum NuiInitializeFlags : uint
    {
		UsesAudio = 0x10000000,
        UsesDepthAndPlayerIndex = 0x00000001,
        UsesColor = 0x00000002,
        UsesSkeleton = 0x00000008,
        UsesDepth = 0x00000020,
		UsesHighQualityColor = 0x00000040
    }
	
	public enum NuiErrorCodes : uint
	{
		FrameNoData = 0x83010001,
		StreamNotEnabled = 0x83010002,
		ImageStreamInUse = 0x83010003,
		FrameLimitExceeded = 0x83010004,
		FeatureNotInitialized = 0x83010005,
		DeviceNotGenuine = 0x83010006,
		InsufficientBandwidth = 0x83010007,
		DeviceNotSupported = 0x83010008,
		DeviceInUse = 0x83010009,
		
		DatabaseNotFound = 0x8301000D,
		DatabaseVersionMismatch = 0x8301000E,
		HardwareFeatureUnavailable = 0x8301000F,
		
		DeviceNotConnected = 0x83010014,
		DeviceNotReady = 0x83010015,
		SkeletalEngineBusy = 0x830100AA,
		DeviceNotPowered = 0x8301027F,
	}

    public enum NuiSkeletonPositionIndex : int
    {
        HipCenter = 0,
        Spine,
        ShoulderCenter,
        Head,
        ShoulderLeft,
        ElbowLeft,
        WristLeft,
        HandLeft,
        ShoulderRight,
        ElbowRight,
        WristRight,
        HandRight,
        HipLeft,
        KneeLeft,
        AnkleLeft,
        FootLeft,
        HipRight,
        KneeRight,
        AnkleRight,
        FootRight,
        Count
    }

    public enum NuiSkeletonPositionTrackingState
    {
        NotTracked = 0,
        Inferred,
        Tracked
    }

    public enum NuiSkeletonTrackingState
    {
        NotTracked = 0,
        PositionOnly,
        SkeletonTracked
    }
	
	public enum NuiImageType
	{
		DepthAndPlayerIndex = 0,	// USHORT
		Color,						// RGB32 data
		ColorYUV,					// YUY2 stream from camera h/w, but converted to RGB32 before user getting it.
		ColorRawYUV,				// YUY2 stream from camera h/w.
		Depth						// USHORT
	}
	
	public enum NuiImageResolution
	{
		resolutionInvalid = -1,
		resolution80x60 = 0,
		resolution320x240,
		resolution640x480,
		resolution1280x960                        // for hires color only
	}
	
	public enum NuiImageStreamFlags
	{
		None = 0x00000000,
		SupressNoFrameData = 0x0001000,
		EnableNearMode = 0x00020000,
		TooFarIsNonZero = 0x0004000
	}
	
    [Flags]
    public enum FrameEdges
    {
        None = 0,
        Right = 1,
        Left = 2,
        Top = 4,
        Bottom = 8
    }

	public struct NuiSkeletonData
    {
        public NuiSkeletonTrackingState eTrackingState;
        public uint dwTrackingID;
        public uint dwEnrollmentIndex_NotUsed;
        public uint dwUserIndex;
        public Vector4 Position;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
        public Vector4[] SkeletonPositions;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 20, ArraySubType = UnmanagedType.Struct)]
        public NuiSkeletonPositionTrackingState[] eSkeletonPositionTrackingState;
        public uint dwQualityFlags;
    }
	
    public struct NuiSkeletonFrame
    {
        public Int64 liTimeStamp;
        public uint dwFrameNumber;
        public uint dwFlags;
        public Vector4 vFloorClipPlane;
        public Vector4 vNormalToGravity;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
        public NuiSkeletonData[] SkeletonData;
    }
	
	public struct NuiTransformSmoothParameters
	{
		public float fSmoothing;
		public float fCorrection;
		public float fPrediction;
		public float fJitterRadius;
		public float fMaxDeviationRadius;
	}
	
    public struct NuiSkeletonBoneRotation
    {
        public Matrix4x4 rotationMatrix;
        public Quaternion rotationQuaternion;
    }

    public struct NuiSkeletonBoneOrientation
    {
        public NuiSkeletonPositionIndex endJoint;
        public NuiSkeletonPositionIndex startJoint;
        public NuiSkeletonBoneRotation hierarchicalRotation;
        public NuiSkeletonBoneRotation absoluteRotation;
    }
	
	public struct NuiImageViewArea
	{
	    public int eDigitalZoom;
	    public int lCenterX;
	    public int lCenterY;
	}
	
	public class NuiImageBuffer
	{
		public int m_Width;
		public int m_Height;
		public int m_BytesPerPixel;
		public IntPtr m_pBuffer;
	}
	
	public struct NuiImageFrame
	{
		public Int64 liTimeStamp;
		public uint dwFrameNumber;
		public NuiImageType eImageType;
		public NuiImageResolution eResolution;
		//[MarshalAsAttribute(UnmanagedType.Interface)]
		public IntPtr pFrameTexture;
		public uint dwFrameFlags_NotUsed;
		public NuiImageViewArea ViewArea_NotUsed;
	}
	
	public struct ColorCust
	{
		public byte b;
		public byte g;
		public byte r;
		public byte a;
	}
	
	public struct NuiLockedRect
	{
		public int pitch;
		public int size;
		//[MarshalAsAttribute(UnmanagedType.U8)] 
		public IntPtr pBits; 
		
	}
	
	public struct ColorBuffer
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 640 * 480, ArraySubType = UnmanagedType.Struct)]
		public ColorCust[] pixels;
	}
	
	public struct DepthBuffer
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 640 * 480, ArraySubType = UnmanagedType.I2)]
		public short[] pixels;
	}
	
	public struct NuiSurfaceDesc
	{
		uint width;
		uint height;
	}
	
	[Guid("13ea17f5-ff2e-4670-9ee5-1297a6e880d1")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport()]
	public interface INuiFrameTexture
	{
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		[PreserveSig]
		int BufferLen();
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		[PreserveSig]
		int Pitch();
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		[PreserveSig]
		int LockRect(uint Level,ref NuiLockedRect pLockedRect,IntPtr pRect, uint Flags);
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		[PreserveSig]
		int GetLevelDesc(uint Level, ref NuiSurfaceDesc pDesc);
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		[PreserveSig]
		int UnlockRect(uint Level);
	}
	
	/* 
	 * kinect NUI (general) functions
	 */
    [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiInitialize")]
    public static extern int NuiInitialize(NuiInitializeFlags dwFlags);
	
	[DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiShutdown")]
    public static extern void NuiShutdown();
	
	[DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiCameraElevationSetAngle")]
	public static extern int NuiCameraElevationSetAngle(int angle);
	
	[DllImport("Kinect10.dll", EntryPoint = "NuiCameraElevationGetAngle")]
    public static extern int NuiCameraElevationGetAngle(out int plAngleDegrees);
	
	[DllImport("Kinect10.dll", EntryPoint = "NuiImageGetColorPixelCoordinatesFromDepthPixelAtResolution")]
    public static extern int NuiImageGetColorPixelCoordinatesFromDepthPixelAtResolution(NuiImageResolution eColorResolution, NuiImageResolution eDepthResolution, ref NuiImageViewArea pcViewArea, int lDepthX, int lDepthY, short sDepthValue, out int plColorX, out int plColorY);
	
    [DllImport("Kinect10.dll", EntryPoint = "NuiGetSensorCount")]
    public static extern int NuiGetSensorCount(out int pCount);
	
    [DllImport("Kinect10.dll", EntryPoint = "NuiCreateSensorByIndex")]
    public static extern int NuiCreateSensorByIndex(int index, ref IntPtr pNuiSensor);
	
	/*
	 * kinect skeleton functions
	 */
	[DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiSkeletonTrackingEnable")]
    public static extern int NuiSkeletonTrackingEnable(IntPtr hNextFrameEvent, uint dwFlags);
	
    [DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiSkeletonGetNextFrame")]
    public static extern int NuiSkeletonGetNextFrame(uint dwMillisecondsToWait, ref NuiSkeletonFrame pSkeletonFrame);
	
	[DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiTransformSmooth")]
    public static extern int NuiTransformSmooth(ref NuiSkeletonFrame pSkeletonFrame, ref NuiTransformSmoothParameters pSmoothingParams);
	
    [DllImport(@"Kinect10.dll", EntryPoint = "NuiSkeletonCalculateBoneOrientations")]
    public static extern int NuiSkeletonCalculateBoneOrientations(ref NuiSkeletonData pSkeletonData, NuiSkeletonBoneOrientation[] pBoneOrientations);

	/*
	 * kinect video functions
	 */
	[DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageStreamOpen")]
    public static extern int NuiImageStreamOpen(NuiImageType eImageType, NuiImageResolution eResolution, uint dwImageFrameFlags_NotUsed, uint dwFrameLimit, IntPtr hNextFrameEvent, ref IntPtr phStreamHandle);
	
	[DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageStreamGetNextFrame")]
    public static extern int NuiImageStreamGetNextFrame(IntPtr phStreamHandle, uint dwMillisecondsToWait, ref IntPtr ppcImageFrame);
	
	[DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageStreamReleaseFrame")]
    public static extern int NuiImageStreamReleaseFrame(IntPtr phStreamHandle, IntPtr ppcImageFrame);
	
	[DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageResolutionToSize")]
    public static extern int NuiImageResolutionToSize(NuiImageResolution eResolution,out uint frameWidth,out uint frameHeight);
	
	[DllImportAttribute(@"Kinect10.dll", EntryPoint = "NuiImageStreamSetImageFrameFlags")]
	public static extern int NuiImageStreamSetImageFrameFlags (IntPtr phStreamHandle, NuiImageStreamFlags dvImageFrameFlags);
	
	
	public static string GetNuiErrorString(int hr)
	{
		string message = string.Empty;
		uint uhr = (uint)hr;
		
		switch(uhr)
		{
			case (uint)NuiErrorCodes.FrameNoData:
				message = "Frame contains no data.";
				break;
			case (uint)NuiErrorCodes.StreamNotEnabled:
				message = "Stream is not enabled.";
				break;
			case (uint)NuiErrorCodes.ImageStreamInUse:
				message = "Image stream is already in use.";
				break;
			case (uint)NuiErrorCodes.FrameLimitExceeded:
				message = "Frame limit is exceeded.";
				break;
			case (uint)NuiErrorCodes.FeatureNotInitialized:
				message = "Feature is not initialized.";
				break;
			case (uint)NuiErrorCodes.DeviceNotGenuine:
				message = "Device is not genuine.";
				break;
			case (uint)NuiErrorCodes.InsufficientBandwidth:
				message = "Bandwidth is not sufficient.";
				break;
			case (uint)NuiErrorCodes.DeviceNotSupported:
				message = "Device is not supported (e.g. Kinect for XBox 360).";
				break;
			case (uint)NuiErrorCodes.DeviceInUse:
				message = "Device is already in use.";
				break;
			case (uint)NuiErrorCodes.DatabaseNotFound:
				message = "Database not found.";
				break;
			case (uint)NuiErrorCodes.DatabaseVersionMismatch:
				message = "Database version mismatch.";
				break;
			case (uint)NuiErrorCodes.HardwareFeatureUnavailable:
				message = "Hardware feature is not available.";
				break;
			case (uint)NuiErrorCodes.DeviceNotConnected:
				message = "Device is not connected.";
				break;
			case (uint)NuiErrorCodes.DeviceNotReady:
				message = "Device is not ready.";
				break;
			case (uint)NuiErrorCodes.SkeletalEngineBusy:
				message = "Skeletal engine is busy.";
				break;
			case (uint)NuiErrorCodes.DeviceNotPowered:
				message = "Device is not powered.";
				break;
				
			default:
				message = "hr=0x" + uhr.ToString("X");
				break;
		}
		
		return message;
	}
	
	public static int GetDepthWidth()
	{
		return Constants.ImageWidth;
	}
	
	public static int GetDepthHeight()
	{
		return Constants.ImageHeight;
	}
	
    public static Vector3 MapSkeletonPointToDepthPoint(Vector3 skeletonPoint)
    {
        float fDepthX;
        float fDepthY;
        float fDepthZ;

		NuiTransformSkeletonToDepthImage(skeletonPoint, out fDepthX, out fDepthY, out fDepthZ);
        
		Vector3 point = new Vector3();
        point.x = (int) ((fDepthX * Constants.ImageWidth) + 0.5f);
        point.y = (int) ((fDepthY * Constants.ImageHeight) + 0.5f);
        point.z = (int) (fDepthZ + 0.5f);

		return point;
    }

    public static Vector3 MapSkeletonPointToColorPoint(Vector3 skeletonPoint)
    {
        float fDepthX;
        float fDepthY;
        float fDepthZ;

		NuiTransformSkeletonToDepthImage(skeletonPoint, out fDepthX, out fDepthY, out fDepthZ);
        
		Vector3 point = new Vector3();
        point.x = (int) ((fDepthX * Constants.ImageWidth) + 0.5f);
        point.y = (int) ((fDepthY * Constants.ImageHeight) + 0.5f);
        point.z = (int) (fDepthZ + 0.5f);

		return point;
    }

    private static void NuiTransformSkeletonToDepthImage(Vector3 vPoint, out float pfDepthX, out float pfDepthY, out float pfDepthZ)
    {
        if (vPoint.z > float.Epsilon)
        {
            pfDepthX = 0.5f + ((vPoint.x * 285.63f) / (vPoint.z * 320f));
            pfDepthY = 0.5f - ((vPoint.y * 285.63f) / (vPoint.z * 240f));
            pfDepthZ = vPoint.z * 1000f;
        }
        else
        {
            pfDepthX = 0f;
            pfDepthY = 0f;
            pfDepthZ = 0f;
        }
    }

	public static int GetSkeletonJointParent(int jointIndex)
	{
		switch(jointIndex)
		{
			case (int)NuiSkeletonPositionIndex.HipCenter:
				return (int)NuiSkeletonPositionIndex.HipCenter;
			case (int)NuiSkeletonPositionIndex.Spine:
				return (int)NuiSkeletonPositionIndex.HipCenter;
			case (int)NuiSkeletonPositionIndex.ShoulderCenter:
				return (int)NuiSkeletonPositionIndex.Spine;
			case (int)NuiSkeletonPositionIndex.Head:
				return (int)NuiSkeletonPositionIndex.ShoulderCenter;
			case (int)NuiSkeletonPositionIndex.ShoulderLeft:
				return (int)NuiSkeletonPositionIndex.ShoulderCenter;
			case (int)NuiSkeletonPositionIndex.ElbowLeft:
				return (int)NuiSkeletonPositionIndex.ShoulderLeft;
			case (int)NuiSkeletonPositionIndex.WristLeft:
				return (int)NuiSkeletonPositionIndex.ElbowLeft;
			case (int)NuiSkeletonPositionIndex.HandLeft:
				return (int)NuiSkeletonPositionIndex.WristLeft;
			case (int)NuiSkeletonPositionIndex.ShoulderRight:
				return (int)NuiSkeletonPositionIndex.ShoulderCenter;
			case (int)NuiSkeletonPositionIndex.ElbowRight:
				return (int)NuiSkeletonPositionIndex.ShoulderRight;
			case (int)NuiSkeletonPositionIndex.WristRight:
				return (int)NuiSkeletonPositionIndex.ElbowRight;
			case (int)NuiSkeletonPositionIndex.HandRight:
				return (int)NuiSkeletonPositionIndex.WristRight;
			case (int)NuiSkeletonPositionIndex.HipLeft:
				return (int)NuiSkeletonPositionIndex.HipCenter;
			case (int)NuiSkeletonPositionIndex.KneeLeft:
				return (int)NuiSkeletonPositionIndex.HipLeft;
			case (int)NuiSkeletonPositionIndex.AnkleLeft:
				return (int)NuiSkeletonPositionIndex.KneeLeft;
			case (int)NuiSkeletonPositionIndex.FootLeft:
				return (int)NuiSkeletonPositionIndex.AnkleLeft;
			case (int)NuiSkeletonPositionIndex.HipRight:
				return (int)NuiSkeletonPositionIndex.HipCenter;
			case (int)NuiSkeletonPositionIndex.KneeRight:
				return (int)NuiSkeletonPositionIndex.HipRight;
			case (int)NuiSkeletonPositionIndex.AnkleRight:
				return (int)NuiSkeletonPositionIndex.KneeRight;
			case (int)NuiSkeletonPositionIndex.FootRight:
				return (int)NuiSkeletonPositionIndex.AnkleRight;
		}
		
		return (int)NuiSkeletonPositionIndex.HipCenter;
	}

	public static int GetSkeletonMirroredJoint(int jointIndex)
	{
		switch(jointIndex)
		{
			case (int)NuiSkeletonPositionIndex.ShoulderLeft:
				return (int)NuiSkeletonPositionIndex.ShoulderRight;
			case (int)NuiSkeletonPositionIndex.ElbowLeft:
				return (int)NuiSkeletonPositionIndex.ElbowRight;
			case (int)NuiSkeletonPositionIndex.WristLeft:
				return (int)NuiSkeletonPositionIndex.WristRight;
			case (int)NuiSkeletonPositionIndex.HandLeft:
				return (int)NuiSkeletonPositionIndex.HandRight;
			case (int)NuiSkeletonPositionIndex.ShoulderRight:
				return (int)NuiSkeletonPositionIndex.ShoulderLeft;
			case (int)NuiSkeletonPositionIndex.ElbowRight:
				return (int)NuiSkeletonPositionIndex.ElbowLeft;
			case (int)NuiSkeletonPositionIndex.WristRight:
				return (int)NuiSkeletonPositionIndex.WristLeft;
			case (int)NuiSkeletonPositionIndex.HandRight:
				return (int)NuiSkeletonPositionIndex.HandLeft;
			case (int)NuiSkeletonPositionIndex.HipLeft:
				return (int)NuiSkeletonPositionIndex.HipRight;
			case (int)NuiSkeletonPositionIndex.KneeLeft:
				return (int)NuiSkeletonPositionIndex.KneeRight;
			case (int)NuiSkeletonPositionIndex.AnkleLeft:
				return (int)NuiSkeletonPositionIndex.AnkleRight;
			case (int)NuiSkeletonPositionIndex.FootLeft:
				return (int)NuiSkeletonPositionIndex.FootRight;
			case (int)NuiSkeletonPositionIndex.HipRight:
				return (int)NuiSkeletonPositionIndex.HipLeft;
			case (int)NuiSkeletonPositionIndex.KneeRight:
				return (int)NuiSkeletonPositionIndex.KneeLeft;
			case (int)NuiSkeletonPositionIndex.AnkleRight:
				return (int)NuiSkeletonPositionIndex.AnkleLeft;
			case (int)NuiSkeletonPositionIndex.FootRight:
				return (int)NuiSkeletonPositionIndex.FootLeft;
		}
		
		return jointIndex;
	}

	public static bool PollSkeleton(ref NuiTransformSmoothParameters smoothParameters, ref NuiSkeletonFrame skeletonFrame)
	{
		bool newSkeleton = false;
		
		int hr = KinectWrapper.NuiSkeletonGetNextFrame(0, ref skeletonFrame);
		if(hr == 0)
		{
			newSkeleton = true;
		}
		
		if(newSkeleton)
		{
			hr = KinectWrapper.NuiTransformSmooth(ref skeletonFrame, ref smoothParameters);
			if(hr != 0)
			{
				Debug.Log("Skeleton Data Smoothing failed");
			}
		}
		
		return newSkeleton;
	}
	
	public static bool PollColor(IntPtr colorStreamHandle, ref Color32[] colorImage)
	{
		IntPtr imageFramePtr = IntPtr.Zero;
		bool newColor = false;
	
		int hr = KinectWrapper.NuiImageStreamGetNextFrame(colorStreamHandle, 0, ref imageFramePtr);
		if (hr == 0)
		{
			newColor = true;
			
			NuiImageFrame imageFrame = (NuiImageFrame)Marshal.PtrToStructure(imageFramePtr, typeof(NuiImageFrame));
			INuiFrameTexture frameTexture = (INuiFrameTexture)Marshal.GetObjectForIUnknown(imageFrame.pFrameTexture);
			
			NuiLockedRect lockedRectPtr = new NuiLockedRect();
			IntPtr r = IntPtr.Zero;
			
			frameTexture.LockRect(0, ref lockedRectPtr, r, 0);
			ExtractColorImage(lockedRectPtr, ref colorImage);
			
			frameTexture.UnlockRect(0);
			hr = KinectWrapper.NuiImageStreamReleaseFrame(colorStreamHandle, imageFramePtr);
		}
		
		return newColor;
	}
	
	public static bool PollDepth(IntPtr depthStreamHandle, bool isNearMode, ref short[] depthPlayerData)
	{
		IntPtr imageFramePtr = IntPtr.Zero;
		bool newDepth = false;

		if (isNearMode)
		{
			KinectWrapper.NuiImageStreamSetImageFrameFlags(depthStreamHandle, NuiImageStreamFlags.EnableNearMode);
		}
		else
		{
			KinectWrapper.NuiImageStreamSetImageFrameFlags(depthStreamHandle, NuiImageStreamFlags.None);
		}
		
		int hr = KinectWrapper.NuiImageStreamGetNextFrame(depthStreamHandle, 0, ref imageFramePtr);
		if (hr == 0)
		{
			newDepth = true;
			
			NuiImageFrame imageFrame = (NuiImageFrame)Marshal.PtrToStructure(imageFramePtr, typeof(NuiImageFrame));
			INuiFrameTexture frameTexture = (INuiFrameTexture)Marshal.GetObjectForIUnknown(imageFrame.pFrameTexture);
			
			NuiLockedRect lockedRectPtr = new NuiLockedRect();
			IntPtr r = IntPtr.Zero;
			
			frameTexture.LockRect(0,ref lockedRectPtr,r,0);
			depthPlayerData = ExtractDepthImage(lockedRectPtr);
			
			frameTexture.UnlockRect(0);
			hr = KinectWrapper.NuiImageStreamReleaseFrame(depthStreamHandle, imageFramePtr);
		}
		
		return newDepth;
	}
	
	private static void ExtractColorImage(NuiLockedRect buf, ref Color32[] colorImage)
	{
		ColorBuffer cb = (ColorBuffer)Marshal.PtrToStructure(buf.pBits, typeof(ColorBuffer));
		int totalPixels = Constants.ImageWidth * Constants.ImageHeight;
		
		for (int pix = 0; pix < totalPixels; pix++)
		{
			int ind = totalPixels - pix - 1;
			
			colorImage[ind].r = cb.pixels[pix].r;
			colorImage[ind].g = cb.pixels[pix].g;
			colorImage[ind].b = cb.pixels[pix].b;
			colorImage[ind].a = 255;
		}
	}
	
	private static short[] ExtractDepthImage(NuiLockedRect lockedRect)
	{
		DepthBuffer db = (DepthBuffer)Marshal.PtrToStructure(lockedRect.pBits, typeof(DepthBuffer));
		return db.pixels;
	}
	
    private static Vector3 GetPositionBetweenIndices(ref Vector3[] jointsPos, NuiSkeletonPositionIndex p1, NuiSkeletonPositionIndex p2) 
	{
		Vector3 pVec1 = jointsPos[(int)p1];
		Vector3 pVec2 = jointsPos[(int)p2];
		
        return pVec2 - pVec1;
    }
           
    //populate matrix using the columns
    private static void PopulateMatrix(ref Matrix4x4 jointOrientation, Vector3 xCol, Vector3 yCol, Vector3 zCol) 
	{
    	jointOrientation.SetColumn(0, xCol);
    	jointOrientation.SetColumn(1, yCol);
    	jointOrientation.SetColumn(2, zCol);
    }

    //constructs an orientation from a vector that specifies the x axis
    private static void MakeMatrixFromX(Vector3 v1, ref Matrix4x4 jointOrientation, bool flip) 
	{
        //matrix columns
        Vector3 xCol;
        Vector3 yCol;
        Vector3 zCol;

        //set first column to the vector between the previous joint and the current one, this sets the two degrees of freedom
        xCol = v1.normalized;

        //set second column to an arbitrary vector perpendicular to the first column
        yCol.x = 0.0f;
        yCol.y = !flip ? xCol.z : -xCol.z;
        yCol.z = !flip ? -xCol.y : xCol.y;
        yCol.Normalize();

        //third column is fully determined by the first two, and it must be their cross product
        zCol = Vector3.Cross(xCol, yCol);

        //copy values into matrix
        PopulateMatrix(ref jointOrientation, xCol, yCol, zCol);
    }

    //constructs an orientation from a vector that specifies the y axis
    private static void MakeMatrixFromY(Vector3 v1, ref Matrix4x4 jointOrientation) 
	{
        //matrix columns
        Vector3 xCol;
        Vector3 yCol;
        Vector3 zCol;

        //set first column to the vector between the previous joint and the current one, this sets the two degrees of freedom
        yCol = v1.normalized;

        //set second column to an arbitrary vector perpendicular to the first column
        xCol.x = yCol.y;
        xCol.y = -yCol.x;
        xCol.z = 0.0f;
        xCol.Normalize();

        //third column is fully determined by the first two, and it must be their cross product
        zCol = Vector3.Cross(xCol, yCol);

        //copy values into matrix
        PopulateMatrix(ref jointOrientation, xCol, yCol, zCol);
    }
   
    //constructs an orientation from a vector that specifies the x axis
    private static void MakeMatrixFromZ(Vector3 v1, ref Matrix4x4 jointOrientation) 
	{
        //matrix columns
        Vector3 xCol;
        Vector3 yCol;
        Vector3 zCol;

         //set first column to the vector between the previous joint and the current one, this sets the two degrees of freedom
        zCol = v1.normalized;

        //set second column to an arbitrary vector perpendicular to the first column
        xCol.x = zCol.y;
        xCol.y = -zCol.x;
        xCol.z = 0.0f;
        xCol.Normalize();

        //third column is fully determined by the first two, and it must be their cross product
        yCol = Vector3.Cross(zCol, xCol);

        //copy values into matrix
        PopulateMatrix(ref jointOrientation, xCol, yCol, zCol);
    }

    //constructs an orientation from 2 vectors: the first specifies the x axis, and the next specifies the y axis
    //uses the first vector as x axis, then constructs the other axes using cross products
    private static void MakeMatrixFromXY(Vector3 xUnnormalized, Vector3 yUnnormalized, ref Matrix4x4 jointOrientation) 
	{
        //matrix columns
        Vector3 xCol;
        Vector3 yCol;
        Vector3 zCol;

        //set up the three different columns to be rearranged and flipped
        xCol = xUnnormalized.normalized;
        zCol = Vector3.Cross(xCol, yUnnormalized.normalized).normalized;
        yCol = Vector3.Cross(zCol, xCol);
		//yCol = yUnnormalized.normalized;
		//zCol = Vector3.Cross(xCol, yCol).normalized;

        //copy values into matrix
        PopulateMatrix(ref jointOrientation, xCol, yCol, zCol);
    }
   
    //constructs an orientation from 2 vectors: the first specifies the x axis, and the next specifies the y axis
    //uses the second vector as y axis, then constructs the other axes using cross products
    private static void MakeMatrixFromYX(Vector3 xUnnormalized, Vector3 yUnnormalized, ref Matrix4x4 jointOrientation) 
	{
        //matrix columns
        Vector3 xCol;
        Vector3 yCol;
        Vector3 zCol;

        //set up the three different columns to be rearranged and flipped
        yCol = yUnnormalized.normalized;
        zCol = Vector3.Cross(xUnnormalized.normalized, yCol).normalized;
        xCol = Vector3.Cross(yCol, zCol);
		//xCol = xUnnormalized.normalized;
		//zCol = Vector3.Cross(xCol, yCol).normalized;

        //copy values into matrix
        PopulateMatrix(ref jointOrientation, xCol, yCol, zCol);
    }
   
    //constructs an orientation from 2 vectors: the first specifies the x axis, and the next specifies the y axis
    //uses the second vector as y axis, then constructs the other axes using cross products
    private static void MakeMatrixFromYZ(Vector3 yUnnormalized, Vector3 zUnnormalized, ref Matrix4x4 jointOrientation) 
	{
        //matrix columns
        Vector3 xCol;
        Vector3 yCol;
        Vector3 zCol;

        //set up the three different columns to be rearranged and flipped
        yCol = yUnnormalized.normalized;
        xCol = Vector3.Cross(yCol, zUnnormalized.normalized).normalized;
        zCol = Vector3.Cross(xCol, yCol);
		//zCol = zUnnormalized.normalized;
		//xCol = Vector3.Cross(yCol, zCol).normalized;

        //copy values into matrix
        PopulateMatrix(ref jointOrientation, xCol, yCol, zCol);
    }
	
	// calculate the joint orientations, based on joint positions and their tracked state
    public static void GetSkeletonJointOrientation(ref Vector3[] jointsPos, ref bool[] jointsTracked, ref Matrix4x4 [] jointOrients)
    {
        Vector3 vx;
        Vector3 vy;
        Vector3 vz;

	    // NUI_SKELETON_POSITION_HIP_CENTER
		if(jointsTracked[(int)NuiSkeletonPositionIndex.HipCenter] && jointsTracked[(int)NuiSkeletonPositionIndex.Spine] &&
			jointsTracked[(int)NuiSkeletonPositionIndex.HipLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.HipRight])
		{
			vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.HipCenter, NuiSkeletonPositionIndex.Spine);
	        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.HipLeft, NuiSkeletonPositionIndex.HipRight);
	        MakeMatrixFromYX(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.HipCenter]);
			
			// make a correction of about 40 degrees back to the front
			Matrix4x4 mat = jointOrients[(int)NuiSkeletonPositionIndex.HipCenter];
			Quaternion quat = Quaternion.LookRotation(mat.GetColumn(2), mat.GetColumn(1));
			quat *= Quaternion.Euler(-40, 0, 0);
			jointOrients[(int)NuiSkeletonPositionIndex.HipCenter].SetTRS(Vector3.zero, quat, Vector3.one);
		}
       
		if(jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderRight])
		{
		    // NUI_SKELETON_POSITION_SPINE
			if(jointsTracked[(int)NuiSkeletonPositionIndex.Spine] && jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderCenter])
			{
		        vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.Spine, NuiSkeletonPositionIndex.ShoulderCenter);
		        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ShoulderLeft, NuiSkeletonPositionIndex.ShoulderRight);
		        MakeMatrixFromYX(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.Spine]);
			}
	       
			if(jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderCenter] && jointsTracked[(int)NuiSkeletonPositionIndex.Head])
			{
			    // NUI_SKELETON_POSITION_SHOULDER_CENTER
				//if(jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderCenter] && jointsTracked[(int)NuiSkeletonPositionIndex.Head])
				{
			        vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ShoulderCenter, NuiSkeletonPositionIndex.Head);
			        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ShoulderLeft, NuiSkeletonPositionIndex.ShoulderRight);
			        MakeMatrixFromYX(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.ShoulderCenter]);
				}
		
			    // NUI_SKELETON_POSITION_HEAD
				//if(jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderCenter] && jointsTracked[(int)NuiSkeletonPositionIndex.Head])
				{
			        vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ShoulderCenter, NuiSkeletonPositionIndex.Head);
			        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ShoulderLeft, NuiSkeletonPositionIndex.ShoulderRight);
			        MakeMatrixFromYX(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.Head]);
			        //MakeMatrixFromY(vy, ref jointOrients[(int)NuiSkeletonPositionIndex.Head]);
				}
			}
		}
       
		if(jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.ElbowLeft] &&
			//jointsTracked[(int)NuiSkeletonPositionIndex.WristLeft])
			jointsTracked[(int)NuiSkeletonPositionIndex.Spine] && jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderCenter])
		{
		    // NUI_SKELETON_POSITION_SHOULDER_LEFT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.ElbowLeft])
			{
		        vx = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ShoulderLeft, NuiSkeletonPositionIndex.ElbowLeft);
		        //vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ElbowLeft, NuiSkeletonPositionIndex.WristLeft);
				vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.Spine, NuiSkeletonPositionIndex.ShoulderCenter);
		        MakeMatrixFromXY(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.ShoulderLeft]);
			}
	       
		    // NUI_SKELETON_POSITION_ELBOW_LEFT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.ElbowLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.WristLeft])
			{
		        vx = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ElbowLeft, NuiSkeletonPositionIndex.WristLeft);
		        //vy = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ShoulderLeft, NuiSkeletonPositionIndex.ElbowLeft);
				vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.Spine, NuiSkeletonPositionIndex.ShoulderCenter);
		        MakeMatrixFromXY(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.ElbowLeft]);
			}
		}
			
       	if(jointsTracked[(int)NuiSkeletonPositionIndex.WristLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.HandLeft] &&
			jointsTracked[(int)NuiSkeletonPositionIndex.Spine] && jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderCenter])
		{
		    // NUI_SKELETON_POSITION_WRIST_LEFT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.WristLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.HandLeft])
			{
		        vx = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.WristLeft, NuiSkeletonPositionIndex.HandLeft);
		        //MakeMatrixFromX(vx, ref jointOrients[(int)NuiSkeletonPositionIndex.WristLeft], false);
				vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.Spine, NuiSkeletonPositionIndex.ShoulderCenter);
		        MakeMatrixFromXY(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.WristLeft]);
			}
			
		    // NUI_SKELETON_POSITION_HAND_LEFT:
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.WristLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.HandLeft])
			{
		        vx = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.WristLeft, NuiSkeletonPositionIndex.HandLeft);
		        //MakeMatrixFromX(vx, ref jointOrients[(int)NuiSkeletonPositionIndex.HandLeft], false);
				vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.Spine, NuiSkeletonPositionIndex.ShoulderCenter);
		        MakeMatrixFromXY(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.HandLeft]);
			}
		}
       
		if(jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderRight] && jointsTracked[(int)NuiSkeletonPositionIndex.ElbowRight] &&
			//jointsTracked[(int)NuiSkeletonPositionIndex.WristRight])
			jointsTracked[(int)NuiSkeletonPositionIndex.Spine] && jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderCenter])
		{
		    // NUI_SKELETON_POSITION_SHOULDER_RIGHT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderRight] && jointsTracked[(int)NuiSkeletonPositionIndex.ElbowRight])
			{
		        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ShoulderRight, NuiSkeletonPositionIndex.ElbowRight);
		        //vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ElbowRight, NuiSkeletonPositionIndex.WristRight);
				vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.Spine, NuiSkeletonPositionIndex.ShoulderCenter);
		        MakeMatrixFromXY(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.ShoulderRight]);
			}
	       
		    // NUI_SKELETON_POSITION_ELBOW_RIGHT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.ElbowRight] && jointsTracked[(int)NuiSkeletonPositionIndex.WristRight])
			{
		        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ElbowRight, NuiSkeletonPositionIndex.WristRight);
		        //vy = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.ShoulderRight, NuiSkeletonPositionIndex.ElbowRight);
				vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.Spine, NuiSkeletonPositionIndex.ShoulderCenter);
		        MakeMatrixFromXY(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.ElbowRight]);
			}
		}
       
		if(jointsTracked[(int)NuiSkeletonPositionIndex.WristRight] && jointsTracked[(int)NuiSkeletonPositionIndex.HandRight] &&
			jointsTracked[(int)NuiSkeletonPositionIndex.Spine] && jointsTracked[(int)NuiSkeletonPositionIndex.ShoulderCenter])
		{
		    // NUI_SKELETON_POSITION_WRIST_RIGHT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.WristRight] && jointsTracked[(int)NuiSkeletonPositionIndex.HandRight])
			{
		        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.WristRight, NuiSkeletonPositionIndex.HandRight);
		        //MakeMatrixFromX(vx, ref jointOrients[(int)NuiSkeletonPositionIndex.WristRight], true);
				vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.Spine, NuiSkeletonPositionIndex.ShoulderCenter);
		        MakeMatrixFromXY(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.WristRight]);
			}
	       
		    // NUI_SKELETON_POSITION_HAND_RIGHT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.WristRight] && jointsTracked[(int)NuiSkeletonPositionIndex.HandRight])
			{
		        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.WristRight, NuiSkeletonPositionIndex.HandRight);
		        //MakeMatrixFromX(vx, ref jointOrients[(int)NuiSkeletonPositionIndex.HandRight], true);
				vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.Spine, NuiSkeletonPositionIndex.ShoulderCenter);
		        MakeMatrixFromXY(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.HandRight]);
			}
		}
        
	    // NUI_SKELETON_POSITION_HIP_LEFT
		if(jointsTracked[(int)NuiSkeletonPositionIndex.HipLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.KneeLeft] &&
			jointsTracked[(int)NuiSkeletonPositionIndex.HipRight])
		{
	        vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.KneeLeft, NuiSkeletonPositionIndex.HipLeft);
	        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.HipLeft, NuiSkeletonPositionIndex.HipRight);
	        MakeMatrixFromYX(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.HipLeft]);
       
		    // NUI_SKELETON_POSITION_KNEE_LEFT
			if(jointsTracked[(int)NuiSkeletonPositionIndex.AnkleLeft])
			{
		        vy = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.KneeLeft, NuiSkeletonPositionIndex.AnkleLeft);
		        //vz = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.AnkleLeft, NuiSkeletonPositionIndex.FootLeft);
		        //MakeMatrixFromYZ(vy, vz, ref jointOrients[(int)NuiSkeletonPositionIndex.KneeLeft]);
		        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.HipLeft, NuiSkeletonPositionIndex.HipRight);
		        MakeMatrixFromYX(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.KneeLeft]);
			}
		}
		
		if(jointsTracked[(int)NuiSkeletonPositionIndex.KneeLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.AnkleLeft] &&
			jointsTracked[(int)NuiSkeletonPositionIndex.FootLeft])
		{
		    // NUI_SKELETON_POSITION_ANKLE_LEFT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.AnkleLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.FootLeft])
			{
		        vy = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.KneeLeft, NuiSkeletonPositionIndex.AnkleLeft);
		        vz = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.FootLeft, NuiSkeletonPositionIndex.AnkleLeft);
		        MakeMatrixFromYZ(vy, vz, ref jointOrients[(int)NuiSkeletonPositionIndex.AnkleLeft]);
		        //MakeMatrixFromZ(vz, ref jointOrients[(int)NuiSkeletonPositionIndex.AnkleLeft]);
			}
	       
		    // NUI_SKELETON_POSITION_FOOT_LEFT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.AnkleLeft] && jointsTracked[(int)NuiSkeletonPositionIndex.FootLeft])
			{
		        vy = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.KneeLeft, NuiSkeletonPositionIndex.AnkleLeft);
		        vz = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.FootLeft, NuiSkeletonPositionIndex.AnkleLeft);
		        MakeMatrixFromYZ(vy, vz, ref jointOrients[(int)NuiSkeletonPositionIndex.FootLeft]);
		        //MakeMatrixFromZ(vz, ref jointOrients[(int)NuiSkeletonPositionIndex.FootLeft]);
			}
		}
       
	    // NUI_SKELETON_POSITION_HIP_RIGHT
		if(jointsTracked[(int)NuiSkeletonPositionIndex.HipRight] && jointsTracked[(int)NuiSkeletonPositionIndex.KneeRight] &&
			jointsTracked[(int)NuiSkeletonPositionIndex.HipLeft])
		{
	        vy = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.KneeRight, NuiSkeletonPositionIndex.HipRight);
	        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.HipLeft, NuiSkeletonPositionIndex.HipRight);
	        MakeMatrixFromYX(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.HipRight]);

		    // NUI_SKELETON_POSITION_KNEE_RIGHT
			if(jointsTracked[(int)NuiSkeletonPositionIndex.AnkleRight])
			{
		        vy = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.KneeRight, NuiSkeletonPositionIndex.AnkleRight);
		        //vz = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.AnkleRight, NuiSkeletonPositionIndex.FootRight);
		        //MakeMatrixFromYZ(vy, vz, ref jointOrients[(int)NuiSkeletonPositionIndex.KneeRight]);
		        vx = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.HipLeft, NuiSkeletonPositionIndex.HipRight);
		        MakeMatrixFromYX(vx, vy, ref jointOrients[(int)NuiSkeletonPositionIndex.KneeRight]);
			}
		}
       
		if(jointsTracked[(int)NuiSkeletonPositionIndex.KneeRight] && jointsTracked[(int)NuiSkeletonPositionIndex.AnkleRight] &&
			jointsTracked[(int)NuiSkeletonPositionIndex.FootRight])
		{
		    // NUI_SKELETON_POSITION_ANKLE_RIGHT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.AnkleRight] && jointsTracked[(int)NuiSkeletonPositionIndex.FootRight])
			{
		        vy = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.KneeRight, NuiSkeletonPositionIndex.AnkleRight);
		        vz = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.FootRight, NuiSkeletonPositionIndex.AnkleRight);
		        MakeMatrixFromYZ(vy, vz, ref jointOrients[(int)NuiSkeletonPositionIndex.AnkleRight]);
		        //MakeMatrixFromZ(vz, ref jointOrients[(int)NuiSkeletonPositionIndex.AnkleRight]);
			}
	       
		    // NUI_SKELETON_POSITION_FOOT_RIGHT
			//if(jointsTracked[(int)NuiSkeletonPositionIndex.AnkleRight] && jointsTracked[(int)NuiSkeletonPositionIndex.FootRight])
			{
		        vy = -GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.KneeRight, NuiSkeletonPositionIndex.AnkleRight);
		        vz = GetPositionBetweenIndices(ref jointsPos, NuiSkeletonPositionIndex.FootRight, NuiSkeletonPositionIndex.AnkleRight);
		        MakeMatrixFromYZ(vy, vz, ref jointOrients[(int)NuiSkeletonPositionIndex.FootRight]);
		        //MakeMatrixFromZ(vz, ref jointOrients[(int)NuiSkeletonPositionIndex.FootRight]);
			}
		}
    }
	
	
	// Gesture related constants, variables and functions
	private const int leftHandIndex = (int)SkeletonJoint.LEFT_HAND;
	private const int rightHandIndex = (int)SkeletonJoint.RIGHT_HAND;
		
	private const int leftElbowIndex = (int)SkeletonJoint.LEFT_ELBOW;
	private const int rightElbowIndex = (int)SkeletonJoint.RIGHT_ELBOW;
		
	private const int leftShoulderIndex = (int)SkeletonJoint.LEFT_SHOULDER;
	private const int rightShoulderIndex = (int)SkeletonJoint.RIGHT_SHOULDER;
	
	private const int hipCenterIndex = (int)SkeletonJoint.HIPS;
	private const int shoulderCenterIndex = (int)SkeletonJoint.NECK;
	private const int leftHipIndex = (int)SkeletonJoint.LEFT_HIP;
	private const int rightHipIndex = (int)SkeletonJoint.RIGHT_HIP;
	
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
	
	private static void SetScreenPos(uint userId, ref GestureData gestureData, ref Vector3[] jointsPos, ref bool[] jointsTracked)
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
			
			if(jointsTracked[hipCenterIndex] && jointsTracked[shoulderCenterIndex] && 
				jointsTracked[leftShoulderIndex] && jointsTracked[rightShoulderIndex])
			{
				Vector3 neckToHips = jointsPos[shoulderCenterIndex] - jointsPos[hipCenterIndex];
				Vector3 rightToLeft = jointsPos[rightShoulderIndex] - jointsPos[leftShoulderIndex];
				
				gestureData.tagVector2.x = rightToLeft.x * 1.2f;
				gestureData.tagVector2.y = neckToHips.y * 1.2f;
				
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
	
	private static void SetZoomFactor(uint userId, ref GestureData gestureData, float initialZoom, ref Vector3[] jointsPos, ref bool[] jointsTracked)
	{
		Vector3 vectorZooming = jointsPos[rightHandIndex] - jointsPos[leftHandIndex];
		
		if(gestureData.tagFloat == 0f || gestureData.userId != userId)
		{
			gestureData.tagFloat = 0.5f; // this is 100%
		}

		float distZooming = vectorZooming.magnitude;
		gestureData.screenPos.z = initialZoom + (distZooming / gestureData.tagFloat);
	}
	
	private static void SetWheelRotation(uint userId, ref GestureData gestureData, Vector3 initialPos, Vector3 currentPos)
	{
		float angle = Vector3.Angle(initialPos, currentPos) * Mathf.Sign(currentPos.y - initialPos.y);
		gestureData.screenPos.z = angle;
	}
	
	// estimate the next state and completeness of the gesture
	public static void CheckForGesture(uint userId, ref GestureData gestureData, float timestamp, ref Vector3[] jointsPos, ref bool[] jointsTracked)
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
						CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, Constants.PoseCompleteDuration);
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
						CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, Constants.PoseCompleteDuration);
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
						CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, Constants.PoseCompleteDuration);
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
						CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, Constants.PoseCompleteDuration);
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
							CheckPoseComplete(ref gestureData, timestamp, jointPos, isInPose, Constants.ClickStayDuration);
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

			// check for SweepLeft
			case Gestures.SweepLeft:
				switch(gestureData.state)
				{
					case 0:  // gesture detection - phase 1
						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
					       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
					       (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) > 0f)
						{
							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
							gestureData.progress = 0.5f;
						}
//						else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
//					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f &&
//					            (jointsPos[leftHandIndex].x - jointsPos[leftElbowIndex].x) > 0f)
//						{
//							SetGestureJoint(ref gestureData, timestamp, leftHandIndex);
//							gestureData.jointPos = jointsPos[leftHandIndex];
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
								(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < -0.2f :
								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
								Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
								Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.08f && 
								(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < -0.2f;

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

			// check for SweepRight
			case Gestures.SweepRight:
				switch(gestureData.state)
				{
					case 0:  // gesture detection - phase 1
//						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
//					       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0f &&
//					       (jointsPos[rightHandIndex].x - jointsPos[rightElbowIndex].x) < 0f)
//						{
//							SetGestureJoint(ref gestureData, timestamp, rightHandIndex);
//							gestureData.jointPos = jointsPos[rightHandIndex];
//							gestureData.progress = 0.5f;
//						}
//						else 
						if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0f &&
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
								(jointsPos[rightHandIndex].x - gestureData.jointPos.x) > 0.2f :
								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
								Mathf.Abs(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < 0.1f &&
								Mathf.Abs(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < 0.08f && 
								(jointsPos[leftHandIndex].x - gestureData.jointPos.x) > 0.2f;

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

			// check for SweepUp
			case Gestures.SweepUp:
				switch(gestureData.state)
				{
					case 0:  // gesture detection - phase 1
						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
					       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) >= -0.1f)
						{
							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
							gestureData.progress = 0.5f;
						}
						else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) >= -0.1f)
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
								(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) > 0.1f && 
								(jointsPos[rightHandIndex].y - gestureData.jointPos.y) > 0.2f && 
								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f :
								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
								(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) > 0.1f &&
								(jointsPos[leftHandIndex].y - gestureData.jointPos.y) > 0.2f && 
								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f;

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

			// check for SweepDown
			case Gestures.SweepDown:
				switch(gestureData.state)
				{
					case 0:  // gesture detection - phase 1
						if(jointsTracked[rightHandIndex] && jointsTracked[rightElbowIndex] &&
					       (jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) >= 0.1f)
						{
							SetGestureJoint(ref gestureData, timestamp, rightHandIndex, jointsPos[rightHandIndex]);
							gestureData.progress = 0.5f;
						}
						else if(jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
					            (jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) >= 0.1f)
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
								(jointsPos[rightHandIndex].y - jointsPos[rightElbowIndex].y) < -0.1f && 
								(jointsPos[rightHandIndex].y - gestureData.jointPos.y) < -0.2f && 
								Mathf.Abs(jointsPos[rightHandIndex].x - gestureData.jointPos.x) < 0.08f :
								jointsTracked[leftHandIndex] && jointsTracked[leftElbowIndex] &&
								(jointsPos[leftHandIndex].y - jointsPos[leftElbowIndex].y) < -0.1f &&
								(jointsPos[leftHandIndex].y - gestureData.jointPos.y) < -0.2f && 
								Mathf.Abs(jointsPos[leftHandIndex].x - gestureData.jointPos.x) < 0.08f;

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

			// check for RightHandCursor
			case Gestures.RightHandCursor:
				switch(gestureData.state)
				{
					case 0:  // gesture detection - phase 1 (perpetual)
						if(jointsTracked[rightHandIndex] && jointsTracked[rightHipIndex] &&
							(jointsPos[rightHandIndex].y - jointsPos[rightHipIndex].y) > -0.1f)
						{
							gestureData.joint = rightHandIndex;
							gestureData.timestamp = timestamp;
							//gestureData.jointPos = jointsPos[rightHandIndex];
							SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
							gestureData.progress = 0.7f;
						}
						else
						{
							// cancel the gesture
							SetGestureCancelled(ref gestureData);
						}
						break;
				
				}
				break;

			// check for LeftHandCursor
			case Gestures.LeftHandCursor:
				switch(gestureData.state)
				{
					case 0:  // gesture detection - phase 1 (perpetual)
						if(jointsTracked[leftHandIndex] && jointsTracked[leftHipIndex] &&
							(jointsPos[leftHandIndex].y - jointsPos[leftHipIndex].y) > -0.1f)
						{
							gestureData.joint = leftHandIndex;
							gestureData.timestamp = timestamp;
							//gestureData.jointPos = jointsPos[leftHandIndex];
							SetScreenPos(userId, ref gestureData, ref jointsPos, ref jointsTracked);
							gestureData.progress = 0.7f;
						}
						else
						{
							// cancel the gesture
							SetGestureCancelled(ref gestureData);
						}
						break;
				
				}
				break;

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

		}
	}

}