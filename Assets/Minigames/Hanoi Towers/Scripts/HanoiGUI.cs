using UnityEngine;
using System.Collections;

namespace HanoiTowers
{

    public class HanoiGUI : MonoBehaviour
    {
        public GameController controller;

        private bool toggleValue = true;
        private int disks = 3;

        //just for debug, will not be visible in final game...
        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(4, 4, 150, 180), "Tests...", GUI.skin.window);

            //GUI.color = new Color(0,1,0, 0.75f);
            GUILayout.Label("Time:  " + (int)(Time.time - controller.getGameStartTime()));
            GUILayout.Label("Score: " + controller.getScore());

            toggleValue = GUILayout.Toggle(toggleValue, "Animace disků");
            controller.disksAnimations = toggleValue;

            GUILayout.Label("Počet disků: " + disks);
            disks = (int)GUILayout.HorizontalSlider(disks, 2, 8);

            if (GUILayout.Button("Reset"))
            {
                controller.numberOfDisks = disks;
                controller.ResetGame();
            }

            GUILayout.EndArea();
        }


    }

}