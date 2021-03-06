﻿using UnityEngine; 
using UnityEngine.UI;
using System.Collections;


public class MinigameDifficultyChooser : MonoBehaviour
{
    
    public GameObject diffSliderGO;
    public Image IconDifficultyLow;
    public Image IconDifficultyMedium;
    public Image IconDifficultyHigh;
    public Sprite DefaultSpriteDiffLow;
    public Sprite DefaultSpriteDiffMedium;
    public Sprite DefaultSpriteDiffHigh;
    public Image MinigameIcon;

    internal Slider diffSlider;

    // Set up scene according to minigameProps
    void Start()
    {
        MGC.Instance.ShowCustomCursor(true);
        MGC.Instance.ResetKinect();

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

            if (props.stats.finishedCount == null)
            {
                Debug.LogError("Can't load minigames stats. Try to manually delete stats file at: '" +
                               Application.persistentDataPath + "/mini-games.stats'");
            }
            else
            {
                //create "checked" icons for difficulties already finished
                for (int i = 0; i <= maxDiff; i++)
                {
                    if (props.stats.finishedCount[i] > 0)
                    {
                        //instantiate
                        GameObject icon = (GameObject) Instantiate(Resources.Load("CheckedIcon") as GameObject);

                        //set parent
                        icon.transform.SetParent(diffSliderGO.transform);

                        //set position and scale
                        x = i * shiftX - 1050;
                        icon.transform.localPosition = new Vector3(x, 370, 0);
                        icon.transform.localScale = new Vector3(1, 1, 1);

                        //TODO better solution, independent on resolution
                    }
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

            if (props.IconDifficultyMedium)
            {
                IconDifficultyMedium.enabled = true;
                IconDifficultyMedium.sprite = props.IconDifficultyMedium;
            }
            // use default
            else
            {
                IconDifficultyMedium.enabled = false;
                //IconDifficultyMedium.sprite = DefaultSpriteDiffMedium;
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

            if(props.minigameIcon)
            {
                MinigameIcon.sprite = props.minigameIcon;
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

    public void SetMinigameIcon(Sprite newIcon)
    {

    }
}
