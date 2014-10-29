/**
 *@file ChooseDifficultyScript.cs
 *@author Ján Bella
 *
 *Logic for ChooseDifficultyScene
 */
using UnityEngine;
using System.Collections;

namespace Puzzle
{
    public class ChooseDifficultyScript : MonoBehaviour
    {
		// demanded size of puzzle (number of pieces)
        public int size;

		/**
		 * Handles MouseDown event
		 * Saving chosen size to resources, loading PuzzleGame
		 */
        void OnMouseDown()
        {
            PlayerPrefs.SetInt("size", size);
            PuzzleStatistics.numberPieces = size;
			MGC.Instance.sceneLoader.LoadScene("PuzzleGame",true);
            //Application.LoadLevel("PuzzleGame");
        }
    }
}