using UnityEngine;
using System.Collections;

public class MinigameController : MonoBehaviour {

    public static int difficutly = 0;
    public static int level = 0;

    //The whole maze including walls, obstacles, win area and player ball
    GameObject maze;

	void Start () {
        //set recieved difficulty from MGC here
        //add 1 to the difficulty - minigame uses 1-3, while MGC uses 0-2
        difficutly = MGC.Instance.selectedMiniGameDiff + 1;//3;
        StartCoroutine(NextLevel());
        MGC.Instance.minigamesProperties.SetPlayed("Physics Maze", difficutly);
	}

    public void LoadNextLevel()
    {
        StartCoroutine(NextLevel());
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1.5f);
        level++;

        //unload finished level
        if (maze != null)
        {
            Destroy(maze);
        }

        GameObject newmaze = Resources.Load("Levels/Maze" + difficutly + "_" + level, typeof(GameObject)) as GameObject;

        //next level exists
        if (newmaze != null)
        {
            maze = Instantiate(newmaze) as GameObject;
        }
        else
        {
            Debug.Log("No more levels to load, end minigame");
            //END MINIGAME HERE
            MGC.Instance.WinMinigame();
        }
        yield return 0;
    }
}
