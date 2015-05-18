using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SocialGame
{
	public class FigureCreate : MonoBehaviour {
#if UNITY_STANDALONE
		public Material mat;
		public GameObject mesh;
		public GameObject checke;
		public GameObject checker;
		
		/// <summary>
		/// create point after pressed E
		/// </summary>
		void Update () {
			if(Input.GetKeyDown(KeyCode.E))
			{
				createPoints();
			}
		}
		/// <summary>
		/// Creates the points.
		/// </summary>
		/// <returns>GestChecker wtih points</returns>
		public GameObject createPoints()
		{
			//int count = transform.childCount;
			Queue<Transform> fronta = new Queue<Transform>();
			fronta.Enqueue(transform);
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
				GameObject clone = (GameObject) GameObject.Instantiate(checke,last.position,Quaternion.identity);
				clone.name = last.name+"-check";
				clone.transform.parent = checkerClone.transform;

			}
			return checkerClone;
		}

		/// <summary>
		/// Figures the copy.
		/// </summary>
		/// <returns>The copy.</returns>
		GameObject figureCopy()
		{
			GameObject figure = (GameObject) GameObject.Instantiate(mesh,mesh.transform.position,Quaternion.AngleAxis(180,Vector3.up));
			ExtendsAvatar avatar = figure.GetComponentInChildren<ExtendsAvatar>();
			if(avatar)
			{
				Destroy(avatar);
			}
			if (figure.transform.parent) 	
			{
				Transform temp = figure.transform.parent;
				figure.transform.parent = null;
				GameObjectEx.DestroyObjectWithAllParents(temp);
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


#endif
	}
}