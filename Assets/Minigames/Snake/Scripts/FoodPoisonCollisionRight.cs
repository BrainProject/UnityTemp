using UnityEngine;
using System.Collections;


namespace MinigameSnake
{
// If a food object is created at the coordinates of an existing poison object, this script deletes it ( object is created with left object - see FoodPoisonCollision)
public class FoodPoisonCollisionRight : MonoBehaviour 
{	
	
	void OnTriggerEnter(Collider c)
	{
			if (c.gameObject.tag == "Poison" || c.gameObject.tag == "Snake" || c.gameObject.tag == "Tail") 
		{
			Destroy(this.gameObject);
			

		}
	}
}
}
