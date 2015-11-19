using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class SetColor : MonoBehaviour
	{
		void Update ()
		{
			if (Input.GetMouseButtonDown(0))
			{
				SetNewColor();
			}
		}

		public void SetNewColor()
		{
			Transform selected;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				selected = hit.transform;
				if(selected.tag == "Board")
				{
					//selected.renderer.guiTexture.color = gameObject.renderer.material.color;
					selected.GetComponent<Renderer>().material.color = gameObject.GetComponent<Renderer>().material.color;
				}
			}
		}
	}
}