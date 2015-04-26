using UnityEngine;
using System.Collections;
using Leap;
using System.Runtime.InteropServices;

public class LeapBehaviorScript : MonoBehaviour {
	
	//Leap Motion controller
	Controller controller = null;
	
	//vectors for current and 5-frames-delay position of hand
	Vector3 palmPosition;			//palm position of current frame to set cursor on position on every new frame
	Vector3 palmPosition5fb;		//palm position 5 frames back -> position, where I wanted to click
	Vector3 pointablePosition;		//current position of pointable(p.e finnger) to compare with 5fd
	Vector3 pointablePosition5fb;	//5 frames delay pointable position to compare with current pointable position -> detection of click
	
	//timer for enable only one click in n frames
	public int gestureTimerDefault = 10;
	bool gestureTimerActive = true;
	int gestureTimer;

	//control if hand is in the fist variables
	bool isFist;
	int isFistCounter;
	
	//structure for output of GeCursorPos method and his instance
	public struct Point
	{
		public int x, y;
	}
	//Point point;
	
	//Windows user32.dll library methods for set and get cursor position
	[DllImport("user32.dll",CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool SetCursorPos(int X, int Y);
	
	[DllImport("user32.dll", SetLastError=true)]
	public static extern bool GetCursorPos(out Point pos);
	
	//Windows user32.dll library method for simulating a mouse click
	[DllImport("user32.dll",CharSet=CharSet.Auto, CallingConvention=CallingConvention.StdCall)]
	public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
	
	//Windows user32.dll libraty method for simulating a keyboard input
	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
	
	//Structure for SendInput method for simulating keyboards inputs
	[StructLayout(LayoutKind.Sequential)]
	internal struct KEYBDINPUT
	{
		public ushort Vk;
		public ushort Scan;
		public uint Flags;
		public uint Time;
		public System.IntPtr ExtraInfo;
	}
	
	[StructLayout(LayoutKind.Sequential)]
	internal struct INPUT
	{
		public uint Type;
		public MOUSEKEYBDHARDWAREINPUT Data;
	}
	
	[StructLayout(LayoutKind.Explicit)]
	internal struct MOUSEKEYBDHARDWAREINPUT
	{
		[FieldOffset(0)]
		public HARDWAREINPUT Hardware;
		[FieldOffset(0)]
		public KEYBDINPUT Keyboard;
		[FieldOffset(0)]
		public MOUSEINPUT Mouse;
	}
	
	[StructLayout(LayoutKind.Sequential)]
	internal struct MOUSEINPUT
	{
		public int X;
		public int Y;
		public uint MouseData;
		public uint Flags;
		public uint Time;
		public System.IntPtr ExtraInfo;
	}
	
	[StructLayout(LayoutKind.Sequential)]
	internal struct HARDWAREINPUT
	{
		public uint Msg;
		public ushort ParamL;
		public ushort ParamH;
	}
	
	
	
	private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
	private const uint MOUSEEVENTF_LEFTUP = 0x04;
	private const uint MOUSEEVENTF_RIGHTDOWN = 0x08;
	private const uint MOUSEEVENTF_RIGHTUP = 0x10;
	
	private const uint KEY_A = 0x41;
	private const uint KEY_D = 0x44;
	private const uint KEY_I = 0x49;
	
	//perform mouse click on the coordinates x and y
	public void DoMouseClick(float x, float y)
	{
		//Call the imported function with the cursor's current position
		uint X = System.Convert.ToUInt32(x);
		uint Y = System.Convert.ToUInt32(y);
		mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
	}

	public void DoMouseDown(float x, float y)
	{
		//Call the imported function with the cursor's current position
		uint X = System.Convert.ToUInt32(x);
		uint Y = System.Convert.ToUInt32(y);
		mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
	}

	public void DoMouseUp(float x, float y)
	{
		//Call the imported function with the cursor's current position
		uint X = System.Convert.ToUInt32(x);
		uint Y = System.Convert.ToUInt32(y);
		mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
	}
	
	public void DoKeyboardInput(uint key){
		SendKeyDown (key);
		SendKeyUp (key);
	}
	
	public static void SendKeyUp(uint keyCode)
	{
		INPUT input = new INPUT {
			Type = 1
		};
		input.Data.Keyboard = new KEYBDINPUT();
		input.Data.Keyboard.Vk = (ushort)keyCode;
		input.Data.Keyboard.Scan = 0;
		input.Data.Keyboard.Flags = 2;
		input.Data.Keyboard.Time = 0;
		input.Data.Keyboard.ExtraInfo = System.IntPtr.Zero;
		INPUT[] inputs = new INPUT[] { input };
		if (SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
			throw new System.Exception();
		
	}
	
	public static void SendKeyDown(uint keyCode)
	{
		INPUT input = new INPUT{
			Type = 1
		};
		input.Data.Keyboard = new KEYBDINPUT ();
		input.Data.Keyboard.Vk = (ushort)keyCode;
		input.Data.Keyboard.Scan = 0;
		input.Data.Keyboard.Flags = 0;
		input.Data.Keyboard.Time = 0;
		input.Data.Keyboard.ExtraInfo = System.IntPtr.Zero;
		INPUT[] inputs = new INPUT[] { input };
		if (SendInput (1, inputs, Marshal.SizeOf (typeof(INPUT))) == 0) {
			throw new System.Exception ();
		}
	}
	
	//width and height of displayed window
	public static int windowWidth;
	public static int windowHeight;
	
	// Use this for initialization
	void Start () {
		windowWidth = UnityEngine.Screen.width;
		windowHeight = UnityEngine.Screen.height;
		controller = new Controller ();
		palmPosition = new Vector3 ();
		palmPosition5fb = new Vector3 ();
		pointablePosition = new Vector3 ();
		pointablePosition5fb = new Vector3 ();
		//point = new Point ();
		//Debug.Log(windowWidth + ", " + windowHeight);
		
		//initiate gesture timer
		gestureTimer = 0;
		gestureTimerActive = false;
		
		//enable swipe, circle and key tap gestures
		controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);
		//controller.EnableGesture (Gesture.GestureType.TYPE_CIRCLE);
		controller.EnableGesture (Gesture.GestureType.TYPEKEYTAP);
		
		//configure gestures
		Config config = controller.Config;
		//config.SetFloat ("Gesture.Circle.MinRadius", 100);
		//config.SetFloat ("Gesture.Swipe.MinLength", 300);
		//config.SetInt32("Gesture.Swipe.MinVelocity", 2500);
		config.SetFloat("Gesture.KeyTap.HistorySeconds", .5f);
		config.SetFloat ("Gesture.KeyTap.MinDownVelocity", 20.0f);
		config.SetFloat("Gesture.KeyTap.MinDistance", 15.0f);
		config.Save ();

		//set fist recognition variables
		isFist = false;
		isFistCounter = 0;
		//UnityEngine.Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void FixedUpdate(){
		
		if (gestureTimerActive) {
			gestureTimer++;
			if(gestureTimer >= gestureTimerDefault){
				gestureTimer = 0;
				gestureTimerActive = false;
			}
		}
		
		if (controller.IsConnected) {
			//Debug.Log ("Leap is connected");
			
			//inicialize the frames
			Frame frame = controller.Frame (0);
			Frame frame5fb = controller.Frame(5);
			
			//initialize the list of hands in current and 5-frames-delay frame
			HandList handList = frame.Hands;
			HandList handList5fb = frame5fb.Hands;
			
			//initialize list of gestures of current frame
			GestureList gestList = frame.Gestures();
			//if(!gestList.IsEmpty) Debug.Log("gestrarrararaaaaa");
			
			
			//set position vector of palm position and tip of pointable object(p.e. finger) position of current frame
			foreach (Hand h in handList) {
				if (h.Equals (handList.Frontmost)) {
					PointableList pointables = h.Pointables;
					foreach (Pointable p in pointables) {
						if(p.Equals(pointables.Frontmost)){
							pointablePosition.x = p.TipPosition.x + 300;
							pointablePosition.y = p.TipPosition.y;
							pointablePosition.z = p.TipPosition.z;
						}
					}
					//Debug.Log ("Handerrrrraaaa");
					palmPosition.x = h.StabilizedPalmPosition.x + 300;
					palmPosition.y = h.StabilizedPalmPosition.y;
					palmPosition.z = h.StabilizedPalmPosition.z;

					if(checkFist(h)){
						isFist = true;
					} else {
						isFist = false;
					}
				}
			}
			
			//set position vector of palm position and tip of pointable object(p.e. finger) position of 5-frames-delay frame
			foreach (Hand j in handList5fb) {
				if (j.Equals (handList5fb.Frontmost)) {
					PointableList pointables = j.Pointables;
					foreach (Pointable p in pointables) {
						if(p.Equals(pointables.Frontmost)){
							pointablePosition5fb.x = p.TipPosition.x + 300;
							pointablePosition5fb.y = p.TipPosition.y;
							pointablePosition5fb.z = p.TipPosition.z;
						}
					}
					palmPosition5fb.x = j.StabilizedPalmPosition.x + 300;
					palmPosition5fb.y = j.StabilizedPalmPosition.y;
					palmPosition5fb.z = j.StabilizedPalmPosition.z;
				}
			}

			//gesture recognition
			if(!gestureTimerActive){
				foreach (Gesture g in gestList) {
					if(g.Type.Equals(Gesture.GestureType.TYPECIRCLE)){
						//Debug.Log("Circle gesture");
						gestureTimerActive = true;
					}
					if(g.Type.Equals(Gesture.GestureType.TYPESWIPE)){
						if(palmPosition.x > palmPosition5fb.x){
							//Debug.Log("Swipe right gesture");
							gestureTimerActive = true;
						} else {
							//Debug.Log("Swipe left gesture");
							gestureTimerActive = true;
						}
					}
					if(g.Type.Equals(Gesture.GestureType.TYPEKEYTAP)){
						DoKeyboardInput(KEY_I);
						gestureTimerActive = true;
					}
				}
			}
			
			//Debug.Log("ClickHand z position: " + positionZ);
			
			if(!handList.IsEmpty){
				//Debug.Log(windowWidth + ", " + windowHeight);
				
				//
				Vector2 screen = ConvertToScreen(palmPosition);
				try {
					if(SetCursorPos(System.Convert.ToInt32(screen.x), System.Convert.ToInt32(screen.y))){
						//GetCursorPos(out point);
						//Debug.Log(screen.x + ", " + screen.y);
						//Debug.Log(point.x + ", " + point.y);
					}
				}
				catch (System.ArgumentOutOfRangeException){
					Debug.Log("Argument out of range");
				}
				catch (System.Security.SecurityException){
					Debug.Log("Security Exception ");
				}
			}
			
			if(!gestureTimerActive){
				//detection of click with thrashold
				int thrashold = 20;
				if((pointablePosition5fb.z - pointablePosition.z) > thrashold){
					DoMouseClick(palmPosition5fb.x, palmPosition5fb.y);
					gestureTimerActive = true;
				}
				
				//swipe detection
				int swipeThrashold = 50;
				if((palmPosition5fb.x - palmPosition.x) > swipeThrashold){
					DoKeyboardInput(KEY_D);
					gestureTimerActive = true;
				}
				
				if((-palmPosition5fb.x + palmPosition.x) > swipeThrashold){
					DoKeyboardInput(KEY_A);
					gestureTimerActive = true;
				}
			}

			//dragging with fist gesture
			if(isFist && (isFistCounter == 0)){
				DoMouseDown(palmPosition.x, palmPosition.y);
				isFistCounter++;
				print("fist start");
			} 

			//releasing drag
			if(!isFist && (isFistCounter == 1)){
				DoMouseUp(palmPosition.x, palmPosition.y);
				isFistCounter = 0;
				print("fist end");
			}

		}
	}
	
//	void LateUpdate(){
//		if (Input.GetKey (KeyCode.A)) {
//			print("key A was pressed");
//		}
//		if (Input.GetKey (KeyCode.D)) {
//			print("key D was pressed");
//		}
//		if (Input.GetKey (KeyCode.I)) {
//			print("key I was pressed");
//		}
//	}
	
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
	
	
//	public void ButtonFunction(){
//		Debug.Log ("Button was pressed");
//	}

	public Vector3 getCursor(){
		return palmPosition;
	}
	
	//calculation of palm position to screen coordinates
	private static Vector2 ConvertToScreen(Vector3 vect)
	{
		Vector2 result = new Vector2();
		result.x = (vect.x - 300) * ((windowWidth/2)/150) + windowWidth/2;
		result.y = windowHeight - ((vect.y - 200) * ((windowHeight) / 200));
		return result;
	}

	
	bool checkFist(Leap.Hand hand){
		float sum = 0;
		foreach (Finger finger in hand.Fingers) {
			Vector3 meta =  ToVector3(finger.Bone(Bone.BoneType.TYPE_METACARPAL).Direction);
			Vector3 proxi = ToVector3(finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Direction);
			Vector3 inter = ToVector3(finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Direction);
			float dMetaProxi = Vector3.Dot(meta, proxi);
			float dProxiInter = Vector3.Dot(proxi, inter);
			sum += dMetaProxi;
			sum += dProxiInter;
		}
		sum = sum/10;
		//print ("robim daco");
		if((sum<=0.5f) && ((hand.Fingers.Extended().Count)==0)){
			return true;
		}else{
			return false;
		}
	}

	static Vector3 ToVector3 (Vector v)
	{
		return new Vector3 (v.x, v.y, v.z);
	}


}
