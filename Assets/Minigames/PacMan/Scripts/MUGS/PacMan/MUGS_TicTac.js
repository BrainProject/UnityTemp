


function OnTriggerEnter(other: Collider)
{
	if((other.GetComponent("MUGS_PacManStatus")))
	{
		other.GetComponent("MUGS_PacManStatus").status -= 1;
		Destroy(transform.gameObject);
	}
}