/*
 * Created by: Milan Doležal
 */ 


using UnityEngine;
using System.Collections;

public class IconBillboarding : MonoBehaviour {
	void Update () {
		this.transform.rotation = Camera.main.transform.rotation;
	}
}
