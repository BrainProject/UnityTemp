using UnityEngine;
using System.Collections;

public class LoadExternalApplication : MonoBehaviour 
{

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 15, 100, 30), "Run"))
        {
            print("Running external application");

            System.Diagnostics.Process.Start("notepad.exe");
            //TODO add necessary code here
        }
        
    }
}
