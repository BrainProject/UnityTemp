using UnityEngine;
using System.Collections;

public class IconClose : MonoBehaviour 
{
    public Texture closeIconTexture;
    public SceneFadeInOut fader;
    public string sceneName;
    
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 60, 30, 50, 50), closeIconTexture))
        {
            fader.EndScene(sceneName);
        }
    }
}
