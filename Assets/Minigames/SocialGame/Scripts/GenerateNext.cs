using UnityEngine;
using System.Collections;

public class GenerateNext : MonoBehaviour {

	public GameObject[] GestChecker;
	public Vector3 position;

	public void Next()
	{
		int rnd = Random.Range(0,GestChecker.Length-1);
		GameObject.Instantiate(GestChecker[rnd],position,Quaternion.identity);
	}

	void Start()
	{
		Next();
	}
}
