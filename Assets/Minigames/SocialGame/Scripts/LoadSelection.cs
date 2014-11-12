using UnityEngine;
using System.Collections;

public class LoadSelection : MonoBehaviour {


	void Start ()
	{
		print(MGC.Instance);
		MGC.Instance.ShowCustomCursor ();
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
