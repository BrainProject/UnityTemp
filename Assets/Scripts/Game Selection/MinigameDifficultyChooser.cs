using UnityEngine; 
using UnityEngine.UI;
using System.Collections;



public class MinigameDifficultyChooser : MonoBehaviour
{
    internal Slider diffSlider;
    public GameObject diffSliderGO;

    // Set up scene according to minigameProps
    void Start()
    {
        diffSlider = diffSliderGO.GetComponent<Slider>();

        Game.MinigameProperties props = MGC.Instance.getSelectedMinigameProps();
        if(props == null)
        {
            Debug.LogError("Error during loading mini-game properties");

            MGC.Instance.sceneLoader.LoadScene(MGC.Instance.getSelectedMinigameName());
        }

        else
        {
            diffSlider.minValue = 0;
            diffSlider.maxValue = props.MaxDifficulty;
            diffSlider.value = props.DifficutlyLastPlayed;
            //print("Max diff value: " + props.MaxDifficulty);

            int x;
            int shiftX = 2160 / props.MaxDifficulty;

            //create "checked" icons for difficulties already finished
            for (int i = 0; i <= props.MaxDifficulty; i++)
            {
                if (props.finishedCount[i] > 0)
                {
                    x = i * shiftX - 960;

                    //instantiate
                    GameObject icon = (GameObject)Instantiate(Resources.Load("CheckedIcon") as GameObject);

                    //set parent
                    icon.transform.SetParent(diffSliderGO.transform);

                    //set position and scale
                    icon.transform.localPosition = new Vector3(x, 370, 0);
                    //icon.transform.localScale = new Vector3(1, 1, 1);

                    //TODO fine tune position and scale based on resolution...
                }
            }

            //TODO set correct difficulties icons for different minigames


        }
    }

    // evoked when "play" button is hitted
    public void StartMinigame()
    {
        //store difficulty into MGC property
        MGC.Instance.selectedMiniGameDiff = (int)diffSlider.value;
        MGC.Instance.getSelectedMinigameProps().DifficutlyLastPlayed = (int)diffSlider.value;

        MGC.Instance.sceneLoader.LoadScene(MGC.Instance.getSelectedMinigameName());
    }

}
