using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIMenu : MonoBehaviour {

	public GameObject image1;
	public GameObject image2;
	public GameObject image3;
	public GameObject image4;
	private List<GameObject> images = new List<GameObject>(4);


	// Use this for initialization
	void Start () {
		images.Add(image1);
		images.Add(image2);
		images.Add(image3);
		images.Add(image4);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI (){	
		if(GUI.Button (new Rect(100,100,205,50),"Znovu")){
			Application.LoadLevel("Coloring");
		}

		if(GUI.Button (new Rect(100,170,205,50),"Další obrázek")){
			int i = 0;
			for (i = 0; i < images.Count; i++) {
				if(images[i].activeSelf)
				{
					images[i].SetActive(false);
					i = ((i + 1) == (images.Count)) ? 0 : (i + 1);
					images[i].SetActive(true);
					break;
				}
			}
		}

		if(GUI.Button (new Rect(100,240,205,50),"Konec")){
			Application.Quit();
		}
	}
}
