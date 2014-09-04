using UnityEngine;
using System.Collections;

public class Lines : MonoBehaviour {
	private LineSet set; 
	public LineRenderer renderLine;
	public Transform[] joint;

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
		} 
		else
		{
			Debug.LogWarning(gameObject.name + "this object not found seting script");
		}
		//count of vertes joints + this object;
		renderLine.SetVertexCount(joint.Length);
	}
	

	void Update () {
		// Redraw line
		for(int i = 0; i < joint.Length;i++)
		{
			renderLine.SetPosition(i,joint[i].position);
		}
	}
}
