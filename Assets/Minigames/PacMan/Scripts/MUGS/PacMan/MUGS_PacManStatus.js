var status:int;
var powerUp:boolean;
var life = 3;
var spawn:Transform;
var time:int;
var on:boolean;


function Start()
{
	status = GameObject.FindGameObjectsWithTag("TicTac").Length;
	powerUp = false;
	time = 0;
	transform.position = spawn.position;
	transform.rotation = Quaternion.identity;
	//Screen.showCursor = false;
	on = true;
	GameObject.Find("Left").GetComponent("RotateAroundBrainBorder").CanRotate = true;
	GameObject.Find("Right").GetComponent("RotateAroundBrainBorder").CanRotate = true;
	//GameObject.Find("Up").GetComponent("RotateAroundBrainBorder").CanRotate = true;
	//GameObject.Find("Down").GetComponent("RotateAroundBrainBorder").CanRotate = true;
}

function Update()
{
	//PowerUp!
	if(powerUp == true && time > 0)
		time -= Time.smoothDeltaTime;
	else
		powerUp = false;
		
		
	if(Input.GetKeyDown("l"))
		++life;
		
	if(Input.GetKeyDown("g"))
		on = !on;
		
	if(Input.GetKeyDown("m"))
	{
		Screen.showCursor = true;
		Application.LoadLevel("LevelSelector");
	}
}


function OnGUI()
{
	if(GameObject.Find("TicTac") == null)
	{
		Time.timeScale = 0.0;
		Screen.showCursor = true;
		transform.GetComponent("MouseLook").enabled = false;
		transform.FindChild("Main Camera").GetComponent("MouseLook").enabled = false;
		transform.GetComponent(FPSInputController).enabled = false;
		GUI.Box(Rect(Screen.width/2-65,Screen.height/2-11,130, 22), "Go back to");
		if(GUI.Button(Rect(Screen.width/2-65,Screen.height/2+15,130, 22), "Menu"))
		{
			Time.timeScale = 1.0;
			Application.LoadLevel("NewMain");
		}
	}
	GUI.Box(Rect(20,20,130,22), "Zbývá " + status + " TicTaců.");
	GUI.Box(Rect(Screen.width - 150,20,130,22), "Zbývá " + life   + " životů.");
	if(!on)
		GUI.Box(Rect(20,50,130,22), "You are GOD!!!!");
	GUI.Box(Rect(20,80,130,40), "Press 'm' to go\nback to Menu");
		//Application.LoadLevel("
}


function Die()
{
	if(on)
	{
		life--;
		transform.position = spawn.position;
		transform.rotation = Quaternion.identity;
		time = 200;
		powerUp = true;
	
		if(life < 0)
			Application.LoadLevel("Other-Portal");
	}
}