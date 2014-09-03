using UnityEngine;
using System.Collections;

public class CreateMGC : MonoBehaviour {

	void Awake () {
		if(MGC.Instance.currentBrainPart == BrainPartName.none)
		{
			MGC.Instance.currentBrainPart = BrainPartName.ParietalLobe;
			MGC.Instance.currentCameraDefaultPosition = Camera.main.GetComponent<MinigameSelection.CameraControl>().currentWaypoint.transform.position;
		}
	}
}
