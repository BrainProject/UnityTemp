using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FigureCreate : MonoBehaviour {
	public GameObject checke;
	public GameObject checker;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.E))
		{
			Debug.LogWarning("go!!");
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
}
