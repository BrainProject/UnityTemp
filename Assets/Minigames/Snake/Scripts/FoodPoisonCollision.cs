using UnityEngine;
using System.Collections;


namespace MinigameSnake
{
// If a food object is created at the coordinates of an existing poison object, this script deletes it and creates a new one somewhere else
public class FoodPoisonCollision : MonoBehaviour 
{	
	public GameObject food;
	public GameObject foodColored;

	void OnTriggerEnter(Collider c)
	{
			if ((c.gameObject.tag == "Poison") || c.gameObject.tag == "Food" || c.gameObject.tag == "Snake" || c.gameObject.tag == "Tail") 
		{
			print("collision food poison");
			Destroy(this.gameObject);
			
			float px = (float) Random.Range(0,10);
			float py = (float) Random.Range(0,10);
			float pz = (float) Random.Range(0,10);
			
			Vector3 foodPosition = new Vector3(px, py, pz);
			
			Instantiate(food, foodPosition, Quaternion.identity);
			//print("created normal food");
			Instantiate(foodColored, foodPosition, Quaternion.identity);
			//print("created right food");
		}
	}
}
}
