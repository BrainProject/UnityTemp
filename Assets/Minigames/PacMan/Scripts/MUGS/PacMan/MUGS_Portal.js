var spawnPoint: Transform;


function Start()
{
	transform.renderer.material.color = Color.blue;
}

function OnTriggerEnter(other: Collider)
{
	if(other.tag == "Player")
		other.transform.position = Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
}