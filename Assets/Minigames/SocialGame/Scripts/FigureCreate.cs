using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FigureCreate : MonoBehaviour {
	public Material mat;
	public GameObject mesh;
	public GameObject checke;
	public GameObject checker;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
		{
			createPoints();
		}
	}

	public GameObject createPoints()
	{
		int count = transform.childCount;
		Queue<Transform> fronta = new Queue<Transform>();
		fronta.Enqueue(transform);
		bool end= false;
		GameObject checkerClone = (GameObject) GameObject.Instantiate(checker,transform.position,Quaternion.identity);
		GameObject fig = figureCopy();
		fig.transform.parent = checkerClone.transform;
		while(fronta.Count>0)
		{
			Transform last = fronta.Dequeue();
			int num = last.childCount;
			for(int i=0; i<num;i++)
			{
				fronta.Enqueue(last.GetChild(i));
			}
			//bugbug
			Debug.LogWarning(last.name);
			GameObject clone = (GameObject) GameObject.Instantiate(checke,last.position,Quaternion.identity);
			clone.name = last.name+"-check";
			clone.transform.parent = checkerClone.transform;

		}
		return checkerClone;
	}

	GameObject figureCopy()
	{
		GameObject figure = (GameObject) GameObject.Instantiate(mesh,transform.position,Quaternion.AngleAxis(180,Vector3.up));
		ExtendsAvatar avatar = figure.GetComponentInChildren<ExtendsAvatar>();
		if(avatar)
		{
			Destroy(avatar);
		}
		FigureCreate creator = figure.GetComponentInChildren<FigureCreate>();
		if(creator)
		{
			Destroy(creator);
		}
		if(mat)
		{
			SkinnedMeshRenderer render = figure.GetComponentInChildren<SkinnedMeshRenderer>();
			if(render)
			{
				render.material = mat;
			}
			if(!render.enabled)
			{
				render.enabled = true;
			}
		}
		return figure;
	}
}
