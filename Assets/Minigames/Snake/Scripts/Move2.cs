using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect;

public class Move2 : MonoBehaviour 
{
	private KinectGestures gestureListener; //gesture listener for Kinect
	public int snakeLength = 3;
	public static float level; 
	private float speed;
	//private float frenquency = 1.0f/speed;
	private int lr = 0; // x axis increment
	private int ud = 0; // z axis increment
	private int fb = 0; // y axis increment
	private bool init = true;

	public GameObject food;
	public GameObject snakeprefab;
	public GameObject foodColored;
	public AudioClip eating;
	public GameObject ko;
	// Use this for initialization
	void Start () 
	{
		//gestureListener = Camera.main.GetComponent<GestureListener>();
		ko = GameObject.Find ("KinectObject");
		if (ko != null) {  //pohyb
						gestureListener = ko.GetComponent<KinectGestures> ();
				}

		// repeats the moving routine every "frequency" seconds
		level = (float)GameObject.Find("_Level Manager_").GetComponent<LevelManager>().level;
		//speed = 1.1f - (0.5f + level/10);
		speed = 2 / (level + 2);
		InvokeRepeating("Move", 0.1f, speed);
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
			print("cerated new food");
			Instantiate(foodColored, foodPosition, Quaternion.identity);
			print("created second food");
			
			Vector3 NewBodyPosition = GameObject.Find("snake" + GameObject.Find("snake1").GetComponent<Move2>().snakeLength).transform.position;
			GameObject newbody = (GameObject)Instantiate (snakeprefab, NewBodyPosition, Quaternion.identity);
			GameObject.Find("snake1").GetComponent<Move2>().snakeLength++;
			newbody.name = "snake" + GameObject.Find("snake1").GetComponent<Move2>().snakeLength;
			print("destroyed food");
			Destroy(c.gameObject);
		}
		if (c.gameObject.tag == "RightFood") {
			Destroy(c.gameObject);
				}
	}

	public void Move()
	{
		// moves the head
		Vector3 pos = transform.position;
		Vector3 oldPos = transform.position;
		pos.x += lr;
		pos.y += ud;
		pos.z += fb;
		transform.position = pos;

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

		if (Input.GetKey ("left")) 
		{
			if (lr == 0) 
			{
				lr = 1;
				ud = 0;
				fb = 0;
				if (init) 
				{
					init = false;
				}
			}
		}		
		if (Input.GetKey ("right") && !init) 
		{
			if (lr == 0) 
			{
				lr = -1;
				ud = 0;
				fb = 0;
			}
		}	
		if (Input.GetKey ("up"))
		{	
			if (ud == 0) 
			{
				lr = 0;
				ud = 1;
				fb = 0;
				if (init) 
				{
					init = false;
				}
			}
		}		
		if (Input.GetKey ("down")) 
		{
			if (ud == 0) 
			{
				lr = 0;
				ud = -1;
				fb = 0;
				if (init) 
				{
					init = false;
				}
			}
		}
		if (Input.GetKey ("f")) 
		{
			if (fb == 0) 
			{
				lr = 0;
				ud = 0;
				fb = 1;
				if (init) 
				{
					init = false;
				}
			}
		}
		if (Input.GetKey ("b")) 
		{
			if (fb == 0) 
			{
				lr = 0;
				ud = 0;
				fb = -1;
				if (init) 
				{
					init = false;
				}
			}
		}
		if (gestureListener) 
		{
			if (gestureListener.IsSwipeLeft ()) 
			{
				if (lr == 0) 
				{
					lr = 1;
					ud = 0;
					fb = 0;
					if (init) 
					{
						init = false;
					}
				}
			}
			if (gestureListener.IsSwipeRight () && !init) 
			{
				if (lr == 0) 
				{
					lr = -1;
					ud = 0;
					fb = 0;
				}
			}
			if (gestureListener.IsSwipeUp ()) 
			{
				if (ud == 0) 
				{
					lr = 0;
					ud = 1;
					fb = 0;
					if (init) 
					{
						init = false;
					}
				}
			}
			if (gestureListener.IsSwipeDown ()) 
			{
				if (ud == 0) 
				{
					lr = 0;
					ud = -1;
					fb = 0;
					if (init) 
					{
						init = false;
					}
				}
			}
			if (gestureListener.IsPull ()) 
			{
				if (fb == 0) 
				{
					lr = 0;
					ud = 0;
					fb = 1;
					if (init) 
					{
						init = false;
					}
				}
			}
			if (gestureListener.IsPush ()) 
			{
				if (fb == 0) 
				{
					lr = 0;
					ud = 0;
					fb = -1;
					if (init) 
					{
						init = false;
					}
				}
			}
		}
	}
}
