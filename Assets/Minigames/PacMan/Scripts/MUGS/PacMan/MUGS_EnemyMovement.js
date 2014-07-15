


function FixedUpdate()
{
	rigidbody.AddForce(Vector3.forward * 10);
}

function OnCollisionEnter(collision: Collision)
{
	gameObject.transform.rotation.y = 90;
}