// comment or uncomment the following #define directives
// depending on whether you use KinectExtras together with KinectManager

//#define USE_KINECT_MANAGER

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;


#if UNITY_STANDALONE
namespace Kinect {
	public class SpeechWrapper 
	{
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
		
	#if USE_KINECT_MANAGER
		public static int InitKinectSensor()
		{
			return 0;
		}
		
		public static void ShutdownKinectSensor()
		{
		}
	#else
		[DllImport(@"KinectUnityWrapper", EntryPoint = "InitKinectSensor")]
	    public static extern int InitKinectSensor(NuiInitializeFlags dwFlags, bool bEnableEvents);

		[DllImport(@"KinectUnityWrapper", EntryPoint = "ShutdownKinectSensor")]
	    public static extern void ShutdownKinectSensor();
		
		public static int InitKinectSensor()
		{
			int hr = InitKinectSensor(NuiInitializeFlags.UsesAudio, false);
			return hr;
		}
	#endif
		
		[DllImport("Kernel32.dll", SetLastError = true)]
		static extern uint FormatMessage( uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, ref IntPtr lpBuffer, uint nSize, IntPtr pArguments);
	 	[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr LocalFree(IntPtr hMem);
		
		// DLL Imports to pull in the necessary Unity functions to make the Kinect go.
		[DllImport("KinectUnityWrapper")]
		public static extern int InitSpeechRecognizer([MarshalAs(UnmanagedType.LPWStr)]string sRecoCriteria, bool bUseKinect, bool bAdaptationOff);
		[DllImport("KinectUnityWrapper")]
		public static extern void FinishSpeechRecognizer();
		[DllImport("KinectUnityWrapper")]
		public static extern int UpdateSpeechRecognizer();
		
		[DllImport("KinectUnityWrapper")]
		public static extern int LoadSpeechGrammar([MarshalAs(UnmanagedType.LPWStr)]string sFileName, short iNewLangCode);
		[DllImport("KinectUnityWrapper")]
		public static extern void SetRequiredConfidence(float fConfidence);

		[DllImport("KinectUnityWrapper")]
		public static extern bool IsSoundStarted();
		[DllImport("KinectUnityWrapper")]
		public static extern bool IsSoundEnded();
		[DllImport("KinectUnityWrapper")]
		public static extern bool IsPhraseRecognized();
		[DllImport("KinectUnityWrapper")]
		public static extern IntPtr GetRecognizedTag();
		[DllImport("KinectUnityWrapper")]
		public static extern void ClearPhraseRecognized();
		
	//	public delegate void SpeechStatusDelegate();
	//	public delegate void SpeechRecoDelegate([MarshalAs(UnmanagedType.LPWStr)]string sRecognizedTag);
	//	
	//	[DllImport("KinectUnityWrapper")]
	//	public static extern void SetSoundStartCallback(SpeechStatusDelegate SoundStartDelegate);
	//	[DllImport("KinectUnityWrapper")]
	//	public static extern void SetSoundEndCallback(SpeechStatusDelegate SoundEndDelegate);
	//	[DllImport("KinectUnityWrapper")]
	//	public static extern void SetSpeechRecoCallback(SpeechRecoDelegate SpeechRecognizedDelegate);
		
		
		// Returns the system error message
		public static string GetSystemErrorMessage(int hr)
		{
			string message = string.Empty;
			uint uhr = (uint)hr;
			
		    const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
		    const uint FORMAT_MESSAGE_IGNORE_INSERTS  = 0x00000200;
		    const uint FORMAT_MESSAGE_FROM_SYSTEM    = 0x00001000;
		
		    IntPtr lpMsgBuf = IntPtr.Zero;
		
		    uint dwChars = FormatMessage(
		        FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
		        IntPtr.Zero,
		        (uint)hr,
		        0, // Default language
		        ref lpMsgBuf,
		        0,
		        IntPtr.Zero);
			
		    if (dwChars > 0)
			{
			    message = Marshal.PtrToStringAnsi(lpMsgBuf).Trim();
			
			    // Free the buffer.
			    LocalFree(lpMsgBuf);
			}
			else
		    {
		        // Handle the error.
		        message = "hr=0x" + uhr.ToString("X");
		    }
		
			return message;
		}
		
		// Copy a resource file to the target
		private static bool CopyResourceFile(string targetFilePath, string resFileName, ref bool bOneCopied, ref bool bAllCopied)
		{
			TextAsset textRes = Resources.Load(resFileName, typeof(TextAsset)) as TextAsset;
			if(textRes == null)
			{
				bOneCopied = false;
				bAllCopied = false;
				
				return false;
			}
			
			FileInfo targetFile = new FileInfo(targetFilePath);
			if(!targetFile.Directory.Exists)
			{
				targetFile.Directory.Create();
			}
			
			if(!targetFile.Exists || targetFile.Length !=  textRes.bytes.Length)
			{
				if(textRes != null)
				{
					using (FileStream fileStream = new FileStream (targetFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
					{
						fileStream.Write(textRes.bytes, 0, textRes.bytes.Length);
					}
					
					bool bFileCopied = File.Exists(targetFilePath);
					
					bOneCopied = bOneCopied || bFileCopied;
					bAllCopied = bAllCopied && bFileCopied;
					
					return bFileCopied;
				}
			}
			
			return false;
		}
		
		// Copies the needed resources into the project directory
		public static bool EnsureKinectWrapperPresence()
		{
			bool bOneCopied = false, bAllCopied = true;
			
			CopyResourceFile("KinectUnityWrapper.dll", "KinectUnityWrapper.dll", ref bOneCopied, ref bAllCopied);
			CopyResourceFile("KinectInteraction180_32.dll", "KinectInteraction180_32.dll", ref bOneCopied, ref bAllCopied);
			CopyResourceFile("FaceTrackData.dll", "FaceTrackData.dll", ref bOneCopied, ref bAllCopied);
			CopyResourceFile("FaceTrackLib.dll", "FaceTrackLib.dll", ref bOneCopied, ref bAllCopied);
			
			CopyResourceFile("msvcp100d.dll", "msvcp100d.dll", ref bOneCopied, ref bAllCopied);
			CopyResourceFile("msvcr100d.dll", "msvcr100d.dll", ref bOneCopied, ref bAllCopied);

			if(!File.Exists("SpeechGrammar.grxml"))
			{
				TextAsset textRes = Resources.Load("SpeechGrammar.grxml", typeof(TextAsset)) as TextAsset;
				
				if(textRes != null)
				{
					string sResText = textRes.text;
					File.WriteAllText("SpeechGrammar.grxml", sResText);
					
					bOneCopied = bOneCopied || File.Exists("SpeechGrammar.grxml");
					bAllCopied = bAllCopied && bOneCopied;
				}
			}

			
			return bOneCopied && bAllCopied;
		}
	}
}
#endif