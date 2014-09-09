using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace Puzzle
{
    public class ChoosePictureScript : MonoBehaviour
    {

        void OnMouseDown()
        {
            PlayerPrefs.SetString("Image", gameObject.renderer.material.mainTexture.name);
            PuzzleStatistics.pictureName = gameObject.renderer.material.mainTexture.name;
            Application.LoadLevel("PuzzleChooseDifficulty");
        }
    }
}
