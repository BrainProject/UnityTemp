using UnityEngine;
using System.Collections;

public class AnimationEvents : MonoBehaviour {
	public GameObject[] Images;

	void DeactivateImages(){
		for (int i = 0; i < Images.Length; i++) {
			Images[i].SetActive(false);	
		}	
	}
}
