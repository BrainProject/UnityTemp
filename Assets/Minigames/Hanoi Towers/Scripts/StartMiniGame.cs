using UnityEngine;
using System.Collections;

public class StartMiniGame : MonoBehaviour 
{
    public MetaGameController gameController;
    public SceneFadeInOut fader;

    public string gameSceneName;


    void Start()
    {
        
    }
    void OnMouseEnter()
    {
        gameController.MoveCamera(true, transform.position);
    }

    void OnMouseExit()
    {
        print("OnMouseExit...");
        gameController.MoveCamera(false, transform.position);
    }

    void OnMouseOver()
    {
        //print("mouse up");
        if (Input.GetButtonDown("Fire1"))
        {
            if (gameSceneName != "")
            {
                fader.EndScene(gameSceneName);
            }
        }        
    }


    void Update()
    {

    }
}
