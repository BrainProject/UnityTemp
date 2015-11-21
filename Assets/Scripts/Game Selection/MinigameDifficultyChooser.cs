using UnityEngine; 
using UnityEngine.UI;
using System.Collections;


public class MinigameDifficultyChooser : MonoBehaviour
{
    
    public GameObject diffSliderGO;
    public Image IconDifficultyLow;
    public Image IconDifficultyHigh;
    public Sprite DefaultSpriteDiffLow;
    public Sprite DefaultSpriteDiffHigh;

    internal Slider diffSlider;

    // Set up scene according to minigameProps
    void Start()
    {

        diffSlider = diffSliderGO.GetComponent<Slider>();

        Game.MinigameProperties props = MGC.Instance.getSelectedMinigameProperties();

        if (!props)
        {
            Debug.LogError("Error during loading mini-game properties");

            MGC.Instance.sceneLoader.LoadScene(MGC.Instance.getSelectedMinigameName());
        }

        else
        {
			print("Setting difficulty choosing screen for mini-game: '" + props.readableName + "'");

            int maxDiff = props.MaxDifficulty;
            diffSlider.minValue = 0;
            diffSlider.maxValue = maxDiff;
            diffSlider.value = props.stats.DifficutlyLastPlayed;
			print("Mini-game '" + props.readableName + "' offers " + (maxDiff+1)+ " difficulties." );

            int x;
            int shiftX = 2160 / maxDiff;

            //create "checked" icons for difficulties already finished
            for (int i = 0; i <= maxDiff; i++)
            {
                if (props.stats.finishedCount[i] > 0)
                {
                    //instantiate
                    GameObject icon = (GameObject)Instantiate(Resources.Load("CheckedIcon") as GameObject);

                    //set parent
                    icon.transform.SetParent(diffSliderGO.transform);

                    //set position and scale
					x = i * shiftX - 1050;
                    icon.transform.localPosition = new Vector3(x, 370, 0);
                    icon.transform.localScale = new Vector3(1, 1, 1);


                    //TODO better solution, independent on resolution
                }
            }

            //TODO set correct difficulties icons for different minigames
            if (props.IconDifficultyLow)
            {
                IconDifficultyLow.sprite = props.IconDifficultyLow;
            }
                // use default
            else
            {
                IconDifficultyLow.sprite = DefaultSpriteDiffLow;
            }

            if (props.IconDifficultyHigh)
            {
                IconDifficultyHigh.sprite = props.IconDifficultyHigh;
            }
            // use default
            else
            {
                IconDifficultyHigh.sprite = DefaultSpriteDiffHigh;
            }


        }
    }

    // evoked when "play" button is hitted
    public void StartMinigame()
    {
        //store difficulty into MGC property
        MGC.Instance.selectedMiniGameDiff = (int)diffSlider.value;
        MGC.Instance.getSelectedMinigameProperties().stats.DifficutlyLastPlayed = (int)diffSlider.value;

        MGC.Instance.sceneLoader.LoadScene(MGC.Instance.getSelectedMinigameName());
    }

}
