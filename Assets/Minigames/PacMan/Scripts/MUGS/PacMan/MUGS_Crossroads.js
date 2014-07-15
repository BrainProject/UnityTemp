private var turnUp = true;
private var turnDown = true;
private var turnLeft = true;
private var turnRight = true;
private var hit: RaycastHit;
private var whereTo = new ArrayList();

function Start()
{
	var up = transform.TransformDirection(Vector3.forward);
	if(Physics.Raycast(transform.position, up, hit, 4) && hit.collider.gameObject.CompareTag("Walls"))
	{
		//print("Up wall.");
		turnUp = false;
	}
	else
	{
		//print("Up is clear.");
		whereTo.Add(0);
	}
		
	var down = transform.TransformDirection(-Vector3.forward);
	if(Physics.Raycast(transform.position, down, hit, 4) && hit.collider.gameObject.CompareTag("Walls"))
	{
		//print("Down wall.");
		turnDown = false;
	}
	else
	{
		//print("Down is clear.");
		whereTo.Add(180);
	}
		
	var left = transform.TransformDirection(-Vector3.right);
	if(Physics.Raycast(transform.position, left, hit, 4) && hit.collider.gameObject.CompareTag("Walls"))
	{
		//print("Left wall.");
		turnLeft = false;
	}
	else
	{
		//print("Left is clear.");
		whereTo.Add(270);
	}
		
	var right = transform.TransformDirection(Vector3.right);
	if(Physics.Raycast(transform.position, right, hit, 4) && hit.collider.gameObject.CompareTag("Walls"))
	{
		//print("Right wall.");
		turnRight = false;
	}
	else
	{
		//print("Right is clear.");
		whereTo.Add(90);
	}
}

function OnTriggerEnter(other: Collider)
{
	if(other.gameObject.CompareTag("Enemy") && (((other.transform.position.y - transform.position.y) < 0.1) || ((other.transform.position.x - transform.position.x) < 0.1)))
	{
		var rand = whereTo[Random.Range(0,whereTo.Count)];
		//Debug.Log("Random: " + rand);
		other.transform.rotation.eulerAngles = Vector3(0,rand,0);
	}
}