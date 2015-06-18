/**
 *@author Tomáš Pouzar
 */
using UnityEngine;
using System.Collections;

namespace Coloring
{
	public class AnimationEvents : MonoBehaviour {
		public Animator paletteAnimator;
		public GameObject[] Images;
		
		public void DisableImages()
		{
			print ("Switching to painting mode.");
			for (int i = 0; i < Images.Length; i++)
				Images[i].SetActive(false);
		}
		
		public void HidePalette()
		{
			paletteAnimator.SetBool("visible", false);
			paletteAnimator.SetTrigger ("animate");
		}
		public void ShowPalette()
		{
			paletteAnimator.SetBool("visible", true);
			paletteAnimator.SetTrigger ("animate");
		}
	}
}