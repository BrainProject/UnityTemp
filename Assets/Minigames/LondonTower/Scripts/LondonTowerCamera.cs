using UnityEngine;
using System.Collections;
using Game;

/// <summary>
/// class for taking screenshot/minimap that show player final goal
/// everething is started from LondonTowerGameManager script
/// here is implemented takins screenshot - (without profeatures) thanks to this it take a while when screen is ready(cant be returned in method)
/// draw minimap/screenshot
/// 
/// </summary>
public class LondonTowerCamera : MonoBehaviour {

    public Texture2D screen;
    public bool takeScreen = false;
    public bool done = false;
    public bool animtionDone = false;
    private Color c;
    private Texture2D texture;
   // public bool ready = false;
    public GameObject plane;
    private float change = 0.25f;
    private Texture2D minimapBacground;
    public SceneLoader fade;
    private float fadeTime = 3.5f;


    void Awake()
    {
        minimapBacground = new Texture2D(1, 1);
        minimapBacground.SetPixel(0, 0, new Color(7.0f/255.0f,24.0f/255.0f,51.0f/255.0f));
        minimapBacground.Apply();
    }
    
    /// <summary>
    /// děla screenshot
    /// </summary>
    void OnPostRender()
    {

        if (takeScreen)
        {
            Debug.Log("run");
            Texture2D tex = new Texture2D(Screen.width, Screen.height);
            tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            tex.Apply();
            screen = tex;
            takeScreen = false;
            done = true;
            c = new Color(0, 0, 0, 0.1f);
            texture = new Texture2D(1, 1);
            SetColor(c);

        }
    }

    /// <summary>
    /// zařizuje přechod přes černou
    /// </summary>
    void Update()
    {
        if (done)
        {
            if (LondonTowerGameManager.state != LondonTowerGameState.animationStart)
            {
                LondonTowerGameManager.state = LondonTowerGameState.animationStart;
            }
            
           fade.FadeIn();
            fadeTime = fadeTime - Time.deltaTime;/*  c.a = c.a + change * Time.deltaTime;
            SetColor(c);
            if (c.a >= 1)
            {
              //  plane.SetActive(true);
              //  plane.renderer.material.mainTexture = screen;
                change = -change;
                LondonTowerGameManager.state = LondonTowerGameState.animationEnd;

            }
            else
                if (c.a < 0.1)
                {
                   
                }*/
            if (fadeTime < 0)
            {

                animtionDone = true;
                done = false;
                LondonTowerGameManager.state = LondonTowerGameState.animationEnd;
            }
        }
       
    }

    /// <summary>
    /// doing transitions throught black
    /// and draw "minimap"
    /// </summary>
    void OnGUI()
    {

        if (done && !animtionDone)
        {
            GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),texture,ScaleMode.StretchToFill);
        }
        if (LondonTowerGameManager.state == LondonTowerGameState.game)
        {
            GUI.DrawTexture(new Rect(Screen.width /35.0f, Screen.height / 30.0f , Screen.width/ 4.2f , Screen.height / 4.2f), minimapBacground);
            GUI.DrawTexture(new Rect(Screen.width / 35.0f, Screen.height / 30.0f, Screen.width / 4.2f, Screen.height / 4.2f), screen);
        }
    }


    /// <summary>
    /// given color set as texture on plane - transition
    /// </summary>
    /// <param name="color"></param>
    private void SetColor(Color color)
    {
        texture.SetPixel(0, 0, c);
        texture.Apply();
    }

    

}
