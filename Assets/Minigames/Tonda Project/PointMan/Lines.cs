using UnityEngine;
using System.Collections;

public class Lines : MonoBehaviour {
	private LineSet set; 
	public LineRenderer renderLine;
	public Transform[] joint;
	public GameObject prefabCheck;

	void Start () {
		//Look to root and in children find LineSet
		set = transform.root.GetComponentInChildren<LineSet>();
		renderLine = gameObject.AddComponent("LineRenderer") as LineRenderer;
		if(set != null)
		{
			//set data from LineSet
			renderLine.SetWidth(set.widthStart, set.widthEnd);
			renderLine.SetColors(set.colorStart,set.colorEnd);
			renderLine.material = set.material;
			prefabCheck = set.prefabCheck;
		} 
		else
		{
			Debug.LogWarning(gameObject.name + "this object not found seting script");
		}
		//count of vertes joints + this object;
		renderLine.SetVertexCount(joint.Length+1);
	}
	

	void Update () {
		// Redraw line
		for(int i = 0; i < joint.Length;i++)
		{
			renderLine.SetPosition(i,joint[i].position);
		}
		renderLine.SetPosition(joint.Length,transform.position);

		#if UNITY_EDITOR
		if(Input.GetKeyDown("g"))
		{
			GameObject gest = GameObject.Instantiate(prefabCheck,transform.position,transform.rotation) as GameObject;
			gest.name = gameObject.name + " Gest";
			string tag = transform.root.tag;
			GameObject[] objs =GameObject.FindGameObjectsWithTag(tag);
			GameObject parent = null;
			Check script = gest.AddComponent<Check>();
			script.target = transform;
			foreach(GameObject obj in objs)
			{
				if(obj.name == "GestChecker")
					parent = obj;
			}
			if(parent != null)
				gest.transform.parent = parent.transform;
		}
		#endif
	}
}
