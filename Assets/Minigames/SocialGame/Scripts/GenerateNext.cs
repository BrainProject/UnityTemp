using UnityEngine;
using System.Collections;

public class GenerateNext : MonoBehaviour {

	public GameObject[] GestChecker;
	public Vector3 position;

	public void Next()
	{
		int rnd = Random.Range(0,GestChecker.Length);
		GameObject.Instantiate(GestChecker[rnd],position,Quaternion.identity);
	}

	void Start()
	{
		Debug.Log("test");
		Next();
	}
}
