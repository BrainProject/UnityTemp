var speed = 5;


function FixedUpdate()
{
	transform.Translate(Vector3.forward * Time.deltaTime * speed);
}

function OnTriggerEnter(other: Collider)
{
	
}