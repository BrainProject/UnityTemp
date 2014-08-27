/*
 * Created by: Milan Doležal
 */ 


using UnityEngine;
using System.Collections;

namespace Game{
	public class IconBillboarding : MonoBehaviour {
		void Update () {
			this.transform.rotation = Camera.main.transform.rotation;
			//this.transform.LookAt (Camera.main.transform.position);
		}
	}
}