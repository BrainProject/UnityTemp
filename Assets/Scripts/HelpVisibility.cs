using UnityEngine;
using System.Collections;

public class HelpVisibility : MonoBehaviour {
	public Animator thisAnimator;
	public GameObject helpPrefab;
	public float helpDuration;
	public Animator neuronAnimator;

	internal GameObject helpClone;

	public void ShowHelpAnimation()
	{
		if(!helpClone)
		{
			if(MGC.Instance.getSelectedMinigameProperties () &&
			   MGC.Instance.getSelectedMinigameProperties ().helpPrefab &&
			   MGC.Instance.getSelectedMinigameProperties().mainScene == Application.loadedLevelName)
			{
				helpPrefab = MGC.Instance.getSelectedMinigameProperties ().helpPrefab;
				helpDuration = MGC.Instance.getSelectedMinigameProperties().helpDuration;
				helpClone = GameObject.Instantiate(helpPrefab) as GameObject;
				helpClone.transform.SetParent(this.transform);
				helpClone.transform.localEulerAngles = Vector3.zero;
				helpClone.transform.localPosition = Vector3.zero;
			}
			
			if (helpClone)
				thisAnimator.SetTrigger ("ShowHelp");
			else
				neuronAnimator.SetTrigger ("wave");
		}
		else
			neuronAnimator.SetTrigger ("wave");
	}
	
	public void StartHelpAnimation()
	{
		helpClone.GetComponent<Animator> ().SetTrigger ("AnimateHelp");
		StartCoroutine("ShowButtons");
	}
	
	public void ReplayHelpAnimation()
	{
		helpClone.GetComponent<Animator> ().SetTrigger ("ReplayHelp");
		helpClone.GetComponent<Animator> ().SetTrigger ("AnimateHelp");
		StartCoroutine("ShowButtons");
	}

	public void HideHelpAnimation()
	{
		thisAnimator.SetTrigger ("HideHelp");
		Destroy (helpClone, 1.5f);
		helpClone = null;
	}

	public void StopShowingButtons()
	{
		StopCoroutine("ShowButtons");
	}

	private IEnumerator ShowButtons()
	{
		yield return new WaitForSeconds (helpDuration);

		MGC.Instance.minigamesGUI.hideHelpIcon.show();
		MGC.Instance.minigamesGUI.replayHelpIcon.show();
	}
}
 	