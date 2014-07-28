private var tmp: boolean;
private var randomNumber: int;

function Start()
{
	tmp = false;
}

function FixedUpdate()
{
	rigidbody.AddRelativeForce(Vector3.forward * 100);
}

function Update()
{
	if(tmp)
	{
		randomNumber = Random.Range(0, 3);
		if(randomNumber > 1)
		{
			//right
			transform.Rotate(Vector3.up * 90);
			tmp = false;
		}
		else
		{
			//left
			transform.Rotate(Vector3.down * 90);
			tmp = false;
		}
	}
}

function OnCollisionStay(collisionInfo: Collision)
{
	tmp = true;
}