/**
 * @file ChoosePicturesSetScript.cs
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
	public class ChoosePicturesSetScript : MonoBehaviour
	{
		public string resourcePackName = "Animals";

        void OnMouseDown()
		{
			PlayerPrefs.SetString("resourcePackName", resourcePackName);
            MGC.Instance.sceneLoader.LoadScene("FindItGame", true);
		}
	}
}