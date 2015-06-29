using UnityEngine;
using System.Collections;

public class HelpVisibility : MonoBehaviour {
	public Animator thisAnimator;
	public GameObject helpPrefab;
	public Animator neuronAnimator;

	public void ShowHelpAnimation()
	{
		if (helpPrefab)
		{
			thisAnimator.SetTrigger("ShowHelp");
		}
		else
			neuronAnimator.SetTrigger ("wave");
	}

	public void StartHelpAnimation()
	{
		helpPrefab.GetComponent<Animator> ().SetTrigger ("AnimateHelp");
	}
}
