var limit = 500;


function Start()
{
	transform.renderer.material.color = Color.red;
}

function OnTriggerEnter(other: Collider)
{
	if((other.GetComponent("MUGS_PacManStatus")))
	{
		other.GetComponent("MUGS_PacManStatus").powerUp = true;
		other.GetComponent("MUGS_PacManStatus").time = limit;
		Destroy(transform.gameObject);
	}
}