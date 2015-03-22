/**
 * @file ChooseImageSet.cs
 * @author Ján Bella
 */
using UnityEngine;
using System.Collections;
using System.IO;
using System;

namespace FindIt
{
    /*
     * Logic for choosing set of images for Find It.
     */
    public class ChooseImageSet : MonoBehaviour
	{
		public string resourcePackName = "Animals";
        public bool custom = false;

        void OnMouseDown()
		{
			PlayerPrefs.SetString("resourcePackName", resourcePackName);
            PlayerPrefs.SetInt("custom", custom ? 1 : 0);
            MGC.Instance.sceneLoader.LoadScene("FindItGame", true);
		}
	}
}