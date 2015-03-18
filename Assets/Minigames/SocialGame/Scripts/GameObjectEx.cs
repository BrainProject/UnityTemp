using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectEx{

	public static GameObject FindGameObjectWithNameTag(string name, string tag)
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

	public static GameObject FindByTagFromList(List<GameObject> list,string tag)
	{
		foreach(GameObject obj in list)
		{
			if(obj.tag.Equals(tag))
			{
				return obj;
			}
		}
		return null;

	
	}

	public static void DestroyObjectWithAllParents(Transform child)
	{
		Transform parent = child;
		while (parent.parent) 
		{
			parent = parent.parent;
		}
		GameObject.Destroy(parent.gameObject);
	}
}
