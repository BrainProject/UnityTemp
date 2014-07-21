using UnityEngine;
using System.Collections;

public class Check : MonoBehaviour {

	public Transform target;

	private Material deafaultMat;
	private bool inPos;

	void Start()
	{
		deafaultMat = renderer.material;
	}

	public void Checked(bool inPosition, Material checkMaterial)
	{
		if(inPosition != inPos)
		{
			if(inPosition)
			{
				renderer.material = checkMaterial;
			}
			else
			{
				renderer.material = deafaultMat;
			}
			inPos=inPosition;
		}
	}


}
