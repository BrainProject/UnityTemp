using UnityEngine;
using System.Collections;


// used to set color for the part of the image
namespace Coloring
{
	public class SetColor : MonoBehaviour
	{
		void Update ()
		{
			if (Input.GetMouseButtonDown(0))
			{
				Transform selected;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					selected = hit.transform;
					if(selected.tag == "Board")
					{
						selected.renderer.material.color = gameObject.renderer.material.color;
					}
				}
			}
		}
	}
}