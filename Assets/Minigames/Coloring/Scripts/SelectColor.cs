/**
 *@author Tomáš Pouzar & Ján Bella
 */

using UnityEngine;
using System.Collections;


namespace Coloring
{
	public class SelectColor : MonoBehaviour {

		public GameObject Brush;
		public LevelManagerColoring thisLevelManager;

#if UNITY_ANDROID
		private Material neuronMaterial;

		void Start()
		{
			neuronMaterial = GameObject.Find("Neuron_body").renderer.material;
		}
#endif

		void OnMouseDown () {
			if(thisLevelManager.painting)
			{
                Transform selected;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    selected = hit.transform;
                    if (selected.tag == "ColoringBlob")
                    {
                        Brush.renderer.material.color = selected.renderer.material.color;
                        thisLevelManager.brushColor = selected.renderer.material.color;
                    }
                }

				
#if UNITY_ANDROID
				neuronMaterial.color = Brush.renderer.material.color;
#endif
			}
		}
	}
}