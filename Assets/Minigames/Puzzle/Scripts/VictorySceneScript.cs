using UnityEngine;
using System.Collections;

namespace Puzzle
{
    public class VictorySceneScript : MonoBehaviour
    {
        void OnGUI()
        {
            if (GUI.Button(new Rect(
                    Screen.width / 2 + 30,
                    Screen.height / 2 - 75,
                    200,
                    50),
                "Nová hra"))
            {
                Application.LoadLevel("ChoosePicture");
            }

            if (GUI.Button(new Rect(
                    Screen.width / 2 + 30,
                    Screen.height / 2 - 15,
                    200,
                    50),
              "Zobraziť štatistiky"))
            {
                Application.LoadLevel("Statistics");
            }

            if (GUI.Button(new Rect(
                Screen.width / 2 + 30,
                Screen.height / 2 + 45,
                200,
                50),
              "Ukončiť hru"))
            {
                Application.Quit();
            }
        }
    }
}
