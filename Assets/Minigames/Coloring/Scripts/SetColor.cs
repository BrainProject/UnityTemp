using UnityEngine;
using System.Collections;

public class SetColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)){ // if left button pressed...

			Transform selected;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				selected = hit.transform;
				if(selected.tag == "Board"){
					//selected.renderer.guiTexture.color = gameObject.renderer.material.color;
					selected.renderer.material.color = gameObject.renderer.material.color;
				}
			}
		}
	}
}
