using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect;
using MinigameSnake;


namespace MinigameSnake
{
public class Move2 : MonoBehaviour 
{
	private KinectGestures gestureListener; //gesture listener for Kinect
	public int snakeLength = 3;
	public static float level; 
	public float speed;
	//private float frenquency = 1.0f/speed;
	private int lr = 0; // x axis increment
	private int ud = 0; // z axis increment
	private int fb = 0; // y axis increment

	private int rlr = 0; // x axis rotation
	private int rud = 0; // z axis rotation
	private int rfb = 0; // y axis rotation

	private bool init = true;

	public GameObject food;
	public GameObject snakeprefab;
	public GameObject foodColored;
	public AudioClip eating;
	public GameObject ko;
	public MGC controller;
	public GameManager manager;
	private Quaternion rot;
	// Use this for initialization
	void Start () 
	{
		controller = MGC.Instance;
		Vector3 rot;
		//gestureListener = Camera.main.GetComponent<GestureListener>();
		ko = GameObject.Find ("KinectObject");
		manager = GameObject.Find ("_GameManager_").GetComponent<GameManager> ();
		if (ko != null) {  //pohyb
				
				//gestureListener = ko.GetComponent<KinectGestures> ();
//			gestureListener = controller.kinectManagerInstance;
				}

		// repeats the moving routine every "frequency" seconds
		level = (float)GameObject.Find("_Level Manager_").GetComponent<LevelManager>().level;
		//speed = 1.1f - (0.5f + level/10);
		speed = 2 / (level/5 + 1.8f);
		InvokeRepeating("Move", 0.1f, speed);
			manager.game = true;
	}

	void OnTriggerEnter(Collider c)
	{	
		if (c.gameObject.tag == "Food")
		{
			audio.PlayOneShot(eating);
			float px = (float) Random.Range(0,10);
			float py = (float) Random.Range(0,10);
			float pz = (float) Random.Range(0,10);
			Vector3 foodPosition = new Vector3 (px, py, pz);
			Instantiate (food, foodPosition, Quaternion.identity);
//			print("cerated new food");
			Instantiate(foodColored, foodPosition, Quaternion.identity);
//			print("created second food");
			
			Vector3 NewBodyPosition = GameObject.Find("snake" + GameObject.Find("snake1").GetComponent<Move2>().snakeLength).transform.position;
			GameObject newbody = (GameObject)Instantiate (snakeprefab, NewBodyPosition, Quaternion.identity);
			GameObject.Find("snake1").GetComponent<Move2>().snakeLength++;
			newbody.name = "snake" + GameObject.Find("snake1").GetComponent<Move2>().snakeLength;
//			print("destroyed food");
			Destroy(c.gameObject);
		}

		if (c.gameObject.tag == "RightFood") {
			Destroy(c.gameObject);
				}

//			print("checking game " + manager.game);


		
	}

	public void Move()
	{
		// moves the head
		Vector3 pos = transform.position;
		Quaternion rotation = transform.rotation;
		//	Vector3 rot;
		Vector3 oldPos = transform.position;
		pos.x += lr;
		pos.y += ud;
		pos.z += fb;
		rotation = rot;
		transform.position = pos;
		transform.rotation = rotation;
		

		// makes the rest of the body follow
		if (transform.position != oldPos) 
		{
			for (int i = snakeLength; i > 2; i--) 
			{
				GameObject.Find("snake" + i).gameObject.transform.position = GameObject.Find("snake" + (i-1)).gameObject.transform.position;
			}
			GameObject.Find ("snake2").gameObject.transform.position = oldPos;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		KinectManager kinectManager = KinectManager.Instance;

		if (Input.GetKey ("left") || Input.GetKey(KeyCode.Keypad1)|| Input.GetKey(KeyCode.J)) 
		{
			if (lr == 0) 
			{
				lr = 1;
				ud = 0;
				fb = 0;
				rot.eulerAngles = new Vector3(0, 0, 0);
				if (init) 
				{
					init = false;
				}
			}
		}		
		if (Input.GetKey ("right") && !init || Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.L)) 
		{
			if (lr == 0) 
			{
				lr = -1;
				ud = 0;
				fb = 0;
				rot.eulerAngles = new Vector3(0, -180, 0);
			}
		}	
		if (Input.GetKey ("up")|| Input.GetKey(KeyCode.U))
		{	
			if (ud == 0) 
			{
				lr = 0;
				ud = 1;
				fb = 0;
				rot.eulerAngles = new Vector3(0, 90, 90);
				if (init) 
				{
					init = false;
				}
			}
		}		
		if (Input.GetKey ("down")|| Input.GetKey(KeyCode.D)) 
		{
			if (ud == 0) 
			{
				lr = 0;
				ud = -1;
				fb = 0;
				rot.eulerAngles = new Vector3(0, -90, -90);
				if (init) 
				{
					init = false;
				}
			}
		}
		if (Input.GetKey(KeyCode.F)) 
		{
			if (fb == 0) 
			{
				lr = 0;
				ud = 0;
				fb = 1;
				rot.eulerAngles = new Vector3(0, 281, 0);
				if (init) 
				{
					init = false;
				}
			}
		}
		if (Input.GetKey(KeyCode.B)) 
		{
			if (fb == 0) 
			{
				lr = 0;
				ud = 0;
				fb = -1;
				rot.eulerAngles = new Vector3(0, 90, 0);
				if (init) 
				{
					init = false;
				}
			}
		}
		if (gestureListener != null) 
		{
			
		}
	}

		public void Stop()
		{
			CancelInvoke();
			print("stop");
			controller = MGC.Instance;
			controller.FinishMinigame ();
			MGC.Instance.minigamesGUI.show (true, false);

			//break;
		}
		public void StopMove()
		{
			CancelInvoke();
		}
}
}