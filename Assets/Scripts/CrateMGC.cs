using UnityEngine;
using System.Collections;

public class CrateMGC : MonoBehaviour {

	void Start () {
		MGC.Instance.currentBrainPart = BrainPartName.ParietalLobe;
		MGC.Instance.currentCameraDefaultPosition = Camera.main.GetComponent<MinigameSelection.CameraControl>().currentWaypoint.transform.position;
	}
}
