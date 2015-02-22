using UnityEngine; 
using UnityEngine.UI;
using System.Collections;



public class MinigameDifficultyChooser : MonoBehaviour
{
    public Slider diffSlider;

    // Set up scene according to minigameProps
    void Start()
    {
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
            diffSlider.value = diffSlider.minValue;

            print("Max diff value: " + props.MaxDifficulty);

            //TODO set proper icons and other stuff of scene here

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
