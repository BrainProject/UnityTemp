var originalColor: Color;
var spawn: Transform;


function Start()
{
	originalColor = gameObject.transform.renderer.material.color;
}

function PowerUpOn()
{
	if(gameObject.renderer.material.color != Color.white)
		gameObject.renderer.material.color = Color.white;
	//gameObject.collider.enabled = false;
	print("PowerUpOn");
}

function PowerUpOff()
{
	gameObject.transform.renderer.material.color = originalColor;
	//gameObject.collider.enabled = true;
}

function OnTriggerEnter(other: Collider)
{
	if(other.gameObject.tag == "Player" && !other.GetComponent("MUGS_PacManStatus").powerUp)
	{
		other.GetComponent("MUGS_PacManStatus").Die();
		Debug.Log("Nom, nom...!");
	}
	if(other.gameObject.tag == "Player" && other.GetComponent("MUGS_PacManStatus").powerUp)
	{
		transform.parent.parent.position = spawn.position;
		transform.parent.parent.rotation = Quaternion.identity;
		Debug.Log("Nezabijej me!!!");
	}
}

function Update()
{
	if(GameObject.Find("First Person Controller").GetComponent("MUGS_PacManStatus").time > 0)
		PowerUpOn();
	if(GameObject.Find("First Person Controller").GetComponent("MUGS_PacManStatus").time <= 0)
		PowerUpOff();
}