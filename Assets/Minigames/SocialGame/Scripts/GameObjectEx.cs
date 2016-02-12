using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectEx{
	/// <summary>
	/// Finds the game object with name and tag.
	/// </summary>
	/// <returns>The game object with name and tag.</returns>
	/// <param name="name">Name.</param>
	/// <param name="tag">Tag.</param>
	public static GameObject FindGameObjectWithNameTag(string nameOfObj, string tag)
	{
		GameObject[] objects =GameObject.FindGameObjectsWithTag(tag);
		foreach(GameObject obj in objects)
		{
			if(obj.name == (nameOfObj))
			{
				return obj;
			}
		}
		return null;
	}
	/// <summary>
	/// Finds the by tag from list.
	/// </summary>
	/// <returns>The object with tag from list.</returns>
	/// <param name="list">List.</param>
	/// <param name="tag">Tag.</param>
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

	/// <summary>
	/// Destroies the object with all parents.
	/// </summary>
	/// <param name="child">Child.</param>
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
