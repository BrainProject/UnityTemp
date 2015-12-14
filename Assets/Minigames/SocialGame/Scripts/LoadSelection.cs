using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSelection : MonoBehaviour {


	void Start ()
	{
		print(MGC.Instance);
		MGC.Instance.ShowCustomCursor (true);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
