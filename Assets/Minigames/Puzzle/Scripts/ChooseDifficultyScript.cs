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
            Application.LoadLevel("Game");
        }
    }
}