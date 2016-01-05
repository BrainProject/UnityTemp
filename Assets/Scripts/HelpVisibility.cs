using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
			   MGC.Instance.getSelectedMinigameProperties().mainScene == SceneManager.GetActiveScene().name)
			{
				helpPrefab = MGC.Instance.getSelectedMinigameProperties ().helpPrefab;
				//helpDuration = MGC.Instance.getSelectedMinigameProperties().helpDuration;
				helpClone = GameObject.Instantiate(helpPrefab) as GameObject;
				helpClone.transform.SetParent(this.transform);
				helpClone.transform.localEulerAngles = Vector3.zero;
				helpClone.transform.localPosition = Vector3.zero;
			}
			
			if (helpClone)
			{
				// For the future selves - you can send this event even though you already took control for GUI by showing the hidden GUI...
				MGC.Instance.TakeControlForGUIAction(true);
				MGC.Instance.minigamesGUI.hide();
				thisAnimator.SetTrigger ("ShowHelp");
			}
			else
				neuronAnimator.SetTrigger ("wave");
		}
		else
			neuronAnimator.SetTrigger ("wave");
	}
	
	public void StartHelpAnimation()
	{
		helpClone.GetComponent<Animator> ().SetTrigger ("AnimateHelp");
		//StartCoroutine("ShowButtons");
	}
	
	public void ReplayHelpAnimation()
	{
		helpClone.GetComponent<Animator> ().SetTrigger ("ReplayHelp");
		helpClone.GetComponent<Animator> ().SetTrigger ("AnimateHelp");
        //ShowButtons();
	}

	public void HideHelpAnimation()
	{
		thisAnimator.SetTrigger ("HideHelp");
		Destroy (helpClone, 1.5f);
		helpClone = null;
		MGC.Instance.TakeControlForGUIAction (false);
	}

    /*
	public void StopShowingButtons()
	{
		StopCoroutine("ShowButtons");
	}*/

	public void ShowButtons()
	{
		//yield return new WaitForSeconds (helpDuration);

		MGC.Instance.minigamesGUI.hideHelpIcon.show();
		MGC.Instance.minigamesGUI.replayHelpIcon.show();
	}
}
 	