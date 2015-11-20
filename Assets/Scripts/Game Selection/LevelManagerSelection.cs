using UnityEngine;
using System.Collections;

namespace MinigameSelection
{
	public class LevelManagerSelection : MonoBehaviour {
		public GameObject frontalLobePos;
		public GameObject occipitalLobePos;
		public GameObject parietalLobePos;
		public GameObject cerebellumPos;
		public GameObject temporalLobePos;
		public GameObject minigameOnSelection;
		public GUITexture kinectRequiredIcon;
		public bool OnSelection { get; set; }

		void Start()
		{
			OnSelection = false;
			print ("this is game selection scene...");

			kinectRequiredIcon.GetComponent<GUITexture>().pixelInset = new Rect (0, 0, Screen.width / 16 * 2, Screen.height / 9 * 2);

			switch (MGC.Instance.currentBrainPart) {
			case BrainPartName.FrontalLobe: //Camera.main.transform.position = GameObject.Find ("GreenPos").transform.position;
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = frontalLobePos;
				break;
			case BrainPartName.ParietalLobe:
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = parietalLobePos;
				break;
			case BrainPartName.OccipitalLobe:
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = occipitalLobePos;
				break;
			case BrainPartName.TemporalLobe:
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = temporalLobePos;
				break;
			case BrainPartName.Cerebellum:
				Camera.main.GetComponent<CameraControl> ().currentWaypoint = cerebellumPos;
				break;
			//case BrainPartName.BrainStem:
				//Camera.main.GetComponent<CameraControl> ().currentWaypoint = GameObject.Find ("BrainStemPos");
				//break;
			}

			if (MGC.Instance.fromMain)
			{
				MGC.Instance.currentCameraDefaultPosition = Camera.main.GetComponent<CameraControl> ().currentWaypoint.transform.position;
			}
			
			//if player comes to selection scene from main, he can leave immediately by pressing Vertical key
			Camera.main.GetComponent<CameraControl> ().ReadyToLeave = MGC.Instance.fromMain;
			Camera.main.transform.position = Camera.main.GetComponent<CameraControl> ().currentWaypoint.transform.position;
			Camera.main.transform.rotation = Camera.main.GetComponent<CameraControl> ().currentWaypoint.transform.rotation;
			MGC.Instance.fromMain = false;
            MGC.Instance.minigamesGUI.backIcon.gameObject.SetActive(true);
            MGC.Instance.minigamesGUI.backIcon.show();
        }

		public void FadeInOutKinectIcon()
		{
			StopCoroutine ("FadeInOutKinect");
			StartCoroutine ("FadeInOutKinect");
		}

		IEnumerator FadeInOutKinect()
		{
			float startTime = Time.time;
			Color startColor = kinectRequiredIcon.GetComponent<GUITexture>().color;
			Color targetColor = kinectRequiredIcon.GetComponent<GUITexture>().color;
			targetColor.a = 1;
			
			while(kinectRequiredIcon.GetComponent<GUITexture>().color.a < 0.51f)
			{
				kinectRequiredIcon.GetComponent<GUITexture>().color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}

			yield return new WaitForSeconds (1);

			startTime = Time.time;
			startColor = kinectRequiredIcon.GetComponent<GUITexture>().color;
			targetColor = kinectRequiredIcon.GetComponent<GUITexture>().color;
			targetColor.a = 0;
			
			while(kinectRequiredIcon.GetComponent<GUITexture>().color.a > 0.01f)
			{
				kinectRequiredIcon.GetComponent<GUITexture>().color = Color.Lerp (startColor, targetColor, (Time.time - startTime));
				yield return null;
			}

			kinectRequiredIcon.GetComponent<GUITexture>().color = targetColor;
		}
	}
}