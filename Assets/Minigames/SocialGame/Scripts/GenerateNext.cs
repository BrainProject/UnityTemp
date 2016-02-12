using UnityEngine;
using System.Collections;

public class GenerateNext : MonoBehaviour {

	public GameObject[] GestChecker;
	public Vector3 position;
	public int SelectedType = -1;

	public void Next()
	{
	
		if (SelectedType >= 0) 
		{
			GameObject.Instantiate(GestChecker[SelectedType],position,Quaternion.identity);
			//Debug.LogWarning(gameObject.name + " is in Debug mod set DebugInt to -1 for cancel debugmod");
			return;
		}
		int rnd = Random.Range(0,GestChecker.Length);
		GameObject.Instantiate(GestChecker[rnd],position,Quaternion.identity);
	}

	void Start()
	{
        SelectedType = MGC.Instance.selectedMiniGameDiff;
        Next();
	}
}
