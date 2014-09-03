using UnityEngine;
using System.Collections;

public class GameObjectEx{

	public static GameObject findGameObjectWithNameTag(string name, string tag)
	{
		GameObject[] objects =GameObject.FindGameObjectsWithTag(tag);
		foreach(GameObject obj in objects)
		{
			if(obj.name.Equals(name))
			{
				return obj;
			}
		}
		return null;
	}
}
