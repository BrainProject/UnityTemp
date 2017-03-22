using UnityEngine;
using System.Collections;

namespace SocialGame{
	public class Lines : MonoBehaviour {
		private LineSet set; 
		public LineRenderer renderLine;
		public Transform[] joint;

		/// <summary>
		/// set lines
		/// </summary>
		void Start () {
			set = transform.root.GetComponentInChildren<LineSet>();
			renderLine = gameObject.AddComponent<LineRenderer>() as LineRenderer;
			if(set != null)
			{
                renderLine.startWidth = set.widthStart;
                renderLine.endWidth = set.widthEnd;
                renderLine.startColor = set.colorStart;
                renderLine.endColor = set.colorEnd;
				//renderLine.SetWidth(set.widthStart, set.widthEnd);
				//renderLine.SetColors(set.colorStart,set.colorEnd);
				renderLine.material = set.material;
			} 
			else
			{
				Debug.LogWarning(gameObject.name + "this object not found seting script");
			}
			//renderLine.SetVertexCount(joint.Length);
            renderLine.numPositions = joint.Length;
		}
		
		/// <summary>
		/// Redraw line.
		/// </summary>
		void Update () {
			for(int i = 0; i < joint.Length;i++)
			{
				renderLine.SetPosition(i,joint[i].position);
			}
		}
	}
}
