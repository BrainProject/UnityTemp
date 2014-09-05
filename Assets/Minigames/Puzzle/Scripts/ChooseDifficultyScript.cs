using UnityEngine;
using System.Collections;

namespace Puzzle
{
    public class ChooseDifficultyScript : MonoBehaviour
    {
        public int size;

        void OnMouseDown()
        {
            PlayerPrefs.SetInt("size", size);
            PuzzleStatistics.numberPieces = size;
            Application.LoadLevel("Game");
            //AutoFade.LoadLevel("Game", 3, 1, Color.white);
            

        }
    }
}