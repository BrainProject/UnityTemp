/**
 *@file ChoosePictureScript.cs
 *@author Ján Bella
 *
 *Logic for ChoosePicureScene
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace Puzzle
{
    public class ChoosePictureScript : MonoBehaviour
    {
        public bool custom = false;
        public string imageName = "Bonobo";
        public string defaultPicsPath = "Pictures";

		/**
		 * Handles MouseDown event
		 * Saving chosen texture to resources, loading ChooseDifficultyScene
		 */
        void OnMouseDown()
        {
            PlayerPrefs.SetString("defaultPicsPath", defaultPicsPath);
            PlayerPrefs.SetString("Image", imageName);
            PlayerPrefs.SetInt("custom", custom ? 1 : 0);

            PuzzleStatistics.pictureName = gameObject.renderer.material.mainTexture.name;
			MGC.Instance.sceneLoader.LoadScene("PuzzleGame",true);
        }
    }
}
