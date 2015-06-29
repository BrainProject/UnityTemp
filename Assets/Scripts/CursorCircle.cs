using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
	public class CursorCircle : MonoBehaviour {
		public Image circleImage;
		public float progress;

		private float delay = 0.5f;

		void Update()
		{
			if (progress >= delay)
				circleImage.fillAmount = (progress - delay) * 2;
			else
				circleImage.fillAmount = 0;
		}
	}
}