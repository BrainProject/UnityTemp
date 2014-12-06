using UnityEngine;
using System.Collections;

/// <summary>
/// Main menu gui for london tower
/// </summary>
public class LondonTowerMainMenu : MonoBehaviour {


    public Texture selectLevel, exitGame, levelSet1, levelSet2, levelSet3, levelSet4, back;


    /// <summary>
    /// 0 main menu, 1 level select
    /// </summary>
    private int guiState = 0;
	


    void OnGUI()
    {

       if (guiState == 0)
           {
               GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 5), selectLevel);    
               if (GUI.Button(new Rect(Screen.width /4, Screen.height /4, Screen.width / 2, Screen.height / 5), "Select level"))
               {
                   guiState =1;
               }
               

               GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height / 2, Screen.width / 2, Screen.height / 5), exitGame);    
               if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 2, Screen.width / 2, Screen.height / 5), "Exit game"))
               {
                   //vrátit se zpět do menu
                   //Application.LoadLevel(Application.loadedLevel);
                   //LoadLevel this
                   Application.Quit();
               }
               

           }

       if (guiState == 1)
        {
            GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height / 9, Screen.width / 2, Screen.height / 7), levelSet1);
            if (GUI.Button(new Rect(Screen.width / 4, Screen.height / 9, Screen.width / 2, Screen.height / 7), "level 1"))
            {

                LondonTowerGameManager.dataSet = 1;
				MGC.Instance.sceneLoader.LoadScene("LondonTowerGame");
                
                //LoadLevel this
            }
            
            GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height * 3 / 9, Screen.width / 2, Screen.height / 7), levelSet2);
            if (GUI.Button(new Rect(Screen.width / 4, Screen.height*3 / 9, Screen.width / 2, Screen.height / 7), "level 2"))
            {
                LondonTowerGameManager.dataSet = 2;
				MGC.Instance.sceneLoader.LoadScene("LondonTowerGame");

                //LoadLevel this
            }
            
            GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height * 5 / 9, Screen.width / 2, Screen.height / 7), levelSet3); 
           if (GUI.Button(new Rect(Screen.width / 4, Screen.height*5 / 9, Screen.width / 2, Screen.height / 7), "level3"))
            {
                LondonTowerGameManager.dataSet = 3;
				MGC.Instance.sceneLoader.LoadScene("LondonTowerGame");

                //LoadLevel this
            }
            
            GUI.DrawTexture(new Rect(Screen.width / 4, Screen.height * 7 / 9, Screen.width / 2, Screen.height / 7), levelSet4);
            if (GUI.Button(new Rect(Screen.width / 4, Screen.height*7 / 9, Screen.width / 2, Screen.height / 7), "level 4"))
            {
                LondonTowerGameManager.dataSet = 4;
				MGC.Instance.sceneLoader.LoadScene("LondonTowerGame");

                //LoadLevel this
            }
            

            GUI.DrawTexture(new Rect(Screen.width - Screen.width / 5, Screen.height * 8 / 9, Screen.width / 6, Screen.height / 8), back);   
           if (GUI.Button(new Rect(Screen.width - Screen.width / 5, Screen.height * 8 / 9, Screen.width / 6, Screen.height / 8), "back to menu"))
            {
                guiState = 0;

                //LoadLevel this
            }
            
        }
    }

}
