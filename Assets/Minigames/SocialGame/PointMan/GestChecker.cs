using UnityEngine;
using System.Collections;

namespace SocialGame
{
public class GestChecker : MonoBehaviour {
	public float distance;
	public GameObject next;
	public bool SnapGest;
	public string clipBone;

	public bool normalRun = true;
	private Vector3 temp;
	// Use this for initialization
	void Start () {
		//Transform root = transform.parent;
		for(int i =0; i <transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			string nameGest = child.name;
			string[] names =nameGest.Split('-');
			GameObject obj = GameObjectEx.findGameObjectWithNameTag(names[0],gameObject.tag);
			Check che =child.GetComponent<Check>();
			if (che != null)
			{
				che.target = obj.transform;
			}
		}
		if(clipBone != null)
		{
			MoveParentOnBone(clipBone);
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool complete = false;
		normalRun = !SnapGest;//bugbug
		for(int i = 0; i< transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			Check script = child.GetComponent<Check>();
			if(script != null && script.active)
			{
				Transform target = script.target;
				bool next = Vector2.Distance(child.position,target.position) < distance;
				if(next)
				{
					complete = script.Checked();
					Debug.DrawRay(target.position,child.position - target.position,Color.green);
				}
				else
				{
					Debug.DrawRay(target.position,child.position - target.position,Color.red);
				}
			}
		}
		if(complete)
		{
			GameObject.Instantiate(this.next);
			Destroy(gameObject);
		}
	}

	void MoveParentOnBone(string boneName)
	{
		GameObject bone = GameObjectEx.findGameObjectWithNameTag(boneName,gameObject.tag);
		if(bone != null)
		{
			Vector3 pos =transform.position;
			Quaternion rot = transform.rotation;
			gameObject.transform.parent= bone.transform;
			transform.localPosition = Vector3.zero + pos;
			transform.localRotation = rot;
		}
	}
}
}
