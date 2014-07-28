using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour 
{
    public float fadeSpeed = 1.5f;

    private bool sceneStarting = true;
    private bool sceneEnding = false;
    private string nextSceneName;

    void Awake()
    {
        guiTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }
	
	
	void Update () 
    {
        if (sceneStarting)
        {
            FadeToClear();
        }
        else if (sceneEnding)
        {
            FadeToColor();
        }
	}

    void FadeToClear()
    {
        print("fading in...");
        guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);

        if (guiTexture.color.a <= 0.05f)
        {
            guiTexture.color = Color.clear;
            guiTexture.enabled = false;
            sceneStarting = false;
        }
    }

    void FadeToColor()
    {
        print("fading out..., alpha = " + guiTexture.color.a);
        guiTexture.color = Color.Lerp(guiTexture.color, Color.white, fadeSpeed * Time.deltaTime);

        if (guiTexture.color.a >= 0.95f)
        {
            print("switching scene to: |" + nextSceneName + "|");

            if (nextSceneName != "")
            {
                Application.LoadLevel(nextSceneName);
            }
            else
            {
                Application.Quit();
            }
        }
    }


    public void EndScene(string sceneName)
    {
        print("fading out started");
        guiTexture.enabled = true;
        sceneEnding = true;
        nextSceneName = sceneName;
    }
}
