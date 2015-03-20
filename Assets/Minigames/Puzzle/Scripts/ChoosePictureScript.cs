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

		/**
		 * Handles MouseDown event
		 * Saving chosen texture to resources, loading ChooseDifficultyScene
		 */
        void OnMouseDown()
        {
            PlayerPrefs.SetString("Image", gameObject.renderer.material.mainTexture.name);
            PuzzleStatistics.pictureName = gameObject.renderer.material.mainTexture.name;
			MGC.Instance.sceneLoader.LoadScene("PuzzleGame",true);
        }
    }
}
