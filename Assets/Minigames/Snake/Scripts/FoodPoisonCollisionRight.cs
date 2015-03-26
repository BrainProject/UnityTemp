using UnityEngine;
using System.Collections;

// If a food object is created at the coordinates of an existing poison object, this script deletes it and creates a new one somewhere else
public class FoodPoisonCollisionRight : MonoBehaviour 
{	
	//public GameObject food;
	//public GameObject foodColored;

	//Destroy the food from right screen

	void OnTriggerEnter(Collider c)
	{
		if (c.gameObject.tag == "Poison") 
		{
			Destroy(this.gameObject);
			

		}
	}
}
