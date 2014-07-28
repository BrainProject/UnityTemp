using UnityEngine;
using System.Collections;

public class GestChecker : MonoBehaviour {
	public float distance;
	public GameObject next;
	public bool SnapGest;
	public Material MatOfCheckedCheck;
	public GameObject clipBone;
	private Transform parent;

	public bool normalRun = true;
	private Vector3 temp;
	// Use this for initialization
	void Start () {
		Transform root = transform.parent;
		for(int i =0; i <transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			string nameGest = child.name;
			string[] names =nameGest.Split(' ');
			for(int j = 0; j < root.childCount; j++)
			{
				Transform childOfRoot = root.GetChild(j);
				Check che =child.GetComponent<Check>();
				if (che == null)
				{
					break;
				}
				if(childOfRoot.name.Equals(names[0]))
				{
					che.target = childOfRoot;
					break;
				}
			}
		}
		if(clipBone != null)
		{
			MoveParentOnBone(clipBone);
			Check che =clipBone.GetComponent<Check>();
			transform.position = che.target.position;
			parent = transform.parent;
			transform.parent = che.target;
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool complete = true;
		normalRun = !SnapGest;//bugbug
		for(int i = 0; i< transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			Check script = child.GetComponent<Check>();
			if(script != null)
			{
				Transform target = script.target;
				bool next = Vector3.Distance(child.position,target.position) < distance;
				if(next)
				{
					Debug.DrawRay(target.position,child.position - target.position,Color.green);
				}
				else
				{
					Debug.DrawRay(target.position,child.position - target.position,Color.red);
				}
				script.Checked(next,MatOfCheckedCheck);
				complete = complete && next;
			}
		}
		if (complete && normalRun)
		{
			Debug.Log("GestComplete");
			if(next != null)
			{
				if(parent != null)
				{
					transform.parent = parent;
					transform.position = temp;					
				}
				GameObject clone = GameObject.Instantiate(next,transform.position,transform.rotation) as GameObject;
				clone.transform.parent = transform.parent;
				clone.tag = gameObject.tag;
				Destroy(gameObject);

			}
		}
	}

	void MoveParentOnBone(GameObject bone)
	{
		if(bone != null)
		{
			Transform[] child = new Transform[transform.childCount];
			for(int i =0;i<child.Length;i++)
			{
				child[i] = transform.GetChild(0);
				child[i].parent = null;
			}
			temp = transform.position;
			transform.position = bone.transform.position;
			for(int i =0;i<child.Length;i++)
			{
				child[i].parent = transform;
			}
		}

	}
}
