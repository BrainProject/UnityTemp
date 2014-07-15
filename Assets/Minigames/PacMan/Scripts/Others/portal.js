


function Start()
{
	Screen.showCursor = false;
}

function OnTriggerEnter(other:Collider)
{
	Application.LoadLevel("MUGS-Workshop_1");
}